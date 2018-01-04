# Copyright (c) 2009-2010 Tyrell Corporation & Moyshe Ben Rabi.
#
# The contents of this file are subject to the Mozilla Public License
# Version 1.1 (the "License"); you may not use this file except in
# compliance with the License. You may obtain a copy of the License at
# http://www.mozilla.org/MPL/
#
# Software distributed under the License is distributed on an "AS IS"
# basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
# License for the specific language governing rights and limitations
# under the License.
#
# The Original Code is an implementation of the Metaverse eXchange Protocol.
#
# The Initial Developer of the Original Code is Akos Maroy and Moyshe Ben Rabi.
# All Rights Reserved.
#
# Contributor(s): Akos Maroy and Moyshe Ben Rabi.
#
# Alternatively, the contents of this file may be used under the terms
# of the Affero General Public License (the  "AGPL"), in which case the
# provisions of the AGPL are applicable instead of those
# above. If you wish to allow use of your version of this file only
# under the terms of the AGPL and not to allow others to use
# your version of this file under the MPL, indicate your decision by
# deleting the provisions above and replace them with the notice and
# other provisions required by the AGPL. If you do not delete
# the provisions above, a recipient may use your version of this file
# under either the MPL or the AGPL.
TARGET = mxp
VERSION = 1.0.0
HEADERS += include/mxp/serialization/serialize.h \
    include/mxp/serialization/deserialize.h \
    include/mxp/serialization.h \
    include/mxp/message/message.h \
    include/mxp/message/acknowledge.h \
    include/mxp/message/modify_rq.h \
    include/mxp/message/program_fragment.h \
    include/mxp/message/detach_rq.h \
    include/mxp/message/list_bubbles_rsp.h \
    include/mxp/message/inject_rsp.h \
    include/mxp/message/interaction_fragment.h \
    include/mxp/message/throttle.h \
    include/mxp/message/synchronization_begin_event.h \
    include/mxp/message/handover_event.h \
    include/mxp/message/handover_rq.h \
    include/mxp/message/examine_rsp.h \
    include/mxp/message/list_bubbles_rq.h \
    include/mxp/message/detach_rsp.h \
    include/mxp/message/join_rsp.h \
    include/mxp/message/challenge_rq.h \
    include/mxp/message/object_fragment.h \
    include/mxp/message/handover_rsp.h \
    include/mxp/message/movement_event.h \
    include/mxp/message/eject_rq.h \
    include/mxp/message/interact_rsp.h \
    include/mxp/message/leave_rq.h \
    include/mxp/message/disappearance_event.h \
    include/mxp/message/join_rq.h \
    include/mxp/message/keepalive.h \
    include/mxp/message/attach_rq.h \
    include/mxp/message/challenge_rsp.h \
    include/mxp/message/perception_event.h \
    include/mxp/message/modify_rsp.h \
    include/mxp/message/interact_rq.h \
    include/mxp/message/bubble_fragment.h \
    include/mxp/message/inject_rq.h \
    include/mxp/message/eject_rsp.h \
    include/mxp/message/action_event.h \
    include/mxp/message/leave_rsp.h \
    include/mxp/message/attach_rsp.h \
    include/mxp/message/examine_rq.h \
    include/mxp/message/synchronization_end_event.h \
    include/mxp/message.h \
    include/mxp.h \
    include/mxp/packet/packet.h \
    include/mxp/packet/message_frame.h \
    include/mxp/packet.h \
    include/mxp/packet/packetizer.h \
    include/mxp/message/for_type.h \
    include/mxp/net/connection.h \
    include/mxp/net.h \
    include/mxp/net/session.h \
    include/mxp/net/communication.h \
    include/mxp/message/response_fragment.h \
    include/mxp/net/session_store.h \
    include/mxp/net/simple_session_store.h \
    include/mxp/net/message_queue_item.h \
    include/mxp/net/simple_session.h \
    include/mxp/net/simple_message_queue_item.h \
    include/mxp/services/mxp_object.h \
    include/mxp/services/bubble.h \
    include/mxp/services.h
SOURCES += src/dummy.cxx \
    src/for_type.cxx \
    src/simple_session_store.cxx \
    src/simple_session.cxx
FORMS +=
RESOURCES +=
include(../../etc/common_lib.pri)
