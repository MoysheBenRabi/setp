#ifndef MXP_MESSAGE_MESSAGE_H
#define MXP_MESSAGE_MESSAGE_H

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

#include <ostream>
#include <boost/type.hpp>

#include <mxp/serialization.h>

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;

/*! An enumeration of message types, as specified by MXP and supported by this
    implementation.
 */
enum type {
    // Control
    ACKNOWLEDGE = 1,
    KEEPALIVE   = 2,
    THROTTLE    = 3,

    // Challenge
    CHALLENGE_RQ  = 4,
    CHALLENGE_RSP = 5,

    // Participant to Bubble Commands
    JOIN_RQ         = 10,
    JOIN_RSP        = 11,
    LEAVE_RQ        = 12,
    LEAVE_RSP       = 13,
    INJECT_RQ       = 14,
    INJECT_RSP      = 15,
    MODIFY_RQ       = 16,
    MODIFY_RSP      = 17,
    EJECT_RQ        = 18,
    EJECT_RSP       = 19,
    INTERACT_RQ     = 20,                                        // +
    INTERACT_RSP    = 21,                                        // +
    EXAMINE_RQ      = 22,                                        
    EXAMINE_RSP     = 23,
    // Bubble to Bubble Commands
    ATTACH_RQ       = 30,
    ATTACH_RSP      = 31,
    DETACH_RQ       = 32,
    DETACH_RSP      = 33,
    HANDOVER_RQ     = 34,
    HANDOVER_RSP    = 35,
    // Common Commands
    LIST_BUBBLES_RQ  = 25,
    LIST_BUBBLES_RSP = 26,
    // Common Events
    PERCEPTION_EVENT            = 40,
    MOVEMENT_EVENT              = 41,
    DISAPPEARANCE_EVENT         = 45,
    HANDOVER_EVENT              = 53,
    ACTION_EVENT                = 60,
    SYNCHRONIZATION_BEGIN_EVENT = 70,
    SYNCHRONIZATION_END_EVENT   = 71
};

/*! Base class for messages.

    Each subclass is expected to implement a copy constructor, an assignment
    operator and an equality comparison operator.

    Each subclass is expected to implement the following function templates:

    \code
    template<typename iterator> unsigned int serialize(iterator & it) const;
    \endcode

    where the message is serialized to the supplied iterator, which will contain
    the serialized message as a series of bytes. the return value is the number
    of increments made on the iterator.

    \code
  template<typename iterator> void deserialize(iterator & it, unsigned int len);
    \endcode

    where the message contents are build up by de-serializing the message
    content from the iterator, reading at most len number of bytes from
    the iterator.
 */
class message {
private:
    /*! The message type. */
    type    _type;

protected:
    /*! Constructor to be used by specific message objects.

        \param t the type of the message being constructed.
    */
    message(type t) : _type(t) {
    }

public:
    /*! Virtual destructor. */
    virtual ~message() {}

    /*! Compare this message to another one.
        This function is called by the operator==() function to allow
        for a virtual equal-comparison operator.

        \param other the other object to compare to.
        \return true if the two objects are the same, false otherwise.
     */
    virtual bool equals(const message & other) const {
        return this == &other || _type == other._type;
    }

    /*! Return the message type.

        \return the type of this particular message.
     */
    type get_type() const { return _type; }

    /*! Calculate and return the size of this message when serialized.

        \return the size of this message in bytes, when serialized.
     */
    virtual unsigned int size() const = 0;

    /*! Serialize this message.
        In general, use the convenience method serialize() to seralize
        a message, instead of calling this method.

        \param writer the byte writer to serialize this message into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
        \see unsigned int serialize(const message_class & message,
             iterator & it)
     */
    virtual unsigned int
    serialize(serialization::byte_writer_base & writer) const = 0;

    /*! De-serialize this message. The contents of this message will be filled
        with the de-serialized values read from the supplied byte reader.
        In general, use the convenience method deserialize() to deseralize
        a message, instead of calling this method.

        \param reader the byte reader to serialize this message from by reading
               a sequence of bytes from it
        \param len the maximum number of reads to make on the supplied
               reader
        \see deserialize(message_class & message,
                         iterator & it,
                         unsigned int len)
     */
    virtual void
    deserialize(serialization::byte_reader_base & reader, unsigned int len) = 0;

};

/*! Compare this message to another one.
    This is not a member function to allow parameter matching for both
    sides of the operator.

    \param msg1 one of the messages to compare to
    \param msg2 the other message to compare to
    \return true if the two objects are the same, false otherwise.
 */
inline bool operator==(const message & msg1, const message & msg2) {
    return msg1.equals(msg2);
}


/*! Serialize a message into an iterator.
    The message will be written as a series of bytes into the iterator.

    \tparam message_class the type of message that will be serialized
    \tparam iterator the iterator type to write the message to
    \param message the message to serialize
    \param it the iterator to write the message to
    \return the number of advances made on the iterator during serialization
 */
template<typename message_class, typename iterator>
unsigned int serialize(const message_class & message, iterator & it) {
    serialization::byte_writer<iterator> writer(it);

    return message.serialize(writer);
}

/*! Deserialize a message from an iterator.
    The message will be read as a series of bytes from the supplied iterator,
    and the message object will be filled with information from the iterator.

    \tparam message_class the type of message that will be deserialized
    \tparam iterator the iterator type to read the message data from
    \param message the message to deserialize
    \param it the iterator to read the raw data from
    \param len the maximum number of increments to make on the iterator
 */
template<typename message_class, typename iterator>
void deserialize(message_class & message,
                 iterator & it,
                 unsigned int len) {
    serialization::byte_reader<iterator> reader(it);

    message.deserialize(reader, len);
}



/*! Write a string containing unicode character points as utf8 to an
    output stream.

    \param os the output stream to write to
    \param str the unicode string to write
    \return the output stream after the string has been written to it.
 */
inline
std::ostream& operator<<(std::ostream & os, const std::wstring & str) {
    std::ostream_iterator<char> it(os);
    utf8::utf32to8(str.begin(), str.end(), it);

    return os;
}

/*! Write a fixed size array into an output stream in human-readable format.

    \param os the output stream to write to
    \param value the array to write to the output stream
    \return the output stream after the array has been written to it.
 */
template<typename T, std::size_t N>
std::ostream& operator<<(std::ostream & os, const array<T, N> & value) {
    os << "{";
    for (unsigned int i = 0; i < N-1; ++i) {
        os << value[i] << ",";
    }
    os << value[N-1] << "}";

    return os;
}

/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param msg the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const message & msg) {
    os << "message[type: " << msg.get_type()
       << ", size: " << msg.size() << "]";

    return os;
}

}
}

#endif

