#   Copyright 2010 Moyshe BenRabi
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

from pymxp.messages import *
from datetime import datetime, timedelta
from uuid import UUID
from pymxp.mxpaux import *

def gen_ResponseFragment():
    instance = ResponseFragment()
    instance.request_message_id = 1
    instance.failure_code = 2
    return instance

def gen_ProgramFragment(base):
    instance = ProgramFragment()
    instance.program_name = "TestProgramName"
    instance.program_major_version = base
    instance.program_minor_version = base + 1
    instance.protocol_major_version = base + 2
    instance.protocol_minor_version = base + 3
    instance.protocol_source_revision = base + 4
    return instance

def gen_ObjectFragment():
    instance = ObjectFragment()
    instance.object_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.object_index = 1
    instance.type_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.parent_object_id = UUID("00000000-0000-0000-0000-000000000000")
    instance.object_name = "TestObjectName"
    instance.type_name = "TestTypeName"
    instance.owner_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.location = [ 1.0,  2.0,  3.0 ]
    instance.velocity = [ 4.0,  5.0,  6.0 ]
    instance.acceleration = [ 7.0,  8.0,  9.0 ]
    instance.orientation = [ 10.0, 11.0, 12.0, 13.0 ]
    instance.angular_velocity = [ 14.0, 15.0, 16.0, 17.0 ]
    instance.angular_acceleration = [ 18.0, 19.0, 20.0, 21.0 ]
    instance.bound_sphere_radius = 22.0
    instance.mass = 23.0
    instance.extension_dialect = "TEST"
    instance.extension_dialect_major_version = 0
    instance.extension_dialect_minor_version = 0
    instance.extension_length = 40
    instance.extension_data = [49, 50, 51, 52, 53, 54, 55, 56, 57, \
                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                           48]
    return instance

def gen_InteractionFragment():
    instance = InteractionFragment()
    instance.interaction_name = "TestInteractionName"
    instance.source_participant_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.source_object_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.target_participant_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.target_object_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.extension_dialect = "TEST"
    instance.extension_dialect_major_version = 0
    instance.extension_dialect_minor_version = 0            
    instance.extension_length = 2
    instance.extension_data = [49, 50]
    return instance

def gen_BubbleFragment():
    instance = BubbleFragment()
    instance.bubble_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.bubble_name = "TestBubble1"
    instance.bubble_asset_cache_url = "TestCloudUrl"
    instance.owner_id = UUID("00000000-0000-0000-0000-000000000000")
    instance.bubble_address = "TestBubbleAddress"
    instance.bubble_port = 1
    instance.bubble_center = [ 2.0, 3.0, 4.0 ]
    instance.bubble_range = 5.0
    instance.bubble_perception_range = 6.0
    instance.bubble_realtime = epoch_2k + timedelta(milliseconds=7);
    return instance


def gen_Acknowledge():
    instance = Acknowledge()
    instance.acknowledged_packet_ids = [1,2,3,4,5]
    return instance

def gen_Keepalive():
    instance = Keepalive()
    return instance

def gen_Throttle():
    instance = Throttle()
    instance.transfer_rate = 10000
    return instance

def gen_ChallengeRequest():
    instance = ChallengeRequest()
    for i in range(0,64):
        instance.challenge_datas[i] = i
    return instance

def gen_ChallengeResponse():
    instance = ChallengeResponse()
    for i in range(0,64):
        instance.challenge_datas[i] = i
    return instance

def gen_JoinRequest():
    instance = JoinRequest()
    instance.bubble_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.avatar_id = UUID("00000000-0000-0000-0000-000000000000")
    instance.bubble_name = "TestBubbleName"
    instance.location_name = "TestLocation"
    instance.participant_identifier = "TestParticipantName"
    instance.participant_secret = "TestParticipantPassphrase"
    instance.participant_realtime = epoch_2k + timedelta(milliseconds=10)
    instance.inedtitiy_provider_url = "IdentityProviderUrl"
    instance.client_program = gen_ProgramFragment(1)
    return instance

def gen_JoinResponse():
    instance = JoinResponse()
    instance.response_header = gen_ResponseFragment()
    instance.bubble_id = UUID("00000000-0000-0000-0000-000000000000")
    instance.participant_id = UUID("00000000-0000-0000-0000-000000000000")
    instance.avatar_id = UUID("00000000-0000-0000-0000-000000000000")
    instance.bubble_name = "TestBubbleName"
    instance.bubble_asset_cache_url = "TestBubbleAssetCacheUrl"
    instance.bubble_range = 3.0
    instance.bubble_perception_range = 4.0
    instance.bubble_realtime = epoch_2k + timedelta(milliseconds=5)
    return instance

def gen_LeaveRequest():
    instance = LeaveRequest()
    return instance

def gen_LeaveResponse():
    instance = LeaveResponse()
    instance.response_header = gen_ResponseFragment()
    return instance

def gen_InjectRequest():
    instance = InjectRequest()
    instance.object_header = gen_ObjectFragment()
    instance.object_header.extension_dialect_minor_version = 25
    instance.object_header.extension_dialect_major_version = 24
    instance.object_header.extension_length = 45
    instance.object_header.extension_data = [49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53]
    return instance

def gen_InjectResponse():
    instance = InjectResponse()
    instance.response_header = gen_ResponseFragment()
    return instance

