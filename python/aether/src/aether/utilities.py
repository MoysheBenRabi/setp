import struct

class Compression(object):
    pass
    
class LogUtil(object):

    @staticmethod
    def Warn(message):
        print(message)
        
    @staticmethod
    def Error(message):
        print(message)
        
    @staticmethod
    def Debug(message):
        print(message)
    
class MathUtil(object):
    pass

class EncodeUtil(object):
    
    @staticmethod
    def encode_uint(value, byte_array, current_index):
        EncodeUtil.copy_string_into_byte_array(struct.pack("I",value),byte_array,current_index,4)
        return current_index + 4
    
    @staticmethod
    def decode_uint(packet_bytes, current_index):
        (val,) = struct.unpack("I",str(packet_bytes[current_index:current_index+4]))
        return (val,current_index + 4)
    
    @staticmethod
    def copy_string_into_byte_array(value, byte_array, current_index, length):
        for i in range(0,length):
            byte_array[current_index+i] = value[i]
            
