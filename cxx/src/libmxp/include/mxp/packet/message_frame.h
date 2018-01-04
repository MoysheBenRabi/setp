#ifndef MXP_PACKET_MESSAGE_FRAME_H
#define MXP_PACKET_MESSAGE_FRAME_H

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
#include <ostream>
#include <boost/cstdint.hpp>

#include <mxp/serialization.h>

namespace mxp {
namespace packet {

using namespace boost;

/*! A message frame. MXP messages are cut into pieces and transported in
    one or more message frames. These message frames can be combined at the
    other end and the original message can be built from them. Messages
    frames themselves are put into packets - with each packet possibly
    holding a number of message frames.
 */
class message_frame {
public:
    /*! The type of the message contained in this message frame.
        \see mxp::message::message::type
     */
    uint8_t type;

    /*! The id of this message. Each message has a unique id within a session.
     */
    uint32_t message_id;

    /*! The number of frames the message is being sent in. */
    uint16_t frame_count;

    /*! The index of this particular message frame, forming the message. */
    uint16_t frame_index;

private:
    /*! The size of the actual message data this frame carries. */
    uint8_t _frame_data_size;

    /*! The frame data. The first frame_data_size bytes are used only. */
    uint8_t _frame_data[255];

public:
    /*! Constructor. */
    message_frame() : type(0u),
                      message_id(0u),
                      frame_count(0u),
                      frame_index(0u),
                      _frame_data_size(0u) {
    }

    /*! Copy constructor.

        \param other the object to base this copy on.
     */
    message_frame(const message_frame & other)
            : type(other.type),
              message_id(other.message_id),
              frame_count(other.frame_count),
              frame_index(other.frame_index),
              _frame_data_size(other._frame_data_size) {

        std::copy(other._frame_data, other._frame_data + other._frame_data_size,
                  _frame_data);
    }

    /*! Assignment operator.

        \param other the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
     */
    message_frame& operator=(const message_frame & other) {
        if (this == &other) {
            return *this;
        }

        type             = other.type;
        message_id       = other.message_id;
        frame_count      = other.frame_count;
        frame_index      = other.frame_index;
        _frame_data_size = other._frame_data_size;

        std::copy(other._frame_data, other._frame_data + other._frame_data_size,
                  _frame_data);

        return *this;
    }

    /*! Equality comparision.

        \param other the other object to compare this to.
        \return true if this and the other object contain the same data.
                false otherwise.
     */
    bool operator==(const message_frame & other) const {
        if (this == &other) {
            return true;
        }

        return type == other.type
            && message_id == other.message_id
            && frame_count == other.frame_count
            && frame_index == other.frame_index
            && _frame_data_size == other._frame_data_size
            && std::equal(_frame_data, _frame_data + _frame_data_size,
                          other._frame_data);
    }

    /*! Return the frame data size.

        \return the frame data size.
     */
    uint8_t frame_data_size() const {
        return _frame_data_size;
    }

    /*! Return the frame data.

        \return the frame data.
     */
    const uint8_t* frame_data() const {
        return _frame_data;
    }

    /*! Set the frame data.

        \tparam iterator an iterator on uint8_t values
        \param data the data to set, a series of bytes
        \param len the length of the data to set in bytes, at most 255
     */
    template<typename iterator>
    void frame_data(iterator data, uint8_t len) {
        std::copy(data, data + len, _frame_data);
        _frame_data_size = len;
    }

    /*! Calculate and return the size of this message frame when serialized.

        \return the size of this message frame in bytes, when serialized.
     */
    unsigned int size() const {
        return 10u + _frame_data_size;
    }

    /*! Serialize this message frame.

        \tparam iterator the iterator to serialize into
        \param it the iterator to serialize this message frame into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
     */
    template<typename iterator>
    unsigned int serialize(iterator & it) const {
        unsigned int counter = 0;

        counter += serialization::write_it<uint8_t>(it, type);
        counter += serialization::write_it<uint32_t>(it, message_id);
        counter += serialization::write_it<uint16_t>(it, frame_count);
        counter += serialization::write_it<uint16_t>(it, frame_index);
        counter += serialization::write_it<uint8_t>(it, _frame_data_size);
        counter += serialization::write_range_it<uint8_t>(it,
                                                 _frame_data, _frame_data_size);

        return counter;
    }

    /*! De-serialize this message frame. The contents of this packet will be
        filled with the de-serialized values read from the supplied iterator.

        \tparam iterator the iterator to serialize from
        \param it the iterator to serialize this message frame from by reading
               a sequence of bytes from the iterator.
        \param len the maximum number of advances made on the supplied iterator
     */
    template<typename iterator>
    void deserialize(iterator & it, unsigned int len) {
        if (len < size()) {
            return;
        }

        type             = serialization::read_it<uint8_t>(it);
        message_id       = serialization::read_it<uint32_t>(it);
        frame_count      = serialization::read_it<uint16_t>(it);
        frame_index      = serialization::read_it<uint16_t>(it);
        _frame_data_size = serialization::read_it<uint8_t>(it);
        serialization::read_range_it<uint8_t>(it,
                                              _frame_data,
                                              _frame_data_size);
    }

};

/*! Write a human-readable representation of a message frame to an output stream.

    \param os the output stream to write to.
    \param frame the message frame to write.
    \return the output stream after the message frame has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const message_frame & frame) {
    os << "message_frame[type: " << ((unsigned int) frame.type)
       << ", message_id: " << frame.message_id
       << ", frame_count: " << frame.frame_count
       << ", frame_index: " << frame.frame_index
       << ", frame_data_size: " << ((unsigned int) frame.frame_data_size())
       << "]";

    return os;
}


}
}

#endif
