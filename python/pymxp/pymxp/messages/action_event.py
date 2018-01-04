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

MXP_MESSAGE_ACTION_EVENT = 60

class ActionEvent(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_ACTION_EVENT
        self.max_action_name = 20
        self.action_name = ''
        self.source_object_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.observation_radius = 0
        self.max_extension_dialect = 4
        self.extension_dialect = ''
        self.extension_dialect_major_version = 0
        self.extension_dialect_minor_version = 0
        self.extension_length = 0
        self.extension_data = [0]

    def clear(self):
        self.action_name = ''
        self.source_object_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.observation_radius = 0
        self.extension_dialect = ''
        self.extension_dialect_major_version = 0
        self.extension_dialect_minor_version = 0
        for i in range(0,self.extension_length):
            self.extension_data[i] = 0
        self.extension_length = 0
        super(ActionEvent,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += 16
        result += 8
        result += 1
        result += 1
        result += 4
        result += self.extension_length * 1
        return result

    def get_extension_data(self, index):
        if index >= self.extension_length:
            raise Exception("Out of extension_datas array bounds: " + str(index))
        return self.extension_datas[index]

    def add_extension_data(self, extension_data):
        self.extension_data[self.extension_length] = extension_data
        self.extension_length += 1


    def serialize(self, writer):
        writer.writeRange(self.action_name,self.max_action_name,'chr')
        writer.write(self.source_object_id,'uuid')
        writer.write(self.observation_radius,'float')
        writer.writeRange(self.extension_dialect,self.max_extension_dialect,'chr')
        writer.write(self.extension_dialect_major_version,'byte')
        writer.write(self.extension_dialect_minor_version,'byte')
        writer.write(self.extension_length,'uint')
        writer.writeRange(self.extension_data,self.extension_length,'byte')

    def deserialize(self, reader):
        (self.action_name, c) = reader.readRange(self.max_action_name,'chr',1)
        (self.source_object_id,c) = reader.read('uuid')
        (self.observation_radius,c) = reader.read('float')
        (self.extension_dialect, c) = reader.readRange(self.max_extension_dialect,'chr',1)
        (self.extension_dialect_major_version,c) = reader.read('byte')
        (self.extension_dialect_minor_version,c) = reader.read('byte')
        (self.extension_length,c) = reader.read('uint')
        (self.extension_data, c) = reader.readRange(self.extension_length,'byte',1)

    def __str__(self):
        return 'ActionEvent('+ self.action_name \
                             + str(self.source_object_id) \
                             + str(self.observation_radius) \
                             + self.extension_dialect \
                             + str(self.extension_dialect_major_version) \
                             + str(self.extension_dialect_minor_version) \
                             + str(self.extension_length) \
                             + str(self.extension_data) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.action_name == other.action_name) and \
               (self.source_object_id == other.source_object_id) and \
               (self.observation_radius == other.observation_radius) and \
               (self.extension_dialect == other.extension_dialect) and \
               (self.extension_dialect_major_version == other.extension_dialect_major_version) and \
               (self.extension_dialect_minor_version == other.extension_dialect_minor_version) and \
               (self.extension_length == other.extension_length) and \
               (self.extension_data == other.extension_data)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.action_name != other.action_name) or \
               (self.source_object_id != other.source_object_id) or \
               (self.observation_radius != other.observation_radius) or \
               (self.extension_dialect != other.extension_dialect) or \
               (self.extension_dialect_major_version != other.extension_dialect_major_version) or \
               (self.extension_dialect_minor_version != other.extension_dialect_minor_version) or \
               (self.extension_length != other.extension_length) or \
               (self.extension_data != other.extension_data)

