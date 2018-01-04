#   Copyright 2009 Moyshe BenRabi
#
#   Licensed under the Apache License, Version 2.0 (the "License");
#   you may not use this file except in compliance with the License.
#   You may obtain a copy of the License at
#
#       http://www.apache.org/licenses/LICENSE-2.0
#
#   Unless required by applicable law or agreed to in writing, software
#   distributed under the License is distributed on an "AS IS" BASIS,
#   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
#   See the License for the specific language governing permissions and
#   limitations under the License.
#
#   This is always generated file. Do not edit directyly.
#   Instead edit messagegen.pl and descr.txt

import threading

from response_fragment import ResponseFragment
from program_fragment import ProgramFragment
from object_fragment import ObjectFragment
from interaction_fragment import InteractionFragment
from bubble_fragment import BubbleFragment

from acknowledge import Acknowledge, MXP_MESSAGE_ACKNOWLEDGE
from keepalive import Keepalive, MXP_MESSAGE_KEEPALIVE
from throttle import Throttle, MXP_MESSAGE_THROTTLE
from challenge_request import ChallengeRequest, MXP_MESSAGE_CHALLENGE_REQUEST
from challenge_response import ChallengeResponse, MXP_MESSAGE_CHALLENGE_RESPONSE
from join_request import JoinRequest, MXP_MESSAGE_JOIN_REQUEST
from join_response import JoinResponse, MXP_MESSAGE_JOIN_RESPONSE
from leave_request import LeaveRequest, MXP_MESSAGE_LEAVE_REQUEST
from leave_response import LeaveResponse, MXP_MESSAGE_LEAVE_RESPONSE
from inject_request import InjectRequest, MXP_MESSAGE_INJECT_REQUEST
from inject_response import InjectResponse, MXP_MESSAGE_INJECT_RESPONSE
from modify_request import ModifyRequest, MXP_MESSAGE_MODIFY_REQUEST
from modify_response import ModifyResponse, MXP_MESSAGE_MODIFY_RESPONSE
from eject_request import EjectRequest, MXP_MESSAGE_EJECT_REQUEST
from eject_response import EjectResponse, MXP_MESSAGE_EJECT_RESPONSE
from interact_request import InteractRequest, MXP_MESSAGE_INTERACT_REQUEST
from interact_response import InteractResponse, MXP_MESSAGE_INTERACT_RESPONSE
from examine_request import ExamineRequest, MXP_MESSAGE_EXAMINE_REQUEST
from examine_response import ExamineResponse, MXP_MESSAGE_EXAMINE_RESPONSE
from attach_request import AttachRequest, MXP_MESSAGE_ATTACH_REQUEST
from attach_response import AttachResponse, MXP_MESSAGE_ATTACH_RESPONSE
from detach_request import DetachRequest, MXP_MESSAGE_DETACH_REQUEST
from detach_response import DetachResponse, MXP_MESSAGE_DETACH_RESPONSE
from handover_request import HandoverRequest, MXP_MESSAGE_HANDOVER_REQUEST
from handover_response import HandoverResponse, MXP_MESSAGE_HANDOVER_RESPONSE
from list_bubbles_request import ListBubblesRequest, MXP_MESSAGE_LIST_BUBBLES_REQUEST
from list_bubbles_response import ListBubblesResponse, MXP_MESSAGE_LIST_BUBBLES_RESPONSE
from perception_event import PerceptionEvent, MXP_MESSAGE_PERCEPTION_EVENT
from movement_event import MovementEvent, MXP_MESSAGE_MOVEMENT_EVENT
from disappearance_event import DisappearanceEvent, MXP_MESSAGE_DISAPPEARANCE_EVENT
from handover_event import HandoverEvent, MXP_MESSAGE_HANDOVER_EVENT
from action_event import ActionEvent, MXP_MESSAGE_ACTION_EVENT
from synchronization_begin_event import SynchronizationBeginEvent, MXP_MESSAGE_SYNCHRONIZATION_BEGIN_EVENT
from synchronization_end_event import SynchronizationEndEvent, MXP_MESSAGE_SYNCHRONIZATION_END_EVENT

