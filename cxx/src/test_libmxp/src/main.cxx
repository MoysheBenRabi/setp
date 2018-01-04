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

#include <boost/test/included/unit_test.hpp>

#include "test_deserialization.h"
#include "test_serialization.h"
#include "test_two_way_serialization.h"

#include "test_message_serialization.h"

#include "test_message_frame.h"
#include "test_packet.h"
#include "test_packetizer.h"

#include "test_session_store.h"
#include "test_connection.h"
#include "test_communication.h"

#include "test_iot_zip.h"

#include "test_bubble.h"

using namespace boost::unit_test;

static void
add_serialization_tests() {
    using namespace mxp::test::serialization;

    test_suite* tsDeserialization = BOOST_TEST_SUITE("deserialization");
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_byte));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_ubyte));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_short));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_ushort));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_int));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_uint));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_long));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_ulong));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_float));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_double));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_array));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_date));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_bytes));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_string));
    tsDeserialization->add(BOOST_TEST_CASE(
                                        &deserialization::test_string_fixed));
    tsDeserialization->add(BOOST_TEST_CASE(
                                    &deserialization::test_string_bad_utf8));
    tsDeserialization->add(BOOST_TEST_CASE(&deserialization::test_uuid));
    framework::master_test_suite().add(tsDeserialization);

    test_suite* tsSerialization = BOOST_TEST_SUITE("serialization");
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_byte));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_ubyte));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_short));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_ushort));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_int));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_uint));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_long));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_ulong));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_float));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_double));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_array));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_date));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_bytes));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_string));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_string_fixed));
    tsSerialization->add(BOOST_TEST_CASE(
                                &serialization::test_string_fixed_truncate));
    tsSerialization->add(BOOST_TEST_CASE(
                        &serialization::test_string_fixed_truncate_partial));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_uuid));
    framework::master_test_suite().add(tsSerialization);

    test_suite* ts2wSerialization = BOOST_TEST_SUITE("two_way_serialization");
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_byte));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_ubyte));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_short));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_ushort));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_int));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_uint));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_long));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_ulong));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_float));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_double));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_array));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_date));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_bytes));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_string));
    ts2wSerialization->add(BOOST_TEST_CASE(
                                    &twserialization::test_string_fixed));
    ts2wSerialization->add(BOOST_TEST_CASE(&twserialization::test_uuid));
    framework::master_test_suite().add(ts2wSerialization);
}

static void
add_message_tests() {
    using namespace mxp::test::message;

    test_suite* tsSerialization = BOOST_TEST_SUITE("message_serialization");
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_acknowledge));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_keepalive));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_throttle));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_challenge_rq));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_challenge_rsp));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_join_rq));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_join_rsp));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_leave_rq));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_leave_rsp));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_inject_rq));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_inject_rsp));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_modify_rq));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_modify_rsp));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_eject_rq));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_eject_rsp));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_interact_rq));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_interact_rsp));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_examine_rq));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_examine_rsp));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_attach_rq));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_attach_rsp));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_detach_rq));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_detach_rsp));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_handover_rq));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_handover_rsp));
    tsSerialization->add(BOOST_TEST_CASE(
        &serialization::test_list_bubbles_rq));
    tsSerialization->add(BOOST_TEST_CASE(
        &serialization::test_list_bubbles_rsp));
    tsSerialization->add(BOOST_TEST_CASE(
        &serialization::test_perception_event));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_movement_event));
    tsSerialization->add(BOOST_TEST_CASE(
        &serialization::test_disappearance_event));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_handover_event));
    tsSerialization->add(BOOST_TEST_CASE(&serialization::test_action_event));
    tsSerialization->add(BOOST_TEST_CASE(
        &serialization::test_synchronization_begin_event));
    tsSerialization->add(BOOST_TEST_CASE(
        &serialization::test_synchronization_end_event));
    framework::master_test_suite().add(tsSerialization);
}

static void
add_packet_tests() {
    using namespace mxp::test::packet;

    test_suite* tsPacket = BOOST_TEST_SUITE("packet");
    tsPacket->add(BOOST_TEST_CASE(&message_frame::test_too_big_data));
    tsPacket->add(BOOST_TEST_CASE(&message_frame::test_serialization));
    tsPacket->add(BOOST_TEST_CASE(&packet::test_too_much_data));
    tsPacket->add(BOOST_TEST_CASE(&packet::test_serialization));
    tsPacket->add(BOOST_TEST_CASE(&packetizer::test_single_frame));
    tsPacket->add(BOOST_TEST_CASE(&packetizer::test_message_packet_round_trip));
    framework::master_test_suite().add(tsPacket);
}

static void
add_net_tests() {
    using namespace mxp::test::net;

    test_suite* tsNet = BOOST_TEST_SUITE("net");
    tsNet->add(BOOST_TEST_CASE(&session_store::test_simple));
    tsNet->add(BOOST_TEST_CASE(&session_store::test_update));
    tsNet->add(BOOST_TEST_CASE(&session_store::test_iterator));
    tsNet->add(BOOST_TEST_CASE(&connection::test_single_packet_roundtrip));
    tsNet->add(BOOST_TEST_CASE(&connection::test_multiple_packet_roundtrip));
    tsNet->add(BOOST_TEST_CASE(&communication::test_connection));
    tsNet->add(BOOST_TEST_CASE(&communication::test_bad_connection));
    tsNet->add(BOOST_TEST_CASE(&communication::test_single_message_roundtrip));
    tsNet->add(BOOST_TEST_CASE(&communication::test_keepalive));
    framework::master_test_suite().add(tsNet);
}

static void
add_iot_tests() {
    using namespace mxp::test::iot;

    test_suite* tsIot = BOOST_TEST_SUITE("iot");
    tsIot->add(BOOST_TEST_CASE(&zip::test_compare_to_zip));
    tsIot->add(BOOST_TEST_CASE(&zip::test_generate_zip));
    framework::master_test_suite().add(tsIot);
}

static void
add_services_tests() {
    using namespace mxp::test::services;

    test_suite* tsServices = BOOST_TEST_SUITE("services");
    tsServices->add(BOOST_TEST_CASE(&bubble::test_simple));
    framework::master_test_suite().add(tsServices);
}

test_suite*
init_unit_test_suite(int , char* []) {
    add_serialization_tests();
    add_message_tests();
    add_packet_tests();
    add_net_tests();
    add_iot_tests();
    add_services_tests();

    return 0;
}

