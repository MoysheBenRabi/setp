#ifndef MXP_MESSAGE_ACKNOWLEDGE
#define MXP_MESSAGE_ACKNOWLEDGE

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

#include <algorithm>
#include <vector>
#include <boost/cstdint.hpp>
#include <boost/foreach.hpp>

#include "mxp/serialization/serialize.h"
#include "mxp/serialization/deserialize.h"
#include "mxp/message/message.h"

namespace mxp {
namespace message {

using namespace mxp;

/*! An Acknowledge message, as specified by MXP.
 */
class acknowledge : public message {
public:
    /*! The maximum number of packets ids an acknowledge message may contain. */
    static const unsigned int MAX_PACKET_IDS = 64u;

    /*! The packet ids that are being acknowledged in this message. */
    std::vector<uint32_t>   packet_ids;

    /*! Constructor. */
    acknowledge() : message(ACKNOWLEDGE) {}

    /*! Virtual destructor. */
    virtual ~acknowledge() {}

    /*! Copy constructor.

        \param ack the object to base this copy on.
     */
    acknowledge(const acknowledge & ack) : message(ACKNOWLEDGE),
                                           packet_ids(ack.packet_ids) {
    }

    /*! Assignment operator.

        \param ack the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
     */
    acknowledge& operator=(const acknowledge & ack) {
        if (this == &ack) {
            return *this;
        }

        packet_ids.clear();
        packet_ids.resize(ack.packet_ids.size());
        std::copy(ack.packet_ids.begin(), ack.packet_ids.end(),
                  packet_ids.begin());

        return *this;
    }

    virtual bool equals(const message & other) const {
        if (this == &other) {
            return true;
        }

        if (get_type() != other.get_type()) {
            return false;
        }

        const acknowledge & ack = dynamic_cast<const acknowledge &>(other);

        return packet_ids.size() == ack.packet_ids.size()
            && std::equal(packet_ids.begin(), packet_ids.end(),
                          ack.packet_ids.begin());
    }

    virtual unsigned int size() const {
        return packet_ids.size() * 4u;
    }

    virtual unsigned int
    serialize(serialization::byte_writer_base & writer) const {
        unsigned int counter = 0;

        BOOST_FOREACH(uint32_t value, packet_ids) {
            counter += serialization::write<uint32_t>(writer, value);
        }

        return counter;
    }

    virtual void
    deserialize(serialization::byte_reader_base & reader, unsigned int len) {
        packet_ids.clear();

        unsigned int begin   = reader.count();
        unsigned int no_ids  =
                    (std::min)((unsigned int) (len / sizeof(uint32_t)),
                               (unsigned int) MAX_PACKET_IDS);
        unsigned int to_read = no_ids * sizeof(uint32_t);
        packet_ids.reserve(no_ids);
        while (reader.count() - begin < to_read) {
            packet_ids.push_back(serialization::read<uint32_t>(reader));
        }
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param ack the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const acknowledge & ack) {
    os << "ack[type: " << ack.get_type()
       << ", size: " << ack.size() << ", values: ";
    BOOST_FOREACH(uint32_t value, ack.packet_ids) {
        os << value << ", ";
    }
    os << "]";

    return os;
}

}
}

#endif
