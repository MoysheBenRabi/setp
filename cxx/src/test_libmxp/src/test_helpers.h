#ifndef TEST_HELPERS_H
#define TEST_HELPERS_H

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
#include <boost/shared_ptr.hpp>
#include <boost/test/unit_test.hpp>

#include <mxp/serialization.h>
#include <mxp/message.h>

namespace mxp {
namespace test {

using namespace boost;
using namespace mxp;

/*! \file This file contains test helper functions.
 */

/*! Perform a two-way serialization check.
    Serialize the provided object into a byte array, then de-serialize it,
    and check the two objects for equality using a boost unit test check.
    if the equality check fails, a boost unit test error is generated.

    \tparam T the class to serialize and deserialize
    \param object the object to serialize and deserialize and then check if the
           deserialized object is the same as the original one
 */
template<typename T>
void check_two_way_serialization(const T & object) {
    std::vector<uint8_t> bytes;
    bytes.reserve(object.size());
    std::back_insert_iterator<std::vector<uint8_t> > it =
                                                    std::back_inserter(bytes);

    unsigned int count = object.serialize(it);

    BOOST_CHECK_EQUAL(count, object.size());

    T other;
    std::vector<uint8_t>::iterator iit = bytes.begin();
    other.deserialize(iit, count);

    BOOST_CHECK_EQUAL(object, other);
}

/*! Perform a two-way serialization check.
    Serialize the provided object into a byte array, then de-serialize it,
    and check the two objects for equality using a boost unit test check.
    if the equality check fails, a boost unit test error is generated.

    \tparam T the class to serialize and deserialize
    \param object pointer to the object to serialize and deserialize and then
           check if the deserialized object is the same as the original one
 */
template<typename T>
void check_two_way_serialization(boost::shared_ptr<T> object) {
    std::vector<uint8_t> bytes;
    bytes.reserve(object->size());
    std::back_insert_iterator<std::vector<uint8_t> > it =
                                                    std::back_inserter(bytes);

    unsigned int count = mxp::message::serialize(*object, it);

    BOOST_CHECK_EQUAL(count, object->size());

    T other;
    std::vector<uint8_t>::iterator iit = bytes.begin();
    mxp::message::deserialize(other, iit, count);

    BOOST_CHECK_EQUAL(*object, other);
}


}
}

#endif // TEST_HELPERS_H
