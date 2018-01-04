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
#include <boost/test/unit_test.hpp>

#include <mxp/packet/packet.h>

#include "test_helpers.h"
#include "test_packet.h"

namespace mxp {
namespace test {
namespace packet {
namespace packet {

using namespace boost;

mxp::packet::packet generate_test_packet() {
    std::vector<mxp::packet::message_frame>  frames(5);

    for (unsigned int i = 0; i < 5; ++i) {
        mxp::packet::message_frame  frame;
        uint8_t                     data[255];

        for (unsigned int j = 0; j < 255; ++j) {
            data[j] = (uint8_t) j;
        }
        frame.frame_data(data, 255);

        frames.push_back(frame);
    }

    mxp::packet::packet p;

    p.session_id      = 1234;
    p.packet_id       = 4567;
    p.first_send_time = posix_time::second_clock::universal_time();
    p.guaranteed      = 0;
    p.resend_count    = 0;
    p.message_frames(frames.begin(), frames.end());

    return p;
}

void test_too_much_data() {
    std::vector<mxp::packet::message_frame>  frames(6);

    for (unsigned int i = 0; i < 6; ++i) {
        mxp::packet::message_frame  frame;
        uint8_t                     data[255];

        for (unsigned int j = 0; j < 255; ++j) {
            data[j] = (uint8_t) j;
        }
        frame.frame_data(data, 255);

        frames.push_back(frame);
    }

    mxp::packet::packet      p;

    // add 5 frames of 265 bytes each - this should work out fine, as the
    // total packet payload will be 1325 bytes, which if below the maximum
    // payload size
    p.message_frames(frames.begin(), frames.begin() + 5);
    BOOST_CHECK_EQUAL(p.message_frames_end() - p.message_frames_begin(), 5);

    // now try to add 6 frames, which would exceed the maximum payload size
    BOOST_CHECK_THROW(p.message_frames(frames.begin(), frames.end()),
                      std::invalid_argument);
}

void test_serialization() {
    check_two_way_serialization(generate_test_packet());
}


}
}
}
}