class MessageFactory(object):        
    
    singleton = None
    #singleton mutex
    message_factory_lock = threading.Lock()
    #mutex for when working with messages
    message_lock = threading.Lock()
    
    @staticmethod
    def current():
        with MessageFactory.message_factory_lock:
            if MessageFactory.singleton == None :
                MessageFactory.singleton = MessageFactory()
            return MessageFactory.singleton

    @staticmethod
    def create_message_typecode(message_type_code):
        if message_type_code == MXP_MESSAGE_ACKNOWLEDGE:
            return Acknowledge()
        if message_type_code == MXP_MESSAGE_KEEPALIVE:
            return Keepalive()
        if message_type_code == MXP_MESSAGE_THROTTLE:
            return Throttle()
        if message_type_code == MXP_MESSAGE_CHALLENGE_REQUEST:
            return ChallengeRequest()
        if message_type_code == MXP_MESSAGE_CHALLENGE_RESPONSE:
            return ChallengeResponse()
        if message_type_code == MXP_MESSAGE_JOIN_REQUEST:
            return JoinRequest()
        if message_type_code == MXP_MESSAGE_JOIN_RESPONSE:
            return JoinResponse()
        if message_type_code == MXP_MESSAGE_LEAVE_REQUEST:
            return LeaveRequest()
        if message_type_code == MXP_MESSAGE_LEAVE_RESPONSE:
            return LeaveResponse()
        if message_type_code == MXP_MESSAGE_INJECT_REQUEST:
            return InjectRequest()
        if message_type_code == MXP_MESSAGE_INJECT_RESPONSE:
            return InjectResponse()
        if message_type_code == MXP_MESSAGE_MODIFY_REQUEST:
            return ModifyRequest()
        if message_type_code == MXP_MESSAGE_MODIFY_RESPONSE:
            return ModifyResponse()
        if message_type_code == MXP_MESSAGE_EJECT_REQUEST:
            return EjectRequest()
        if message_type_code == MXP_MESSAGE_EJECT_RESPONSE:
            return EjectResponse()
        if message_type_code == MXP_MESSAGE_INTERACT_REQUEST:
            return InteractRequest()
        if message_type_code == MXP_MESSAGE_INTERACT_RESPONSE:
            return InteractResponse()
        if message_type_code == MXP_MESSAGE_EXAMINE_REQUEST:
            return ExamineRequest()
        if message_type_code == MXP_MESSAGE_EXAMINE_RESPONSE:
            return ExamineResponse()
        if message_type_code == MXP_MESSAGE_ATTACH_REQUEST:
            return AttachRequest()
        if message_type_code == MXP_MESSAGE_ATTACH_RESPONSE:
            return AttachResponse()
        if message_type_code == MXP_MESSAGE_DETACH_REQUEST:
            return DetachRequest()
        if message_type_code == MXP_MESSAGE_DETACH_RESPONSE:
            return DetachResponse()
        if message_type_code == MXP_MESSAGE_HANDOVER_REQUEST:
            return HandoverRequest()
        if message_type_code == MXP_MESSAGE_HANDOVER_RESPONSE:
            return HandoverResponse()
        if message_type_code == MXP_MESSAGE_LIST_BUBBLES_REQUEST:
            return ListBubblesRequest()
        if message_type_code == MXP_MESSAGE_LIST_BUBBLES_RESPONSE:
            return ListBubblesResponse()
        if message_type_code == MXP_MESSAGE_PERCEPTION_EVENT:
            return PerceptionEvent()
        if message_type_code == MXP_MESSAGE_MOVEMENT_EVENT:
            return MovementEvent()
        if message_type_code == MXP_MESSAGE_DISAPPEARANCE_EVENT:
            return DisappearanceEvent()
        if message_type_code == MXP_MESSAGE_HANDOVER_EVENT:
            return HandoverEvent()
        if message_type_code == MXP_MESSAGE_ACTION_EVENT:
            return ActionEvent()
        if message_type_code == MXP_MESSAGE_SYNCHRONIZATION_BEGIN_EVENT:
            return SynchronizationBeginEvent()
        if message_type_code == MXP_MESSAGE_SYNCHRONIZATION_END_EVENT:
            return SynchronizationEndEvent()

    def __init__(self):
        self.code_type_to_message = {}
        self.code_type_to_type = {}
        self.type_to_code_type = {}
    
        self.type_to_reserved_count = {}
        self.type_to_released_count = {}

        self.add_message_type(Acknowledge())
        self.add_message_type(Keepalive())
        self.add_message_type(Throttle())
        self.add_message_type(ChallengeRequest())
        self.add_message_type(ChallengeResponse())
        self.add_message_type(JoinRequest())
        self.add_message_type(JoinResponse())
        self.add_message_type(LeaveRequest())
        self.add_message_type(LeaveResponse())
        self.add_message_type(InjectRequest())
        self.add_message_type(InjectResponse())
        self.add_message_type(ModifyRequest())
        self.add_message_type(ModifyResponse())
        self.add_message_type(EjectRequest())
        self.add_message_type(EjectResponse())
        self.add_message_type(InteractRequest())
        self.add_message_type(InteractResponse())
        self.add_message_type(ExamineRequest())
        self.add_message_type(ExamineResponse())
        self.add_message_type(AttachRequest())
        self.add_message_type(AttachResponse())
        self.add_message_type(DetachRequest())
        self.add_message_type(DetachResponse())
        self.add_message_type(HandoverRequest())
        self.add_message_type(HandoverResponse())
        self.add_message_type(ListBubblesRequest())
        self.add_message_type(ListBubblesResponse())
        self.add_message_type(PerceptionEvent())
        self.add_message_type(MovementEvent())
        self.add_message_type(DisappearanceEvent())
        self.add_message_type(HandoverEvent())
        self.add_message_type(ActionEvent())
        self.add_message_type(SynchronizationBeginEvent())
        self.add_message_type(SynchronizationEndEvent())
        
    def add_message_type(self, message):
        self.code_type_to_type[message.type_code] = type(message)
        self.type_to_code_type[type(message)] = message.type_code
        self.type_to_reserved_count[type(message)] = 0
        self.type_to_released_count[type(message)] = 0
        self.code_type_to_message[message.type_code] = []
    
    def reserve_message_type(self, message_type):
        return self.reserve_message_typecode(self.type_to_code_type[message_type])
                
    def reserve_message_typecode(self, message_type_code):
        with self.message_lock:
            if message_type_code not in self.code_type_to_type.keys():
                raise Exception("Unknown message type code: {0}".format(message_type_code))
            message_type = self.code_type_to_type[message_type_code]
            
            self.type_to_reserved_count[message_type] = self.type_to_reserved_count[message_type] + 1
            
            if len(self.code_type_to_message[message_type_code]) == 0:
                message = message_type()
                message.is_auto_release = True
                return message
            
            return self.code_type_to_message[message_type_code].pop()
            
    def release_message(self, message):
        if message.is_auto_release:
            with self.message_lock:
                message_type = type(message)
                self.type_to_released_count[message_type] += 1
                
                if message in self.code_type_to_message[message.type_code] :
                    raise Exception("Message was already released: {0}".format(message))
                
                message.clear()
                self.code_type_to_message[message.type_code].append(message)
    
    def __str__(self):
        with self.message_lock:
            message_str = "MessageFactor {"
            
            for t in self.type_to_code_type.keys():
                if len(self.code_type_to_message[self.type_to_code_type[t]]) != 0 or self.type_to_reserved_count[t] != 0 or self.type_to_released_count[t] != 0:
                    message_str += "{0} ({1}|{2}|{3}) ".format(t, len(self.code_type_to_message[self.type_to_code_type[t]]), self.type_to_reserved_count[t] , self.type_to_released_count[t])
            message_str += "}"
            return message_str

type(Acknowledge)
