/* Copyright (c) 2009-2010 Tyrell Corporation & Moyshe Ben Rabi.

   The contents of this file are subject to the Mozilla Public License
   Version 1.1 (the "License"); you may not use this file except in
   compliance with the License. You may obtain a copy of the License at
   http://www.mozilla.org/MPL/

   Software distributed under the License is distributed on an "AS IS"
   basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
   License for the specific language governing rights and limitations
   under the License.

   The Original Code is an implementation of the Metaverse eXchange Protocol.

   The Initial Developer of the Original Code is Akos Maroy and Moyshe Ben Rabi.
   All Rights Reserved.

   Contributor(s): Akos Maroy and Moyshe Ben Rabi.

   Alternatively, the contents of this file may be used under the terms
   of the Affero General Public License (the  "AGPL"), in which case the
   provisions of the AGPL are applicable instead of those
   above. If you wish to allow use of your version of this file only
   under the terms of the AGPL and not to allow others to use
   your version of this file under the MPL, indicate your decision by
   deleting the provisions above and replace them with the notice and
   other provisions required by the AGPL. If you do not delete
   the provisions above, a recipient may use your version of this file
   under either the MPL or the AGPL.
*/

#include <string>
#include <vector>
#include <boost/smart_ptr.hpp>
#include <boost/date_time.hpp>
#include <boost/test/unit_test.hpp>

#include <mxp/packet.h>
#include <mxp/message.h>

#include "zip.h"
#include "unzip.h"
#include "test_iot_zip.h"
#include "reference_messages.h"

