class LinkedListNode(object):
	
	def __init__(self, value, next=None):
		self.value = value
		self.next = next

	def __str__(self):
		return str(self.value)

class LinkedList(object):
	
	def __init__(self):
		self.head = None
		self.tail = None
		self.size = 0

	def append(self, value):
		new_node = LinkedListNode(value)

		if self.tail:
			self.tail.next = new_node
		else:
			# If tail is None, we are adding to an empty list
			self.head = new_node
		self.tail = new_node
		self.size += 1
		return new_node
        
	def remove(self,  item_to_delete): 
		previous_node = None
		for i in self:     
			if i == item_to_delete:
				if i == self.head:
					if i.next == None:
						self.head = None
						self.tail = None
					else:
						self.head = i.next
					return
				if i == self.tail:
					self.tail = previous_node
					previous_node.next = None
					return
				else:
					previous_node.next = i.next
					return
			previous_node = i
			
	def __iter__(self):
		node = self.head
		while node:
			yield node
			node = node.next
		else:
			raise StopIteration
		
	def __str__(self):
		return ('[' +
				','.join([str(node) for node in self]) +
				']')
