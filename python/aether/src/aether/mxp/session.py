#<Tommil> There is one particular detail in session forming which may not be obvious. The answering party needs to send ack to the connection request message as the very first message
#<Tommil> the calling party is using this ack to bind together upstream and downstream session session ids
import datetime
from collections import deque
from threading import Lock
from aether.mxp import *
from aether.collections import *

#Enumeration of the various states our session could be in.
class SessionState:
    connecting = 0
    connected = 1 
    disconnected = 2
    
    @staticmethod
    def to_string(session_state):
        if session_state == SessionState.connecting:
            return "connecting"
        elif session_state == SessionState.connected :
            return "connected"
        elif session_state == SessionState.disconnected:
            return "disconnected"
        else:
            raise Exception("Invalid session state.")

#Structure for keeping track of how many frames have been completed on a message we are building.
class MessageEntry(object):

    def __init__(self):
        self.frames_completed = 0
        self.message = None

#This class in responsible for multiplexing a number of messages into packets.
class Session(object):
    MAX_MESSAGES_TO_MULTIPLEX = 10
    
    def __init__(self):
        #unreleated to frames
        self.outgoing_session_id = 0
        self.incoming_session_id = 0
        self.session_state = SessionState.connecting       
        self.remote_end_point = None
        
        self.bubble = None
        
        self.bytes_sent = 0
        self.send_rate = 0
        self.send_rate_update_time = datetime.now()
        self.send_rate_time_window = 0.2
        
        self.first_packet_id = 0
        
        self.creation_time = datetime.now()
        self.connect_time = datetime.now()
        self.disconnect_time = datetime.now()
        
        #frame related
        
        #count of messages sent and received
        self.messages_sent = 0
        self.messages_received = 0
        
        self.is_incoming = False
        self.debug_messages = False
        self.is_first_ack_sent = False
        
        #inbound collections
        self.inbound_messages = deque()
        self.inbound_control_messages = deque()
        self.partial_inbound_messages = {}
        
        #outbound collections
        self.outbound_messages = deque()
        self.outbound_acknowledge_messages = []
        self.partial_outbound_messages = LinkedList()
        self.current_partial_outbound_message = None
        
        self.packets_waiting_acknowledge = {}
        
        #collection mutexes
        self.inbound_messages_lock = Lock()
        self.inbound_control_messages_lock = Lock()
        self.partial_inbound_messages_lock = Lock()
        self.outbound_messages_lock = Lock()
        self.outbound_acknowledge_messages_lock = Lock()
        self.partial_outbound_messages_lock = Lock()
        self.current_partial_outbound_message_lock = Lock()
        self.packets_waiting_acknowledge_lock = Lock()
    
    def set_state_connected(self):
        if self.session_state == SessionState.connecting:
            self.session_state = SessionState.connected
            self.connect_time = datetime.now()
        else:
            raise Exception("Session can only change state to connected from connecting: {0}".format(SessionState.to_string(self.session_state)))
    
    #get the next message we are to write into a packet
    def get_partial_outbound_message(self):
        with self.partial_outbound_messages_lock:
            partial_outbound_message_count = self.partial_outbound_messages.size
        
        #If we are multiplexing less than 10 messages, lets get more messages to multiplex
        #Multiplexing helps prevent large messages from becoming blocking
        if( partial_outbound_message_count < Session.MAX_MESSAGES_TO_MULTIPLEX):
            with self.partial_outbound_messages_lock:
                if self.partial_outbound_messages.size > 0:
                    entry = MessageEntry()
                    entry.message = self.outbound_messages.pop()
                    self.partial_outbound_messages.append(entry)
        
        with self.partial_outbound_messages_lock:
            if( partial_outbound_message_count == 0):
                return None
            
            #We use a round robin technique among a linked list to get a frame from each message
            if self.current_partial_outbound_message == None and self.partial_outbound_messages.size > 0:
                self.current_partial_outbound_message = self.partial_outbound_messages.head
            
            self.last_send_time = datetime.now()
            
            message = self.current_partial_outbound_message.value
            
            self.current_partial_outbound_message = self.current_partial_outbound_message.next
            
            return message
        
    def complete_outbound_message(self, message_entry):
        if self.debug_messages:
            LogUtil.Debug("Session {0} Sent:\n {1}".format(self.incoming_session_id,  message_entry.message))
        with self.partial_outbound_messages_lock:
            self.messages_sent += 1
            self.partial_outbound_messages.remove(message_entry)
            if self.current_partial_outbound_message != None and self.current_partial_outbound_messages.value == message:
                self.current_partial_outbound_message = None
            MessageFactory.current().release_message(message_entry.message)
        #TODO should these be delegated to transmitter or server and hub completed outbound message event handlers?
        if  type(message_entry.message) == DetachResponseMessage or type(message_entry.message) == LeaveResponseMessage :
            self.set_state_disconnected()
        
        
    def get_partial_inbound_message(self,  id,  type,  frame_count,  frame_index):
        with self.partial_inbound_messages_lock:
            self.last_receive_time = datetime.now()
            if id in self.partial_inbound_messages.keys():
                return self.partial_inbound_messages[id]
            else:
                if frame_index > 0:
                    pass
                    #TODO Figure out how to handle frames which arrive before message initialization frame.
                    #we need to receive the first frame first to properly initialize reading rest of the frames.
                message = MessageFactory.current().reserve_message(type)
                message.message_id = id
                message.frame_count = frame_count
                entry = MessageEntry()
                entry.message = message
                self.partial_inbound_messages[message.message_id] = entry
                return entry
        
    def complete_inbound_message(self, message):
        self.messages_received += 1
        with self.partial_inbound_messages_lock:
            del self.partial_inbound_messages[message.message_id]
        
        if self.debug_messages:
            LogUtil.Debug("Session {0} Received:\n {1}".format(self.incoming_session_id,  message))
            
        if  type(message) == ThrottleMessage or type(message) == AcknowledgeMessage or type(message) == KeepaliveMessage:
                with self.inbound_control_messages_lock:
                    self.inbound_control_messages.append(message)
        else:
            with self.inbound_messages_lock:
                self.inbound_messages.append(message)
        
    def send(self,  message):
        pass
    
    def send_acknowledgement_messages(self):
        with self.outbound_acknowledge_messages_lock:
            for message in self.outbound_acknowledge_messages:
                self.send(message)
                self.is_first_ack_sent = True
            self.outbound_acknowledge_messages.clear()
    
    def get_outbound_message_count(self):
        with self.outbound_messages_lock:
            return len(self.outbound_messages)
        
    def get_partially_sent_message_count(self):
        with self.partial_inbound_messages_lock:
            return len(partial_inbound_messages)
        
    def drop_unquaranteed_outbound_messages(self,  number_to_drop):
        for i in range(0, number_to_drop):
            with self.outbound_messages_lock:
                if len(self.outbound_messages) == 0:
                    break
                message = self.outbound_messages.pop()
            
            if message.quaranteed:
                with self.partial_outbound_messages_lock:
                    entry = MessageEntry()
                    entry.message = message
                    self.partial_outbound_messages.append(entry)
    
    #Functions for accessing packets waiting in for acknowledgement.
    def get_packets_waiting_acknowledge(self):
        return self.packets_waiting_acknowledge.values()
        
    def add_packet_waiting_acknowledgement(self,  packet):
        if len(self.packets_waiting_acknowledge) > MXP_MAX_PACKETS_WAITING_ACKNOWLEDGE:
            self.packets_waiting_acknowledge.clear()
        packets_waiting_acknowledge.add[packet.packet_id] = packet
    
    def remove_packet_waiting_acknowledge(self,  packet_id):
        if packet_id in self.packets_waiting_acknowledge.keys():
            PacketFactory.current().release_packet(self.packets_waiting_acknowledge[packet_id])
            del self.packets_waiting_acknowledge[packet_id]
        
    
        
    
