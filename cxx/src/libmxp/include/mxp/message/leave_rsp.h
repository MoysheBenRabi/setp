#ifndef MXP_MESSAGE_LEAVE_RSP
#define MXP_MESSAGE_LEAVE_RSP

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
#include "response_fragment.h"

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;
using namespace mxp::serialization;

/*! A Leave response message, as specified by MXP.
 */
class leave_rsp : public message {
public:
    /*! The response header. */
    response_fragment response_header;

    /*! Constructor. */
    leave_rsp() : message(LEAVE_RSP) {}

    /*! Virtual destructor. */
    virtual ~leave_rsp() {}

    /*! Copy constructor.

        \param lr the object to base this copy on.
    */
    leave_rsp(const leave_rsp & lr) : message(LEAVE_RSP),
                                      response_header(lr.response_header) {
    }

    /*! Assignment operator.

        \param lr the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
    */
    leave_rsp& operator=(const leave_rsp & lr) {
        if (this == &lr) {
            return *this;
        }

        response_header = lr.response_header;

        return *this;
    }

    virtual bool equals(const message & other) const {
        if (this == &other) {
            return true;
        }

        if (get_type() != other.get_type()) {
            return false;
        }

        const leave_rsp & lr = dynamic_cast<const leave_rsp &>(other);

        return response_header == lr.response_header;
    }

    virtual unsigned int size() const {
        return response_header.size();
    }

    virtual unsigned int
    serialize(byte_writer_base & writer) const {
        return response_header.serialize(writer);
    }

    virtual void
    deserialize(byte_reader_base & reader, unsigned int len) {
        if (len < size()) {
            return;
        }

        response_header.deserialize(reader,len);
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param lv_rsp the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const leave_rsp & lv_rsp) {
    os << "leave_rsp[type: " << lv_rsp.get_type();
    os << ",response_header=";
    os << lv_rsp.response_header;
    os << "]";

    return os;
}

}
}

#endif
