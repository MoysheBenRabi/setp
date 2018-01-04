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

#include <boost/test/unit_test.hpp>

#include "mxp/message.h"

#include "test_helpers.h"
#include "reference_messages.h"
#include "test_message_serialization.h"

namespace mxp {
namespace test {
namespace message {
namespace serialization {

using namespace boost;
using namespace mxp;
using namespace mxp::test::iot;

void test_acknowledge() {
    shared_ptr<mxp::message::acknowledge> ack = generate_acknowledge();
    BOOST_CHECK_EQUAL(ack->get_type(), mxp::message::ACKNOWLEDGE);
    check_two_way_serialization(ack);
}

void test_keepalive() {
    shared_ptr<mxp::message::keepalive> ka = generate_keepalive();
    BOOST_CHECK_EQUAL(ka->get_type(), mxp::message::KEEPALIVE);
    check_two_way_serialization(ka);
}

void test_throttle() {
    shared_ptr<mxp::message::throttle> th = generate_throttle();
    BOOST_CHECK_EQUAL(th->get_type(), mxp::message::THROTTLE);
    check_two_way_serialization(th);
}

void test_challenge_rq() {
    shared_ptr<mxp::message::challenge_rq> cr = generate_challenge_rq();
    BOOST_CHECK_EQUAL(cr->get_type(), mxp::message::CHALLENGE_RQ);
    check_two_way_serialization(cr);
}

void test_challenge_rsp() {
    shared_ptr<mxp::message::challenge_rsp> cr = generate_challenge_rsp();
    BOOST_CHECK_EQUAL(cr->get_type(), mxp::message::CHALLENGE_RSP);
    check_two_way_serialization(cr);
}

void test_join_rq() {
    shared_ptr<mxp::message::join_rq> jr = generate_join_rq();
    BOOST_CHECK_EQUAL(jr->get_type(), mxp::message::JOIN_RQ);
    check_two_way_serialization(jr);
}

void test_join_rsp() {
    shared_ptr<mxp::message::join_rsp> jr = generate_join_rsp();
    BOOST_CHECK_EQUAL(jr->get_type(), mxp::message::JOIN_RSP);
    check_two_way_serialization(jr);
}

void test_leave_rq() {
    shared_ptr<mxp::message::leave_rq> lr = generate_leave_rq();
    BOOST_CHECK_EQUAL(lr->get_type(), mxp::message::LEAVE_RQ);
    check_two_way_serialization(lr);
}

void test_leave_rsp() {
    shared_ptr<mxp::message::leave_rsp> lr = generate_leave_rsp();
    BOOST_CHECK_EQUAL(lr->get_type(), mxp::message::LEAVE_RSP);
    check_two_way_serialization(lr);
}

void test_inject_rq() {
    shared_ptr<mxp::message::inject_rq> ir = generate_inject_rq();
    BOOST_CHECK_EQUAL(ir->get_type(), mxp::message::INJECT_RQ);
    check_two_way_serialization(ir);
}

void test_inject_rsp() {
    shared_ptr<mxp::message::inject_rsp> ir = generate_inject_rsp();
    BOOST_CHECK_EQUAL(ir->get_type(), mxp::message::INJECT_RSP);
    check_two_way_serialization(ir);
}

void test_modify_rq() {
    shared_ptr<mxp::message::modify_rq> mr = generate_modify_rq();
    BOOST_CHECK_EQUAL(mr->get_type(), mxp::message::MODIFY_RQ);
    check_two_way_serialization(mr);
}

void test_modify_rsp() {
    shared_ptr<mxp::message::modify_rsp> mr = generate_modify_rsp();
    BOOST_CHECK_EQUAL(mr->get_type(), mxp::message::MODIFY_RSP);
    check_two_way_serialization(mr);
}

void test_eject_rq() {
    shared_ptr<mxp::message::eject_rq> er = generate_eject_rq();
    BOOST_CHECK_EQUAL(er->get_type(), mxp::message::EJECT_RQ);
    check_two_way_serialization(er);
}

void test_eject_rsp() {
    shared_ptr<mxp::message::eject_rsp> er = generate_eject_rsp();
    BOOST_CHECK_EQUAL(er->get_type(), mxp::message::EJECT_RSP);
    check_two_way_serialization(er);
}

void test_interact_rq() {
    shared_ptr<mxp::message::interact_rq> ir = generate_interact_rq();
    BOOST_CHECK_EQUAL(ir->get_type(), mxp::message::INTERACT_RQ);
    check_two_way_serialization(ir);
}

void test_interact_rsp() {
    shared_ptr<mxp::message::interact_rsp> ir = generate_interact_rsp();
    BOOST_CHECK_EQUAL(ir->get_type(), mxp::message::INTERACT_RSP);
    check_two_way_serialization(ir);
}

void test_examine_rq() {
    shared_ptr<mxp::message::examine_rq> er = generate_examine_rq();
    BOOST_CHECK_EQUAL(er->get_type(), mxp::message::EXAMINE_RQ);
    check_two_way_serialization(er);
}

void test_examine_rsp() {
    shared_ptr<mxp::message::examine_rsp> er = generate_examine_rsp();
    BOOST_CHECK_EQUAL(er->get_type(), mxp::message::EXAMINE_RSP);
    check_two_way_serialization(er);
}

void test_attach_rq() {
    shared_ptr<mxp::message::attach_rq> ar = generate_attach_rq();
    BOOST_CHECK_EQUAL(ar->get_type(), mxp::message::ATTACH_RQ);
    check_two_way_serialization(ar);
}

void test_attach_rsp() {
    shared_ptr<mxp::message::attach_rsp> ar = generate_attach_rsp();
    BOOST_CHECK_EQUAL(ar->get_type(), mxp::message::ATTACH_RSP);
    check_two_way_serialization(ar);
}

void test_detach_rq() {
    shared_ptr<mxp::message::detach_rq> dr = generate_detach_rq();
    BOOST_CHECK_EQUAL(dr->get_type(), mxp::message::DETACH_RQ);
    check_two_way_serialization(dr);
}

void test_detach_rsp() {
    shared_ptr<mxp::message::detach_rsp> dr = generate_detach_rsp();
    BOOST_CHECK_EQUAL(dr->get_type(), mxp::message::DETACH_RSP);
    check_two_way_serialization(dr);
}

void test_handover_rq() {
    shared_ptr<mxp::message::handover_rq> hr = generate_handover_rq();
    BOOST_CHECK_EQUAL(hr->get_type(), mxp::message::HANDOVER_RQ);
    check_two_way_serialization(hr);
}

void test_handover_rsp() {
    shared_ptr<mxp::message::handover_rsp> hr = generate_handover_rsp();
    BOOST_CHECK_EQUAL(hr->get_type(), mxp::message::HANDOVER_RSP);
    check_two_way_serialization(hr);
}

void test_list_bubbles_rq() {
    shared_ptr<mxp::message::list_bubbles_rq> lbr = generate_list_bubbles_rq();
    BOOST_CHECK_EQUAL(lbr->get_type(), mxp::message::LIST_BUBBLES_RQ);
    check_two_way_serialization(lbr);
}

void test_list_bubbles_rsp() {
    shared_ptr<mxp::message::list_bubbles_rsp> lbr = generate_list_bubbles_rsp();
    BOOST_CHECK_EQUAL(lbr->get_type(), mxp::message::LIST_BUBBLES_RSP);
    check_two_way_serialization(lbr);
}

void test_perception_event() {
    shared_ptr<mxp::message::perception_event> pe= generate_perception_event();
    BOOST_CHECK_EQUAL(pe->get_type(), mxp::message::PERCEPTION_EVENT);
    check_two_way_serialization(pe);
}

void test_movement_event() {
    shared_ptr<mxp::message::movement_event> me = generate_movement_event();
    BOOST_CHECK_EQUAL(me->get_type(), mxp::message::MOVEMENT_EVENT);
    check_two_way_serialization(me);
}

void test_disappearance_event() {
    shared_ptr<mxp::message::disappearance_event> 
        de = generate_disappearance_event();
    BOOST_CHECK_EQUAL(de->get_type(), mxp::message::DISAPPEARANCE_EVENT);
    check_two_way_serialization(de);
}

void test_handover_event() {
    shared_ptr<mxp::message::handover_event> he = generate_handover_event();
    BOOST_CHECK_EQUAL(he->get_type(), mxp::message::HANDOVER_EVENT);
    check_two_way_serialization(he);
}

void test_action_event() {
    shared_ptr<mxp::message::action_event> ae = generate_action_event();
    BOOST_CHECK_EQUAL(ae->get_type(), mxp::message::ACTION_EVENT);
    check_two_way_serialization(ae);
}

void test_synchronization_begin_event() {
    shared_ptr<mxp::message::synchronization_begin_event> 
        sbe = generate_synchronization_begin_event();
    BOOST_CHECK_EQUAL(sbe->get_type(), 
        mxp::message::SYNCHRONIZATION_BEGIN_EVENT);
    check_two_way_serialization(sbe);
}

void test_synchronization_end_event() {
    shared_ptr<mxp::message::synchronization_end_event> 
        see = generate_synchronization_end_event();
    BOOST_CHECK_EQUAL(see->get_type(), 
        mxp::message::SYNCHRONIZATION_END_EVENT);
    check_two_way_serialization(see);
}

}
}
}
}
