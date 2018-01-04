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

class ObjectFragment(object): 

    def __init__(self):
        self.object_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.object_index = 0
        self.type_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.parent_object_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.max_object_name = 20
        self.object_name = ''
        self.max_type_name = 20
        self.type_name = ''
        self.owner_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.location = 0
        self.velocity = 0
        self.acceleration = 0
        self.orientation = 0
        self.angular_velocity = 0
        self.angular_acceleration = 0
        self.bound_sphere_radius = 0
        self.mass = 0
        self.max_extension_dialect = 4
        self.extension_dialect = ''
        self.extension_dialect_major_version = 0
        self.extension_dialect_minor_version = 0
        self.extension_length = 0
        self.extension_datas = [0]

    def clear(self):
        self.object_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.object_index = 0
        self.type_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.parent_object_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.object_name = ''
        self.type_name = ''
        self.owner_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.location = 0
        self.velocity = 0
        self.acceleration = 0
        self.orientation = 0
        self.angular_velocity = 0
        self.angular_acceleration = 0
        self.bound_sphere_radius = 0
        self.mass = 0
        self.extension_dialect = ''
        self.extension_dialect_major_version = 0
        self.extension_dialect_minor_version = 0
        for i in range(0,self.extension_length):
            self.extension_datas[i] = 0
        self.extension_length = 0
        super(ObjectFragment,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += 16
        result += 4
        result += 16
        result += 16
        result += 16
        result += 12
        result += 12
        result += 12
        result += 16
        result += 16
        result += 16
        result += 8
        result += 8
        result += 1
        result += 1
        result += 4
        result += self.extension_length
        return result

    def get_extension_data(self, index):
        if index >= self.extension_length:
            raise Exception("Out of extension_data array bounds: " + str(index))
        return self.extension_data[index]

    def add_extension_data(self, extension_data):
        self.extension_data[self.extension_length] = packet_id
        self.extension_length += 1


    def serialize(self, writer):
        writer.write(self.object_id,'uuid')
        writer.write(self.object_index,'uint')
        writer.write(self.type_id,'uuid')
        writer.write(self.parent_object_id,'uuid')
        writer.writeRange(self.object_name,self.max_object_name,'chr')
        writer.writeRange(self.type_name,self.max_type_name,'chr')
        writer.write(self.owner_id,'uuid')
        writer.write(self.location,'vector3f')
        writer.write(self.velocity,'vector3f')
        writer.write(self.acceleration,'vector3f')
        writer.write(self.orientation,'vector4f')
        writer.write(self.angular_velocity,'vector4f')
        writer.write(self.angular_acceleration,'vector4f')
        writer.write(self.bound_sphere_radius,'float')
        writer.write(self.mass,'float')
        writer.writeRange(self.extension_dialect,self.max_extension_dialect,'chr')
        writer.write(self.extension_dialect_major_version,'byte')
        writer.write(self.extension_dialect_minor_version,'byte')
        writer.write(self.extension_length,'uint')
        writer.writeRange(self.extension_data,self.extension_length,'byte')

    def deserialize(self, reader):
        (self.object_id, c) = reader.read('uuid')
        (self.object_index, c) = reader.read('uint')
        (self.type_id, c) = reader.read('uuid')
        (self.parent_object_id, c) = reader.read('uuid')
        (self.object_name, c) = reader.readRange(self.max_object_name,'chr',1)
        (self.type_name, c) = reader.readRange(self.max_type_name,'chr',1)
        (self.owner_id, c) = reader.read('uuid')
        (self.location, c) = reader.read('vector3f')
        (self.velocity, c) = reader.read('vector3f')
        (self.acceleration, c) = reader.read('vector3f')
        (self.orientation, c) = reader.read('vector4f')
        (self.angular_velocity, c) = reader.read('vector4f')
        (self.angular_acceleration, c) = reader.read('vector4f')
        (self.bound_sphere_radius, c) = reader.read('float')
        (self.mass, c) = reader.read('float')
        (self.extension_dialect, c) = reader.readRange(self.max_extension_dialect,'chr',1)
        (self.extension_dialect_major_version, c) = reader.read('byte')
        (self.extension_dialect_minor_version, c) = reader.read('byte')
        (self.extension_length, c) = reader.read('uint')
        (self.extension_data, c) = reader.readRange(self.extension_length,'byte',1)

    def __str__(self):
        return 'ObjectFragment('+str(self.object_id) \
                                + str(self.object_index) \
                                + str(self.type_id) \
                                + str(self.parent_object_id) \
                                + self.object_name \
                                + self.type_name \
                                + str(self.owner_id) \
                                + str(self.location) \
                                + str(self.velocity) \
                                + str(self.acceleration) \
                                + str(self.orientation) \
                                + str(self.angular_velocity) \
                                + str(self.angular_acceleration) \
                                + str(self.bound_sphere_radius) \
                                + str(self.mass) \
                                + self.extension_dialect \
                                + str(self.extension_dialect_major_version) \
                                + str(self.extension_dialect_minor_version) \
                                + str(self.extension_length) \
                                + str(self.extension_data)+')'

    def __eq__(self,other):
       return True and \
        self.object_id == other.object_id and \
        self.object_index == other.object_index and \
        self.type_id == other.type_id and \
        self.parent_object_id == other.parent_object_id and \
               (self.object_name == other.object_name) and \
               (self.type_name == other.type_name) and \
        self.owner_id == other.owner_id and \
        self.location == other.location and \
        self.velocity == other.velocity and \
        self.acceleration == other.acceleration and \
        self.orientation == other.orientation and \
        self.angular_velocity == other.angular_velocity and \
        self.angular_acceleration == other.angular_acceleration and \
        self.bound_sphere_radius == other.bound_sphere_radius and \
        self.mass == other.mass and \
               (self.extension_dialect == other.extension_dialect) and \
        self.extension_dialect_major_version == other.extension_dialect_major_version and \
        self.extension_dialect_minor_version == other.extension_dialect_minor_version and \
        self.extension_length == other.extension_length and \
        self.extension_data == other.extension_data

    def __ne__(self,other):
       return True or \
        self.object_id != other.object_id or \
        self.object_index != other.object_index or \
        self.type_id != other.type_id or \
        self.parent_object_id != other.parent_object_id or \
               (self.object_name != other.object_name) or \
               (self.type_name != other.type_name) or \
        self.owner_id != other.owner_id or \
        self.location != other.location or \
        self.velocity != other.velocity or \
        self.acceleration != other.acceleration or \
        self.orientation != other.orientation or \
        self.angular_velocity != other.angular_velocity or \
        self.angular_acceleration != other.angular_acceleration or \
        self.bound_sphere_radius != other.bound_sphere_radius or \
        self.mass != other.mass or \
               (self.extension_dialect != other.extension_dialect) or \
        self.extension_dialect_major_version != other.extension_dialect_major_version or \
        self.extension_dialect_minor_version != other.extension_dialect_minor_version or \
        self.extension_length != other.extension_length or \
        self.extension_data != other.extension_data

