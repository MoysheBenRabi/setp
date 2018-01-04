#ifndef MXP_MESSAGE_INTERACTION_FRAGMENT
#define MXP_MESSAGE_INTERACTION_FRAGMENT

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

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;
using namespace mxp::serialization;

/*! An interaction fragment, as defined by MXP
*/
class interaction_fragment {
public:

    /*! Name of the interaction */
    std::wstring        interaction_name;          // 20
    /*! Source participant id */
    uuids::uuid         source_participant_id;     // 16
    /*! Source object id */
    uuids::uuid         source_object_id;          // 16
    /*! Target participant id */
    uuids::uuid         target_participant_id;     // 16
    /*! Target object id */
    uuids::uuid         target_object_id;          // 16

    /*! Extension dialect */
    std::wstring         extension_dialect;               // 4
    /*! Extension dialect major version */
    uint8_t              extension_dialect_major_version; // 1
    /*! Extension dialect minor version */
    uint8_t              extension_dialect_minor_version; // 1
    //uint32_t           extension_length;                // 4
    /*! Extension data */
    std::vector<uint8_t> extension_data;                  // X

    interaction_fragment() {
    }

    /*! Calculate and return the size of this fragment when serialized.

        \return the size of this fragment in bytes, when serialized.
     */
    unsigned int size() const {
        return 94 + extension_data.size();
    }

    /*! Serialize this fragment.

        \param writer the byte writer to serialize this fragment into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
        \see unsigned int serialize(const fragment_class & fragment,
             iterator & it)
     */
    unsigned int
    serialize(byte_writer_base & writer) const {
        unsigned int counter = 0;

        counter += write<std::wstring>(writer, interaction_name,20);
        counter += write<uuids::uuid>(writer, source_participant_id);
        counter += write<uuids::uuid>(writer, source_object_id);
        counter += write<uuids::uuid>(writer, target_participant_id);
        counter += write<uuids::uuid>(writer, target_object_id);

        counter += write<std::wstring>(writer,extension_dialect,4);
        counter += write<uint8_t>(writer, extension_dialect_major_version);
        counter += write<uint8_t>(writer, extension_dialect_minor_version);
        counter += write<uint32_t>(writer, extension_data.size());
        counter += write_range<uint8_t>(writer,
                                        extension_data.begin(),
                                        extension_data.size());
        return counter;
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
    deserialize(byte_reader_base & reader, unsigned int len ) {
        if (len < size()) {
            return;
        }

        interaction_name      = read<std::wstring>(reader,20);
        source_participant_id = read<uuids::uuid>(reader);
        source_object_id      = read<uuids::uuid>(reader);
        target_participant_id = read<uuids::uuid>(reader);
        target_object_id      = read<uuids::uuid>(reader);

        extension_dialect               = read<std::wstring>(reader,4);
        extension_dialect_major_version = read<uint8_t>(reader);
        extension_dialect_minor_version = read<uint8_t>(reader);
        uint32_t length                 = read<uint32_t>(reader);
        extension_data.resize(length);
        read_range<uint8_t>(reader, extension_data.begin(), length);
    }
};

/*! Equality comparator.

    \param first the first object to equal.
    \param second the second object to equal.
    \return true if the two objects are equal, false otherwise.
 */
inline
bool operator == (const interaction_fragment &first, const interaction_fragment &second) {
    return first.interaction_name      == second.interaction_name &&
           first.source_participant_id == second.source_participant_id &&
           first.source_object_id      == second.source_object_id &&
           first.target_participant_id == second.target_participant_id &&
           first.target_object_id      == second.target_object_id &&
           first.extension_dialect     == second.extension_dialect &&
           first.extension_dialect_major_version
               == second.extension_dialect_major_version &&
           first.extension_dialect_minor_version
               == second.extension_dialect_minor_version &&
           std::equal(first.extension_data.begin(),first.extension_data.end(),
                      second.extension_data.begin());
}

/*! Write a human-readable representation of a fragment to an output stream.

    \param os the output stream to write to.
    \param fragment fragment to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const interaction_fragment & fragment) {
    os << "{";
    os << "interaction_name = ";
    os << fragment.interaction_name;
    os << ", source_participant_id = ";
    os << fragment.source_participant_id;
    os << ", source_object_id = ";
    os << fragment.source_object_id;
    os << ", target_participant_id = ";
    os << fragment.target_participant_id;
    os << ", target_object_id = ";
    os << fragment.target_object_id;
    os << ", extension_dialect = ";
    os << fragment.extension_dialect;
    os << ", extension_dialect_major_version = ";
    os << fragment.extension_dialect_major_version;
    os << ", extension_dialect_minor_version = ";
    os << fragment.extension_dialect_minor_version;
    os << ", extension_length = ";
    os << fragment.extension_data.size();
    os << ", extension_data {";
    for (uint32_t i = 0; i < fragment.extension_data.size(); ++i)
    {
        os << fragment.extension_data[i];
        if (i < fragment.extension_data.size() - 1) os << ", ";
    }
    os << "}";
    os << "}";

    return os;
}

}
}


#endif
