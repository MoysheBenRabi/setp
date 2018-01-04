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


class ResponseFragment(object): 

    def __init__(self):
        self.request_message_id = 0
        self.failure_code = 0

    def clear(self):
        self.request_message_id = 0
        self.failure_code = 0
        super(ResponseFragment,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += 4
        result += 1
        return result


    def serialize(self, writer):
        writer.write(self.request_message_id,'uint')
        writer.write(self.failure_code,'byte')

    def deserialize(self, reader):
        (self.request_message_id, c) = reader.read('uint')
        (self.failure_code, c) = reader.read('byte')

    def __str__(self):
        return 'ResponseFragment('+str(self.request_message_id) \
                                + str(self.failure_code)+')'

    def __eq__(self,other):
       return True and \
        self.request_message_id == other.request_message_id and \
        self.failure_code == other.failure_code

    def __ne__(self,other):
       return True or \
        self.request_message_id != other.request_message_id or \
        self.failure_code != other.failure_code
