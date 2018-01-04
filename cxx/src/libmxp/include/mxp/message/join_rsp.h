#ifndef MXP_JOIN_RSP
#define MXP_JOIN_RSP

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
#include <boost/date_time.hpp>
#include <mxp/serialization/serialize.h>
#include <mxp/serialization/deserialize.h>
#include <mxp/message/message.h>
#include "program_fragment.h"
#include "response_fragment.h"

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;
using namespace mxp::serialization;

/*! A Join response message, as specified by MXP.
 */
class join_rsp : public message {
public:
    /*! The response header. */
    response_fragment   response_header;

    /*! The id of the bubble just joined. */
    uuids::uuid         bubble_id;               // 16

    /*! The id of the participant who just joined, may be empty. */
    uuids::uuid         participant_id;          // 16

    /*! The id of the avatar just joined, may be emtpy. */
    uuids::uuid         avatar_id;               // 16

    /*! The name of the bubble just joined. */
    std::wstring        bubble_name;             // 40

    /*! The URL to access assets for this bubble. */
    std::wstring        bubble_asset_cache_url;  // 50

    /*! The range of the bubble. */
    float               bubble_range;            // 4

    /*! The perception range within the bubble. */
    float               bubble_perception_range; // 4

    /*! The current time at the bubble, in UTC. */
    posix_time::ptime   bubble_realtime;         // 8

    /*! The server program which sent the response. */
    program_fragment    server_program;          // 33

    /*! Constructor. */
    join_rsp() : message(JOIN_RSP) {}

    /*! Virtual destructor. */
    virtual ~join_rsp() {}

    /*! Copy constructor.

        \param jn_rsp the object to base this copy on.
    */
    join_rsp(const join_rsp & jn_rsp) : message(JOIN_RSP),
                        response_header(jn_rsp.response_header),
                        bubble_id(jn_rsp.bubble_id),
                        participant_id(participant_id),
                        avatar_id(jn_rsp.avatar_id),
                        bubble_name(jn_rsp.bubble_name),
                        bubble_asset_cache_url(jn_rsp.bubble_asset_cache_url),
                        bubble_range(jn_rsp.bubble_range),
                        bubble_perception_range(jn_rsp.bubble_perception_range),
                        bubble_realtime(jn_rsp.bubble_realtime),
                        server_program(jn_rsp.server_program) {
    }

    /*! Assignment operator.

        \param jn_rsp the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
    */
    join_rsp& operator=(const join_rsp & jn_rsp) {
        if (this == &jn_rsp) {
            return *this;
        }

        response_header = jn_rsp.response_header;

        bubble_id               = jn_rsp.bubble_id;
        participant_id          = jn_rsp.participant_id;
        avatar_id               = jn_rsp.avatar_id;
        bubble_name             = jn_rsp.bubble_name;
        bubble_asset_cache_url  = jn_rsp.bubble_asset_cache_url;
        bubble_range            = jn_rsp.bubble_range;
        bubble_perception_range = jn_rsp.bubble_perception_range;
        bubble_realtime         = jn_rsp.bubble_realtime;

        server_program          = jn_rsp.server_program;

        return *this;
    }

    virtual bool equals(const message & other) const {
        if (this == &other) {
            return true;
        }

        if (get_type() != other.get_type()) {
            return false;
        }

        const join_rsp & jr = dynamic_cast<const join_rsp &>(other);

        return response_header         == jr.response_header
            && bubble_id               == jr.bubble_id
            && participant_id          == jr.participant_id
            && avatar_id               == jr.avatar_id
            && bubble_name             == jr.bubble_name
            && bubble_asset_cache_url  == jr.bubble_asset_cache_url
            && bubble_range            == jr.bubble_range
            && bubble_perception_range == jr.bubble_perception_range
            && bubble_realtime         == jr.bubble_realtime
            && server_program          == jr.server_program;
    }

    virtual unsigned int size() const {
        return response_header.size()
             + 16 + 16 + 16 + 40 + 50 + 4 + 4 + 8
             + server_program.size();
    }

    /*! Serialize this message.

        \param writer the byte writer to serialize this message into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
     */
    virtual unsigned int
    serialize(byte_writer_base & writer) const {
        unsigned int counter = 0u;

        counter += response_header.serialize(writer);

        counter += write<uuids::uuid>(writer, bubble_id);
        counter += write<uuids::uuid>(writer, participant_id);
        counter += write<uuids::uuid>(writer, avatar_id);
        counter += write<std::wstring>(writer, bubble_name, 40);
        counter += write<std::wstring>(writer, bubble_asset_cache_url, 50);
        counter += write<float>(writer, bubble_range);
        counter += write<float>(writer, bubble_perception_range);
        counter += write<posix_time::ptime>(writer, bubble_realtime);

        counter += server_program.serialize(writer);

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
        if (len < size()) {
            return;
        }

        unsigned int start = reader.count();

        response_header.deserialize(reader, len);

        bubble_id               = read<uuids::uuid>(reader);
        participant_id          = read<uuids::uuid>(reader);
        avatar_id               = read<uuids::uuid>(reader);
        bubble_name             = read<std::wstring>(reader, 40);
        bubble_asset_cache_url  = read<std::wstring>(reader, 50);
        bubble_range            = read<float>(reader);
        bubble_perception_range = read<float>(reader);
        bubble_realtime         = read<posix_time::ptime>(reader);

        server_program.deserialize(reader, len - (reader.count() - start));
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param jn_rsp the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const join_rsp & jn_rsp) {
    os << "join_rsp[type: " << jn_rsp.get_type();
    os << ",response_header=";
    os << jn_rsp.response_header;
    os << ",bubble_id=";
    os << jn_rsp.bubble_id;
    os << ",participant_id=";
    os << jn_rsp.participant_id;
    os << ",avatar_id=";
    os << jn_rsp.avatar_id;
    os << ",bubble_name=";
    os << jn_rsp.bubble_name;
    os << ",bubble_asset_cache_url=";
    os << jn_rsp.bubble_asset_cache_url;
    os << ",bubble_range=";
    os << jn_rsp.bubble_range;
    os << ",bubble_perception_range=";
    os << jn_rsp.bubble_perception_range;
    os << ",bubble_realtime=";
    os << jn_rsp.bubble_realtime;
    os << ",program_header=";
    os << jn_rsp.server_program;
    os << "]";

    return os;
}

}
}

#endif
