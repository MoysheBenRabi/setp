#ifndef MXP_JOIN_RQ
#define MXP_JOIN_RQ

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

#include <string>
#include <boost/cstdint.hpp>
#include <boost/date_time.hpp>
#include <boost/uuid/uuid.hpp>
#include <mxp/serialization/serialize.h>
#include <mxp/serialization/deserialize.h>
#include <mxp/message/message.h>
#include "program_fragment.h"

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;
using namespace mxp::serialization;

/*! A Join request message, as specified by MXP.
 */
class join_rq : public message {
public:
    /*! The bubble id to join, or an all-zero id to join the default bubble. */
    uuids::uuid         bubble_id;               // 16

    /*! The id of the avatar. */
    uuids::uuid         avatar_id;               // 16

    /*! The name of the bubble to join, may be emtpy. */
    std::wstring        bubble_name;             // 40

    /*! Landing location, may be emtpy. */
    std::wstring        location_name;           // 28

    /*! The id of the participant wanting to join. */
    std::wstring        participant_identifier;  // 32

    /*! The passphrase or authentication token of the participant. */
    std::wstring        participant_secret;      // 32

    /*! The current time at the participant, in UTC, not local time. */
    posix_time::ptime   participant_realtime;    // 8

    /*! Idenitity provider URL, e.g. OpenId provider, may be empty. */
    std::wstring        identity_provider_url;   // 50

    /*! Details of the client program used to join. */
    program_fragment    client_program;

    /*! Constructor. */
    join_rq() : message(JOIN_RQ) {}

    /*! Virtual destructor. */
    virtual ~join_rq() {}

    /*! Copy constructor.

        \param jn_rq the object to base this copy on.
    */
    join_rq(const join_rq & jn_rq) : message(JOIN_RQ),
                        bubble_id(jn_rq.bubble_id),
                        avatar_id(jn_rq.avatar_id),
                        bubble_name(jn_rq.bubble_name),
                        location_name(jn_rq.location_name),
                        participant_identifier(jn_rq.participant_identifier),
                        participant_secret(jn_rq.participant_secret),
                        participant_realtime(jn_rq.participant_realtime),
                        identity_provider_url(jn_rq.identity_provider_url),
                        client_program(jn_rq.client_program) {
    }

    /*! Assignment operator.

        \param jn_rq the object to base this object on.
        \return a reference to this object, after the assignemt has been made.

    */
    join_rq& operator=(const join_rq & jn_rq) {
        if (this == &jn_rq) {
            return *this;
        }

        bubble_id              = jn_rq.bubble_id;
        avatar_id              = jn_rq.avatar_id;
        bubble_name            = jn_rq.bubble_name;
        location_name          = jn_rq.location_name;
        participant_identifier = jn_rq.participant_identifier;
        participant_secret     = jn_rq.participant_secret;
        participant_realtime   = jn_rq.participant_realtime;
        identity_provider_url  = jn_rq.identity_provider_url;
        client_program         = jn_rq.client_program;

        return *this;
    }

    virtual bool equals(const message & other) const {
        if (this == &other) {
            return true;
        }

        if (get_type() != other.get_type()) {
            return false;
        }

        const join_rq & jr = dynamic_cast<const join_rq &>(other);

        return bubble_id              == jr.bubble_id
            && avatar_id              == jr.avatar_id
            && bubble_name            == jr.bubble_name
            && location_name          == jr.location_name
            && participant_identifier == jr.participant_identifier
            && participant_secret     == jr.participant_secret
            && participant_realtime   == jr.participant_realtime
            && identity_provider_url  == jr.identity_provider_url
            && client_program         == jr.client_program;
    }

    virtual unsigned int size() const {
        return 16 + 16 + 40 + 28 + 32 + 32 + 8 + 50 + client_program.size();
    }

    virtual unsigned int
    serialize(byte_writer_base & writer) const {
        unsigned int count = 0;

        count += write<uuids::uuid>(writer, bubble_id);
        count += write<uuids::uuid>(writer, avatar_id);
        count += write<std::wstring>(writer, bubble_name, 40);
        count += write<std::wstring>(writer, location_name, 28);
        count += write<std::wstring>(writer, participant_identifier, 32);
        count += write<std::wstring>(writer, participant_secret, 32);
        count += write<posix_time::ptime>(writer, participant_realtime);
        count += write<std::wstring>(writer, identity_provider_url, 50);
        count += client_program.serialize(writer);

        return count;
    }

    virtual void
    deserialize(byte_reader_base & reader, unsigned int len) {
        if (len < size()) {
            return;
        }

        unsigned int start = reader.count();

        bubble_id              = read<uuids::uuid>(reader);
        avatar_id              = read<uuids::uuid>(reader);
        bubble_name            = read<std::wstring>(reader, 40);
        location_name          = read<std::wstring>(reader, 28);
        participant_identifier = read<std::wstring>(reader, 32);
        participant_secret     = read<std::wstring>(reader, 32);
        participant_realtime   = read<posix_time::ptime>(reader);
        identity_provider_url  = read<std::wstring>(reader, 50);

        client_program.deserialize(reader, len - (reader.count() - start));
    }
};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param jn_rq the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const join_rq & jn_rq) {
    os << "join_rq[type: " << jn_rq.get_type();
    os << ",bubble_id=";
    os << jn_rq.bubble_id;
    os << ",avatar_id=";
    os << jn_rq.avatar_id;
    os << ",bubble_name=";
    os << jn_rq.bubble_name;
    os << ",location_name=";
    os << jn_rq.location_name;
    os << ",participant_identifier=";
    os << jn_rq.participant_identifier;
    os << ",participant_secret=";
    os << jn_rq.participant_secret;
    os << ",participant_realtime=";
    os << jn_rq.participant_realtime;
    os << ",identity_provider_url=";
    os << jn_rq.identity_provider_url;
    os << ",program_fragment=";
    os << jn_rq.client_program;
    os << "]";

    return os;
}

}
}
#endif
