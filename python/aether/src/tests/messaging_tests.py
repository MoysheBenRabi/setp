import unittest
from aether.mxp import *
from aether.mxp.messaging import *

class MessageFactoryTestCase(unittest.TestCase):
    def runTest(self):
        factory = MessageFactory.current()
        message = factory.reserve_message_type(AcknowledgeMessage)
        factory.release_message(message)
        message = factory.reserve_message_type(AcknowledgeMessage)
        print(str(factory))
        
        assert message.frame_data_size(0) == 0, "Frame size not explected value"
        assert message.packet_id_count == 0, "Number of packets contained not expected"
        message.add_packet_id(123)
        assert message.frame_data_size(0) == 4, "Frame size not explected value"
        assert message.get_packet_id(0) == 123, "Packet id retreieved not what was expected"
        assert message.packet_id_count == 1, "Number of packets contained not expected"
        
        
        ba = bytearray([0]*4)
        current_index = message.encode_frame_data(0,ba,0)
        assert current_index == 4, "Offset not expected after encoding"
        assert ba == bytearray([123]+[0]*3)
        
        message.clear()
        assert message.frame_data_size(0) == 0, "Frame size not explected value"
        assert message.packet_id_count == 0, "Number of packets contained not expected"
        
        ba = bytearray([12]+[0]*3+[34]+[0]*3+[56]+[0]*3)
        message = factory.reserve_message_type(AcknowledgeMessage)
        message.decode_from_data(0,ba,0,len(ba))
        assert message.packet_id_count == 3, "Number of packets contained not expected, actual value: "+str(message.packet_id_count)
        assert message.get_packet_id(0) == 12, "Packet id retreieved not what was expected"
        assert message.get_packet_id(1) == 34, "Packet id retreieved not what was expected"
        assert message.get_packet_id(2) == 56, "Packet id retreieved not what was expected"

messagingTestSuite = unittest.TestSuite()
messagingTestSuite.addTest(MessageFactoryTestCase())
runner = unittest.TextTestRunner()
runner.run(messagingTestSuite)
