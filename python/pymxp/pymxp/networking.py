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

import asyncore, socket
import threading

from pymxp.messages.serialization import Reader,Writer
from pymxp.multiplexer import Session, Splitter, Packet
from pymxp.messages.message_base import Message

class MxpSession(object):
    def __init__(self):
        pass

class MxpClient(asyncore.dispatcher):

    def __init__(self, host, port):
        asyncore.dispatcher.__init__(self)
        self.create_socket(socket.AF_INET, socket.SOCK_DGRAM )
        self.connect( (host, port) )
        self.buffer = []

    def handle_close(self):
        self.close()

    def handle_read(self):
        print self.recvfrom(1452)
        pass

    def writable(self):
        return (len(self.buffer) > 0)

    def handle_write(self):
        s = Splitter()
        p = Packet()        
        i = 0
        for message in self.buffer:
            data = s.split(message)            
            p.frames = data        
        writer = Writer()
        p.serialize(writer)
        self.send(writer.byteArray)

class MxpServer(asyncore.dispatcher):

    def __init__(self, ip, port):        
        asyncore.dispatcher.__init__(self)
        self.create_socket(socket.AF_INET, socket.SOCK_DGRAM )
        self.bind((ip,port))
        self.buffer = []
        self.sessions = {}

    def handle_connect(self):
        pass

    def handle_close(self):
        self.close()

    def handle_read(self):
        (data, source) = self.recvfrom(1452)
        
        if data != '':
            packet = Packet(Reader(data))

        key = (source,packet.session_id)

        if self.sessions.has_key(key):
            session = self.sessions[key]
        else:
            session = Session(self)
            self.sessions[key] = session
            
        session.recieve(data)

    def writable(self):
        return (len(self.buffer) > 0)

    def handle_write(self):
        sent = self.send(self.buffer)
        self.buffer = self.buffer[sent:]
