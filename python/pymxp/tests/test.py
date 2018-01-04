#   Copyright 2010 Moyshe BenRabi
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

import unittest
import copy

from pymxp.messages import *
from pymxp.multiplexer import *
from pymxp.messages.serialization import Reader,Writer
from reference import *
from uuid import UUID
from datetime import datetime
from zipfile import ZipFile

class SerializeTestCaseBase(unittest.TestCase):
    def testSingle(self,value,typeName):
        a = value;
        b = 0;
        writer = Writer();      
        writer.write(a,typeName);
        reader = Reader(writer.byteArray);
        (b, c) = reader.read(typeName);
        assert (type(a) == type(b)), "Type mismatch (type(a)="+str(type(a))+", type(b)="+str(type(b))+")" 
        assert (a == b), "Value mismatch. (a="+str(a)+" b="+str(b)+")"

    def testRange(self,value,count,typeName):
        a = value;
        b = 0;
        writer = Writer();      
        writer.writeRange(a,count,typeName);
        reader = Reader(writer.byteArray);
        (b, c) = reader.readRange(count,typeName);
        assert (type(a) == type(b)), "Type mismatch (type(a)="+str(type(a))+", type(b)="+str(type(b))+")" 
        #assert (count == c), "Count mismatch (count ="+str(count)+" c = "+ str(c)+")"
        assert (a == b), "Value mismatch. (a = "+str(a)+" b="+str(b)+")"

    def testOpenRange(self, value, typeName):
        a = value;
        b = 0;
        writer = Writer();      
        writer.writeRange(a,len(value),typeName);
        reader = Reader(writer.byteArray);
        (b, c) = reader.readRange(0,typeName,4);
        assert (type(a) == type(b)), "Type mismatch (type(a)="+str(type(a))+", type(b)="+str(type(b))+")" 
        #assert (count == c), "Count mismatch (count ="+str(count)+" c = "+ str(c)+")"
        assert (a == b), "Value mismatch. (a = "+str(a)+" b="+str(b)+")"

        

#Serialization tests
class SerializeIntegerTestCase(SerializeTestCaseBase):    
    def runTest(self):
        self.testSingle(123456789,'uint')

class SerializeFloatTestCase(SerializeTestCaseBase):
    def runTest(self):
        self.testSingle(123.456789,'float')        

