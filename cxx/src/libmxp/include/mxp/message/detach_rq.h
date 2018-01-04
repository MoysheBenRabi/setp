#ifndef MXP_MESSAGE_DETACH_RQ
#define MXP_MESSAGE_DETACH_RQ

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

#include <boost/type.hpp>
#include "mxp/serialization/serialize.h"
#include "mxp/serialization/deserialize.h"
#include "mxp/message/message.h"

namespace mxp {
namespace message {

using namespace mxp;

/*! A Detach request message, as specified by MXP.
 */
class detach_rq : public message {
public:

    /*! Constructor. */
    detach_rq() : message(DETACH_RQ) {}

    /*! Virtual destructor. */
    virtual ~detach_rq() {}

    /*! Copy constructor. */
    detach_rq(const detach_rq & ) : message(DETACH_RQ) {
    }

    /*! Assignment operator.

        \param dch_rq the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
    */
    detach_rq& operator=(const detach_rq & dch_rq) {
        if (this == &dch_rq) {
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
    \param dch_rq the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const detach_rq & dch_rq) {
    os << "detach_rq[type: " << dch_rq.get_type();
    os << "]";
    return os;
}

}
}

#endif

