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
from object_fragment import ObjectFragment

MXP_MESSAGE_INJECT_REQUEST = 14

class InjectRequest(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_INJECT_REQUEST
        self.object_header = ObjectFragment()

    def clear(self):
        self.object_header = ObjectFragment()
        super(InjectRequest,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += ObjectFragment.frame_data_size(frame_index)
        return result


    def serialize(self, writer):
        self.object_header.serialize(writer)

    def deserialize(self, reader):
        self.object_header.deserialize(reader)

    def __str__(self):
        return 'InjectRequest('+ str(self.object_header) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.object_header == other.object_header)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.object_header != other.object_header)
