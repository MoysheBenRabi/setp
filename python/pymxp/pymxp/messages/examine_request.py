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

MXP_MESSAGE_EXAMINE_REQUEST = 22

class ExamineRequest(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_EXAMINE_REQUEST
        self.object_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.object_index = 0

    def clear(self):
        self.object_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.object_index = 0
        super(ExamineRequest,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += 16
        result += 4
        return result


    def serialize(self, writer):
        writer.write(self.object_id,'uuid')
        writer.write(self.object_index,'uint')

    def deserialize(self, reader):
        (self.object_id,c) = reader.read('uuid')
        (self.object_index,c) = reader.read('uint')

    def __str__(self):
        return 'ExamineRequest('+ str(self.object_id) \
                                + str(self.object_index) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.object_id == other.object_id) and \
               (self.object_index == other.object_index)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.object_id != other.object_id) or \
               (self.object_index != other.object_index)

