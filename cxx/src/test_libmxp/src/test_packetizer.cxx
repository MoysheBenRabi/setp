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

#include <iterator>
#include <vector>
#include <boost/smart_ptr.hpp>
#include <boost/test/unit_test.hpp>

#include <mxp/message.h>
#include <mxp/packet.h>

#include "test_helpers.h"
#include "reference_messages.h"
#include "test_packetizer.h"

namespace mxp {
namespace test {
namespace packet {
namespace packetizer {

using namespace boost;
using namespace mxp;
using namespace mxp::test::iot;

void test_single_frame() {
    typedef std::vector<mxp::packet::message_frame> container;

    shared_ptr<mxp::message::acknowledge> ack(generate_acknowledge());

    // convert the message into a single message frame
    container                               frames;
    unsigned int                            counter;
    std::back_insert_iterator<container>    it = std::back_inserter(frames);

    counter = mxp::packet::message_to_message_frames(
            (shared_ptr<mxp::message::message>) ack, 1, it);

    BOOST_CHECK_EQUAL(1u, frames.size());
    BOOST_CHECK_EQUAL(ack->get_type(), frames[0].type);
    BOOST_CHECK_EQUAL(1u, frames[0].message_id);
    BOOST_CHECK_EQUAL(1u, frames[0].frame_count);
    BOOST_CHECK_EQUAL(0u, frames[0].frame_index);
    BOOST_CHECK_EQUAL(counter, frames[0].size());

    container::iterator iit = frames.begin();
    shared_ptr<mxp::message::message> msg = message_frames_to_message(iit);

    BOOST_CHECK_EQUAL(1, iit - frames.begin());
    BOOST_CHECK(msg.get());
    BOOST_CHECK_EQUAL(ack->get_type(), msg->get_type());

    BOOST_CHECK_EQUAL(*ack,
                      *static_pointer_cast<mxp::message::acknowledge>(msg));
}

// TODO: write test_multiple_frames()


void test_message_packet_round_trip() {

    typedef std::vector<shared_ptr<mxp::message::message> >  message_container;
    typedef std::vector<mxp::packet::packet>                 packet_container;

    message_container   messages;
    packet_container    packets;
    std::back_insert_iterator<packet_container>  pit =
                                                    std::back_inserter(packets);

    shared_ptr<mxp::message::message> msg = generate_acknowledge();
    messages.push_back(msg);

    messages_to_packets(messages.begin(),
                        messages.end(),
                        0u,
                        pit,
                        0u,
                        0u,
                        posix_time::second_clock::universal_time(),
                        (uint8_t) 0);

    BOOST_CHECK_EQUAL(1u, packets.size());
    BOOST_CHECK_EQUAL(0u, packets[0].packet_id);
    BOOST_CHECK_EQUAL(0u, packets[0].session_id);
    BOOST_CHECK_EQUAL(0u, packets[0].guaranteed);
    BOOST_CHECK_EQUAL(0u, packets[0].resend_count);
    BOOST_CHECK_EQUAL(1,
           packets[0].message_frames_end() - packets[0].message_frames_begin());
    BOOST_CHECK(packets[0].size() < 1452u);

    // so far so good - let's turn the packets back into messages
    mxp::packet::message_frame_map  message_frames;

    messages.clear();
    std::back_insert_iterator<message_container> mit =
                                                std::back_inserter(messages);

    packets_to_messages(packets.begin(), packets.end(), mit, message_frames);

    BOOST_CHECK_EQUAL(0u, message_frames.size());
    BOOST_CHECK_EQUAL(1u, messages.size());

    shared_ptr<mxp::message::message> mmsg = messages[0];

    BOOST_CHECK_EQUAL(*msg, *mmsg);
}


}
}
}
}
