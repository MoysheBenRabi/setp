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
from interaction_fragment import InteractionFragment

MXP_MESSAGE_INTERACT_RESPONSE = 21

class InteractResponse(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_INTERACT_RESPONSE
        self.response_header = ResponseFragment()
        self.response = InteractionFragment()

    def clear(self):
        self.response_header = ResponseFragment()
        self.response = InteractionFragment()
        super(InteractResponse,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += ResponseFragment.frame_data_size(frame_index)
        result += InteractionFragment.frame_data_size(frame_index)
        return result


    def serialize(self, writer):
        self.response_header.serialize(writer)
        self.response.serialize(writer)

    def deserialize(self, reader):
        self.response_header.deserialize(reader)
        self.response.deserialize(reader)

    def __str__(self):
        return 'InteractResponse('+ str(self.response_header) \
                                + str(self.response) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.response_header == other.response_header) and \
               (self.response == other.response)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.response_header != other.response_header) or \
               (self.response != other.response)

