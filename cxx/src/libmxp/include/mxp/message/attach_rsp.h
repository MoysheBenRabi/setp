#ifndef MXP_MESSAGE_ATTACH_RSP
#define MXP_MESSAGE_ATTACH_RSP

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
#include "response_fragment.h"
#include "bubble_fragment.h"
#include "program_fragment.h"

namespace mxp {
namespace message {

using namespace mxp;

/*! A Attach request message, as specified by MXP.
 */
class attach_rsp : public message {
public:

    /*! Attach response fragment */
    response_fragment   response_header;      // 5
    /*! The Target bubble fragment */
    bubble_fragment     target_bubble;        // 195
    /*! Description of the target bubble */
    program_fragment    target_bubble_server; // 33


    /*! Constructor. */
    attach_rsp() : message(ATTACH_RSP) {}

    /*! Virtual destructor. */
    virtual ~attach_rsp() {}

    /*! Copy constructor.

        \param atch_rsp the object to base this copy on.
    */
    attach_rsp(const attach_rsp & atch_rsp) : message(ATTACH_RSP),
        response_header(atch_rsp.response_header),
        target_bubble(atch_rsp.target_bubble),
        target_bubble_server(atch_rsp.target_bubble_server) {
    }

    /*! Assignment operator.

        \param atch_rsp the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
    */
    attach_rsp& operator=(const attach_rsp & atch_rsp) {
        if (this == &atch_rsp) {
            return *this;
        }
        response_header = atch_rsp.response_header;
        target_bubble = atch_rsp.target_bubble;
        target_bubble_server = atch_rsp.target_bubble_server;
        return *this;
    }

    /*! equals.

        \param other the object to equal with this one.
        \return true if objects is equal and false afterwards.
    */
    virtual bool equals(const message & other) const {
        return (this == &other) ||
              ((get_type() == other.get_type()) &&
               (response_header == ((attach_rsp &) other).response_header) &&
               (target_bubble == ((attach_rsp &) other).target_bubble) &&
               (target_bubble_server == ((attach_rsp &) other).target_bubble_server));
    }

    virtual unsigned int size() const {
        return 233;
    }

    /*! Serialize this message.

        \param writer the byte writer to serialize this message into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
     */
    virtual unsigned int
    serialize(serialization::byte_writer_base & writer) const {
        int counter = 0;
        counter += response_header.serialize(writer);
        counter += target_bubble.serialize(writer);
        counter += target_bubble_server.serialize(writer);
        return counter;
    }

    /*! De-serialize this message. The contents of this message will be filled
        with the de-serialized values read from the supplied byte reader.

        \param reader the byte reader to serialize this message from by reading
               a sequence of bytes from it
        \param len the maximum number of reads to make on the supplied
               reader
     */
    virtual void
    deserialize(serialization::byte_reader_base & reader, unsigned int len) {
        response_header.deserialize(reader,len);
        target_bubble.deserialize(reader,len);
        target_bubble_server.deserialize(reader,len);
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param atch_rsp the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const attach_rsp & atch_rsp) {
    os << "attach_rsp[type: " << atch_rsp.get_type();
    os << ",response_header=";
    os << atch_rsp.response_header;
    os << ",target_bubble=";
    os << atch_rsp.target_bubble;
    os << ",target_bubble_server=";
    os << atch_rsp.target_bubble_server;
    os << "]";
    return os;
}

}
}
#endif
