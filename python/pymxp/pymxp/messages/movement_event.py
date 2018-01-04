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

MXP_MESSAGE_MOVEMENT_EVENT = 41

class MovementEvent(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_MOVEMENT_EVENT
        self.object_index = 0
        self.location = 0
        self.orientation = 0

    def clear(self):
        self.object_index = 0
        self.location = 0
        self.orientation = 0
        super(MovementEvent,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += 4
        result += 12
        result += 16
        return result


    def serialize(self, writer):
        writer.write(self.object_index,'uint')
        writer.write(self.location,'vector3f')
        writer.write(self.orientation,'vector4f')

    def deserialize(self, reader):
        (self.object_index,c) = reader.read('uint')
        (self.location,c) = reader.read('vector3f')
        (self.orientation,c) = reader.read('vector4f')

    def __str__(self):
        return 'MovementEvent('+ str(self.object_index) \
                                + str(self.location) \
                                + str(self.orientation) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.object_index == other.object_index) and \
               (self.location == other.location) and \
               (self.orientation == other.orientation)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.object_index != other.object_index) or \
               (self.location != other.location) or \
               (self.orientation != other.orientation)

