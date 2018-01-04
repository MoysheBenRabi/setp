#ifndef MXP_MESSAGE_OBJECT_FRAGMENT
#define MXP_MESSAGE_OBJECT_FRAGMENT

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

#include <vector>
#include <boost/cstdint.hpp>
#include <boost/array.hpp>
#include <boost/uuid/uuid.hpp>
#include "mxp/serialization/serialize.h"
#include "mxp/serialization/deserialize.h"


namespace mxp {
namespace message {

using namespace boost;
using namespace mxp::serialization;

/*! A object fragment, as defined by MXP
 */
class object_fragment {
public:

    /*! ID of the object */
    uuids::uuid         object_id;                       // 16
    /*! Index of the object */
    uint32_t            object_index;                    // 4
    /*! Id of the object type */
    uuids::uuid         type_id;                         // 16
    /*! Id of the parent object */
    uuids::uuid         parent_object_id;                // 16
    /*! Name of the object */
    std::wstring        object_name;                     // 20
    /*! Type of the object */
    std::wstring        type_name;                       // 20
    /*! Owner participant id */
    uuids::uuid         owner_id;                        // 16
    /*! Current location of the object */
    array<float, 3>     location;                        // 12
    /*! Current velocity of the object */
    array<float, 3>     velocity;                        // 12
    /*! Current acceleration of the object */
    array<float, 3>     acceleration;                    // 12
    /*! Current orientation of the object */
    array<float, 4>     orientation;                     // 16
    /*! Current angular velocity of the object */
    array<float, 4>     angular_velocity;                // 16
    /*! Current angular acceleration of the object */
    array<float, 4>     angular_acceleration;            // 16
    /*! Current angular bounding sphere radius of the object */
    float               bounding_sphere_radius;          // 4
    /*! Mass of the object */
    float               mass;                            // 4

    /*! Extension dialect */
    std::wstring        extension_dialect;               // 4
    /*! Extension dialect major version */
    uint8_t             extension_dialect_major_version; // 1
    /*! Extension dialect minor version */
    uint8_t             extension_dialect_minor_version; // 1
    //uint32_t          extension_length;                // 4
    /*! Extension data */
    std::vector<uint8_t> extension_data;                 // X

    object_fragment() {
    }

    /*! Calculate and return the size of this fragment when serialized.

        \return the size of this fragment in bytes, when serialized.
     */
    unsigned int size() const {
        return 210 + extension_data.size();
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
        counter += write<uuids::uuid>(writer, object_id);
        counter += write<uint32_t>(writer, object_index);
        counter += write<uuids::uuid>(writer, type_id);
        counter += write<uuids::uuid>(writer, parent_object_id);
        counter += write<std::wstring>(writer, object_name, 20);
        counter += write<std::wstring>(writer, type_name, 20);
        counter += write<uuids::uuid>(writer, owner_id);
        counter += write<array<float, 3> >(writer, location);
        counter += write<array<float, 3> >(writer, velocity);
        counter += write<array<float, 3> >(writer, acceleration);
        counter += write<array<float, 4> >(writer, orientation);
        counter += write<array<float, 4> >(writer, angular_velocity);
        counter += write<array<float, 4> >(writer, angular_acceleration);
        counter += write<float>(writer, bounding_sphere_radius);
        counter += write<float>(writer, mass);

        counter += write<std::wstring>(writer, extension_dialect, 4);
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
    deserialize(byte_reader_base & reader, unsigned int len) {
        if (len < size()) {
            return;
        }

        object_id = read<uuids::uuid>(reader);
        object_index = read<uint32_t>(reader);
        type_id = read<uuids::uuid>(reader);
        parent_object_id = read<uuids::uuid>(reader);
        object_name = read<std::wstring>(reader,20);
        type_name = read<std::wstring>(reader,20);
        owner_id = read<uuids::uuid>(reader);
        location = read<array<float, 3> >(reader);
        velocity = read<array<float, 3> >(reader);
        acceleration = read<array<float, 3> >(reader);
        orientation = read<array<float, 4> >(reader);
        angular_velocity = read<array<float, 4> >(reader);
        angular_acceleration = read<array<float, 4> >(reader);
        bounding_sphere_radius = read<float>(reader);
        mass = read<float>(reader);

        extension_dialect = read<std::wstring>(reader,4);
        extension_dialect_major_version = read<uint8_t>(reader);
        extension_dialect_minor_version = read<uint8_t>(reader);

        uint32_t length = read<uint32_t>(reader);
        extension_data.resize(length);
        read_range<uint8_t>(reader, extension_data.begin(),length);

    }
};

/*! Equality comparator.

    \param first the first object to equal.
    \param second the second object to equal.
    \return true if the two objects are equal, false otherwise.
 */
inline
bool operator == (const object_fragment &first, const object_fragment &second) {
    return (first.object_id == second.object_id) &&
           (first.object_index == second.object_index) &&
           (first.type_id == second.type_id) &&
           (first.parent_object_id == second.parent_object_id) &&
           (first.object_name == second.object_name) &&
           (first.type_name == second.type_name) &&
           (first.owner_id == second.owner_id) &&
           (first.location == second.location) &&
           (first.velocity == second.velocity) &&
           (first.acceleration == second.acceleration) &&
           (first.orientation == second.orientation) &&
           (first.angular_velocity == second.angular_velocity) &&
           (first.angular_acceleration == second.angular_acceleration) &&
           (first.bounding_sphere_radius == second.bounding_sphere_radius) &&
           (first.mass == second.mass) &&
           (first.extension_dialect == second.extension_dialect) &&
           (first.extension_dialect_major_version ==
                second.extension_dialect_major_version) &&
           (first.extension_dialect_minor_version ==
                second.extension_dialect_minor_version) &&
           std::equal(first.extension_data.begin(),first.extension_data.end(),
               second.extension_data.begin());
}

/*! Write a human-readable representation of a fragment to an output stream.

    \param os the output stream to write to.
    \param fragment fragment to write.
    \return the output stream after the message has been written to it.
 */
inline
std::ostream& operator<<(std::ostream &os, const object_fragment & fragment) {
    os << "{";
    os << ", object_index = ";
    os << fragment.object_id;
    os << ", object_index = ";
    os << fragment.object_index;
    os << ", type_id = ";
    os << fragment.type_id;
    os << ", parent_object_id = ";
    os << fragment.parent_object_id;
    os << ", object_name = ";
    os << fragment.object_name;
    os << ", type_name = ";
    os << fragment.type_name;
    os << ", owner_id = ";
    os << fragment.owner_id;
    os << ", location = ";
    os << fragment.location;
    os << ", velocity = ";
    os << fragment.velocity;
    os << ", acceleration = ";
    os << fragment.acceleration;
    os << ", orientation = ";
    os << fragment.orientation;
    os << ", angular_velocity = ";
    os << fragment.angular_velocity;
    os << ", angular_acceleration = ";
    os << fragment.angular_acceleration;
    os << ", bounding_sphere_radius = ";
    os << fragment.bounding_sphere_radius;
    os << ", mass = ";
    os << fragment.mass;
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
