import socket 
from collections import deque
import thread
import threading
from datetime import datetime
import time
import uuid

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

class Bubble(object):
    
    def __init__(self,  id,  name,  range,  perception_range):
        self.id = id
        self.name = name
        self.range = range
        self.perception_range = perception_range
        pass
    
    def clear(self):
        self.id = uuid.UUID('{00000000-0000-0000-0000-000000000000}')
        self.name = ""        
        self.range = float(0)
        self.perception_range = float(0)
    
    # authorize a connecting participant and return ( authorization success, id of partcipant, id of avatar )
    def participant_connect_authorize(self,  session,  message):
        return (true, uuid.UUID('{00000000-0000-0000-0000-000000000000}'),  uuid.UUID('{00000000-0000-0000-0000-000000000000}'))
        
    def participant_connected(self,  session,  join_request_message,  participant_id,  avatar_id):
        pass
        
    def participant_connect_failure(self,  session,  message):
        pass
    
    def participant_message_received(self,  session,  message):
        pass
        
    def participant_disconnected(self,  session,  message):
        pass
        
    def bubble_connect_authorize(self,  session,  attach_request_message):
        pass
        
    def bubble_connected(self,  session,  message):
        pass
        
    def bubble_connect_failure(self,  session,  message):
        pass
        
    def bubble_message_received(self,  session,  message):
        pass
        
    def bubble_disconnected(self,  session):
        pass
        
    def bubble_list_requested(self,  session,  list_bubbles_message):
        pass
        
    def bubble_list_received(self,  session,  list_bubbles_message):
        pass
        
    def __str__(self):
        return "id: {0}, name: {1}, range: {2}, perception range: {3}".format(self.id,  self.name,  self.range,  self.perception_range)
        

class Transmitter(object):
    
    def __init__(self,  port):
        self.udp_client = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.port = port
        self.network_thread = None
        self.receive_thread = None
        self.request_thread_exit = False
        self.incoming_pending_sessions = deque()
        self.id_to_sessions = {}
        self.session_key_to_sessions = {}
        self.session_management_lock = threading.Lock()
        
        self.sessions_to_remove = []
        self.packets_to_remove = []
        
        self.session_id_counter = 0
        self.session_packet_id_counter = 0
        
        self.packets_sent = 0
        self.packets_received = 0
        
        self.bytes_sent = 0
        self.bytes_received = 0
        self.last_bytes_sent = 0
        self.last_bytes_received = 0
        
        self.send_rate_update_time = datetime.now()
        self.last_receive_rate_update_time = datetime.now()
        
        self.send_rate = 0
        self.receive_rate = 0
        self.rate_time_window = 1

        self.maximum_session_send_rate = 250000
    
    @property
    def is_alive(self):
        return self.network_thread != None and self.receive_thread != None 
        
    def startup(self):
        if self.udp_client == None:
            raise Exception("Restart after stop is not supported.")
        
        if( self.network_thread != None ):
            raise Exception("Network thread is already executing.")
            
        if(self.receive_thread != None):
            raise Exception("Receiver thread is already executing.")
            
        self.requestThreadExit = False
        self.network_thread = thread.start_new_thread(self.process, ()) 
        self.receive_thread = thread.start_new_thread(self.process_receive, ()) 
        
    def shutdown(self):
        self.request_thread_exit = True
        self.network_thread =None
        self.receive_thread = None
        self.udp_client = None
        
    def process(self):
        try:
            while self.request_thread_exit == False:
                self.handle_control_messages()
                self.send()
                self.remove_disconnected_sessions()
                time.sleep(10)
        except Exception as e:
            print("An exception occurred on network thread: {0}".format(e))
            self.request_thread_exit = True
            self.udp_client.close()
        finally:
            print("Network thread exited")
    
    def handle_control_messages(self):
        print("handle_control_messages")
        pass
        
    def send(self):
        sessions = id_to_sessions.values()
        
        for session in sessions:
            session.SendAcknowledgeMessages()
            
        pass
        
    def remove_disconnected_sessions(self):
        print("remove_disconnected_sessions")
        pass
        
    def process_receive(self):
        while self.request_thread_exit == False:
            try:
                self.receive()
            except Exception as e:
                print("Error occurred on receive: {0}".format(e))

    def receive(self):
        print("receive")
        pass
