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
from bubble_fragment import BubbleFragment
from program_fragment import ProgramFragment

MXP_MESSAGE_ATTACH_RESPONSE = 31

class AttachResponse(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_ATTACH_RESPONSE
        self.response_header = ResponseFragment()
        self.target_bubble = BubbleFragment()
        self.target_bubble_server = ProgramFragment()

    def clear(self):
        self.response_header = ResponseFragment()
        self.target_bubble = BubbleFragment()
        self.target_bubble_server = ProgramFragment()
        super(AttachResponse,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += ResponseFragment.frame_data_size(frame_index)
        result += BubbleFragment.frame_data_size(frame_index)
        result += ProgramFragment.frame_data_size(frame_index)
        return result


    def serialize(self, writer):
        self.response_header.serialize(writer)
        self.target_bubble.serialize(writer)
        self.target_bubble_server.serialize(writer)

    def deserialize(self, reader):
        self.response_header.deserialize(reader)
        self.target_bubble.deserialize(reader)
        self.target_bubble_server.deserialize(reader)

    def __str__(self):
        return 'AttachResponse('+ str(self.response_header) \
                                + str(self.target_bubble) \
                                + str(self.target_bubble_server) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.response_header == other.response_header) and \
               (self.target_bubble == other.target_bubble) and \
               (self.target_bubble_server == other.target_bubble_server)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.response_header != other.response_header) or \
               (self.target_bubble != other.target_bubble) or \
               (self.target_bubble_server != other.target_bubble_server)

