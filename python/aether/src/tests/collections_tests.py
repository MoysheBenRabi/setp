import unittest
from aether.collections import LinkedList

class CollectionsTestCase(unittest.TestCase):
    def runTest(self):
        ll = LinkedList()
        ll1 = ll.append(1)
        ll2 =ll.append(2)
        ll3 =ll.append(3)
        assert ll.head.value == 1,  "Head value of linked list not what was expected."
        assert ll.tail.value == 3,  "Head value of linked list not what was expected."
        assert str(ll) == "[1,2,3]",  "String output of linked list not what was expected."
        
        expected_count = 3
        count = 0
        for i in ll:
            count += 1
        assert count==expected_count,  "Did not iterate through all the members expected."
        
        #Test removing middle node of linked list.
        ll.remove( ll2 )
        assert str(ll) == "[1,3]",  "String output of linked list not what was expected. Result: {0}".format(str(ll))
        assert ll.head == ll1,  "Head not expected node."
        assert ll.tail == ll3,  "Tail not expected node."
        assert ll1.next == ll3,  "Tail not expected node."
        assert ll3.next == None,  "Tail not expected node."
        expected_count = 2
        count = 0
        for i in ll:
            count += 1
        assert count==expected_count,  "Did not iterate through all the members expected."
        
        # Test removing tail node of linked list.
        ll.remove( ll3 )
        assert str(ll) == "[1]",  "String output of linked list not what was expected. Result: {0}".format(str(ll))
        assert ll.head == ll1,  "Head not expected node."
        assert ll.tail ==ll1,  "Tail not expected node."
        assert ll1.next == None,  "Tail not expected node."
        expected_count = 1
        count = 0
        for i in ll:
            count += 1
        assert count==expected_count,  "Did not iterate through all the members expected."
        
        # Test removing node that is only node of value.
        ll.remove( ll1 )
        assert str(ll) == "[]",  "String output of linked list not what was expected. Result: {0}".format(str(ll))
        assert ll.head == None,  "Head not expected node."
        assert ll.tail == None,  "Tail not expected node."
        expected_count = 0
        count = 0
        for i in ll:
            count += 1
        assert count==expected_count,  "Did not iterate through all the members expected."
        
        # Test removing from a linked list front with multiple values.Yah
        ll1 = ll.append(1)
        ll2 =ll.append(2)
        ll3 =ll.append(3)
        ll.remove( ll1 )
        assert str(ll) == "[2,3]",  "String output of linked list not what was expected. Result: {0}".format(str(ll))
        assert ll.head == ll2,  "Head not expected node."
        assert ll.tail == ll3,  "Tail not expected node."
        assert ll2.next == ll3,  "Tail not expected node."
        assert ll3.next == None,  "Tail not expected node."
        expected_count = 2
        count = 0
        for i in ll:
            count += 1
        assert count==expected_count,  "Did not iterate through all the members expected."

testSuite = unittest.TestSuite()
testSuite.addTest(CollectionsTestCase())
runner = unittest.TextTestRunner()
runner.run(testSuite)
