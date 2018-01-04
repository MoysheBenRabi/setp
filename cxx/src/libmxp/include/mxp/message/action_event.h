#ifndef MXP_MESSAGE_ACTION_EVENT
#define MXP_MESSAGE_ACTION_EVENT

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
#include <boost/uuid/uuid.hpp>
#include "mxp/serialization/serialize.h"
#include "mxp/serialization/deserialize.h"
#include "mxp/message/message.h"

namespace mxp {
namespace message {

using namespace boost;
using namespace mxp;
using namespace mxp::serialization;

/*! An action event message, as specified by MXP.
 */
class action_event : public message {
public:

    /*! Name of the action */
    std::wstring         action_name;                     // 20
    /*! ID of a source object */
    uuids::uuid          source_object_id;                // 16
    /*! Observation radius of the action */
    float                observation_radius;              // 4

    /*! Extension dialect */
    std::wstring         extension_dialect;               // 4
    /*! Extension dialect major version */
    uint8_t              extension_dialect_major_version; // 1
    /*! Extension dialect minor version */
    uint8_t              extension_dialect_minor_version; // 1
    //uint32_t           extension_length;                // 4
    /*! Extension data */
    std::vector<uint8_t> extension_data;                  // X

    /*! Constructor. */
    action_event() : message(ACTION_EVENT) {}

    /*! Virtual destructor. */
    virtual ~action_event() {}

    /*! Copy constructor.

        \param a_evt the object to base this copy on.

        \remarks
        \remark becouse of nature of action_event message it is do nothing.
    */
    action_event(const action_event & a_evt) :
        message(ACTION_EVENT),
        action_name(a_evt.action_name),
        source_object_id(a_evt.source_object_id),
        observation_radius(a_evt.observation_radius),

        extension_dialect(a_evt.extension_dialect),
        extension_dialect_major_version(a_evt.extension_dialect_major_version),
        extension_dialect_minor_version(a_evt.extension_dialect_minor_version),
        extension_data(a_evt.extension_data){
    }

    /*! Assignment operator.

        \param a_evt the object to base this object on.
        \return a reference to this object, after the assignemt has been made.

    */
    action_event& operator=(const action_event & a_evt) {
        if (this == &a_evt) {
            return *this;
        }

        action_name                    = a_evt.action_name;
        source_object_id               = a_evt.source_object_id;
        observation_radius             = a_evt.observation_radius;
        extension_dialect              = a_evt.extension_dialect;

        extension_dialect_major_version= a_evt.extension_dialect_major_version;
        extension_dialect_minor_version= a_evt.extension_dialect_minor_version;
        extension_data                 = a_evt.extension_data;
        return *this;
    }

    /*! equals.

        \param other the object to equal with this one.
        \return true if objects is equal and false afterwards.
    */
    virtual bool equals(const message & other) const {
        return (this == &other) || ((get_type() == other.get_type()) &&
           
           (extension_dialect == ((action_event&)other).extension_dialect) &&
           (extension_dialect_major_version == 
                ((action_event&)other).extension_dialect_major_version) &&
           (extension_dialect_minor_version == 
                ((action_event&)other).extension_dialect_minor_version) &&
            std::equal(extension_data.begin(),extension_data.end(),
               ((action_event&)other).extension_data.begin()));
    }

    virtual unsigned int size() const {
        return 50 + extension_data.size();
    }

    /*! Serialize this message.

        \param writer the byte writer to serialize this message into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
     */
    virtual unsigned int
    serialize(serialization::byte_writer_base & writer) const {
        
        unsigned int counter = 0;
        
        counter += write<std::wstring>(writer,action_name,20);
        counter += write<uuids::uuid>(writer,source_object_id);
        counter += write<float>(writer,observation_radius);

        counter += write<std::wstring>(writer,extension_dialect,4);
        counter += write<uint8_t>(writer, extension_dialect_major_version);
        counter += write<uint8_t>(writer, extension_dialect_minor_version);
        counter += write<uint32_t>(writer, extension_data.size());
        counter += write_range<uint8_t>(writer,
                                        extension_data.begin(),
                                        extension_data.size());
        return counter;
    }

    /*! De-serialize this message. The contents of this message will be filled
        with the de-serialized values read from the supplied byte reader.

        \param reader the byte reader to serialize this message from by reading
               a sequence of bytes from it
     */
    virtual void
    deserialize(serialization::byte_reader_base & reader, unsigned int ) {

        action_name = read<std::wstring>(reader,20);
        source_object_id = read<uuids::uuid>(reader);
        observation_radius = read<float>(reader);

        extension_dialect = read<std::wstring>(reader,4);
        extension_dialect_major_version = read<uint8_t>(reader);
        extension_dialect_minor_version = read<uint8_t>(reader);

        uint32_t length = serialization::read<uint32_t>(reader);
        extension_data.resize(length);
        read_range<uint8_t>(reader, extension_data.begin(), length);
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param a_evt the message to write.
    \return the output stream after the message has been written to it.
*/
inline
std::ostream& operator<<(std::ostream &os, const action_event & a_evt) {
    os << "action_event[type: " << a_evt.get_type();
    os << ", action_name = ";
    os << a_evt.action_name;
    os << ", source_object_id = ";
    os << a_evt.source_object_id;
    os << ", observation_radius = ";
    os << a_evt.observation_radius;

    os << ", extension_dialect = ";
    os << a_evt.extension_dialect;
    os << ", extension_dialect_major_version = ";
    os << a_evt.extension_dialect_major_version;
    os << ", extension_dialect_minor_version = ";
    os << a_evt.extension_dialect_minor_version;
    os << ", extension_length = ";
    os << a_evt.extension_data.size();
    os << ", extension_data {";
    for (uint32_t i = 0; i < a_evt.extension_data.size(); ++i)        
    {
        os << a_evt.extension_data[i];
        if (i < a_evt.extension_data.size() - 1) os << ", ";
    }
    os << "}";
    os << "]";
    return os;
}

}
}
#endif
