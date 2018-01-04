import unittest
from aether.utilities import *

class ByteConverterCase(unittest.TestCase):
    def runTest(self):
        ba = bytearray([0]*4)
        EncodeUtil.copy_string_into_byte_array("AAAA", ba, 0, 4)
        assert ba == bytearray([65]*4), "Did not copy string to array correctly"
        
        ba = bytearray([67]*8)
        current_index = 0
        current_index = EncodeUtil.encode_uint(65, ba, current_index)
        current_index = EncodeUtil.encode_uint(66, ba, current_index)
        assert current_index == 8, "Did not increment index correctly."
        current_index = 0
        (val,current_index) = EncodeUtil.decode_uint(ba, current_index)
        print(str(val))
        assert val==65, "Unexpected first value, actual value :"+val
        (val,current_index) = EncodeUtil.decode_uint(ba, current_index)
        assert val==66, "Unexpected second value, actual value :"+val
        
        assert ba == bytearray([65]+([0]*3)+[66]+([0]*3)), "Did not copy string to array correctly."
        pass

utilityTestSuite = unittest.TestSuite()
utilityTestSuite.addTest(ByteConverterCase())
runner = unittest.TextTestRunner()
runner.run(utilityTestSuite)
