import uuid
import threading
import collections
from aether.mxp import *
from aether.mxp.networking import *
from aether.utilities import *

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

#Defines an object that represents a message that can be broken down into several frames of data to be sent across the wire
class PacketMessage(object): 
    def frame_data_size(self, frame_index):
        raise NotImplementedError()
        
    def encode_frame_data(self, frame_index, packet_bytes, start_index):
        raise NotImplementedError()
        
    def decode_frame_data(self, frame_index, packet_bytes, start_index, length):
        raise NotImplementedError()

class Message(PacketMessage):
    
    message_counter_lock = threading.Lock()
    message_id_counter = 0
    
    def __init__(self):
        self.type_code = 0
        self.quarenteed = False
        self.frame_count = 0
        self.is_auto_release = False
        self.message_id = 0
        with self.message_counter_lock:
            self.message_id_counter = self.message_id_counter + 1
            self.message_id = self.message_id_counter
    
    def clear(self):
        self.message_id_counter += 1
        self.message_id = self.message_id_counter
    
MXP_MESSAGE_ACKNOWLEDGE = 1
        
class ThrottleMessage(Message):
    pass
    
class AcknowledgeMessage(Message):
    
    def __init__(self):
        self.max_packet_id_count = MXP_MAX_FRAME_DATA_SIZE / 4
        self.packet_ids = [0] * self.max_packet_id_count
        self.packet_id_count = 0
        self.type_code = MXP_MESSAGE_ACKNOWLEDGE
        self.frame_count = 1
        self.quarenteed = False
        
    def frame_data_size(self, frame_index):
        return self.packet_id_count * 4
        
    def add_packet_id(self, packet_id):
        if self.packet_id_count == self.max_packet_id_count:
            raise Exception("Acknowledge message is already full")
        self.packet_ids[self.packet_id_count] = packet_id
        self.packet_id_count += 1
        
    def get_packet_id(self, index):
        if index >= self.packet_id_count:
            raise Exception("Ot of packet id array bounds: " + str(index))
        return self.packet_ids[index]
    
    def clear(self):
        for i in range(0,self.packet_id_count):
            self.packet_ids[i] = 0
        self.packet_id_count = 0
        super(AcknowledgeMessage,self).clear()
    
    def encode_frame_data(self, frame_index, packet_bytes, start_index):
        current_index = start_index
        for i in range(0,self.packet_id_count):
            current_index = EncodeUtil.encode_uint(self.packet_ids[i], packet_bytes, start_index)
        return current_index

    def decode_from_data(self, frame_index, packet_bytes, start_index, length):
        self.packet_id_count = length/4
        current_index = start_index
        for i in range(0,self.packet_id_count):
            (self.packet_ids[i], current_index) = EncodeUtil.decode_uint(packet_bytes, current_index)
        return current_index

class KeepaliveMessage(Message):
    pass
    
class DetachResponseMessage(Message):
    pass
    
class LeaveResponseMessage(Message):
    pass
