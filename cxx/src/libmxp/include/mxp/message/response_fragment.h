#ifndef MXP_MESSAGE_RESPONSE_FRAGMENT
#define MXP_MESSAGE_RESPONSE_FRAGMENT

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

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;
using namespace mxp::serialization;

/*! A response fragment, as defined by MXP.
*/
class response_fragment {
public:
    /*! The request message id this is a response to. */
    uint32_t request_message_id;     // 4

    /*! The response code, 0 == success, otherwise the error code. */
    uint8_t failure_code;            // 1

    /*! Constructor. */
    response_fragment(): request_message_id(0), failure_code(0) {
    }

    /*! Copy constructor.

        \param other the other response fragment object to copy.
     */
    response_fragment(const response_fragment & other)
            : request_message_id(other.request_message_id),
              failure_code(other.failure_code) {
    }

    /*! Assignment operator.

        \param other the other fragment object to assign this one to.
        \return a reference to this object, after the assignment.
     */
    response_fragment & operator=(const response_fragment & other) {
        if (this == &other) {
            return *this;
        }

        request_message_id = other.request_message_id;
        failure_code       = other.failure_code;

        return *this;
    }

    /*! Calculate and return the size of this fragment when serialized.

        \return the size of this fragment when serialized, in bytes.
     */
    unsigned int size() const {
        return 5;
    }

    /*! Serialize this fragment.

        \param writer the byte writer to serialize this fragment into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
     */
    unsigned int
    serialize(byte_writer_base & writer) const {
        unsigned int count = 0;

        count += write<uint32_t>(writer, request_message_id);
        count += write<uint8_t>(writer, failure_code);

        return count;
    }

    /*! De-serialize this fragment. The contents of this fragment will be filled
        with the de-serialized values read from the supplied byte reader.

        \param reader the byte reader to serialize this fragment from by reading
               a sequence of bytes from it
        \param len the maximum number of reads to make on the supplied
               reader
     */
    void
    deserialize(byte_reader_base & reader, unsigned int len) {
        if (len < size()) {
            return;
        }

        request_message_id = serialization::read<uint32_t>(reader);
        failure_code       = serialization::read<uint8_t>(reader);
    }
};

/*! Equality comparision for response_fragment objects.

    \param first one of the response fragments to compare
    \param second the other respones fragment to compare.
    \return true if the two response fragments are equal, false otherwise
 */
inline
bool operator==(const response_fragment &first,
                const response_fragment &second) {

    return first.request_message_id == second.request_message_id
        && first.failure_code       == second.failure_code;
}

/*! Write a human-readable representation of a fragment to an output stream.

    \param os the output stream to write to.
    \param fragment fragment to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os,const response_fragment & fragment) {
    os << "{"
       << "request_message_id:" << fragment.request_message_id
       << ", " << fragment.request_message_id
       << "failure_code:" << fragment.failure_code
       << "}";

    return os;
}

}
}


#endif
