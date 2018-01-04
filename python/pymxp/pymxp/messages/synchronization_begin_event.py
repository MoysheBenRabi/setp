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

MXP_MESSAGE_SYNCHRONIZATION_BEGIN_EVENT = 70

class SynchronizationBeginEvent(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_SYNCHRONIZATION_BEGIN_EVENT
        self.object_count = 0

    def clear(self):
        self.object_count = 0
        super(SynchronizationBeginEvent,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += 4
        return result


    def serialize(self, writer):
        writer.write(self.object_count,'uint')

    def deserialize(self, reader):
        (self.object_count,c) = reader.read('uint')

    def __str__(self):
        return 'SynchronizationBeginEvent('+ str(self.object_count) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.object_count == other.object_count)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.object_count != other.object_count)

