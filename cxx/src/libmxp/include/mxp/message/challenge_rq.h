#ifndef MXP_MESSAGE_CHALLENGE_RQ
#define MXP_MESSAGE_CHALLENGE_RQ

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
#include <boost/cstdint.hpp>
#include <boost/array.hpp>
#include <boost/foreach.hpp>

#include <mxp/serialization/serialize.h>
#include <mxp/serialization/deserialize.h>
#include <mxp/message/message.h>

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;
using namespace mxp::serialization;

/*! A Challenge request message, as specified by MXP.
 */
class challenge_rq : public message {
public:

    /*! Challenge request data. */
    array<uint8_t, 64> challenge_rq_data;

    /*! Constructor. */
    challenge_rq() : message(CHALLENGE_RQ) {}

    /*! Virtual destructor. */
    virtual ~challenge_rq() {}

    /*! Copy constructor.

        \param chl_rq the object to base this copy on.
    */
    challenge_rq(const challenge_rq & chl_rq) : message(CHALLENGE_RQ),
        challenge_rq_data(chl_rq.challenge_rq_data) {
    }

    /*! Assignment operator.

        \param chl_rq the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
    */
    challenge_rq& operator=(const challenge_rq & chl_rq) {
        if (this == &chl_rq) {
            return *this;
        }

        challenge_rq_data = chl_rq.challenge_rq_data;

        return *this;
    }

    virtual bool equals(const message & other) const {
        if (this == &other) {
            return true;
        }

        if (get_type() != other.get_type()) {
            return false;
        }

        const challenge_rq & chl_rq = dynamic_cast<const challenge_rq &>(other);

        return std::equal(challenge_rq_data.begin(),
                          challenge_rq_data.end(),
                          chl_rq.challenge_rq_data.begin());
    }

    virtual unsigned int size() const {
        return challenge_rq_data.size();
    }

    virtual unsigned int
    serialize(byte_writer_base & writer) const {
        unsigned int counter = 0;

        counter += write<array<uint8_t, 64> >(writer, challenge_rq_data);

        return counter;
    }

    virtual void
    deserialize(byte_reader_base & reader, unsigned int len) {
        if (len < size()) {
            return;
        }

        challenge_rq_data = read<array<uint8_t, 64> >(reader);
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param chl_rq the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const challenge_rq & chl_rq) {
    os << "challenge_rq[type: " << chl_rq.get_type()
       << ", size: " << chl_rq.size() << ", values: ";
    BOOST_FOREACH(uint8_t value, chl_rq.challenge_rq_data) {
        os << value << ", ";
    }
    os << "]";

    return os;
}

}
}

#endif
