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


class ProgramFragment(object): 

    def __init__(self):
        self.max_program_name = 25
        self.program_name = ''
        self.program_major_version = 0
        self.program_minor_version = 0
        self.protocol_major_version = 0
        self.protocol_minor_version = 0
        self.protocol_source_revision = 0

    def clear(self):
        self.program_name = ''
        self.program_major_version = 0
        self.program_minor_version = 0
        self.protocol_major_version = 0
        self.protocol_minor_version = 0
        self.protocol_source_revision = 0
        super(ProgramFragment,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += 1
        result += 1
        result += 1
        result += 1
        result += 4
        return result


    def serialize(self, writer):
        writer.writeRange(self.program_name,self.max_program_name,'chr')
        writer.write(self.program_major_version,'byte')
        writer.write(self.program_minor_version,'byte')
        writer.write(self.protocol_major_version,'byte')
        writer.write(self.protocol_minor_version,'byte')
        writer.write(self.protocol_source_revision,'uint')

    def deserialize(self, reader):
        (self.program_name, c) = reader.readRange(self.max_program_name,'chr',1)
        (self.program_major_version, c) = reader.read('byte')
        (self.program_minor_version, c) = reader.read('byte')
        (self.protocol_major_version, c) = reader.read('byte')
        (self.protocol_minor_version, c) = reader.read('byte')
        (self.protocol_source_revision, c) = reader.read('uint')

    def __str__(self):
        return 'ProgramFragment('+self.program_name \
                                + str(self.program_major_version) \
                                + str(self.program_minor_version) \
                                + str(self.protocol_major_version) \
                                + str(self.protocol_minor_version) \
                                + str(self.protocol_source_revision)+')'

    def __eq__(self,other):
       return True and \
               (self.program_name == other.program_name) and \
        self.program_major_version == other.program_major_version and \
        self.program_minor_version == other.program_minor_version and \
        self.protocol_major_version == other.protocol_major_version and \
        self.protocol_minor_version == other.protocol_minor_version and \
        self.protocol_source_revision == other.protocol_source_revision

    def __ne__(self,other):
       return True or \
               (self.program_name != other.program_name) or \
        self.program_major_version != other.program_major_version or \
        self.program_minor_version != other.program_minor_version or \
        self.protocol_major_version != other.protocol_major_version or \
        self.protocol_minor_version != other.protocol_minor_version or \
        self.protocol_source_revision != other.protocol_source_revision

