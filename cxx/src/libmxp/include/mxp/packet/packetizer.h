#ifndef MXP_PACKET_PACKETIZER_H
#define MXP_PACKET_PACKETIZER_H

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

#include <algorithm>
#include <stdexcept>
#include <set>
#include <map>
#include <vector>
#include <boost/smart_ptr.hpp>
#include <boost/cstdint.hpp>
#include <boost/foreach.hpp>

#include <mxp/message.h>
#include <mxp/packet/message_frame.h>
#include <mxp/packet/packet.h>

namespace mxp {
namespace packet {

using namespace boost;
using namespace mxp;

/*! A map of message, frames, keyed by message ids. Such a map contains
    a number of message frames for messages to be deserialized.
    For each entry in the map, a vector of message frames is contained,
    equal is size to the number of message frames the message is cut
    up into. For message frames not yet received, the corresponding
    frame entry is 'clear', that is, contains the default values
    for the message frame (all zeros).

    This type is actually:

    \code
    std::map<uint32_t, std::vector<message_frame> >
    \endcode
 */
typedef std::map<uint32_t, std::vector<message_frame> > message_frame_map;


/*! Turn a message into message frames, by serializing the message and
    breaking it up into a number of message fragments as needed.

    \tparam frame_iterator an iterator on message_frame objects
    \param message a pointer to the message to turn into message fragments.
    \param message_id the unique id of this message within the active session
    \param frames an iterator of message frames. the generated frames
           will be appended to this iterator. on return, this iterator will
           point to one beyond the last added message frame.
    \return the total size of the message fragments if serialized, including
            the message frame headers, that were added to the fragments list.
    \see mxp::message::message
    \see mxp::packet::message_frame
 */
template<typename frame_iterator>
unsigned int message_to_message_frames(
                                shared_ptr<message::message>        message,
                                uint32_t                            message_id,
                                frame_iterator                    & frames) {
    // first, serialize the message
    unsigned int message_size = message->size();
	std::vector<uint8_t>	data;
	data.reserve(message->size());
	std::back_insert_iterator<std::vector<uint8_t> > dit =
                                                    std::back_inserter(data);

    mxp::message::serialize(*message, dit);

    // put the message into message frames, each holding up to 255 bytes
    // of the message data
    unsigned int frame_count = message_size / 255u;
    if (frame_count * 255u < message_size) {
        ++frame_count;
    }
    if (frame_count == 0) {
        ++frame_count;
    }
    unsigned int frame_index = 0;
    unsigned int counter     = 0;

    if (message_size == 0) {
        // create an empty frame for marker messages of 0 length
        message_frame frame;

        frame.type        = message->get_type();
        frame.message_id  = message_id;
        frame.frame_count = (uint16_t) frame_count;
        frame.frame_index = (uint16_t) frame_index;

        *frames++ = frame;

        counter += frame.size();
    } else {
        // create as many frames as needed to send the whole message
        unsigned int                    index = 0;
		std::vector<uint8_t>::iterator  it    = data.begin();

        while (index < message_size) {
            unsigned int chunk_size = (std::min)(message_size - index, 255u);

            message_frame frame;

            frame.type        = message->get_type();
            frame.message_id  = message_id;
            frame.frame_count = (uint16_t) frame_count;
            frame.frame_index = (uint16_t) frame_index;
            frame.frame_data(it, chunk_size);

            *frames = frame;
            frames++;

            counter += frame.size();
            index   += chunk_size;
            it      += chunk_size;
            frame_index++;
        }
    }

    return counter;
}

/*! Turn a number of message frames back into a message, by getting the
    serialized data out of message frames, putting then together, and
    de-serializing them to form an appropriate message structure.
    It is expected that all frames required for the message are present,
    and are presented in proper order.

    \tparam frame_iterator an iterator on message_frame objects
    \param frames an iterator on the message frames that make up a message,
           ordered by their frame_index property. upon return, the iterator
           will point to one beyond the last frame processed.
    \return the de-serialized message that the message frames contained.
    \throws std::invalid_argument if the message frames are not in order,
            or not all message frames are present
    \see mxp::message::message
    \see mxp::packet::message_frame
 */
template<typename frame_iterator>
shared_ptr<message::message> message_frames_to_message(
                                        frame_iterator & frames) {

    unsigned int frame_count = frames->frame_count;
    unsigned int frame_index = frames->frame_index;
    unsigned int type_id     = frames->type;

    if (frame_index != 0) {
        throw std::invalid_argument("initial message frame is not the first");
    }
    // TODO: check that all message frames are here and they are in order

    // calculate the size of the message
    frame_iterator it           = frames;
    unsigned int   message_size = 0u;
    for (unsigned int i = 0; i < frame_count; ++i) {
        message_size += it->frame_data_size();
        ++it;
    }

    // extract the message raw data
    std::vector<uint8_t>    data;
    data.reserve(message_size);
    std::back_insert_iterator<std::vector<uint8_t> > dit =
                                                std::back_inserter(data);
    for (unsigned int i = 0; i < frame_count; ++i) {
        std::copy(frames->frame_data(),
                  frames->frame_data() + frames->frame_data_size(),
                  dit);
        ++frames;
    }

    // create the appropriate message object and deserialize from the raw data
    shared_ptr<mxp::message::message> msg = mxp::message::for_type(type_id);
    std::vector<uint8_t>::iterator    iit  = data.begin();

    mxp::message::deserialize(*msg, iit, message_size);

    return msg;
}

/*! Convert a series of messages into a series of packets transport them.
    This is done by serializing the messages, putting them into message
    frames, and then putting these message frames into packets.
    Each generated packet will have a resend count of 0, and parameters
    set as supplied to this function.

    \tparam message_iterator an iterator on boost::shared_ptr<message::messsage>
            pointers
    \tparam packet_iterator an iterator on packet objects
    \param messages_begin an iterator pointing to the first message to add
    \param messages_end an iterator pointing one beyond the last message to add
    \param base_message_id messages will be assigned ids starting with
           this id. each subsequent message will be assigned an id
           one greater. in total, messages ids between base_message_id
           and base_message_id + (messaged_end - messages_begin) will be used.
    \param packets an iterator pointing to packets. the new packets generated
           will be appended after this.
    \param session_id the session_id to set in the newly created packets.
    \param base_packet_id packets will be assigned ids starting from this id,
           with each new packet getting an id greater than the previous one.
           in total, packet ids between base_packet_id and
           base_packet_id + (packets returned - packets original) will be used,
           thus the number of advances on the packets iterator determine
           how much are used
    \param first_send_time the first sending time to set for the generated
           packets.
    \param guaranteed the guaranteed flag to set for each newly generated
           packet.
 */
template<typename message_iterator, typename packet_iterator>
void messages_to_packets(message_iterator   messages_begin,
                         message_iterator   messages_end,
                         uint32_t           base_message_id,
                         packet_iterator  & packets,
                         uint32_t           session_id,
                         uint32_t           base_packet_id,
                         posix_time::ptime  first_send_time,
                         uint8_t            guaranteed) {

    if (messages_end - messages_begin == 0) {
        return;
    }

    typedef std::vector<mxp::packet::message_frame> container;

    // first turn the messages into message frames
    container  frames;
    frames.reserve(messages_end - messages_begin);
    std::back_insert_iterator<container>    fit = std::back_inserter(frames);

    for (message_iterator it = messages_begin; it != messages_end; ++it) {
        message_to_message_frames(*it, base_message_id++, fit);
    }

    // TODO: re-use the last packet, if there's still room

    // add additional packets as necessary
    mxp::packet::packet  p;
    p.session_id      = session_id;
    p.packet_id       = base_packet_id++;
    p.first_send_time = first_send_time;
    p.guaranteed      = guaranteed;
    p.resend_count    = 0u;

    BOOST_FOREACH(mxp::packet::message_frame & frame, frames) {

        if (p.available() > frame.size()) {
            p.add_message_frame(frame);
        } else {
            *packets++ = p;

            p.session_id      = session_id;
            p.packet_id       = base_packet_id++;
            p.first_send_time = first_send_time;
            p.guaranteed      = guaranteed;
            p.resend_count    = 0u;

            p.clear_message_frames();
        }
    }

    if (!p.empty()) {
        *packets++ = p;
    }
}

/*! Convert a series of packets into messages.
    This is achieved by getting the message frames out of the packets,
    assembling messages from the frames and de-serializing these masseges.
    Messages that can fully be de-serialized are deserialized and returned
    in messages. Messages that have missing message fragments are collected
    in a map of message fragments, keyed by the message id. this map can be
    re-used in subsequent calls to complete these messages.

    \param packets_begin an iterator pointing to the first packet to process
    \param packets_end an iterator pointing one beyond the last packet to
           process
    \param messages messages de-serialized from the supplied packets
           are appended to this iterator. this iterator points to
           boost::shared_ptr<message::message> objects.
    \param message_frames a map of message ids mapped to a vector of message
           frames. used to store messages with missing frames. the message
           frame vectors contain each frame at their respective index,
           and thus is as long as many frames the message is made up in,
           with the missing frames as all-zero values at their respective
           indexes. any messages that cannot be processed because of missing
           frames are put here. this map can be re-used for subsequent calls
           so as the missing frames re received, the complete messages are
           processed and returned.
 */
template<typename packet_iterator, typename message_iterator>
void packets_to_messages(const packet_iterator      packets_begin,
                         const packet_iterator      packets_end,
                         message_iterator         & messages,
                         message_frame_map        & message_frames) {

    // first extract the message frames from the packets, and put them
    // into the message frame map, keyed by the message id
    for (packet_iterator it = packets_begin; it != packets_end; ++it) {
        packet::message_frame_iterator frame_begin = it->message_frames_begin();
        packet::message_frame_iterator frame_end   = it->message_frames_end();

        for (packet::message_frame_iterator fit = frame_begin;
             fit != frame_end; ++fit) {

            uint32_t                     message_id = fit->message_id;
            std::vector<message_frame> & frames = message_frames[message_id];

            // if this is a new entry in the map, fill it with empty
            // frames, as many as this message is broken up into
            if (frames.empty()) {
                frames.resize(fit->frame_count);
            }

            frames[fit->frame_index] = *fit;
        }
    }

    // now process each entry in the message frames map, and build up
    // messages which have all their frames
    std::set<uint32_t> messages_to_remove;

    BOOST_FOREACH(message_frame_map::value_type & mfv, message_frames) {

        bool have_all_frames = true;
        BOOST_FOREACH(const message_frame & frame, mfv.second) {
            if (frame.frame_count == 0) {
                have_all_frames = false;
                break;
            }
        }

        if (have_all_frames) {
            std::vector<message_frame>::iterator mbegin = mfv.second.begin();
            shared_ptr<message::message> msg =
                                    message_frames_to_message(mbegin);

            *messages++ = msg;
            messages_to_remove.insert(mfv.first);
        }
    }

    // remove the message frames that were processed
    BOOST_FOREACH(uint32_t message_id, messages_to_remove) {
        message_frames.erase(message_id);
    }
}



}
}

#endif
