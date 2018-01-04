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
#include <boost/test/unit_test.hpp>

#include "mxp/packet/message_frame.h"

#include "test_helpers.h"
#include "test_message_frame.h"

namespace mxp {
namespace test {
namespace packet {
namespace message_frame {

using namespace boost;

void test_too_big_data() {
    mxp::packet::message_frame  frame;
    uint8_t                     data[300];

    for (unsigned int i = 0; i < 300; ++i) {
        data[i] = (uint8_t) (i % 256);
    }

    // setting this value should go through fine
    frame.frame_data(data, 128);
    BOOST_CHECK_EQUAL(128, frame.frame_data_size());
    BOOST_CHECK_EQUAL_COLLECTIONS(data, data + 128,
                                  frame.frame_data(),
                                  frame.frame_data() + frame.frame_data_size());

    // setting too much data will result in truncation
    frame.frame_data(data, (uint8_t) 300);
    BOOST_CHECK_EQUAL(300 % 256, frame.frame_data_size());
    BOOST_CHECK_EQUAL_COLLECTIONS(data, data + (300 % 256),
                                  frame.frame_data(),
                                  frame.frame_data() + frame.frame_data_size());
}

void test_serialization() {
    mxp::packet::message_frame  frame;
    uint8_t                     data[200];

    for (unsigned int i = 0; i < 200; ++i) {
        data[i] = (uint8_t) i;
    }

    frame.type        = 1;
    frame.message_id  = 2;
    frame.frame_count = 1;
    frame.frame_index = 0;
    frame.frame_data(data, 128);

    check_two_way_serialization(frame);
}


}
}
}
}
