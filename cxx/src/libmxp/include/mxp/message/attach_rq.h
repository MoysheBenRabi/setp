#ifndef MXP_MESSAGE_ATTACH_RQ
#define MXP_MESSAGE_ATTACH_RQ

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
#include <boost/uuid/uuid.hpp>
#include <mxp/serialization/serialize.h>
#include <mxp/serialization/deserialize.h>
#include <mxp/message/message.h>
#include "bubble_fragment.h"
#include "program_fragment.h"

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;
using namespace mxp::serialization;

/*! An attach request message, as specified by MXP.
 */
class attach_rq : public message {
public:

    /*! ID of a target buuble */
    uuids::uuid         target_bubble_id;     // 16
    /*! The Source buuble fragment */
    bubble_fragment     source_bubble;        // 195
    /*! Description of the source bubble server */
    program_fragment    source_bubble_server; // 33


    /*! Constructor. */
    attach_rq() : message(ATTACH_RQ) {}

    /*! Virtual destructor. */
    virtual ~attach_rq() {}

    /*! Copy constructor.

        \param atch_rq the object to base this copy on.
    */
    attach_rq(const attach_rq & atch_rq) : message(ATTACH_RQ),
        target_bubble_id(atch_rq.target_bubble_id),
        source_bubble(atch_rq.source_bubble),
        source_bubble_server(atch_rq.source_bubble_server) {
    }

    /*! Assignment operator.

        \param atch_rq the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
    */
    attach_rq& operator=(const attach_rq & atch_rq) {
        if (this == &atch_rq) {
            return *this;
        }
        target_bubble_id = atch_rq.target_bubble_id;
        source_bubble = atch_rq.source_bubble;
        source_bubble_server = atch_rq.source_bubble_server;
        return *this;
    }

    /*! equals.

        \param other the object to equal with this one.
        \return true if objects is equal and false afterwards.
    */
    virtual bool equals(const message & other) const {
        return (this == &other) ||
              ((get_type() == other.get_type()) &&
               (target_bubble_id == ((attach_rq &) other).target_bubble_id) &&
               (source_bubble == ((attach_rq &) other).source_bubble) &&
               (source_bubble_server == ((attach_rq &) other).source_bubble_server));
    }

    virtual unsigned int size() const {
        return 244;
    }

    /*! Serialize this message.

        \param writer the byte writer to serialize this message into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
     */
    virtual unsigned int
    serialize(serialization::byte_writer_base & writer) const {
        unsigned int counter = 0;

        counter += write<uuids::uuid>(writer, target_bubble_id);
        counter += source_bubble.serialize(writer);
        counter += source_bubble_server.serialize(writer);

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

        target_bubble_id = serialization::read<uuids::uuid>(reader);
        source_bubble.deserialize(reader,len);
        source_bubble_server.deserialize(reader,len);
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param atch_rq the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const attach_rq & atch_rq) {
    os << "attach_rq[type: " << atch_rq.get_type();
    os << ",target_bubble_id=";
    os << atch_rq.target_bubble_id;
    os << ",source_bubble=";
    os << atch_rq.source_bubble;
    os << ",source_bubble_server=";
    os << atch_rq.source_bubble_server;
    os << "]";
    return os;
}

}
}
#endif
