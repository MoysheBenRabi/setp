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
from datetime import datetime
from pymxp.mxpaux import epoch_2k

class BubbleFragment(object): 

    def __init__(self):
        self.bubble_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.max_bubble_name = 40
        self.bubble_name = ''
        self.max_bubble_asset_cache_url = 51
        self.bubble_asset_cache_url = ''
        self.owner_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.max_bubble_address = 40
        self.bubble_address = ''
        self.bubble_port = 0
        self.bubble_center = [0, 0 , 0]
        self.bubble_range = 0
        self.bubble_perception_range = 0
        self.bubble_realtime = epoch_2k

    def clear(self):
        self.bubble_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.bubble_name = ''
        self.bubble_asset_cache_url = ''
        self.owner_id = UUID('{00000000-0000-0000-0000-000000000000}')
        self.bubble_address = ''
        self.bubble_port = 0
        self.bubble_center = 0
        self.bubble_range = 0
        self.bubble_perception_range = 0
        self.bubble_realtime = epoch_2k
        super(BubbleFragment,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += 16
        result += 16
        result += 4
        result += 12
        result += 8
        result += 8
        result += 8
        return result

    def serialize(self, writer):
        writer.write(self.bubble_id,'uuid')
        writer.writeRange(self.bubble_name,self.max_bubble_name,'chr')
        writer.writeRange(self.bubble_asset_cache_url,self.max_bubble_asset_cache_url,'chr')
        writer.write(self.owner_id,'uuid')
        writer.writeRange(self.bubble_address,self.max_bubble_address,'chr')
        writer.write(self.bubble_port,'uint')
        writer.write(self.bubble_center,'vector3f')
        writer.write(self.bubble_range,'float')
        writer.write(self.bubble_perception_range,'float')
        writer.write(self.bubble_realtime,'time')

    def deserialize(self, reader):
        (self.bubble_id, c) = reader.read('uuid')
        (self.bubble_name, c) = reader.readRange(self.max_bubble_name,'chr',1)
        (self.bubble_asset_cache_url, c) = reader.readRange(self.max_bubble_asset_cache_url,'chr',1)
        (self.owner_id, c) = reader.read('uuid')
        (self.bubble_address, c) = reader.readRange(self.max_bubble_address,'chr',1)
        (self.bubble_port, c) = reader.read('uint')
        (self.bubble_center, c) = reader.read('vector3f')
        (self.bubble_range, c) = reader.read('float')
        (self.bubble_perception_range, c) = reader.read('float')
        (self.bubble_realtime, c) = reader.read('time')

    def __str__(self):
        return 'BubbleFragment('+str(self.bubble_id) \
                               + self.bubble_name \
                               + self.bubble_asset_cache_url \
                                + str(self.owner_id) \
                               + self.bubble_address \
                                + str(self.bubble_port) \
                                + str(self.bubble_center) \
                                + str(self.bubble_range) \
                                + str(self.bubble_perception_range) \
                                + str(self.bubble_realtime)+')'

    def __eq__(self,other):
       return True and \
        self.bubble_id == other.bubble_id and \
               (self.bubble_name == other.bubble_name) and \
               (self.bubble_asset_cache_url == other.bubble_asset_cache_url) and \
        self.owner_id == other.owner_id and \
               (self.bubble_address == other.bubble_address) and \
        self.bubble_port == other.bubble_port and \
        self.bubble_center == other.bubble_center and \
        self.bubble_range == other.bubble_range and \
        self.bubble_perception_range == other.bubble_perception_range and \
        self.bubble_realtime == other.bubble_realtime

    def __ne__(self,other):
       return True or \
        self.bubble_id != other.bubble_id or \
               (self.bubble_name != other.bubble_name) or \
               (self.bubble_asset_cache_url != other.bubble_asset_cache_url) or \
        self.owner_id != other.owner_id or \
               (self.bubble_address != other.bubble_address) or \
        self.bubble_port != other.bubble_port or \
        self.bubble_center != other.bubble_center or \
        self.bubble_range != other.bubble_range or \
        self.bubble_perception_range != other.bubble_perception_range or \
        self.bubble_realtime != other.bubble_realtime

