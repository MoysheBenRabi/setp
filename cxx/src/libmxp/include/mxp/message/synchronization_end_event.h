#ifndef MXP_MESSAGE_SYNCHRONIZATION_END_EVENT
#define MXP_MESSAGE_SYNCHRONIZATION_END_EVENT

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
#include <boost/type.hpp>
#include "mxp/serialization/serialize.h"
#include "mxp/serialization/deserialize.h"
#include "mxp/message/message.h"

namespace mxp {
namespace message {

using namespace mxp;
using namespace mxp::serialization;

/*! A Synchronization end message, as specified by MXP.
 */
class synchronization_end_event : public message {
public:

    /*! Constructor. */
    synchronization_end_event() : message(SYNCHRONIZATION_END_EVENT) {}

    /*! Virtual destructor. */
    virtual ~synchronization_end_event() {}

    /*! Copy constructor.

     */
    synchronization_end_event(const synchronization_end_event & ) :
        message(SYNCHRONIZATION_END_EVENT) {
    }

    /*! Assignment operator.

        \param se_evt the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
     */
    synchronization_end_event& operator = 
    (const synchronization_end_event & se_evt) {
        if (this == &se_evt) {
            return *this;
        }

        return *this;
    }

    /*! equals.

        \param other the object to equal with this one.
        \return true if objects is equal and false afterwards.
     */
    virtual bool equals(const message & other) const {
        return (this == &other) || (get_type() == other.get_type());
    }

    virtual unsigned int size() const {
        return 0;
    }

    /*! Serialize this message.
       \return the number of advances made on the iterator during serialization
     */
    virtual unsigned int
    serialize(serialization::byte_writer_base & ) const {
        return 0;
    }

    /*! De-serialize this message. The contents of this message will be filled
        with the de-serialized values read from the supplied byte reader.
     */
    virtual void
    deserialize(serialization::byte_reader_base &, unsigned int ) {
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param se_evt the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<
(std::ostream &os, const synchronization_end_event & se_evt) {
    os << "synchronization_end_event[type: " << se_evt.get_type();
    os << "]";
    return os;
}

}
}

#endif

