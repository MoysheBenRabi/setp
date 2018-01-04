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

#include <stdexcept>
#include <boost/smart_ptr.hpp>

#include <mxp/message/message.h>
#include <mxp/message/acknowledge.h>
#include <mxp/message/throttle.h>
#include <mxp/message/action_event.h>
#include <mxp/message/attach_rq.h>
#include <mxp/message/attach_rsp.h>
#include <mxp/message/challenge_rq.h>
#include <mxp/message/challenge_rsp.h>
#include <mxp/message/detach_rq.h>
#include <mxp/message/detach_rsp.h>
#include <mxp/message/disappearance_event.h>
#include <mxp/message/eject_rq.h>
#include <mxp/message/eject_rsp.h>
#include <mxp/message/examine_rq.h>
#include <mxp/message/examine_rsp.h>
#include <mxp/message/handover_event.h>
#include <mxp/message/handover_rq.h>
#include <mxp/message/handover_rsp.h>
#include <mxp/message/inject_rq.h>
#include <mxp/message/inject_rsp.h>
#include <mxp/message/interact_rq.h>
#include <mxp/message/interact_rsp.h>
#include <mxp/message/join_rq.h>
#include <mxp/message/join_rsp.h>
#include <mxp/message/keepalive.h>
#include <mxp/message/leave_rq.h>
#include <mxp/message/leave_rsp.h>
#include <mxp/message/list_bubbles_rq.h>
#include <mxp/message/list_bubbles_rsp.h>
#include <mxp/message/modify_rq.h>
#include <mxp/message/modify_rsp.h>
#include <mxp/message/movement_event.h>
#include <mxp/message/perception_event.h>
#include <mxp/message/synchronization_begin_event.h>
#include <mxp/message/synchronization_end_event.h>

#include <mxp/message/for_type.h>

namespace mxp {
namespace message {

using namespace boost;

/*! Create a specific message based on the type of the message.

    \param type the type of the message
    \return a pointer to a message object of the corresponding type
    \throws std::invalid_argument if the supplied message type is invalid.
 */
shared_ptr<message> for_type(unsigned int type) {
    switch (type) {
        case ACKNOWLEDGE:
            return shared_ptr<acknowledge>(new acknowledge());
        case KEEPALIVE:
            return shared_ptr<keepalive>(new keepalive());
        case THROTTLE:
            return shared_ptr<throttle>(new throttle());
        case CHALLENGE_RQ:
            return shared_ptr<challenge_rq>(new challenge_rq());
        case CHALLENGE_RSP:
            return shared_ptr<challenge_rsp>(new challenge_rsp());
        case JOIN_RQ:
            return shared_ptr<join_rq>(new join_rq());
        case JOIN_RSP:
            return shared_ptr<join_rsp>(new join_rsp());
        case LEAVE_RQ:
            return shared_ptr<leave_rq>(new leave_rq());
        case LEAVE_RSP:
            return shared_ptr<leave_rsp>(new leave_rsp());
        case INJECT_RQ:
            return shared_ptr<inject_rq>(new inject_rq());
        case INJECT_RSP:
            return shared_ptr<inject_rsp>(new inject_rsp());
        case MODIFY_RQ:
            return shared_ptr<modify_rq>(new modify_rq());
        case MODIFY_RSP:
            return shared_ptr<modify_rsp>(new modify_rsp());
        case EJECT_RQ:
            return shared_ptr<eject_rq>(new eject_rq());
        case EJECT_RSP:
            return shared_ptr<eject_rsp>(new eject_rsp());
        case INTERACT_RQ:
            return shared_ptr<interact_rq>(new interact_rq());
        case INTERACT_RSP:
            return shared_ptr<interact_rsp>(new interact_rsp());
        case EXAMINE_RQ:
            return shared_ptr<examine_rq>(new examine_rq());
        case EXAMINE_RSP:
            return shared_ptr<examine_rsp>(new examine_rsp());
        case ATTACH_RQ:
            return shared_ptr<attach_rq>(new attach_rq());
        case ATTACH_RSP:
            return shared_ptr<attach_rsp>(new attach_rsp());
        case DETACH_RQ:
            return shared_ptr<detach_rq>(new detach_rq());
        case DETACH_RSP:
            return shared_ptr<detach_rsp>(new detach_rsp());
        case HANDOVER_RQ:
            return shared_ptr<handover_rq>(new handover_rq());
        case HANDOVER_RSP:
            return shared_ptr<handover_rsp>(new handover_rsp());
        case LIST_BUBBLES_RQ:
            return shared_ptr<list_bubbles_rq>(new list_bubbles_rq());
        case LIST_BUBBLES_RSP:
            return shared_ptr<list_bubbles_rsp>(new list_bubbles_rsp());
        case PERCEPTION_EVENT:
            return shared_ptr<perception_event>(new perception_event());
        case MOVEMENT_EVENT:
            return shared_ptr<movement_event>(new movement_event());
        case DISAPPEARANCE_EVENT:
            return shared_ptr<disappearance_event>(new disappearance_event());
        case HANDOVER_EVENT:
            return shared_ptr<handover_event>(new handover_event());
        case ACTION_EVENT:
            return shared_ptr<action_event>(new action_event());
        case SYNCHRONIZATION_BEGIN_EVENT:
            return shared_ptr<synchronization_begin_event>(new synchronization_begin_event());
        case SYNCHRONIZATION_END_EVENT:
            return shared_ptr<synchronization_end_event>(new synchronization_end_event());

        default:
            throw std::invalid_argument(
                                "message of the supplied type does not exist");
    }
}

}
}
