#ifndef MXP_TEST_MESSAGE_SERIALIZATION_H
#define MXP_TEST_MESSAGE_SERIALIZATION_H

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

namespace mxp {
namespace test {
namespace message {
namespace serialization {

void test_acknowledge();
void test_keepalive();
void test_throttle();
void test_challenge_rq();
void test_challenge_rsp();
void test_join_rq();
void test_join_rsp();
void test_leave_rq();
void test_leave_rsp();
void test_inject_rq();
void test_inject_rsp();
void test_modify_rq();
void test_modify_rsp();
void test_eject_rq();
void test_eject_rsp();
void test_interact_rq();
void test_interact_rsp();
void test_examine_rq();
void test_examine_rsp();
void test_attach_rq();
void test_attach_rsp();
void test_detach_rq();
void test_detach_rsp();
void test_handover_rq();
void test_handover_rsp();
void test_list_bubbles_rq();
void test_list_bubbles_rsp();
void test_perception_event();
void test_movement_event();
void test_disappearance_event();
void test_handover_event();
void test_action_event();
void test_synchronization_begin_event();
void test_synchronization_end_event();

}
}
}
}

#endif
