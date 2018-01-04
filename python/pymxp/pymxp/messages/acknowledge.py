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

MXP_MESSAGE_ACKNOWLEDGE = 1

class Acknowledge(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_ACKNOWLEDGE
        self.max_acknowledged_packet_id = MXP_MAX_FRAME_DATA_SIZE / 4
        self.acknowledged_packet_ids = [0] * self.max_acknowledged_packet_id
        self.acknowledged_packet_id_count = 0

    def clear(self):
        for i in range(0,self.acknowledged_packet_id_count):
            self.acknowledged_packet_ids[i] = 0
        self.acknowledged_packet_id_count = 0
        super(Acknowledge,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += self.acknowledged_packet_id_count * 4
        return result

    def get_acknowledged_packet_id(self, index):
        if index >= self.acknowledged_packet_id_count:
            raise Exception("Out of acknowledged_packet_ids array bounds: " + str(index))
        return self.acknowledged_packet_ids[index]

    def add_acknowledged_packet_id(self, acknowledged_packet_id):
        if self.acknowledged_packet_id_coun == self.max_packet_id_count:
            raise Exception("Too many acknowledged_packet_ids")
        self.packet_ids[self.acknowledged_packet_id_count] = packet_id
        self.packet_id_count += 1


    def serialize(self, writer):
        for id in self.acknowledged_packet_ids:
            c = writer.write(id,'uint')
        return c

    def deserialize(self, reader):
        c = 0
        c1 = 0        
        id = 0
        self.acknowledged_packet_ids = []
        while not reader.eof():            
            (id, c1) = reader.read('uint')
            self.acknowledged_packet_ids.append(id)
            if reader.eof(): break
            c += c1
        return c

    def __str__(self):
        return 'Acknowledge('+ str(self.acknowledged_packet_ids) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.acknowledged_packet_ids == other.acknowledged_packet_ids)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.acknowledged_packet_ids != other.acknowledged_packet_ids)

