import unittest
from aether.mxp import *
from aether.mxp.messaging import *
from aether.mxp.transmitter  import *

class TransmitterTestCase(unittest.TestCase):
    def runTest(self):
        t = Transmitter(MXP_DEFAULT_SERVER_PORT)       
        assert t.is_alive == False,  "transmitter does not initially show up as is_alive = False"
        t.startup()   
        assert t.is_alive == True,  "transmitter does show up as is_alive = True when startup "
        t.shutdown()
        assert t.is_alive == False,  "transmitter does not show up as is_alive = False after shut down"

messagingTestSuite = unittest.TestSuite()
messagingTestSuite.addTest(TransmitterTestCase())
runner = unittest.TextTestRunner()
runner.run(messagingTestSuite)
