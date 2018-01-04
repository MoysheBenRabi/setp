#ifndef MXP_SERVICES_MXP_OBJECT_H
#define MXP_SERVICES_MXP_OBJECT_H

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
#include <iostream>

#include <boost/array.hpp>
#include <boost/smart_ptr.hpp>
#include <boost/uuid/uuid.hpp>


namespace mxp {
namespace services {

using namespace boost;

/*! An object that exists in an MXP bubble.
 */
class mxp_object {
private:
    /*! The unique id of the object. */
    uuids::uuid         _id;

    /*! The human-readable name of the object. */
    std::wstring        _name;

    /*! The type of the object. */
    std::wstring        _type;

    /*! The owner of the object. */
    uuids::uuid         _owner_id;

    /*! The current location of the object. */
    array<float, 3>     _location;


public:
    /*! Constructor.

        \param id the unique id of the object
        \param name the name of the object
        \param type the type of the object
        \param owner_id the owner of the object
        \param location the initial location of the object
    */
    mxp_object(uuids::uuid      id,
               std::wstring     name,
               std::wstring     type,
               uuids::uuid      owner_id,
               array<float, 3>  location)
                   : _id(id),
                    _name(name),
                    _type(type),
                    _owner_id(owner_id),
                    _location(location) {
    }

    /*! Copy constructor.

        \param other the other object to construct this from.
     */
    mxp_object(const mxp_object & other)
        : _id(other._id),
          _name(other._name),
          _type(other._type),
          _owner_id(other._owner_id),
          _location(other._location) {
    }

    /*! Virtual destructor. */
    virtual ~mxp_object() {
    }

    /*! Assignment operator.

        \param other the other object to assign this to.
     */
    mxp_object& operator=(const mxp_object & other) {
        if (this == &other) {
            return *this;
        }

        _id       = other._id;
        _name     = other._name;
        _type     = other._type;
        _owner_id = other._owner_id;
        _location = other._location;
    }

    /*! Compare this MXP object to another one.

        \param other the other object to compare this to.
        \return true of the two objects are the same, false otherwise.
     */
    virtual bool equals(const mxp_object & other) const {
        if (this == &other) {
            return true;
        }

        return _id       == other._id
            && _name     == other._name
            && _type     == other._type
            && _owner_id == other._owner_id
            && _location == other._location;
    }

    /*! Get the id of the object.

        \return the unique id of the object.
     */
    const uuids::uuid & id() const {
        return _id;
    }

    /*! Get the name of the object.

        \return the name of the object.
     */
    const std::wstring & name() const {
        return _name;
    }

    /*! Get the type of the object.

        \return the type of the object.
     */
    const std::wstring & type() const {
        return _type;
    }

    /*! Get the objects owners id.

        \return the id of the owner of the object.
     */
    const uuids::uuid & owner_id() const {
        return _owner_id;
    }

    /*! Get the location of the object.

        \return the location of the object.
     */
    const array<float, 3> & location() const {
        return _location;
    }

    /*! Set the location of the object.

        \param location the new location of the object.
     */
    void location(array<float, 3> location) {
        _location = location;
    }
};

/*! Compare two MXP objects.

    \param lhs one of the objects to compare
    \param rhs the other object to compare
    \return true if the two objects are equal, false otherwise.
 */
inline bool operator==(const mxp_object & lhs, const mxp_object & rhs) {
    return lhs.equals(rhs);
}

/*! Write a human-readable representation of an MXP object to an output stream.

    \param os the output stream to write to.
    \param object the MXP object to write to
    \return the output stream after the object has been written to it.
*/
inline
std::wostream& operator<<(std::wostream &os, const mxp_object & object) {
    os << "mxp_object[id: " << object.id()
       << ", name: " << object.name()
       << ", type: " << object.type()
       << "]";

    return os;
}



}
}

#endif
