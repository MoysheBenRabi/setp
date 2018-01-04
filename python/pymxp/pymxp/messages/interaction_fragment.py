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

from uuid import *

class InteractionFragment(object): 

    def __init__(self):
        self.max_interaction_name = 20
        self.interaction_name = ''
        self.source_participant_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.source_object_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.target_participant_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.target_object_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.max_extension_dialect = 4
        self.extension_dialect = ''
        self.extension_dialect_major_version = 0
        self.extension_dialect_minor_version = 0
        self.extension_length = 0
        self.extension_data = [0]

    def clear(self):
        self.interaction_name = ''
        self.source_participant_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.source_object_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.target_participant_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.target_object_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.extension_dialect = ''
        self.extension_dialect_major_version = 0
        self.extension_dialect_minor_version = 0
        for i in range(0,self.extension_length):
            self.extension_data[i] = 0             
        self.extension_length = 0
        super(InteractionFragment,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += 16
        result += 16
        result += 16
        result += 16
        result += 1
        result += 1
        result += 4
        result += self.extension_length
        return result

    def get_extension_data(self, index):
        if index >= self.extension_data_length:
            raise Exception("Out of extension_data array bounds: " + str(index))
        return self.extension_data[index]

    def add_extension_data(self, extension_data):
        self.extension_data[self.extension_length] = packet_id
        self.extension_length += 1


    def serialize(self, writer):
        writer.writeRange(self.interaction_name,self.max_interaction_name,'chr')
        writer.write(self.source_participant_id,'uuid')
        writer.write(self.source_object_id,'uuid')
        writer.write(self.target_participant_id,'uuid')
        writer.write(self.target_object_id,'uuid')
        writer.writeRange(self.extension_dialect,self.max_extension_dialect,'chr')
        writer.write(self.extension_dialect_major_version,'byte')
        writer.write(self.extension_dialect_minor_version,'byte')                   
        writer.write(self.extension_length,'uint')
        if self.extension_length:
            writer.writeRange(self.extension_data,self.extension_length,'byte')

    def deserialize(self, reader):
        (self.interaction_name, c) = reader.readRange(self.max_interaction_name,'chr',1)
        (self.source_participant_id, c) = reader.read('uuid')
        (self.source_object_id, c) = reader.read('uuid')
        (self.target_participant_id, c) = reader.read('uuid')
        (self.target_object_id, c) = reader.read('uuid')
        (self.extension_dialect, c) = reader.readRange(self.max_extension_dialect,'chr',1)
        (self.extension_dialect_major_version, c) = reader.read('byte')
        (self.extension_dialect_minor_version, c) = reader.read('byte')
        (self.extension_length, c) = reader.read('uint')
        if self.extension_length:
            (self.extension_data, c) = reader.readRange(self.extension_length,'byte',1)

    def __str__(self):
        return 'InteractionFragment('+self.interaction_name \
                                + str(self.source_participant_id) \
                                + str(self.source_object_id) \
                                + str(self.target_participant_id) \
                                + str(self.target_object_id) \
                                + self.extension_dialect \
                                + str(self.extension_dialect_major_version) \
                                + str(self.extension_dialect_minor_version) \
                                + str(self.extension_length) \
                                + str(self.extension_data)+')'

    def __eq__(self,other):
       return True and \
               (self.interaction_name == other.interaction_name) and \
        self.source_participant_id == other.source_participant_id and \
        self.source_object_id == other.source_object_id and \
        self.target_participant_id == other.target_participant_id and \
        self.target_object_id == other.target_object_id and \
               (self.extension_dialect == other.extension_dialect) and \
        self.extension_dialect_major_version == other.extension_dialect_major_version and \
        self.extension_dialect_minor_version == other.extension_dialect_minor_version and \
        self.extension_length == other.extension_length and \
        self.extension_data == other.extension_data

    def __ne__(self,other):
       return True or \
               (self.interaction_name != other.interaction_name) or \
        self.source_participant_id != other.source_participant_id or \
        self.source_object_id != other.source_object_id or \
        self.target_participant_id != other.target_participant_id or \
        self.target_object_id != other.target_object_id or \
               (self.extension_dialect != other.extension_dialect) or \
        self.extension_dialect_major_version != other.extension_dialect_major_version or \
        self.extension_dialect_minor_version != other.extension_dialect_minor_version or \
        self.extension_length != other.extension_length or \
        self.extension_data != other.extension_data
