import unittest
from aether.mxp import *
from aether.mxp.session import *

class SessionTestCase(unittest.TestCase):
    def runTest(self):
        assert SessionState.to_string(SessionState.connecting)=="connecting",  "To string of session state connecting not what was expected. Returned value: {0}".format(SessionState.to_string(SessionState.connecting))
        assert SessionState.to_string(SessionState.connected)=="connected",  "To string of session state connected not what was expected. Returned value: {0}".format(SessionState.to_string(SessionState.connected))
        assert SessionState.to_string(SessionState.disconnected)=="disconnected",  "To string of session state disconnected not what was expected. Returned value: {0}".format(SessionState.to_string(SessionState.disconnected))

testSuite = unittest.TestSuite()
testSuite.addTest(SessionTestCase())
runner = unittest.TextTestRunner()
runner.run(testSuite)
