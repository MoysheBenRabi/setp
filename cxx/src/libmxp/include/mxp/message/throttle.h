#ifndef MXP_MESSAGE_THROTTLE
#define MXP_MESSAGE_THROTTLE

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
#include <mxp/serialization/serialize.h>
#include <mxp/serialization/deserialize.h>
#include <mxp/message/message.h>

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;
using namespace mxp::serialization;

/*! A Throttle message, as specified by MXP.
 */
class throttle : public message {
public:
    /*! Bytes per second the sender is ready to receive. */
    uint32_t transfer_rate;

    /*! Constructor. */
    throttle() : message(THROTTLE) {}

    /*! Virtual destructor. */
    virtual ~throttle() {}

    /*! Copy constructor.

        \param thr the object to base this copy on.
    */
    throttle(const throttle & thr) : message(THROTTLE),
                                     transfer_rate(thr.transfer_rate) {
    }

    /*! Assignment operator.

        \param thr the object to base this object on.
        \return a reference to this object, after the assignemt has been made.

    */
    throttle& operator=(const throttle & thr) {
        if (this == &thr) {
            return *this;
        }

        transfer_rate = thr.transfer_rate;

        return *this;
    }

    /*! Equality comparator.

        \param other the object to equal with this one.
        \return true if the two objects are equal, false otherwise.
    */
    virtual bool equals(const message & other) const {

        if (this == &other) {
            return true;
        }

        if (get_type() != other.get_type()) {
            return false;
        }

        const throttle & thr = dynamic_cast<const throttle &>(other);

        return transfer_rate == thr.transfer_rate;
    }

    virtual unsigned int size() const {
        return 4;
    }

    virtual unsigned int
    serialize(byte_writer_base & writer) const {
        return write<uint32_t>(writer, transfer_rate);
    }

    virtual void
    deserialize(byte_reader_base & reader, unsigned int len) {
        if (len < size()) {
            return;
        }

        transfer_rate = serialization::read<uint32_t>(reader);
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param thr the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const throttle & thr) {
    os << "throttle[type: " << thr.get_type();
    os << ", transfer_rate";
    os << thr.transfer_rate;
    os << "]";

    return os;
}

}
}

#endif
