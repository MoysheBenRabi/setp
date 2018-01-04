#ifndef MXP_PACKET_PACKET_H
#define MXP_PACKET_PACKET_H

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
#include <vector>
#include <stdexcept>
#include <ostream>
#include <boost/cstdint.hpp>
#include <boost/date_time.hpp>
#include <boost/foreach.hpp>

#include <mxp/serialization.h>
#include <mxp/packet/message_frame.h>

namespace mxp {
namespace packet {

using namespace boost;

/*! A packet - a basic unit that is sent between nodes participating in
    an MXP exchange. Packets contain a number of message frames, which in
    turn carry the messages themselves.
 */
class packet {
public:
    /*! An iterator that iterates through the message frames contained in
        a packet.
     */
    typedef std::vector<message_frame>::const_iterator message_frame_iterator;

    /*! The maximum size of a packet, in bytes. */
    static const unsigned int max_size = 1452u;

    /*! The maximum size of the useful payload of a packet, in bytes. */
    static const unsigned int max_payload_size = 1452u - 18u;

public:
    /*! The session id this packet is part of. */
    uint32_t session_id;

    /*! The id of this packet. Each packet has a unique id within a session. */
    uint32_t packet_id;

    /*! The first time this packet was sent. */
    posix_time::ptime first_send_time;

    /*! Flag to indicate if an Acknowledge message is expected to indicate
        that this packet has arrived at the destination. 1 if so, 0 if no
        acknowledgement is expected. */
    uint8_t guaranteed;

    /*! The number of times this packet has been resent. */
    uint8_t resend_count;

private:
    /*! The message frames contained in this packet. */
    std::vector<message_frame>  _message_frames;

public:
    /*! Constructor. */
    packet() : session_id(0u),
               packet_id(0u),
               first_send_time(gregorian::date(2000, 1, 1)),
               guaranteed(0u),
               resend_count(0u) {
    }

    /*! Copy constructor.

        \param other the object to base this copy on.
     */
    packet(const packet & other) : session_id(other.session_id),
                                   packet_id(other.packet_id),
                                   first_send_time(other.first_send_time),
                                   guaranteed(other.guaranteed),
                                   resend_count(other.resend_count),
                                   _message_frames(other._message_frames) {
    }

    /*! Assignment operator.

        \param other the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
     */
    packet& operator=(const packet & other) {
        if (this == &other) {
            return *this;
        }

        session_id      = other.session_id;
        packet_id       = other.packet_id;
        first_send_time = other.first_send_time;
        guaranteed      = other.guaranteed;
        resend_count    = other.resend_count;

        _message_frames.clear();
        _message_frames.resize(other._message_frames.size());
        std::copy(other._message_frames.begin(), other._message_frames.end(),
                  _message_frames.begin());

        return *this;
    }

    /*! Equality comparision.

        \param other the other object to compare this to.
        \return true if this and the other object contain the same data.
                false otherwise.
     */
    bool operator==(const packet & other) const {
        if (this == &other) {
            return true;
        }

        return session_id == other.session_id
            && packet_id == other.packet_id
            && first_send_time == other.first_send_time
            && guaranteed == other.guaranteed
            && resend_count == other.resend_count
            && _message_frames.size() == other._message_frames.size()
            && std::equal(_message_frames.begin(), _message_frames.end(),
                          other._message_frames.begin());
    }

    /*! Get the message frames contained in this packet.

        \return an iterator to the first message frame contained in this packet
        \see #message_frames_end()
     */
    const message_frame_iterator message_frames_begin() const {
        return _message_frames.begin();
    }

    /*! Get the message frames contained in this packet.

        \return an iterator that is one beyond the last message frame in this
                packet
        \see #message_frames_begin()
     */
    const message_frame_iterator  message_frames_end() const {
        return _message_frames.end();
    }

    /*! Clear all message frames in this packet.
     */
    void clear_message_frames() {
        _message_frames.clear();
    }

