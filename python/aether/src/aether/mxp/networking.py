import uuid
import threading
import collections
import struct
from aether.mxp import *
from aether.utilities import *

class Packet(object):
    
    def __init__(self):
        self.clear()
        self.packet_bytes = bytearray([0] * MXP_MAX_PACKET_SIZE)
    
    def clear(self):
        self.session_id = 0
        self.packet_id = 0
        self.first_send_time = 0
        self.quaranteed = False
        self.resend_count = 0
        self.data_start_index = 0
        self.packet_length = 0
        self.last_send_time = None
        
    def __eq__(self,  other):
        if type(self) != type(other):
            return False
        return this.packet_id == other.package_id
        
    def __hash__(self):
        return this.packet_id.__hash__()
        
    def __str__(self):
        packet_str = "Packet { \n"
        
        for attr in dir(self):
            packet_str += "   " + str(getattr(self, attr))+"\n"
            
        packet_str += "}"
            
        return packet_str;
    
class PacketEncoder(object):
    
    packet_header_frmt = 'IILBB'
    packet_header_length = struct.calcsize('IILBB')
    
    @staticmethod
    def encode_packet_header(packet):
        #Encode the first number of bytes that make up the header, replacing the existing in the packet's bytes.
        packet.packet_bytes = struct.pack(PacketEncoder.packet_header_frmt, packet.session_id, packet.packet_id, packet.first_send_time, packet.quaranteed, packet.resend_count) + packet.packet_bytes[PacketEncoder.packet_header_length:]
        packet.data_start_index = PacketEncoder.packet_header_length
        
    @staticmethod
    def decode_packet_header(packet):
        #Decode the first number of bytes that make up the header.
        (packet.session_id, packet.packet_id, packet.first_send_time, packet.quaranteed, packet.resend_count) = struct.unpack(PacketEncoder.packet_header_frmt, str(packet.packet_bytes[0:PacketEncoder.packet_header_length]))
        packet.data_start_index = PacketEncoder.packet_header_length
    
    @staticmethod
    def encode_packet_data(session,  packet):
        frame_quaranteed = False
        current_index = packet.data_start_index
        for i in range(0, 150):
            previous_index = current_index
            
            (packet.packet_bytes, current_index,  frame_quaranteed) = FrameEncoder.encode_frame(session,  packet.packet_bytes,  current_index)
            
            if current_index > MXP_MAX_PACKET_SIZE - MXP_FRAME_DATA_SIZE:
                break #Packet is full.
            if current_index == previous_index:
                break #Nothing queued to send.
        return frame_quaranteed
        
    @staticmethod
    def decode_packet_data(session,  packet):
        packet_quaranteed = False
        
        current_index = packet.data_start_index
        
        for i in range(0, 150):
            frame_quaranteed = false
            
            (current_index,  frame_quaranteed) = FrameEncoder.decode_frame(session,  packet.packet_bytes,  current_index)
            
            if frame_quaranteed:
                packet_quaranteed = True
                
            if current_index == packet.packet_length:
                break
        
        return packet_quaranteed
    
class FrameEncoder(object):
    
    message_frame_frmt = 'BIihBh'
    message_frame_length = struct.calcsize('BIihBh')
    
    #Encodes frame data on session and encodes its data into a packet. Returns new packet bytes, the new current index of data operation, and if the data is quaranteed.
    @staticmethod
    def encode_frame(session,  packet_bytes,  start_index):
        # See if there is anything to add to our packet
        message_entry = session.get_partial_outbound_message()
        if message_entry == None:
            # If not, continue as normal
            return (packet_bytes, start_index,  False)
        
        # If there is something, get its message
        message = message_entry.message
        
        current_index = start_index
        frame_count = message.frame_count
        frame_size = message.frame_data_size(message_entry.frames_completed)
        
        #Encode message into into data
        frame_bytes = struct.pack(message_frame_frmt, message.type_code,  message.message_id,  frame_count,  message_entry.frames_completed,  frame_size)
        
        try:
            current_index = message.encode_frame_data(message_entry.frames_completed,  packet_bytes,  current_index)
            message_entry.frames_completed += 1
        except Exception as e:
            LogUtil.Error("Error sending message: "+str(e))
            session.complete_outbound_message(message_entry)
        
        #If message is completely filled. Lets send it off.
        if message_entry.frames_completed == message.frame_count:
            session.complete_outbound_message(message_entry)
        
        return (str.join(packet_bytes, frame_bytes), current_index,  message.quaranteed)

        
    @staticmethod
    def decode_frame(session,  packet_bytes,  start_index,  frame_quaranteed):
        current_index = start_index+message_frame_length
        (message_type, message_id, frame_count, frame_index, frame_size) = packet_bytes[start_index:current_index]
        
        message_entry = session.get_partial_inbound_message(message_id,  message_type,  frame_count,  frame_index)
        
        if message_entry != None:
            message = message_entry.message
            frame_quaranteed = message.quaranteed
            current_index = message.decode_from_data( frame_index,  packet_bytes,  current_index,  frame_size)
            message_entry.frames_completed += 1
            
            if message_entry.frames_completed == message.frame_count:
                session.complete_inbound_message(message)
        else:
            # TODO Should these frames be stored for applying after the initial packet has arrived.
            # Should fix problems in situation where packet containing the initial packet is dropped or 
            # if the packet with later frame just happens to arrive first.
            LogUtil.Warn("Ignored frame which arrived before message initialization frame. Possible reconnect.");
        return current_index
    
class PacketFactory(object):
    
    singleton = None
    #singleton mutex
    packet_factory_lock = threading.Lock()
    #mutex for when working with packets
    packet_lock = threading.Lock()
    
    @staticmethod
    def current():
        with PacketFactory.packet_factory_lock:
            if PacketFactory.singleton == None :
                PacketFactory.singleton = PacketFactory()
            return PacketFactory.singleton
    
    def __init__(self):
        self.packets_reserved = 0
        self.packets_released = 0
        self.packets = collections.deque()
        
    def reserve_packet(self):
        with self.packet_lock:
            self.packets_reserved += 1
            if len(self.packets) > 0:
                return self.packets.pop()
            else:
                return Packet()
            
    def release_packet(self, packet):
        with self.packet_lock:
            self.packets_released += 1
            packet.clear()
            self.packets.append(packet)
    
    def __str__(self):
        return "PacketFactory [pool={0}, reserved={1}, released={2}]".format(len(self.packets), self.packets_reserved,  self. packets_released)
