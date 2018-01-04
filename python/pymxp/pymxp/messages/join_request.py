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

from message_base import *
from uuid import *
from program_fragment import ProgramFragment

MXP_MESSAGE_JOIN_REQUEST = 10

class JoinRequest(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_JOIN_REQUEST
        self.bubble_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.avatar_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.max_bubble_name = 40
        self.bubble_name = ''
        self.max_location_name = 28
        self.location_name = ''
        self.max_participant_identifier = 32
        self.participant_identifier = ''
        self.max_participant_secret = 32
        self.participant_secret = ''
        self.participant_realtime = 0
        self.max_inedtitiy_provider_url = 50
        self.inedtitiy_provider_url = ''
        self.client_program = ProgramFragment()

    def clear(self):
        self.bubble_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.avatar_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.bubble_name = ''
        self.location_name = ''
        self.participant_identifier = ''
        self.participant_secret = ''
        self.participant_realtime = 0
        self.inedtitiy_provider_url = ''
        self.client_program = ProgramFragment()
        super(JoinRequest,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += 16
        result += 16
        result += 8
        result += ProgramFragment.frame_data_size(frame_index)
        return result


    def serialize(self, writer):
        writer.write(self.bubble_id,'uuid')
        writer.write(self.avatar_id,'uuid')
        writer.writeRange(self.bubble_name,self.max_bubble_name,'chr')
        writer.writeRange(self.location_name,self.max_location_name,'chr')
        writer.writeRange(self.participant_identifier,self.max_participant_identifier,'chr')
        writer.writeRange(self.participant_secret,self.max_participant_secret,'chr')
        writer.write(self.participant_realtime,'time')
        writer.writeRange(self.inedtitiy_provider_url,self.max_inedtitiy_provider_url,'chr')
        self.client_program.serialize(writer)

    def deserialize(self, reader):
        (self.bubble_id,c) = reader.read('uuid')
        (self.avatar_id,c) = reader.read('uuid')
        (self.bubble_name, c) = reader.readRange(self.max_bubble_name,'chr',1)
        (self.location_name, c) = reader.readRange(self.max_location_name,'chr',1)
        (self.participant_identifier, c) = reader.readRange(self.max_participant_identifier,'chr',1)
        (self.participant_secret, c) = reader.readRange(self.max_participant_secret,'chr',1)
        (self.participant_realtime,c) = reader.read('time')
        (self.inedtitiy_provider_url, c) = reader.readRange(self.max_inedtitiy_provider_url,'chr',1)
        self.client_program.deserialize(reader)

    def __str__(self):
        return 'JoinRequest('+ str(self.bubble_id) \
                                + str(self.avatar_id) \
                               + self.bubble_name \
                               + self.location_name \
                               + self.participant_identifier \
                               + self.participant_secret \
                                + str(self.participant_realtime) \
                               + self.inedtitiy_provider_url \
                                + str(self.client_program) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.bubble_id == other.bubble_id) and \
               (self.avatar_id == other.avatar_id) and \
               (self.bubble_name == other.bubble_name) and \
               (self.location_name == other.location_name) and \
               (self.participant_identifier == other.participant_identifier) and \
               (self.participant_secret == other.participant_secret) and \
               (self.participant_realtime == other.participant_realtime) and \
               (self.inedtitiy_provider_url == other.inedtitiy_provider_url) and \
               (self.client_program == other.client_program)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.bubble_id != other.bubble_id) or \
               (self.avatar_id != other.avatar_id) or \
               (self.bubble_name != other.bubble_name) or \
               (self.location_name != other.location_name) or \
               (self.participant_identifier != other.participant_identifier) or \
               (self.participant_secret != other.participant_secret) or \
               (self.participant_realtime != other.participant_realtime) or \
               (self.inedtitiy_provider_url != other.inedtitiy_provider_url) or \
               (self.client_program != other.client_program)

