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

MXP_MESSAGE_THROTTLE = 3

class Throttle(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_THROTTLE
        self.transfer_rate = 0

    def clear(self):
        self.transfer_rate = 0
        super(Throttle,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += 4
        return result


    def serialize(self, writer):
        writer.write(self.transfer_rate,'uint')

    def deserialize(self, reader):
        (self.transfer_rate,c) = reader.read('uint')

    def __str__(self):
        return 'Throttle('+ str(self.transfer_rate) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.transfer_rate == other.transfer_rate)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.transfer_rate != other.transfer_rate)

