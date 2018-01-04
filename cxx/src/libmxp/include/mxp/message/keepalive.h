#ifndef MXP_MESSAGE_KEEPALIVE
#define MXP_MESSAGE_KEEPALIVE

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

#include <mxp/serialization/serialize.h>
#include <mxp/serialization/deserialize.h>
#include <mxp/message/message.h>

namespace mxp {
namespace message {

using namespace mxp;

/*! A Keepalive message, as specified by MXP.
 */
class keepalive : public message {
public:

    /*! Constructor. */
    keepalive() : message(KEEPALIVE) {}

    /*! Virtual destructor. */
    virtual ~keepalive() {}

    /*! Copy constructor. */
    keepalive(const keepalive & ) : message(KEEPALIVE) {
    }

    /*! Assignment operator.

        \param klv the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
     */
    keepalive& operator=(const keepalive & klv) {
        if (this == &klv) {
            return *this;
        }

        return *this;
    }

    /*! Equality comparator.

        \param other the object to equal with this one.
        \return true if the two objects are equal, false otherwise.
     */
    virtual bool equals(const message & other) const {
        return (this == &other) || (get_type() == other.get_type());
    }

    virtual unsigned int size() const {
        return 0;
    }

    virtual unsigned int
    serialize(serialization::byte_writer_base & ) const {
        return 0;
    }

    virtual void
    deserialize(serialization::byte_reader_base & , unsigned int ) {
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param klv the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const keepalive & klv) {
    os << "keepalive[type: " << klv.get_type();
    os << "]";

    return os;
}

}
}

#endif