    /*! Set the message frames in this packet.

        \tparam iterator an iterator pointing to message_frame objects
        \param frames_begin an iterator to the first frame to be contained
               in this packet
        \param frames_end an iterator to one beyond the last frame in this
               packet
        \throws std::invalid_argument if the total size of message frames
                would exceed the maximum package load size of 1434 bytes.
     */
    template<typename iterator>
    void message_frames(iterator frames_begin, iterator frames_end) {
        unsigned int counter = 0;
        for (iterator it = frames_begin; it != frames_end; ++it) {
            counter += it->size();
        }

        if (counter > max_payload_size) {
            throw std::invalid_argument(
                    "packet can't handle this much payload");
        }

        _message_frames.clear();
        _message_frames.resize(frames_end - frames_begin);
        std::copy(frames_begin, frames_end, _message_frames.begin());
    }

    /*! Add a message frame to this packet.

        \param frame a message frame to be added to the packet
        \throws std::invalid_argument if the total size of message frames
                would exceed the maximum package load size of 1434 bytes.
     */
    void add_message_frame(const message_frame & frame) {
        if (frame.size() > available()) {
            throw std::invalid_argument(
                    "packet can't handle this much payload");
        }

        _message_frames.push_back(frame);
    }

    /*! Tell if the packet contains any message frames.

        \return true if the packet is empty, false if it contains message
                frames.
     */
    bool empty() const {
        return _message_frames.empty();
    }

    /*! Calculate and return the size of this packet when serialized.

        \return the size of this packet in bytes, when serialized.
     */
    unsigned int size() const {
        unsigned int counter = 18u;

        BOOST_FOREACH(message_frame frame, _message_frames) {
            counter += frame.size();
        }

        return counter;
    }

    /*! Tell how much useful payload this packet can transfer, in addition
        to what's already in it.

        \return the size of the additional useful payload this packet can
                transfer, in bytes.
     */
    unsigned int available() const {
        return max_size - size();
    }

    /*! Serialize this packet.

        \tparam iterator the iterator to serialize into
        \param it the iterator to serialize this packet into, as a sequence
               of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
     */
    template<typename iterator>
    unsigned int serialize(iterator & it) const {
        unsigned int counter = 0;

        counter += serialization::write_it<uint32_t>(it, session_id);
        counter += serialization::write_it<uint32_t>(it, packet_id);
        counter += serialization::write_it<posix_time::ptime>(it,
                                                              first_send_time);
        counter += serialization::write_it<uint8_t>(it, guaranteed);
        counter += serialization::write_it<uint8_t>(it, resend_count);

        BOOST_FOREACH(message_frame frame, _message_frames) {
            counter += frame.serialize(it);
        }

        return counter;
    }

    /*! De-serialize this packet. The contents of this packet will be filled
        with the de-serialized values read from the supplied iterator.

        \tparam iterator the iterator to serialize from
        \param it the iterator to serialize this packet from by reading
               a sequence of bytes from the iterator.
        \param len the maximum number of advances made on the supplied iterator
     */
    template<typename iterator>
    void deserialize(iterator & it, unsigned int len) {
        if (len < size()) {
            return;
        }

        iterator begin = it;

        session_id      = serialization::read_it<uint32_t>(it);
        packet_id       = serialization::read_it<uint32_t>(it);
        first_send_time = serialization::read_it<posix_time::ptime>(it);
        guaranteed      = serialization::read_it<uint8_t>(it);
        resend_count    = serialization::read_it<uint8_t>(it);

        _message_frames.clear();
        while (it - begin < (signed int) len) {
            message_frame   frame;

            frame.deserialize(it, len - (it - begin));
            _message_frames.push_back(frame);
        }
    }

};

/*! Write a human-readable representation of a packet to an output stream.

    \param os the output stream to write to.
    \param p the packet to write.
    \return the output stream after the packet has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const packet & p) {
    os << "packet[session_id: " << p.session_id
       << ", packet_id: " << p.packet_id
       << ", first_send_time: " << p.first_send_time
       << ", guaranteed: " << ((unsigned int) p.guaranteed)
       << ", resend_count: " << ((unsigned int) p.resend_count);

    for (packet::message_frame_iterator it = p.message_frames_begin();
         it != p.message_frames_end(); ++it) {

        os << *it;
    }

    os << "]";

    return os;
}

}
}

#endif
