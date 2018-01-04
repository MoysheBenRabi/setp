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

import threading

#MXP CONSTANTS
MXP_MAX_PACKET_SIZE = 1500
MXP_MAX_FRAME_DATA_SIZE = 255
MXP_PROTOCOL_MAJOR_VERSION = 0
MXP_PROTOCOL_MINOR_VERSION = 1
MXP_PROTOCOL_SOURCE_REVIOSION = 0
MXP_TIME_OUT_SECONDS = 5
MXP_MAX_IDLE_TIME_SECONDS = 3
MXP_MAX_ACKNOWLEDGEMENT_WAIT_TIME_MILLISECONDS = 250
MXP_MAX_RESEND_COUNT = 3
MXP_MAX_PACKETS_WAITING_ACKNOWLEDGE = 1000
MXP_BUBBLE_TO_BUBBLE_CONNECT_DELAY_SECONDS = 3
MXP_DEFAULT_SERVER_PORT = 1253
MXP_DEFAULT_HUB_PORT = 1254

#MXP RESPONSE CODES
MXP_SUCCESS = 0
MXP_UNAUTHORIZE_OPERATION = 1
MXP_UNKNOWN_OBJECT_ID = 2
    
class PacketMessage(object): 
    ''' Defines an object that represents a message that can be broken down into
    several frames of data to be sent across the wire '''

    def frame_data_size(self, frame_index):
        raise NotImplementedError()
        
    def encode_frame_data(self, frame_index, packet_bytes, start_index):
        raise NotImplementedError()
        
    def decode_frame_data(self, frame_index, packet_bytes, start_index, length):
        raise NotImplementedError()

class Message(PacketMessage):
    ''' Base class for all messages '''    

    message_counter_lock = threading.Lock()
    message_id_counter = 0
    
    def __init__(self):
        self.type_code = 0
        self.guarenteed = False
        self.frame_count = 0
        self.is_auto_release = False
        self.message_id = 0
        with self.message_counter_lock:
            self.message_id_counter = self.message_id_counter + 1
            self.message_id = self.message_id_counter
    
    def clear(self):
        self.message_id_counter += 1
        self.message_id = self.message_id_counter

class MessageFactory(object):        
    
    singleton = None
    #singleton mutex
    message_factory_lock = threading.Lock()
    #mutex for when working with messages
    message_lock = threading.Lock()
    
    @staticmethod
    def current():
        with MessageFactory.message_factory_lock:
            if MessageFactory.singleton == None :
                MessageFactory.singleton = MessageFactory()
            return MessageFactory.singleton
    
    def __init__(self):
        self.code_type_to_message = {}
        self.code_type_to_type = {}
        self.type_to_code_type = {}
    
        self.type_to_reserved_count = {}
        self.type_to_released_count = {}
        self.add_message_type(AcknowledgeMessage())
        
    def add_message_type(self, message):
        self.code_type_to_type[message.type_code] = type(message)
        self.type_to_code_type[type(message)] = message.type_code
        self.type_to_reserved_count[type(message)] = 0
        self.type_to_released_count[type(message)] = 0
        self.code_type_to_message[message.type_code] = collections.deque()
    
    def reserve_message_type(self, message_type):
        return self.reserve_message_typecode(self.type_to_code_type[message_type])
        
    def reserve_message_typecode(self, message_type_code):
        with self.message_lock:
            if message_type_code not in self.code_type_to_type.keys():
                raise Exception("Unknown message type code: {0}".format(message_type_code))
            message_type = self.code_type_to_type[message_type_code]
            
            print(self.type_to_reserved_count.keys())
            print(message_type)
            self.type_to_reserved_count[message_type] = self.type_to_reserved_count[message_type] + 1
            
            if len(self.code_type_to_message[message_type_code]) == 0:
                message = message_type()
                message.is_auto_release = True
                return message
            
            return self.code_type_to_message[message_type_code].pop()
            
    def release_message(self, message):
        if message.is_auto_release:
            with self.message_lock:
                message_type = type(message)
                self.type_to_released_count[message_type] += 1
                
                if message in self.code_type_to_message[message.type_code] :
                    raise Exception("Message was already released: {0}".format(message))
                
                message.clear()
                self.code_type_to_message[message.type_code].append(message)
    
    def __str__(self):
        with self.message_lock:
            message_str = "MessageFactor {"
            
            for t in self.type_to_code_type.keys():
                if len(self.code_type_to_message[self.type_to_code_type[t]]) != 0 or self.type_to_reserved_count[t] != 0 or self.type_to_released_count[t] != 0:
                    message_str += "{0} ({1}|{2}|{3}) ".format(t, len(self.code_type_to_message[self.type_to_code_type[t]]), self.type_to_reserved_count[t] , self.type_to_released_count[t])
            message_str += "}"
            return message_str

