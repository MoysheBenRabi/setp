#ifndef MXP_MESSAGE_BUBBLE_FRAGMENT
#define MXP_MESSAGE_BUBBLE_FRAGMENT

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
#include <boost/array.hpp>
#include <boost/date_time.hpp>
#include <boost/uuid/uuid.hpp>
#include <mxp/serialization/serialize.h>
#include <mxp/serialization/deserialize.h>

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;
using namespace mxp::serialization;

/*! A bubble fragment, as defined by MXP
*/
class bubble_fragment {
public:

    /*! ID of the bubble */
    uuids::uuid         bubble_id;               // 16
    /*! Name of the bubble */
    std::wstring        bubble_name;             // 40
    /*! URL of the bubble asset cache */
    std::wstring        bubble_asset_cache_url;  // 51
    /*! ID of the bubble's owner */
    uuids::uuid         owner_id;                // 16
    /*! Address of the bubble */
    std::wstring        bubble_address;          // 40
    /*! Port of the bubble */
    uint32_t            bubble_port;             // 4
    /*! Location of the bubble's center */
    array<float, 3>     bubble_center;           // 12
    /*! Size of the bubble */
    float               bubble_range;            // 4
    /*! Maximum range of perception event population */
    float               bubble_perception_range; // 4
    /*! Bubble's time */
    posix_time::ptime   bubble_realtime;         // 8

    bubble_fragment() {
    }

    /*! Calculate and return the size of this fragment when serialized.

        \return the size of this fragment in bytes, when serialized.
     */
    unsigned int size() const {
        return 195;
    }

    /*! Serialize this fragment.

        \param writer the byte writer to serialize this fragment into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
        \see unsigned int serialize(const fragment_class & fragment,
             iterator & it)
     */
    unsigned int
    serialize(serialization::byte_writer_base & writer) const {
        unsigned int count = 0u;

        count += write<uuids::uuid>(writer, bubble_id);
        count += write<std::wstring>(writer, bubble_name, 40);
        count += write<std::wstring>(writer, bubble_asset_cache_url, 51);
        count += write<uuids::uuid>(writer, owner_id);
        count += write<std::wstring>(writer, bubble_address, 40);
        count += write<uint32_t>(writer, bubble_port);
        count += write<array<float, 3> >(writer, bubble_center);
        count += write<float>(writer, bubble_range);
        count += write<float>(writer, bubble_perception_range);
        count += write<posix_time::ptime>(writer, bubble_realtime);

        return count;
    }

    /*! De-serialize this fragment. The contents of this fragment will be filled
        with the de-serialized values read from the supplied byte reader.

        \param reader the byte reader to serialize this fragment from by reading
               a sequence of bytes from it
        \param len the maximum number of reads to make on the supplied
               reader
        \see deserialize(fragment_class & fragment,
                         iterator & it,
                         unsigned int len)
     */
    void
    deserialize(serialization::byte_reader_base & reader, unsigned int len ) {
        if (len < size()) {
            return;
        }

        bubble_id               = read<uuids::uuid>(reader);
        bubble_name             = read<std::wstring>(reader, 40);
        bubble_asset_cache_url  = read<std::wstring>(reader, 51);
        owner_id                = read<uuids::uuid>(reader);
        bubble_address          = read<std::wstring>(reader, 40);
        bubble_port             = read<uint32_t>(reader);
        bubble_center           = read<array<float, 3> >(reader);
        bubble_range            = read<float>(reader);
        bubble_perception_range = read<float>(reader);
        bubble_realtime         = read<posix_time::ptime>(reader);
    }
};

/*! Equality comparator.

    \param first the first object to equal.
    \param second the second object to equal.
    \return true if the two objects are equal, false otherwise.
 */
inline
bool operator == (const bubble_fragment &first, const bubble_fragment &second) {
    return first.bubble_id               == second.bubble_id &&
           first.bubble_name             == second.bubble_name &&
           first.bubble_asset_cache_url  == second.bubble_asset_cache_url &&
           first.owner_id                == second.owner_id &&
           first.bubble_address          == second.bubble_address &&
           first.bubble_port             == second.bubble_port &&
           first.bubble_center           == second.bubble_center &&
           first.bubble_range            == second.bubble_range &&
           first.bubble_perception_range == second.bubble_perception_range &&
           first.bubble_realtime         == second.bubble_realtime;
}

/*! Write a human-readable representation of a fragment to an output stream.

    \param os the output stream to write to.
    \param fragment frament to write.
    \return the output stream after the message has been written to it.
 */
inline
std::ostream& operator<<(std::ostream &os, const bubble_fragment & fragment) {
    os << "{";
    os << "bubble_id=";
    os << fragment.bubble_id;
    os << ",bubble_name=";
    os << fragment.bubble_name;
    os << ",bubble_asset_cache_url=";
    os << fragment.bubble_asset_cache_url;
    os << ",owner_id=";
    os << fragment.owner_id;
    os << ",bubble_address=";
    os << fragment.bubble_address;
    os << ",bubble_port=";
    os << fragment.bubble_port;
    os << ",bubble_center=";
    os << fragment.bubble_center;
    os << ",bubble_range=";
    os << fragment.bubble_range;
    os << ",bubble_perception_range=";
    os << fragment.bubble_perception_range;
    os << ",bubble_realtime=";
    os << fragment.bubble_realtime;
    os << "}";
    return os;
}
}
}


#endif
