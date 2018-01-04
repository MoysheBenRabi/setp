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
from bubble_fragment import BubbleFragment

MXP_MESSAGE_LIST_BUBBLES_RESPONSE = 26

class ListBubblesResponse(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_LIST_BUBBLES_RESPONSE
        self.bubbles = []

    def clear(self):
        self.bubbles = []
        super(ListBubblesResponse,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += BubbleFragment.frame_data_size(frame_index)*len(self.bubbles)
        return result


    def serialize(self, writer):
        for bubble in self.bubbles:
            bubble.serialize(writer)
            # byte[60] padding = 0
            for i in range(0,60):
                writer.write(0,'byte')        

    def deserialize(self, reader):
        while not reader.eof():
            bubble = BubbleFragment()
            bubble.deserialize(reader)
            self.bubbles.append(bubble)
            # byte[60] padding = 0
            for i in range(0,60):
                reader.read('byte')
        
    def __str__(self):
        result = 'ListBubblesResponse('
        for bubble in self.bubbles:
            result += str(bubble)
        result += ')'
        return result

    def __eq__(self,other):
        result = len(self.bubbles) == len(other.bubbles)
        if result:
            for i in range(0,len(self.bubbles)):
                result = self.bubbles[i] == other.bubbles[i]
                if not result: break     
        return result

    def __ne__(self,other):
        result = len(self.bubbles) != len(other.bubbles)
        if not result:
            for i in range(0,len(self.bubbles)):
                result = self.bubbles[i] != other.bubbles[i]
                if result: break
        return result