namespace mxp {
namespace test {
namespace iot {
namespace zip {

using namespace boost;
using namespace mxp;

static const std::string reference_zip_file_name =
                                        "var/mxp_0_5_reference_messages.zip";

static const posix_time::ptime timestamp(gregorian::date(2009, 11, 5),
                                         posix_time::time_duration(15, 33, 25));

/*! Get a list of reference packets from a zip file containing reference
    packets in binary form.

    \param name the name of the packet file within the reference zip file
    \return a vector of packets that was read from the reference zip file.
            an empty vector is returned if there was no such packet in the
            zip file.
 */
static std::vector<packet::packet>
get_reference_packets(const std::string name) {
    std::vector<packet::packet>     packets;

    // first open the zip file, and read the entry from as specified by name
    unzFile zfile = unzOpen(reference_zip_file_name.c_str());

    if (!zfile) {
        return packets;
    }

    unz_file_info   file_info;
    if (UNZ_OK != unzLocateFile(zfile, name.c_str(), 2)
     || unzGetCurrentFileInfo(zfile, &file_info, 0, 0, 0, 0, 0, 0)
     || unzOpenCurrentFile(zfile)) {

        unzClose(zfile);
        return packets;
    }

    std::vector<uint8_t>    bytes;
    bytes.resize(file_info.uncompressed_size);

    // accessing the contents of the vector as a single array is safe
    // according to Nicolai Josuttis in his STL book, section 6.2.3.
    if ((signed int) bytes.size() !=
                        unzReadCurrentFile(zfile, &bytes[0], bytes.size())) {
        unzClose(zfile);
        return packets;
    }
    unzClose(zfile);

    // now let's de-serialize the packets from the entry read from the zip file
    std::vector<uint8_t>::iterator it = bytes.begin();
    while (it != bytes.end()) {
        packet::packet  p;

        p.deserialize(it, bytes.size() - (it - bytes.begin()));
        packets.push_back(p);
    }


    return packets;
}

/*! Compare a message to a reference serialized version that is stored in a
    reference zip file.

    \param name the name of the reference message in the zip file
    \param msg a message to compare the de-serialized message from the zip file
           to
    \param guaranteed of the message was supposed to be a guaranteed message
 */
template<typename M>
void compare_message_to_zip(const std::string       name,
                            shared_ptr<M>           msg,
                            bool                    guaranteed) {
    typedef std::vector<shared_ptr<mxp::message::message> >  message_container;

    // get the packets from the zip file
    std::vector<packet::packet> packets = get_reference_packets(name);

    BOOST_CHECK_EQUAL(1u, packets.size());

    BOOST_CHECK_EQUAL(packets[0].session_id, 1u);
    BOOST_CHECK_EQUAL(packets[0].packet_id, 1u);
    BOOST_CHECK_EQUAL(packets[0].first_send_time, timestamp);
    BOOST_CHECK_EQUAL(packets[0].guaranteed, guaranteed
                                           ? (uint8_t) 1 : (uint8_t) 0);
    BOOST_CHECK_EQUAL(packets[0].resend_count, 0u);

    // get the messages from the packets, and check them
    message_container               messages;
    mxp::packet::message_frame_map  message_frames;
    std::back_insert_iterator<message_container> mit =
                                                std::back_inserter(messages);

    packets_to_messages(packets.begin(), packets.end(), mit, message_frames);

    BOOST_CHECK_EQUAL(message_frames.size(), 0u);
    BOOST_CHECK_EQUAL(messages.size(), 1u);
    BOOST_CHECK_EQUAL(messages[0]->get_type(), msg->get_type());

    shared_ptr<M> mmsg = dynamic_pointer_cast<M>(messages[0]);

    BOOST_CHECK_EQUAL(*mmsg, *msg);
}

/*! Add a message to a zip file.

    \param name the name of the message inside the zip file
    \param zfile the zip file
    \param message a pointer to the message to add
    \param message_id the id of the message to add
    \param guaranteed if the message is to be serialized as a guaranteed message
 */
static void add_message(const std::string               name,
                        zipFile                         zfile,
                        shared_ptr<message::message>    message,
                        uint32_t                        message_id,
                        bool                            guaranteed) {

    // turn the message into packets
    std::vector<shared_ptr<message::message> >  messages;
    messages.push_back(message);

    typedef std::vector<packet::packet> container;
    container packets;
    std::back_insert_iterator<container> pit = std::back_inserter(packets);

    packet::messages_to_packets(messages.begin(), messages.end(),
                                message_id,
                                pit,
                                1, // the session id
                                1, // the base packet id
                                timestamp,
                                guaranteed ? (uint8_t) 1 : (uint8_t) 0);

    if (packets.empty()) {
        return;
    }

    // serialize all generated packets
    unsigned int size = 0;
    BOOST_FOREACH(packet::packet & p, packets) {
        size += p.size();
    }

    std::vector<uint8_t> bytes;
    bytes.resize(size);
    std::vector<uint8_t>::iterator it = bytes.begin();

    BOOST_FOREACH(packet::packet & p, packets) {
        p.serialize(it);
    }

    // add a zip entry with the serialized data
    if (zipOpenNewFileInZip(zfile,
                            name.c_str(),
                            0, 0, 0, 0, 0, 0,
                            Z_DEFLATED, Z_DEFAULT_COMPRESSION)) {
        return;
    }

    // accessing the contents of the vector as a single array is safe
    // according to Nicolai Josuttis in his STL book, section 6.2.3.
    zipWriteInFileInZip(zfile, &bytes[0], bytes.size());
    zipCloseFileInZip(zfile);
}

/*! Generate reference messages, and store them in a zip file.

    \param zip_file_name the name of the zip file to store the messges in.
           a new file will be created with this name - anthing that might exist
           already will be overwritten.
 */
static void generate_messages(const std::string & zip_file_name) {
    // TODO: we should encapsulate this handler in an object, as per
    //       the RAII pattern
    zipFile zfile = zipOpen(zip_file_name.c_str(), APPEND_STATUS_CREATE);

    if (!zfile) {
        return;
    }

    add_message("messages/acknowledge.dat", zfile,
                generate_acknowledge(), 1, false);
    add_message("messages/keepalive.dat", zfile,
                generate_keepalive(), 1, true);
    add_message("messages/throttle.dat", zfile,
                generate_throttle(), 1, true);
    add_message("messages/challengerequest.dat", zfile,
                generate_challenge_rq(), 1, true);
    add_message("messages/challengeresponse.dat", zfile,
                generate_challenge_rsp(), 1, true);
    add_message("messages/joinrequest.dat", zfile,
                generate_join_rq(), 1, true);
    add_message("messages/joinresponse.dat", zfile,
                generate_join_rsp(), 1, true);
    add_message("messages/leaverequest.dat", zfile,
                generate_leave_rq(), 1, true);
    add_message("messages/leaveresponse.dat", zfile,
                generate_leave_rsp(), 1, false);
    add_message("messages/injectrequest.dat", zfile,
                generate_inject_rq(), 1, true);
    add_message("messages/injectresponse.dat", zfile,
                generate_inject_rsp(), 1, true);
    add_message("messages/modifyrequest.dat", zfile,
                generate_modify_rq(), 1, true);
    add_message("messages/modifyresponse.dat", zfile,
                generate_modify_rsp(), 1, true);
    add_message("messages/ejectrequest.dat", zfile,
                generate_eject_rq(), 1, true);
    add_message("messages/ejectresponse.dat", zfile,
                generate_eject_rsp(), 1, true);
    add_message("messages/interactrequest.dat", zfile,
                generate_interact_rq(), 1, true);
    add_message("messages/interactresponse.dat", zfile,
                generate_interact_rsp(), 1, true);
    add_message("messages/examinerequest.dat", zfile,
                generate_examine_rq(), 1, true);
    add_message("messages/examineresponse.dat", zfile,
                generate_examine_rsp(), 1, true);
    add_message("messages/attachrequest.dat", zfile,
                generate_attach_rq(), 1, true);
    add_message("messages/attachresponse.dat", zfile,
                generate_attach_rsp(), 1, true);
    add_message("messages/detachrequest.dat", zfile,
                generate_detach_rq(), 1, true);
    add_message("messages/detachresponse.dat", zfile,
                generate_detach_rsp(), 1, false);
    add_message("messages/handoverrequest.dat", zfile,
                generate_handover_rq(), 1, true);
    add_message("messages/handoverresponse.dat", zfile,
                generate_handover_rsp(), 1, true);
    add_message("messages/listbubblesrequest.dat", zfile,
                generate_list_bubbles_rq(), 1, true);
    add_message("messages/listbubblesresponse.dat", zfile,
                generate_list_bubbles_rsp(), 1, true);
    add_message("messages/perceptionevent.dat", zfile,
                generate_perception_event(), 1, true);
    add_message("messages/movementevent.dat", zfile,
                generate_movement_event(), 1, false);
    add_message("messages/disappearanceevent.dat", zfile,
                generate_disappearance_event(), 1, true);
    add_message("messages/handoverevent.dat", zfile,
                generate_handover_event(), 1, true);
    add_message("messages/actionevent.dat", zfile,
                generate_action_event(), 1, true);
    add_message("messages/synchronizationbeginevent.dat", zfile,
                generate_synchronization_begin_event(), 1, true);
    add_message("messages/synchronizationendevent.dat", zfile,
                generate_synchronization_end_event(), 1, true);


    zipClose(zfile, 0);
}

void test_compare_to_zip() {
    compare_message_to_zip("messages/acknowledge.dat", generate_acknowledge(),
                           false);
    compare_message_to_zip("messages/keepalive.dat", generate_keepalive(),
                           true);
    compare_message_to_zip("messages/throttle.dat", generate_throttle(),
                           true);
    compare_message_to_zip("messages/challengerequest.dat",
                           generate_challenge_rq(), true);
    compare_message_to_zip("messages/challengeresponse.dat",
                           generate_challenge_rsp(), true);
    compare_message_to_zip("messages/joinrequest.dat",
                           generate_join_rq(), true);
    compare_message_to_zip("messages/joinresponse.dat",
                           generate_join_rsp(), true);
    compare_message_to_zip("messages/leaverequest.dat",
                           generate_leave_rq(), true);
    compare_message_to_zip("messages/leaveresponse.dat",
                           generate_leave_rsp(), false);
    compare_message_to_zip("messages/injectrequest.dat",
                           generate_inject_rq(), true);
    compare_message_to_zip("messages/injectresponse.dat",
                           generate_inject_rsp(), true);
    compare_message_to_zip("messages/modifyrequest.dat",
                           generate_modify_rq(), true);
    compare_message_to_zip("messages/modifyresponse.dat",
                           generate_modify_rsp(), true);
    compare_message_to_zip("messages/ejectrequest.dat",
                           generate_eject_rq(), true);
    compare_message_to_zip("messages/ejectresponse.dat",
                           generate_eject_rsp(), true);
    compare_message_to_zip("messages/interactrequest.dat",
                           generate_interact_rq(), true);
    compare_message_to_zip("messages/interactresponse.dat",
                           generate_interact_rsp(), true);
    compare_message_to_zip("messages/examinerequest.dat",
                           generate_examine_rq(), true);
    compare_message_to_zip("messages/examineresponse.dat",
                           generate_examine_rsp(), true);
    compare_message_to_zip("messages/attachrequest.dat",
                           generate_attach_rq(), true);
    compare_message_to_zip("messages/attachresponse.dat",
                           generate_attach_rsp(), true);
    compare_message_to_zip("messages/detachrequest.dat",
                           generate_detach_rq(), true);
    compare_message_to_zip("messages/detachresponse.dat",
                           generate_detach_rsp(), false);
    compare_message_to_zip("messages/handoverrequest.dat",
                           generate_handover_rq(), true);
    compare_message_to_zip("messages/handoverresponse.dat",
                           generate_handover_rsp(), true);
    compare_message_to_zip("messages/listbubblesrequest.dat",
                           generate_list_bubbles_rq(), true);
    compare_message_to_zip("messages/listbubblesresponse.dat",
                           generate_list_bubbles_rsp(), true);
    compare_message_to_zip("messages/perceptionevent.dat",
                           generate_perception_event(), true);
    compare_message_to_zip("messages/movementevent.dat",
                           generate_movement_event(), false);
    compare_message_to_zip("messages/disappearanceevent.dat",
                           generate_disappearance_event(), true);
    compare_message_to_zip("messages/handoverevent.dat",
                           generate_handover_event(), true);
    compare_message_to_zip("messages/actionevent.dat",
                           generate_action_event(), true);
    compare_message_to_zip("messages/synchronizationbeginevent.dat",
                           generate_synchronization_begin_event(), true);
    compare_message_to_zip("messages/synchronizationendevent.dat",
                           generate_synchronization_end_event(), true);
}

void test_generate_zip() {
    generate_messages("tmp/messages.zip");

    // TODO: compare the generated zip file to the stored reference zip file
}

}
}
}
}
