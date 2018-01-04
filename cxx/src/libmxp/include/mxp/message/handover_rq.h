#ifndef MXP_MESSAGE_HANDOVER_RQ
#define MXP_MESSAGE_HANDOVER_RQ

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

#include <boost/uuid/uuid.hpp>
#include <mxp/serialization/serialize.h>
#include <mxp/serialization/deserialize.h>
#include <mxp/message/message.h>
#include "object_fragment.h"

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;
using namespace mxp::serialization;

/*! A Inject request message, as specified by MXP.
 */
class handover_rq : public message {
public:

    /*! ID of the source bubble (the bubble from there object moved) */
    uuids::uuid     source_bubble_id; // 16
    /*! ID of the target bubble (the bubble to move object to) */
    uuids::uuid     target_bubble_id; // 16
    /*! Description of the object */
    object_fragment object_header;    // X

    /*! Constructor. */
    handover_rq() : message(HANDOVER_RQ) {}

    /*! Virtual destructor. */
    virtual ~handover_rq() {}

    /*! Copy constructor.

        \param hovr_rq the object to base this copy on.
    */
    handover_rq(const handover_rq & hovr_rq) : message(HANDOVER_RQ),
        source_bubble_id(hovr_rq.source_bubble_id),
        target_bubble_id(hovr_rq.target_bubble_id),
        object_header(hovr_rq.object_header) {
    }

    /*! Assignment operator.

        \param hovr_rq the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
    */
    handover_rq& operator=(const handover_rq & hovr_rq) {
        if (this == &hovr_rq) {
            return *this;
        }
        source_bubble_id = hovr_rq.source_bubble_id;
        target_bubble_id = hovr_rq.target_bubble_id;
        object_header = hovr_rq.object_header;
        return *this;
    }

    /*! equals.

        \param other the object to equal with this one.
        \return true if objects is equal and false afterwards.

    */
    virtual bool equals(const message & other) const {
        return (this == &other) ||
               ((get_type() == other.get_type()) &&
                (source_bubble_id == ((handover_rq &)other).source_bubble_id) &&
                (target_bubble_id == ((handover_rq &)other).target_bubble_id) &&
                (object_header == ((handover_rq &)other).object_header));
    }

    virtual unsigned int size() const {
        return 32+object_header.size();
    }

    /*! Serialize this message.

        \param writer the byte writer to serialize this message into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
     */
    virtual unsigned int
    serialize(byte_writer_base & writer) const {
        unsigned int counter = 0;

        counter += write(writer, source_bubble_id);
        counter += write(writer, target_bubble_id);
        counter += object_header.serialize(writer);

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
    deserialize(byte_reader_base & reader, unsigned int len) {

        source_bubble_id = read<uuids::uuid>(reader);
        target_bubble_id = read<uuids::uuid>(reader);
        object_header.deserialize(reader, len - 32u);
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param hovr_rq the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const handover_rq & hovr_rq) {
    os << "handover_rq[type: " << hovr_rq.get_type();
    os << ",source_bubble=";
    os << hovr_rq.source_bubble_id;
    os << ",target_bubble=";
    os << hovr_rq.target_bubble_id;
    os << ",object_heder=";
    os << hovr_rq.object_header;
    os << "]";

    return os;
}

}
}
#endif
