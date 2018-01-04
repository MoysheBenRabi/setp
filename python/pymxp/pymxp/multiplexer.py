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

import array
import threading

from messages.serialization import Reader,Writer
from messages import *
from mxpaux import epoch_2k

class Frame(object):
    """ Frame class
        Frame header:
            1 : byte  : type
            4 : uint  : message_id 
            2 : short : frame_count
            2 : short : frame_index
            1 : byte  : frame_data_size        
    """
    def __init__(self, frameData = None):
        if frameData != None:
            self.deserialize(frameData)
        else: 
            self.type            = 0
            self.message_id      = 0
            self.frame_count     = 0
            self.frame_index     = 0
            self.frame_data_size = 0
            self.frame_data      = ''

    def __str__(self):
        return 'Frame('+ str(self.type) + ',' \
                          + str(self.message_id) + ',' \
                          + str(self.frame_count) + ',' \
                          + str(self.frame_index) + ',' \
                          + str(self.frame_data_size) + ',' \
                          + 'FrameData[' + str(self.frame_data) + ','+'])'

    def __eq__(self,other):
        pass

    def __ne__(self,other):
        pass

    def serialize(self,frameData):
        frameData.write(self.type,'byte')        
        frameData.write(self.message_id,'uint')
        frameData.write(self.frame_count,'short')
        frameData.write(self.frame_index,'short')
        self.frame_data_size = len(self.frame_data)
        frameData.write(self.frame_data_size,'byte')    
        if self.frame_data_size:
            frameData.writeRange(self.frame_data, self.frame_data_size,'byte')
        
    def deserialize(self,frameData):
        (self.type, c)            = frameData.read('byte')        
        (self.message_id, c)      = frameData.read('uint')
        (self.frame_count, c)     = frameData.read('short')
        (self.frame_index, c)     = frameData.read('short')
        (self.frame_data_size, c) = frameData.read('byte')
        if self.frame_data_size:
            (self.frame_data, c)      = frameData.readRange(self.frame_data_size,'byte')
        else:
             self.frame_data = []

class Splitter(object):

    message_id_lock  = threading.Lock()
    message_id = 0

    def __init__(self):
        messageHandelers = {}

    def register(self, messageType, handeler):
        pass

    def join(self, frames):
        """ Parse bunch of frames and form message
        Naive multiplexer implemmentaion - assumes that
        frames array form one message and there are no
        messages split acros multiple frame bunches
        """        
        messageData = array.array('c')
        for frame in frames:
            for byte in frame.frame_data:
                messageData.append(chr(byte))
        message = MessageFactory.create_message_typecode(frames[0].type)        
        reader = Reader()
        reader.byteArray = messageData
        message.deserialize(reader)
        return message       

    def split(self, message):
        """ Parse message and form frames.
        """               
        with Splitter.message_id_lock:
            msgId = Splitter.message_id
            Splitter.message_id += 1

        frameNo = 0
        frameCnt = 0
        index = 0
        i = 0
        frameData = []        
        frames = []
        writer = Writer()
        message.serialize(writer)
        for i1 in range(0,len(writer.byteArray)):            
            frameData.append(ord(writer.byteArray[i1]))
            i += 1
            if (i1 == len(writer.byteArray) - 1) or (i == 255) :
                frame = Frame()
                frame.type = message.type_code
                frame.message_id = msgId
                frame.frame_count = 0;
                frame.frame_index = index
                frame.frame_data_size = i
                frame.frame_data = frameData
                frames.append(frame)
                frameData = []
                i = 0
                index += 1

        count = len(frames)
        for frame in frames:
            frame.frame_count = count

        return frames

class Packet(object):
    """ Packet class
        Packet header:
            4 : uint : session_id
            4 : uint : packet_id
            8 : time : first_send_time
            1 : byte : guaranteed
            1 : byte : resend_count
    """
    def __init__(self, packetData = None):
        if packetData != None:
            self.deserialize(packetData)
        else:
            self.session_id   = 0
            self.packet_id    = 0
            self.time         = epoch_2k
            self.guaranteed   = 0
            self.resend_count = 0

        

    def __str__(self):
        result = 'Packet('+ str(self.session_id) + ',' + str(self.packet_id) + ',' + str(self.time) + ',' + str(self.resend_count) + 'Frames['
        for frame in self.frames:
            result += ','
            result += str(frame)
        result += '])'
        return result

    def __eq__(self,other):
        pass

    def __ne__(self,other):
        pass

    def serialize(self,packetData):
        packetData.currentIndex = 0
        packetData.write(self.session_id,'uint')
        packetData.write(self.packet_id,'uint')
        packetData.write(self.time,'time',)
        packetData.write(self.guaranteed,'byte')        
        packetData.write(self.resend_count,'byte')        
        for frame in self.frames:
            frame.serialize(packetData)

    def deserialize(self,packetData):
        packetData.currentIndex = 0
        (self.session_id, c)   = packetData.read('uint')
        (self.packet_id, c)    = packetData.read('uint')
        (self.time, c)         = packetData.read('time')
        (self.guaranteed, c)   = packetData.read('byte')        
        (self.resend_count, c) = packetData.read('byte')        
        self.frames = []
        while not packetData.eof():
            frame = Frame(packetData)
            self.frames.append(frame)


class Channel(object):
    """ Base class for all channles
    """
    def __init__(self):
        pass

class GuaranteedChannel(Channel):
    """ Guaranteed channel
    """
    def __init__(self):
        pass

    def resieve(self, session, packet):
        """ Resieving chunk of raw data from some data source
        """
        pass


class UnguaranteedChannel(Channel):
    """ Unguaranteed channel
    """
    def __init__(self):
        pass

    def resieve(self, session, packet):
        """ Resieving chunk of raw data from some data source
        """
        pass


class Session(object):
    """ Session
    """
    def __init__(self,transmitter):                        
        self.gchannel = GuaranteedChannel()
        self.ugchannel = UnguaranteedChannel()
        self.transmitter = transmitter
        self.sessionId = 0
    
    def recieve(self, rawData):
        if rawData != '':
            packet = Packet(Reader(rawData))
        if packet.guaranteed:
            self.gchannel.resieve(self,packet)
        else:
            self.ugchannel.resieve(self,packet)
