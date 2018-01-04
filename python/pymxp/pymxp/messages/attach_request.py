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
from bubble_fragment import BubbleFragment
from program_fragment import ProgramFragment

MXP_MESSAGE_ATTACH_REQUEST = 30

class AttachRequest(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_ATTACH_REQUEST
        self.target_bubble_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.source_bubble = BubbleFragment()
        self.source_bubble_server = ProgramFragment()

    def clear(self):
        self.target_bubble_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.source_bubble = BubbleFragment()
        self.source_bubble_server = ProgramFragment()
        super(AttachRequest,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += 16
        result += BubbleFragment.frame_data_size(frame_index)
        result += ProgramFragment.frame_data_size(frame_index)
        return result


    def serialize(self, writer):
        writer.write(self.target_bubble_id,'uuid')
        self.source_bubble.serialize(writer)
        self.source_bubble_server.serialize(writer)

    def deserialize(self, reader):
        (self.target_bubble_id,c) = reader.read('uuid')
        self.source_bubble.deserialize(reader)
        self.source_bubble_server.deserialize(reader)

    def __str__(self):
        return 'AttachRequest('+ str(self.target_bubble_id) \
                                + str(self.source_bubble) \
                                + str(self.source_bubble_server) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.target_bubble_id == other.target_bubble_id) and \
               (self.source_bubble == other.source_bubble) and \
               (self.source_bubble_server == other.source_bubble_server)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.target_bubble_id != other.target_bubble_id) or \
               (self.source_bubble != other.source_bubble) or \
               (self.source_bubble_server != other.source_bubble_server)

