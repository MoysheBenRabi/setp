#ifndef MXP_MESSAGE_MOVEMENT_EVENT
#define MXP_MESSAGE_MOVEMENT_EVENT

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

#include <boost/cstdint.hpp>
#include <boost/array.hpp>
#include <mxp/serialization/serialize.h>
#include <mxp/serialization/deserialize.h>
#include <mxp/message/message.h>
#include "object_fragment.h"

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;
using namespace mxp::serialization;

/*! A Movement event message, as specified by MXP.
 */
class movement_event : public message {
public:

    /*! Index of a moved object */
    uint32_t            object_index;   // 4
    /*! New location */
    array<float, 3>     location;       // 12
    /*! New orientation */
    array<float, 4>     orientation;    // 16

    /*! Constructor. */
    movement_event() : message(MOVEMENT_EVENT) {}

    /*! Virtual destructor. */
    virtual ~movement_event() {}

    /*! Copy constructor.

        \param m_evt the object to base this copy on.
     */
    movement_event(const movement_event & m_evt): message(MOVEMENT_EVENT),
        object_index(m_evt.object_index),
        location(m_evt.location),
        orientation(m_evt.orientation) {
    }

    /*! Assignment operator.

        \param m_evt the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
     */
    movement_event& operator=(const movement_event & m_evt) {
        if (this == &m_evt) {
            return *this;
        }
        object_index = m_evt.object_index;
        location = m_evt.location;
        orientation = m_evt.orientation;
        return *this;
    }

    /*! equals.

        \param other the object to equal with this one.
        \return true if objects is equal and false afterwards.
     */
    virtual bool equals(const message & other) const {
        return (this == &other) || ((get_type() == other.get_type()) &&
            (object_index == ((movement_event&)other).object_index) &&
            (location == ((movement_event&)other).location) &&
            (orientation == ((movement_event&)other).orientation));
    }

    virtual unsigned int size() const {
        return 32;
    }

    /*! Serialize this message.

        \param writer the byte writer to serialize this message into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
     */
    virtual unsigned int
    serialize(byte_writer_base & writer) const {
        unsigned int counter = 0;

        counter += write<uint32_t>(writer, object_index);
        counter += write<array<float, 3> >(writer, location);
        counter += write<array<float, 4> >(writer, orientation);

        return counter;
    }

    /*! De-serialize this message. The contents of this message will be filled
        with the de-serialized values read from the supplied byte reader.

        \param reader the byte reader to serialize this message from by reading
               a sequence of bytes from it
     */
    virtual void
    deserialize(byte_reader_base & reader, unsigned int ) {

        object_index = read<uint32_t>(reader);
        location     = read<array<float, 3> >(reader);
        orientation  = read<array<float, 4> >(reader);
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param m_evt the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const movement_event & m_evt) {
    os << "movement_event[type: " << m_evt.get_type();
    os << ", object_index=";
    os << m_evt.object_index;
    os << ", location=";
    os << m_evt.location;
    os << ", orientation=";
    os << m_evt.orientation;
    os << "]";

    return os;
}

}
}
#endif
