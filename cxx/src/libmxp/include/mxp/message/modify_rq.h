#ifndef MXP_MESSAGE_MODIFY_RQ
#define MXP_MESSAGE_MODIFY_RQ

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

#include "mxp/serialization/serialize.h"
#include "mxp/serialization/deserialize.h"
#include "mxp/message/message.h"
#include "object_fragment.h"

namespace mxp {
namespace message {

using namespace mxp;

/*! A Modify request message, as specified by MXP.
 */
class modify_rq : public message {
public:

    /*! Modification description */
    object_fragment object_header;

    /*! Constructor. */
    modify_rq() : message(MODIFY_RQ) {}

    /*! Virtual destructor. */
    virtual ~modify_rq() {}

    /*! Copy constructor.

        \param mdf_rq the object to base this copy on.
     */
    modify_rq(const modify_rq & mdf_rq) : message(MODIFY_RQ) {
        object_header = mdf_rq.object_header;
    }

    /*! Assignment operator.

        \param mdf_rq the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
     */
    modify_rq& operator=(const modify_rq & mdf_rq) {
        if (this == &mdf_rq) {
            return *this;
        }
        object_header = mdf_rq.object_header;
        return *this;
    }

    /*! equals.

        \param other the object to equal with this one.
        \return true if objects is equal and false afterwards.
     */
    virtual bool equals(const message & other) const {
       return (this == &other) || 
              ((get_type() == other.get_type()) && 
              (object_header == ((modify_rq &) other).object_header));
    }

    virtual unsigned int size() const {
        return object_header.size();
    }

    /*! Serialize this message.

        \param writer the byte writer to serialize this message into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
     */
    virtual unsigned int
    serialize(serialization::byte_writer_base & writer) const {
        return object_header.serialize(writer);
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
        object_header.deserialize(reader,len);
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param mdf_rq the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const modify_rq & mdf_rq) {
    os << "modify_rq[type: " << mdf_rq.get_type();
    os << ",object_header=";
    os << mdf_rq.object_header;
    os << "]";
    return os;
}

}
}

#endif

