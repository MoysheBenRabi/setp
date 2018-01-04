#ifndef MXP_REFERENCE_MESSAGES_H
#define MXP_REFERENCE_MESSAGES_H

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

#include <mxp/message.h>

namespace mxp {
namespace test {
namespace iot {

using namespace boost;
using namespace mxp;

shared_ptr<message::acknowledge>     generate_acknowledge();
shared_ptr<message::keepalive>       generate_keepalive();
shared_ptr<message::throttle>        generate_throttle();
shared_ptr<message::challenge_rq>    generate_challenge_rq();
shared_ptr<message::challenge_rsp>   generate_challenge_rsp();
shared_ptr<message::join_rq>         generate_join_rq();
shared_ptr<message::join_rsp>        generate_join_rsp();
shared_ptr<message::leave_rq>        generate_leave_rq();
shared_ptr<message::leave_rsp>       generate_leave_rsp();
shared_ptr<message::inject_rq>       generate_inject_rq();
shared_ptr<message::inject_rsp>      generate_inject_rsp();
shared_ptr<message::modify_rq>       generate_modify_rq();
shared_ptr<message::modify_rsp>      generate_modify_rsp();
shared_ptr<message::eject_rq>        generate_eject_rq();
shared_ptr<message::eject_rsp>       generate_eject_rsp();
shared_ptr<message::interact_rq>     generate_interact_rq();
shared_ptr<message::interact_rsp>    generate_interact_rsp();
shared_ptr<message::examine_rq>      generate_examine_rq();
shared_ptr<message::examine_rsp>     generate_examine_rsp();
shared_ptr<message::attach_rq>       generate_attach_rq();
shared_ptr<message::attach_rsp>      generate_attach_rsp();
shared_ptr<message::detach_rq>       generate_detach_rq();
shared_ptr<message::detach_rsp>      generate_detach_rsp();
shared_ptr<message::handover_rq>     generate_handover_rq();
shared_ptr<message::handover_rsp>    generate_handover_rsp();
shared_ptr<message::list_bubbles_rq> generate_list_bubbles_rq();
shared_ptr<message::list_bubbles_rsp> generate_list_bubbles_rsp();
shared_ptr<message::perception_event> generate_perception_event();
shared_ptr<message::movement_event> generate_movement_event();
shared_ptr<message::disappearance_event> generate_disappearance_event();
shared_ptr<message::action_event> generate_action_event();
shared_ptr<message::handover_event> generate_handover_event();
shared_ptr<message::synchronization_begin_event> 
generate_synchronization_begin_event();
shared_ptr<message::synchronization_end_event> 
generate_synchronization_end_event();


}
}
}

#endif

