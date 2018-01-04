#ifndef MXP_MESSAGE_LIST_BUBBLES_RQ
#define MXP_MESSAGE_LIST_BUBBLES_RQ

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

/*! A List bubbles request message, as specified by MXP.
 */
class list_bubbles_rq : public message {
public:

    /*! List type enumeration */
    enum LIST_TYPE {
        /*! List only hosted bubbles */
        LIST_TYPE_HOSTED    = 0,
        /*! List only linked bubbles */
        LIST_TYPE_LINKED    = 1,
        /*! List only connected bubbles */
        LIST_TYPE_CONNECTED = 2
    };

    /*! List type */
    uint8_t list_type;

    /*! Constructor. */
    list_bubbles_rq() : message(LIST_BUBBLES_RQ) {}

    /*! Virtual destructor. */
    virtual ~list_bubbles_rq() {}

    /*! Copy constructor.

        \param lbs_rq the object to base this copy on.
    */
    list_bubbles_rq(const list_bubbles_rq & lbs_rq) : message(LIST_BUBBLES_RQ),
                                                  list_type(lbs_rq.list_type) {
    }

    /*! Assignment operator.

        \param lbs_rq the object to base this object on.
        \return a reference to this object, after the assignemt has been made.

    */
    list_bubbles_rq& operator=(const list_bubbles_rq & lbs_rq) {
        if (this == &lbs_rq) {
            return *this;
        }
        list_type = lbs_rq.list_type;
        return *this;
    }

    /*! equals.

        \param other the object to equal with this one.
        \return true if objects is equal and false afterwards.
    */
    virtual bool equals(const message & other) const {
        return (this == &other) || ((get_type() == other.get_type()) &&
               (list_type == ((list_bubbles_rq &) other).list_type ));
    }

    virtual unsigned int size() const {
        return 1;
    }

    /*! Serialize this message.

        \param writer the byte writer to serialize this message into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
     */
    virtual unsigned int
    serialize(serialization::byte_writer_base & writer) const {
        return serialization::write<uint8_t>(writer,list_type);
    }

    /*! De-serialize this message. The contents of this message will be filled
        with the de-serialized values read from the supplied byte reader.

        \param reader the byte reader to serialize this message from by reading
               a sequence of bytes from it
     */
    virtual void
    deserialize(serialization::byte_reader_base & reader, unsigned int ) {
        list_type = serialization::read<uint8_t>(reader);
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param lbs_rq the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const list_bubbles_rq & lbs_rq) {
    os << "list_bubbles_rq[type: " << lbs_rq.get_type();
    os << ", list_type=";
    os << lbs_rq.list_type;
    os << "]";
    return os;
}

}
}
#endif