class SerializeUUIDTestCase(SerializeTestCaseBase):
    def runTest(self):
        self.testSingle(UUID("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),'uuid')

class SerializeTimeTestCase(SerializeTestCaseBase):
    def runTest(self):
        self.testSingle(datetime(2000,1,1),'time')

class SerializeByteRangeTestCase(SerializeTestCaseBase):
    def runTest(self):
        self.testRange([10,20,34,50,50,10],6,'byte')

class SerializeUintRangeTestCase(SerializeTestCaseBase):
    def runTest(self):
        self.testRange([10,20,34,50,50,10],6,'uint')

class SerializeUintOpenRangeTestCase(SerializeTestCaseBase):
    def runTest(self):
        self.testOpenRange([10,20,34,50,50,10],'uint')

class SerializeFloatRangeTestCase(SerializeTestCaseBase):
    def runTest(self):
        self.testRange([10.5,20.4,34.6,50.7,50.8,10.9],6,'float')

class SerializeStringTestCase(SerializeTestCaseBase):
    def runTest(self):
        self.testSingle('TestString','str')

class SerializeFixedStringTestCase(SerializeTestCaseBase):
    def runTest(self):
        self.testRange('TestString',10,'chr')


#Message tests
class MessageFactoryTestCase(unittest.TestCase):
    def runTest(self):
        factory = MessageFactory.current()
        msg = factory.reserve_message_type(Acknowledge)        
        assert msg.type_code == MXP_MESSAGE_ACKNOWLEDGE, "Wrong msg type" 
        factory.release_message(msg)
        msg1 = factory.reserve_message_type(Acknowledge)
        assert msg.type_code == MXP_MESSAGE_ACKNOWLEDGE, "Wrong msg type" 
        msg2 = factory.reserve_message_type(Keepalive)
        assert msg1.type_code == MXP_MESSAGE_ACKNOWLEDGE, "Wrong msg type" 
        assert msg2.type_code == MXP_MESSAGE_KEEPALIVE, "Wrong msg type" 
        factory.release_message(msg1)
        factory.release_message(msg2)

class MessagesSelftestCase(unittest.TestCase):
    def test(self, instance):
        writer = Writer()
        count = instance.serialize(writer)
        reader = Reader(writer.byteArray)
        # TODO: Message factory
        instance1 = copy.deepcopy(instance)    
        instance1.clear()
        instance1.deserialize(reader)        
        assert instance == instance1, "Instance din not match instance1 ["+str(instance)+" = "+str(instance1)+"]"

    def runTest(self):        
        self.test(gen_Acknowledge())
        self.test(gen_Keepalive())
        self.test(gen_Throttle())
        self.test(gen_ChallengeRequest())
        self.test(gen_ChallengeResponse())
        self.test(gen_JoinRequest())
        self.test(gen_JoinResponse())
        self.test(gen_LeaveRequest())
        self.test(gen_LeaveResponse())
        self.test(gen_InjectRequest())
        self.test(gen_InjectResponse())
        self.test(gen_ModifyRequest())
        self.test(gen_ModifyResponse())
        self.test(gen_EjectRequest())
        self.test(gen_EjectResponse())
        self.test(gen_InteractRequest())
        self.test(gen_InteractResponse())
        self.test(gen_ExamineRequest())
        self.test(gen_ExamineResponse())
        self.test(gen_AttachRequest())
        self.test(gen_AttachResponse())
        self.test(gen_DetachRequest())
        self.test(gen_DetachResponse())
        self.test(gen_HandoverRequest())
        self.test(gen_HandoverResponse())
        self.test(gen_ListBubblesRequest())
        self.test(gen_ListBubblesResponse())
        self.test(gen_PerceptionEvent())
        self.test(gen_MovementEvent())
        self.test(gen_DisappearanceEvent())
        self.test(gen_HandoverEvent())
        self.test(gen_ActionEvent())
        self.test(gen_SynchronizationBeginEvent())
        self.test(gen_SynchronizationEndEvent())

def getMessageName(msg):
    msgNames = {MXP_MESSAGE_ACKNOWLEDGE: 'acknowledge', \
                MXP_MESSAGE_KEEPALIVE: 'keepalive', \
                MXP_MESSAGE_THROTTLE: 'throttle', \
                MXP_MESSAGE_CHALLENGE_REQUEST: 'challengerequest', \
                MXP_MESSAGE_CHALLENGE_RESPONSE: 'challengeresponse', \
                MXP_MESSAGE_JOIN_REQUEST: 'joinrequest', \
                MXP_MESSAGE_JOIN_RESPONSE: 'joinresponse', \
                MXP_MESSAGE_LEAVE_REQUEST: 'leaverequest', \
                MXP_MESSAGE_LEAVE_RESPONSE: 'leaveresponse', \
                MXP_MESSAGE_INJECT_REQUEST: 'injectrequest', \
                MXP_MESSAGE_INJECT_RESPONSE: 'injectresponse', \
                MXP_MESSAGE_MODIFY_REQUEST: 'modifyrequest', \
                MXP_MESSAGE_MODIFY_RESPONSE: 'modifyresponse', \
                MXP_MESSAGE_EJECT_REQUEST: 'ejectrequest', \
                MXP_MESSAGE_EJECT_RESPONSE: 'ejectresponse', \
                MXP_MESSAGE_INTERACT_REQUEST: 'interactrequest', \
                MXP_MESSAGE_INTERACT_RESPONSE: 'interactresponse', \
                MXP_MESSAGE_EXAMINE_REQUEST: 'examinerequest', \
                MXP_MESSAGE_EXAMINE_RESPONSE: 'examineresponse', \
                MXP_MESSAGE_ATTACH_REQUEST: 'attachrequest', \
                MXP_MESSAGE_ATTACH_RESPONSE: 'attachresponse', \
                MXP_MESSAGE_DETACH_REQUEST: 'detachrequest', \
                MXP_MESSAGE_DETACH_RESPONSE: 'detachresponse', \
                MXP_MESSAGE_HANDOVER_REQUEST: 'handoverrequest', \
                MXP_MESSAGE_HANDOVER_RESPONSE: 'handoverresponse', \
                MXP_MESSAGE_LIST_BUBBLES_REQUEST: 'listbubblesrequest', \
                MXP_MESSAGE_LIST_BUBBLES_RESPONSE: 'listbubblesresponse', \
                MXP_MESSAGE_PERCEPTION_EVENT: 'perceptionevent', \
                MXP_MESSAGE_MOVEMENT_EVENT: 'movementevent', \
                MXP_MESSAGE_DISAPPEARANCE_EVENT: 'disappearanceevent', \
                MXP_MESSAGE_HANDOVER_EVENT: 'handoverevent', \
                MXP_MESSAGE_ACTION_EVENT: 'actionevent', \
                MXP_MESSAGE_SYNCHRONIZATION_BEGIN_EVENT: 'synchronizationbeginevent', \
                MXP_MESSAGE_SYNCHRONIZATION_END_EVENT: 'synchronizationendevent'}
    return msgNames[msg.type_code]

class MessagesIotZipTestCase(unittest.TestCase):

    def test(self, zipf, instance):
        packet = Packet(Reader(zipf.read("messages/"+getMessageName(instance)+".dat")))        
        multiplexer = Multiplexer()        
        instance1 = multiplexer.demultiplex(packet.frames)   
        assert instance == instance1, "Instance din not match instance1 ["+str(instance)+" = "+str(instance1)+"]"        

    def runTest(self):        
        zipf = ZipFile('./mxp_0_5_reference_messages.zip','r')
        names = zipf.namelist()
        self.test(zipf,gen_Acknowledge())
        self.test(zipf,gen_Keepalive())
        self.test(zipf,gen_Throttle())
        self.test(zipf,gen_ChallengeRequest())
        self.test(zipf,gen_ChallengeResponse())
        self.test(zipf,gen_JoinRequest())
        self.test(zipf,gen_JoinResponse())
        self.test(zipf,gen_LeaveRequest())
        self.test(zipf,gen_LeaveResponse())
        self.test(zipf,gen_InjectRequest()) 
        self.test(zipf,gen_InjectResponse())
        self.test(zipf,gen_ModifyRequest()) # Multiyframe
        self.test(zipf,gen_ModifyResponse())
        self.test(zipf,gen_EjectRequest())
        self.test(zipf,gen_EjectResponse())
        self.test(zipf,gen_InteractRequest())
        self.test(zipf,gen_InteractResponse())
        self.test(zipf,gen_ExamineRequest())
        self.test(zipf,gen_ExamineResponse())
        self.test(zipf,gen_AttachRequest())
        self.test(zipf,gen_AttachResponse())
        self.test(zipf,gen_DetachRequest())
        self.test(zipf,gen_DetachResponse())
        self.test(zipf,gen_HandoverRequest()) 
        self.test(zipf,gen_HandoverResponse())
        self.test(zipf,gen_ListBubblesRequest())
        self.test(zipf,gen_ListBubblesResponse()) # Multiyframe
        self.test(zipf,gen_PerceptionEvent())
        self.test(zipf,gen_MovementEvent())
        self.test(zipf,gen_DisappearanceEvent())
        self.test(zipf,gen_HandoverEvent())
        self.test(zipf,gen_ActionEvent())
        self.test(zipf,gen_SynchronizationBeginEvent())
        self.test(zipf,gen_SynchronizationEndEvent())
        zipf.close()        

class MultiplexerTestCase(unittest.TestCase):
    def runTest(self):
        message = gen_JoinRequest()
        multiplexer = Multiplexer()
        data = multiplexer.multiplex(message)
        message1 = multiplexer.demultiplex(data)
        assert message == message1, 'message != message1'
        

class MessagesFullIotTestCase(unittest.TestCase):
    def runTest(self):        
        assert False, "Not implemented"
    

serializationTestSuite = unittest.TestSuite()
runner = unittest.TextTestRunner()

serializationTestSuite.addTest(SerializeIntegerTestCase())                               
serializationTestSuite.addTest(SerializeFloatTestCase())
serializationTestSuite.addTest(SerializeUUIDTestCase())
serializationTestSuite.addTest(SerializeTimeTestCase())
serializationTestSuite.addTest(SerializeByteRangeTestCase())
serializationTestSuite.addTest(SerializeUintRangeTestCase())
serializationTestSuite.addTest(SerializeUintOpenRangeTestCase())
serializationTestSuite.addTest(SerializeFloatRangeTestCase())
serializationTestSuite.addTest(SerializeFixedStringTestCase())
serializationTestSuite.addTest(SerializeStringTestCase())

runner.run(serializationTestSuite)

messagingTestSuite = unittest.TestSuite()
messagingTestSuite.addTest(MessageFactoryTestCase())
messagingTestSuite.addTest(MessagesSelftestCase())
messagingTestSuite.addTest(MessagesIotZipTestCase())
messagingTestSuite.addTest(MultiplexerTestCase())
messagingTestSuite.addTest(MessagesFullIotTestCase())

runner.run(messagingTestSuite)

