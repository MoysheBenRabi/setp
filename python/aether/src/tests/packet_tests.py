import unittest
from aether.mxp.networking import *

class PacketEncoderCase(unittest.TestCase):
    def runTest(self):
        p = Packet()
        p.session_id = 1
        p.packet_id = 2
        p.first_send_time = 3
        p.quaranteed = True
        p.resend_count = 4
        p.data_start_index = 5
        p.packet_length = 6
        p.last_send_time = 123
        PacketEncoder.encode_packet_header(p)
        assert len(p.packet_bytes) == MXP_MAX_PACKET_SIZE
        assert p.data_start_index == PacketEncoder.packet_header_length,  "Packet encoder did not encode to expected length"
        
        p2 = Packet()
        p2.packet_bytes = p.packet_bytes
        PacketEncoder.decode_packet_header(p2)
        assert p2.session_id == 1,  "Packet encoder did not decode session_id"
        assert p2.packet_id == 2,  "Packet encoder did not decode packet_id"
        assert p2.first_send_time == 3,  "Packet encoder did not decode first_send_time"
        assert p2.quaranteed == True,  "Packet encoder did not decode quaranteed"
        assert p2.resend_count == 4,  "Packet encoder did not decode resend_count"
        assert p2.data_start_index == PacketEncoder.packet_header_length,  "Packet encoder did not decode to expected length"
        
        

class PacketTestCase(unittest.TestCase):
    def runTest(self):
        #test initialization
        p = Packet()
        assert p.session_id == 0,  "Packet not initialized properly"
        assert p.packet_id == 0,  "Packet not initialized properly"
        assert p.first_send_time == 0,  "Packet not initialized properly"
        assert p.quaranteed == False,  "Packet not initialized properly"
        assert p.resend_count == 0,  "Packet not initialized properly"
        assert p.data_start_index == 0,  "Packet not initialized properly"
        assert p.packet_length == 0,  "Packet not initialized properly"
        assert p.last_send_time == None,  "Packet not initialized properly"
        assert len(p.packet_bytes) == MXP_MAX_PACKET_SIZE,  "Packet not initialized properly"
        assert p.packet_bytes == bytearray([0] * MXP_MAX_PACKET_SIZE),  "Packet not initialized properly"
        
        #test clearing
        p.session_id = 1
        p.packet_id = 1
        p.first_send_time = 1
        p.quaranteed = True
        p.resend_count = 1
        p.data_start_index = 1
        p.packet_length = 1
        p.last_send_time = 123
        p.clear()
        assert p.session_id == 0,  "Packet not cleared properly"
        assert p.packet_id == 0,  "Packet not cleared properly"
        assert p.first_send_time == 0,  "Packet not cleared properly"
        assert p.quaranteed == False,  "Packet not cleared properly"
        assert p.resend_count == 0,  "Packet not cleared properly"
        assert p.data_start_index == 0,  "Packet not cleared properly"
        assert p.packet_length == 0,  "Packet not cleared properly"
        assert p.last_send_time == None,  "Packet not cleared properly"
        
        #test using factory
        PacketFactory.singleton = None
        packet_factory = PacketFactory.current()
        assert packet_factory.packets_reserved == 0,  "Packet factory not initialized properly, actual value"+str(packet_factory.packets_reserved)
        assert packet_factory.packets_released == 0,  "Packet factory not initialized properly, actual value"+str(packet_factory.packets_released)
        assert len(packet_factory.packets) == 0,  "Packet factory not initialized properly"
        print(str(packet_factory))
        #reserve first time should give us a new packet
        packet = packet_factory.reserve_packet()
        assert packet_factory.packets_reserved == 1,   "Did not properly increment value on reserve"
        assert packet_factory.packets_released == 0,   "Did not properly increment value on reserve"
        assert len(packet_factory.packets) == 0,  "Packet did not reserve properly"
        #release should clear it out and keep it in pool
        packet.session_id = 1
        packet.packet_id = 1
        packet.first_send_time = 1
        packet.quaranteed = True
        packet.resend_count = 1
        packet.data_start_index = 1
        packet.packet_length = 1
        packet.last_send_time = 123
        packet_factory.release_packet(packet)
        assert packet_factory.packets_reserved == 1,   "Did not properly increment value on release"
        assert packet_factory.packets_reserved == 1,   "Did not properly increment value on release"
        assert len(packet_factory.packets) == 1,  "Packet did not release properly"
        assert packet.session_id == 0,  "Packet not cleared properly"
        assert packet.packet_id == 0,  "Packet not cleared properly"
        assert packet.first_send_time == 0,  "Packet not cleared properly"
        assert packet.quaranteed == False,  "Packet not cleared properly"
        assert p.resend_count == 0,  "Packet not cleared properly"
        assert packet.data_start_index == 0,  "Packet not cleared properly"
        assert packet.packet_length == 0,  "Packet not cleared properly"
        assert packet.last_send_time == None,  "Packet not cleared properly"
        
        #reserve should give us the used packet
        packet = packet_factory.reserve_packet()
        assert packet_factory.packets_reserved == 2,   "Did not properly increment value on reserve"
        assert packet_factory.packets_released == 1,   "Did not properly increment value on reserve"
        assert len(packet_factory.packets) == 0,  "Packet did not reserve properly"
        #reserve should give us a new packet since we used the one we released already
        packet = packet_factory.reserve_packet()
        assert packet_factory.packets_reserved == 3,   "Did not properly increment value on reserve"
        assert packet_factory.packets_released == 1,   "Did not properly increment value on reserve"
        assert len(packet_factory.packets) == 0,  "Packet did not reserve properly"
        
        assert "PacketFactory [pool=0, reserved=3, released=1]" == str(packet_factory),  "string representation was not what was expected"

packetsTestSuite = unittest.TestSuite()
packetsTestSuite.addTest(PacketTestCase())
packetsTestSuite.addTest(PacketEncoderCase())
runner = unittest.TextTestRunner()
runner.run(packetsTestSuite)
