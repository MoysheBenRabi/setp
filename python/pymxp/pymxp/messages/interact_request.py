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
from interaction_fragment import InteractionFragment

MXP_MESSAGE_INTERACT_REQUEST = 20

class InteractRequest(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_INTERACT_REQUEST
        self.request = InteractionFragment()

    def clear(self):
        self.request = InteractionFragment()
        super(InteractRequest,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += InteractionFragment.frame_data_size(frame_index)
        return result


    def serialize(self, writer):
        self.request.serialize(writer)

    def deserialize(self, reader):
        self.request.deserialize(reader)

    def __str__(self):
        return 'InteractRequest('+ str(self.request) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.request == other.request)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.request != other.request)

