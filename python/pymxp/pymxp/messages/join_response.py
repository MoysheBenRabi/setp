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
from response_fragment import ResponseFragment
from uuid import *

MXP_MESSAGE_JOIN_RESPONSE = 11

class JoinResponse(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_JOIN_RESPONSE
        self.response_header = ResponseFragment()
        self.bubble_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.participant_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.avatar_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.max_bubble_name = 40
        self.bubble_name = ''
        self.max_bubble_asset_cache_url = 50
        self.bubble_asset_cache_url = ''
        self.bubble_range = 0
        self.bubble_perception_range = 0
        self.bubble_realtime = 0

    def clear(self):
        self.response_header = ResponseFragment()
        self.bubble_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.participant_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.avatar_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.bubble_name = ''
        self.bubble_asset_cache_url = ''
        self.bubble_range = 0
        self.bubble_perception_range = 0
        self.bubble_realtime = 0
        super(JoinResponse,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += ResponseFragment.frame_data_size(frame_index)
        result += 16
        result += 16
        result += 16
        result += 8
        result += 8
        result += 8
        return result


    def serialize(self, writer):
        self.response_header.serialize(writer)
        writer.write(self.bubble_id,'uuid')
        writer.write(self.participant_id,'uuid')
        writer.write(self.avatar_id,'uuid')
        writer.writeRange(self.bubble_name,self.max_bubble_name,'chr')
        writer.writeRange(self.bubble_asset_cache_url,self.max_bubble_asset_cache_url,'chr')
        writer.write(self.bubble_range,'float')
        writer.write(self.bubble_perception_range,'float')
        writer.write(self.bubble_realtime,'time')

    def deserialize(self, reader):
        self.response_header.deserialize(reader)
        (self.bubble_id,c) = reader.read('uuid')
        (self.participant_id,c) = reader.read('uuid')
        (self.avatar_id,c) = reader.read('uuid')
        (self.bubble_name, c) = reader.readRange(self.max_bubble_name,'chr',1)
        (self.bubble_asset_cache_url, c) = reader.readRange(self.max_bubble_asset_cache_url,'chr',1)
        (self.bubble_range,c) = reader.read('float')
        (self.bubble_perception_range,c) = reader.read('float')
        (self.bubble_realtime,c) = reader.read('time')

    def __str__(self):
        return 'JoinResponse('+ str(self.response_header) \
                                + str(self.bubble_id) \
                                + str(self.participant_id) \
                                + str(self.avatar_id) \
                               + self.bubble_name \
                               + self.bubble_asset_cache_url \
                                + str(self.bubble_range) \
                                + str(self.bubble_perception_range) \
                                + str(self.bubble_realtime) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.response_header == other.response_header) and \
               (self.bubble_id == other.bubble_id) and \
               (self.participant_id == other.participant_id) and \
               (self.avatar_id == other.avatar_id) and \
               (self.bubble_name == other.bubble_name) and \
               (self.bubble_asset_cache_url == other.bubble_asset_cache_url) and \
               (self.bubble_range == other.bubble_range) and \
               (self.bubble_perception_range == other.bubble_perception_range) and \
               (self.bubble_realtime == other.bubble_realtime)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.response_header != other.response_header) or \
               (self.bubble_id != other.bubble_id) or \
               (self.participant_id != other.participant_id) or \
               (self.avatar_id != other.avatar_id) or \
               (self.bubble_name != other.bubble_name) or \
               (self.bubble_asset_cache_url != other.bubble_asset_cache_url) or \
               (self.bubble_range != other.bubble_range) or \
               (self.bubble_perception_range != other.bubble_perception_range) or \
               (self.bubble_realtime != other.bubble_realtime)

