#ifndef MXP_SERVICES_BUBBLE_H
#define MXP_SERVICES_BUBBLE_H

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

#include <map>
#include <string>
#include <stdexcept>

#include <boost/array.hpp>
#include <boost/smart_ptr.hpp>
#include <boost/function.hpp>
#include <boost/bind.hpp>
#include <boost/iterator.hpp>
#include <boost/iterator/transform_iterator.hpp>
#include <boost/uuid/uuid.hpp>

#include "mxp_object.h"


namespace mxp {
namespace services {

using namespace boost;

/*! Class representing an MXP bubble.
 */
class bubble {
private:
    typedef std::map<uuids::uuid, shared_ptr<mxp_object> > mxp_object_map_type;

    typedef function<mxp_object_map_type::value_type::second_type
                        (const mxp_object_map_type::value_type &)>
                                                    mxp_object_map_get_value;

    typedef transform_iterator<mxp_object_map_get_value,
                               mxp_object_map_type::const_iterator>
                                                            mxp_object_iterator;

    /*! The unique id of the bubble. */
    uuids::uuid     _id;

    /*! The human-readable name of the bubble. */
    std::wstring    _name;

    /*! The center of the bubble. */
    array<float, 3> _center;

    /*! The range / radius of the bubble. */
    float           _range;

    /*! The range of perception within the bubble. */
    float           _perception_range;

    /*! A map of MXP objects - keyed by their unique id. */
    std::map<uuids::uuid, shared_ptr<mxp_object> >      _objects;


public:
    /*! Constructor.

        \param id the unique id of the bubble
        \param name the name of the bubble
        \param center the center of the bubble
        \param range the range / radius of the bubble
        \param perception_range the range of perception within the bubble
     */
    bubble(const uuids::uuid      & id,
           const std::wstring     & name,
           const array<float, 3>  & center,
           float                    range,
           float                    perception_range)
               : _id(id),
                 _name(name),
                 _center(center),
                 _range(range),
                 _perception_range(perception_range) {
    }

    /*! Return the id of the bubble.

        \return the id of the bubble.
     */
    const uuids::uuid & id() const {
        return _id;
    }

    /*! Return the number of objects in this bubble.

        \return the number of objects in this bubble.
     */
    unsigned int size() const {
        return _objects.size();
    }

    /*! Tell if an object exists in the bubble.

        \param id the id of the object to check for.
        \return true if the object is contained in the bubble, false otherwise.
     */
    bool contains(const uuids::uuid & id) const {
        return _objects.find(id) != _objects.end();
    }

    /*! Get a specific object from the bubble.

        \param id the id of the object to get
        \return a reference to the object in question.
        \throws std::invalid_argument if the object with the specified id
                was not found.
     */
    shared_ptr<const mxp_object> get(const uuids::uuid & id) const {
        mxp_object_map_type::const_iterator it = _objects.find(id);

        if (it == _objects.end()) {
            throw std::invalid_argument("no such object id");
        }

        return it->second;
    }

    /*! Inject an MXP object into the bubble. If an object with the same unique
        id already exists in the bubble, it is replaced with the new one.

        \param object the MXP object to inject.
     */
    void inject(shared_ptr<mxp_object>  object) {
        _objects[object->id()] = object;
    }

    /*! Eject (remove) an MXP object from the bubble.

        \param id the id of the object to eject.
     */
    void eject(const uuids::uuid & id) {
        if (contains(id)) {
            _objects.erase(id);
        }
    }

    /*! Return a begin iterator for the MXP objects contained in the bubble.

        \return an iterator of shared_ptr<mxp_object> pointers,
                pointing to the first MXP object in the bubble.
        \see #objects_end()
     */
    mxp_object_iterator objects_begin() const {
        mxp_object_map_get_value momgv =
                            bind(&mxp_object_map_type::value_type::second, _1);

        return make_transform_iterator(_objects.begin(), momgv);
    }

    /*! Return an end iterator for the MXP objects contained in the bubble.

        \return an iterator of shared_ptr<mxp_object> pointers,
                pointing to one beyond the last MXP object in the bubble.
        \see #objects_begin()
     */
    mxp_object_iterator objects_end() const {
        mxp_object_map_get_value momgv =
                            bind(&mxp_object_map_type::value_type::second, _1);

        return make_transform_iterator(_objects.end(), momgv);
    }
};

}
}

#endif
