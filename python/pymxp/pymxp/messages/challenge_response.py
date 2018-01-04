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

MXP_MESSAGE_CHALLENGE_RESPONSE = 5

class ChallengeResponse(Message): 

    def __init__(self):
        self.type_code = MXP_MESSAGE_CHALLENGE_RESPONSE
        self.max_challenge_data = 64
        self.challenge_datas = [0] * self.max_challenge_data
        self.challenge_data_length = 0

    def clear(self):
        for i in range(0,self.challenge_data_length):
            self.challenge_datas[i] = 0
        self.challenge_data_length = 0
        super(ChallengeResponse,self).clear()

    def frame_data_size(self, frame_index):
        result = 0
        result += self.challenge_data_length * 1
        return result

    def get_challenge_data(self, index):
        if index >= self.challenge_data_length:
            raise Exception("Out of challenge_datas array bounds: " + str(index))
        return self.challenge_datas[index]

    def add_challenge_data(self, challenge_data):
        if self.challenge_data_coun == 64:
            raise Exception("Too many challenge_datas")
        self.challenge_datas[self.challenge_data_length] = challenge_data
        self.challenge_data_length += 1


    def serialize(self, writer):
        writer.writeRange(self.challenge_datas,self.max_challenge_data,'byte')

    def deserialize(self, reader):
        (self.challenge_datas, c) = reader.readRange(self.max_challenge_data,'byte',1)

    def __str__(self):
        return 'ChallengeResponse('+ str(self.challenge_datas) +')'

    def __eq__(self,other):
        return (self.type_code == other.type_code) and \
               (self.challenge_datas == other.challenge_datas)

    def __ne__(self,other):
        return (self.type_code != other.type_code) or \
               (self.challenge_datas != other.challenge_datas)

