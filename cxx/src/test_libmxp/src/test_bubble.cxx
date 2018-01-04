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

#include <boost/shared_ptr.hpp>
#include <boost/array.hpp>
#include <boost/uuid/uuid.hpp>
#include <boost/test/unit_test.hpp>

#include <mxp/services.h>

#include "test_bubble.h"

namespace mxp {
namespace test {
namespace services {
namespace bubble {

using namespace boost;
using namespace mxp;
using namespace mxp::services;

void test_simple() {
    array<float, 3>     location = {{ 1.0, 2.0, 3.0 }};
    shared_ptr<mxp_object>  object(new mxp_object(
                        uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                        L"Object Name",
                        L"Object Type",
                        uuids::uuid("11111111-2222-3333-4444-555555555555"),
                        location ));

    array<float, 3>     center = {{ 4.0, 5.0, 6.0 }};
    mxp::services::bubble b(uuids::uuid("66666666-7777-8888-9999-000000000000"),
                            L"Bubble Name",
                            center,
                            10.0,
                            5.0);

    // check the empty bubble
    BOOST_CHECK_EQUAL(b.size(), 0u);
    BOOST_CHECK(!b.contains(object->id()));
    BOOST_CHECK(b.objects_begin() == b.objects_end());

    // add the object into the bubble, and check if it is in there
    b.inject(object);

    BOOST_CHECK_EQUAL(b.size(), 1u);
    BOOST_CHECK(b.contains(object->id()));
    BOOST_CHECK(*object == *b.get(object->id()));
    BOOST_CHECK(b.objects_begin() != b.objects_end());
    BOOST_CHECK(*object == **b.objects_begin());

    // remove the object from the bubble, and see that its gone
    b.eject(object->id());

    BOOST_CHECK_EQUAL(b.size(), 0u);
    BOOST_CHECK(!b.contains(object->id()));
    BOOST_CHECK(b.objects_begin() == b.objects_end());
}



}
}
}
}