def gen_ModifyRequest():
    instance = ModifyRequest()
    instance.object_header = gen_ObjectFragment()
    instance.object_header.extension_length = 398
    instance.object_header.extension_data = [ 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                           48, 49, 50, 51, 52, 53, 54, 55, 56 ] 
    return instance

def gen_ModifyResponse():
    instance = ModifyResponse()
    instance.response_header = gen_ResponseFragment()
    return instance

def gen_EjectRequest():
    instance = EjectRequest()
    instance.object_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    return instance

def gen_EjectResponse():
    instance = EjectResponse()
    instance.response_header = gen_ResponseFragment()
    return instance

def gen_InteractRequest():
    instance = InteractRequest()
    instance.request = gen_InteractionFragment()
    instance.request.extension_length = 161
    instance.request.extension_data = [ 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49 ]
    return instance

def gen_InteractResponse():
    instance = InteractResponse()
    instance.response_header = gen_ResponseFragment()
    instance.response = gen_InteractionFragment()
    instance.response.extension_length = 156
    instance.response.extension_data = [49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                    48, 49, 50, 51, 52, 53, 54]
    return instance

def gen_ExamineRequest():
    instance = ExamineRequest()
    instance.object_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.object_index = 1
    return instance

def gen_ExamineResponse():
    instance = ExamineResponse()
    instance.response_header = gen_ResponseFragment()
    instance.object_header = gen_ObjectFragment()
    return instance

def gen_AttachRequest():
    instance = AttachRequest()
    instance.target_bubble_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.source_bubble = gen_BubbleFragment()
    instance.source_bubble_server = gen_ProgramFragment(1)
    return instance

def gen_AttachResponse():
    instance = AttachResponse()
    instance.response_header = gen_ResponseFragment()
    instance.target_bubble = gen_BubbleFragment()
    instance.target_bubble_server = gen_ProgramFragment(1)
    return instance

def gen_DetachRequest():
    instance = DetachRequest()
    return instance

def gen_DetachResponse():
    instance = DetachResponse()
    instance.response_header = gen_ResponseFragment()
    return instance

def gen_HandoverRequest():
    instance = HandoverRequest()
    instance.source_bubble_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.target_bubble_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.object_header = gen_ObjectFragment()
    instance.object_header.extension_length = 13
    instance.object_header.extension_data = [ 49, 50, 51, 52, 53,54, 55, 56, 57, \
                                          48, 49, 50, 51 ]
    return instance

def gen_HandoverResponse():
    instance = HandoverResponse()
    instance.response_header = gen_ResponseFragment()
    return instance

def gen_ListBubblesRequest():
    instance = ListBubblesRequest()
    instance.list_type = 0 # [0 = hosted, 1 = linked, 2 = connected] 
    return instance

def gen_ListBubblesResponse():
    instance = ListBubblesResponse()    
    bubbleFragment = gen_BubbleFragment()
    bubbleFragment.bubble_name = 'TestBubble1'
    instance.bubbles.append(bubbleFragment)
    bubbleFragment = gen_BubbleFragment()
    bubbleFragment.bubble_name = 'TestBubble2'
    instance.bubbles.append(bubbleFragment)
    bubbleFragment = gen_BubbleFragment()
    bubbleFragment.bubble_name = 'TestBubble3'
    instance.bubbles.append(bubbleFragment)
    bubbleFragment = gen_BubbleFragment()
    bubbleFragment.bubble_name = 'TestBubble4'
    instance.bubbles.append(bubbleFragment)
    bubbleFragment = gen_BubbleFragment()
    bubbleFragment.bubble_name = 'TestBubble5'
    instance.bubbles.append(bubbleFragment)
    return instance

def gen_PerceptionEvent():
    instance = PerceptionEvent()
    instance.object_header = gen_ObjectFragment()
    instance.object_header.extension_length = 45
    instance.object_header.extension_data = [49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                         48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                         48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                         48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                         48, 49, 50, 51, 52, 53 ]
    return instance

def gen_MovementEvent():
    instance = MovementEvent()
    instance.object_index = 1
    instance.location = [ 1.0, 2.0, 3.0 ]
    instance.orientation = [ 10.0, 11.0, 12.0, 13.0 ]
    return instance

def gen_DisappearanceEvent():
    instance = DisappearanceEvent()
    instance.object_index = 1
    return instance

def gen_HandoverEvent():
    instance = HandoverEvent()
    instance.source_bubble_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.target_bubble_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
    instance.object_header = gen_ObjectFragment()
    instance.object_header.extension_length = 13
    instance.object_header.extension_data = [ 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                                          48, 49, 50, 51 ]
    return instance

def gen_ActionEvent():
    instance = ActionEvent()
    instance.action_name = "TestInteractionName"
    instance.source_object_id = UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee" )
    instance.observation_radius = 100
    instance.extension_dialect = "TEST"
    instance.extension_dialect_major_version = 1
    instance.extension_dialect_minor_version = 2
    instance.extension_length = 205
    instance.extension_data = [ 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, \
                            48, 49, 50, 51, 52, 53 ]
    return instance

def gen_SynchronizationBeginEvent():
    instance = SynchronizationBeginEvent()
    instance.object_count = 1
    return instance

def gen_SynchronizationEndEvent():
    instance = SynchronizationEndEvent()
    return instance

