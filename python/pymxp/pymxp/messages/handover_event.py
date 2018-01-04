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
from object_fragment import ObjectFragment

MXP_MESSAGE_HANDOVER_EVENT = 53

class HandoverEvent(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_HANDOVER_EVENT
        self.source_bubble_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.target_bubble_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.object_header = ObjectFragment()

    def clear(self):
        self.source_bubble_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.target_bubble_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.object_header = ObjectFragment()
        super(HandoverEvent,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += 16
        result += 16
        result += ObjectFragment.frame_data_size(frame_index)
        return result


    def serialize(self, writer):
        writer.write(self.source_bubble_id,'uuid')
        writer.write(self.target_bubble_id,'uuid')
        self.object_header.serialize(writer)

    def deserialize(self, reader):
        (self.source_bubble_id,c) = reader.read('uuid')
        (self.target_bubble_id,c) = reader.read('uuid')
        self.object_header.deserialize(reader)

    def __str__(self):
        return 'HandoverEvent('+ str(self.source_bubble_id) \
                                + str(self.target_bubble_id) \
                                + str(self.object_header) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.source_bubble_id == other.source_bubble_id) and \
               (self.target_bubble_id == other.target_bubble_id) and \
               (self.object_header == other.object_header)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.source_bubble_id != other.source_bubble_id) or \
               (self.target_bubble_id != other.target_bubble_id) or \
               (self.object_header != other.object_header)

