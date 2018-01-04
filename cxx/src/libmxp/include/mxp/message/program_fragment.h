#ifndef MXP_MESSAGE_PROGRAMM_FRAGMENT
#define MXP_MESSAGE_PROGRAMM_FRAGMENT

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
#include <mxp/serialization/serialize.h>
#include <mxp/serialization/deserialize.h>

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;
using namespace mxp::serialization;

/*! A program fragment, as defined by MXP.
*/
class program_fragment {
public:
    /*! The program's name. */
    std::wstring    program_name;                // 25

    /*! Program major version. */
    uint8_t         program_major_version;       // 1

    /*! Program minor version. */
    uint8_t         program_minor_version;       // 1

    /*! Protocol major version. */
    uint8_t         protocol_major_version;      // 1

    /*! Protocol minor version. */
    uint8_t         protocol_minor_version;      // 1

    /*! Protocol source revision. */
    uint32_t        protocol_source_revision;    // 4

    /*! Constructor. */
    program_fragment(): program_major_version(0),
                        program_minor_version(0),
                        protocol_major_version(0),
                        protocol_minor_version(0),
                        protocol_source_revision(0) {
    }

    /*! Copy constructor.

        \param other the other program fragment object to copy.
     */
    program_fragment(const program_fragment & other)
            : program_name(other.program_name),
              program_major_version(other.program_major_version),
              program_minor_version(other.program_minor_version),
              protocol_major_version(other.protocol_major_version),
              protocol_minor_version(other.protocol_minor_version),
              protocol_source_revision(other.protocol_source_revision) {
    }

    /*! Assignment operator.

        \param other the other fragment object to assign this one to.
        \return a reference to this object, after the assignment.
     */
    program_fragment & operator=(const program_fragment & other) {
        if (this == &other) {
            return *this;
        }

        program_name             = other.program_name;
        program_major_version    = other.program_major_version;
        program_minor_version    = other.program_minor_version;
        protocol_major_version   = other.protocol_major_version;
        protocol_minor_version   = other.protocol_minor_version;
        protocol_source_revision = other.protocol_source_revision;

        return *this;
    }

    /*! Calculate and return the size of this fragment when serialized.

        \return the size of this fragment when serialized, in bytes.
     */
    unsigned int size() const {
        return 33;
    }

    /*! Serialize this fragment.

        \param writer the byte writer to serialize this fragment into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
     */
    unsigned int
    serialize(byte_writer_base & writer) const {
        unsigned int count = 0;

        count += write<std::wstring>(writer, program_name, 25);
        count += write<uint8_t>(writer, program_major_version);
        count += write<uint8_t>(writer, program_minor_version);
        count += write<uint8_t>(writer, protocol_major_version);
        count += write<uint8_t>(writer, protocol_minor_version);
        count += write<uint32_t>(writer, protocol_source_revision);

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

        program_name             = read<std::wstring>(reader, 25);
        program_major_version    = read<uint8_t>(reader);
        program_minor_version    = read<uint8_t>(reader);
        protocol_major_version   = read<uint8_t>(reader);
        protocol_minor_version   = read<uint8_t>(reader);
        protocol_source_revision = read<uint32_t>(reader);
    }
};

/*! Equality comparision for program_fragment objects.

    \param first one of the program fragments to compare
    \param second the other program fragment to compare.
    \return true if the two program fragments are equal, false otherwise
 */
inline
bool operator==(const program_fragment &first, const program_fragment &second) {
    return first.program_name             == second.program_name
        && first.program_major_version    == second.program_major_version
        && first.program_minor_version    == second.program_minor_version
        && first.protocol_major_version   == second.protocol_major_version
        && first.protocol_minor_version   == second.protocol_minor_version
        && first.protocol_source_revision == second.protocol_source_revision;
}

/*! Write a human-readable representation of a fragment to an output stream.

    \param os the output stream to write to.
    \param fragment fragment to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const program_fragment & fragment) {
    os << "{";
    os << "program_name=";
    os << fragment.program_name;
    os << ",program_major_version=";
    os << fragment.program_major_version;
    os << ",program_minor_version=";
    os << fragment.program_minor_version;
    os << ",protocol_major_version=";
    os << fragment.protocol_major_version;
    os << ",protocol_minor_version=";
    os << fragment.protocol_minor_version;
    os << ",protocol_source_revision=";
    os << fragment.protocol_source_revision;
    os << "}";

    return os;
}

}
}

#endif
