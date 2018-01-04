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

# Two stream like classes the writer and the reader just 
# like in a cxx implementation.
# Question is: can it somehow deduce simple type?
# If so it is possible to create smart dispatcher class.

from uuid import UUID
from time import mktime
from datetime import datetime
import struct
import array
from pymxp.mxpaux import * 

def copyStringIntoByteArray(value, byteArray, currentIndex, length):
    for i in range(0,length):
        byteArray.append(value[i])

class DispatchError(Exception):
    def __init__(self,msg):
        self.value = msg
    def __str__(self):
        return repr(self.value)

class Dispatchable(object):
    
    @staticmethod
    def dispatch(typeName,encode):
        if encode:
            cmd = 'encode'
        else:
            cmd = 'decode'
        cmd = cmd+'_'+typeName
        if hasattr(Dispatchable,cmd):
            result = getattr(Dispatchable,cmd)
        else:
            raise DispatchError('Type '+typeName+' undefined. (comand is '+cmd+')')    
        return result

#uint

    @staticmethod
    def encode_uint(value, byteArray, currentIndex):
        copyStringIntoByteArray(struct.pack("I",value),byteArray,currentIndex,4)
        return currentIndex + 4
    
    @staticmethod
    def decode_uint(byteArray, currentIndex):
        (val,) = struct.unpack("I",(byteArray[currentIndex:currentIndex+4]))
        return (val,currentIndex + 4)

    @staticmethod
    def encode_short(value, byteArray, currentIndex):
        copyStringIntoByteArray(struct.pack("h",value),byteArray,currentIndex,2)
        return currentIndex + 2
    
    @staticmethod
    def decode_short(byteArray, currentIndex):
        (val,) = struct.unpack("h",(byteArray[currentIndex:currentIndex+2]))
        return (val,currentIndex + 2)
    

#float

    @staticmethod
    def encode_float(value, byteArray, currentIndex):
        copyStringIntoByteArray(struct.pack("f",value),byteArray,currentIndex,4)
        return currentIndex + 4

    @staticmethod
    def decode_float(byteArray, currentIndex):
        (val,) = struct.unpack("f",(byteArray[currentIndex:currentIndex+4]))
        return (val,currentIndex + 4)

#uuid

    @staticmethod
    def encode_uuid(value, byteArray, currentIndex):
        for i in range(0,16):
            byteArray.append(value.bytes[i])
        return currentIndex + 16

    @staticmethod
    def decode_uuid(byteArray, currentIndex):
        return (UUID(bytes=byteArray[currentIndex:currentIndex+16]),currentIndex+16)

#date

    @staticmethod
    def encode_time(value, byteArray, currentIndex):
        t = (mkts(value) - epoch_2kts)    
        copyStringIntoByteArray(struct.pack("q",t),byteArray,currentIndex,8)
        return currentIndex + 8

    @staticmethod
    def decode_time(byteArray, currentIndex):
        (t,) = struct.unpack("q",(byteArray[currentIndex:currentIndex+8]))
        time = datetime.fromtimestamp((epoch_2kts+t)/1e3)
        return (time,currentIndex + 8)

#byte

    @staticmethod
    def encode_byte(value, byteArray, currentIndex):
        copyStringIntoByteArray(struct.pack("B",value),byteArray,currentIndex,1)
        return currentIndex + 1

    @staticmethod
    def decode_byte(byteArray, currentIndex):
        (val,) = struct.unpack("B",(byteArray[currentIndex:currentIndex+1]))
        return (val,currentIndex + 1)

#data

    @staticmethod
    def encode_data(value, byteArray, currentIndex):
        return Dispatchable.encode_byte(value, byteArray, currentIndex)
        

    @staticmethod
    def decode_data(byteArray, currentIndex):
        return Dispatchable.decode_byte(byteArray, currentIndex)

#string

    @staticmethod
    def encode_str(value, byteArray, currentIndex):
        for i in range(0, len(value)):
            byteArray.append(value[i]);
        return len(value)

    @staticmethod
    def decode_str(byteArray, currentIndex):
        result = ''
        for i in range(0, len(byteArray)):
            result += byteArray[currentIndex + i];
        return (result,currentIndex + len(result))

    @staticmethod
    def encode_chr(value, byteArray, currentIndex):
        byteArray.append(value[0]);
        return(currentIndex+1)

    @staticmethod
    def decode_chr(byteArray, currentIndex):        
        return (byteArray[currentIndex],currentIndex + 1)
            
#vector3f

    @staticmethod
    def encode_vector3f(value, byteArray, currentIndex):
        c = Dispatchable.encode_float(value[0], byteArray, currentIndex)
        c = Dispatchable.encode_float(value[1], byteArray, c)
        c = Dispatchable.encode_float(value[2], byteArray, c)
        return c
        

    @staticmethod
    def decode_vector3f(byteArray, currentIndex):
        v = [0, 0, 0];
        (v[0],c) = Dispatchable.decode_float(byteArray, currentIndex)
        (v[1],c) = Dispatchable.decode_float(byteArray, c)
        (v[2],c) = Dispatchable.decode_float(byteArray, c)
        return (v,c)

#vector4f

    @staticmethod
    def encode_vector4f(value, byteArray, currentIndex):        
        c = Dispatchable.encode_float(value[0], byteArray, currentIndex)
        c = Dispatchable.encode_float(value[1], byteArray, c)
        c = Dispatchable.encode_float(value[2], byteArray, c)
        c = Dispatchable.encode_float(value[3], byteArray, c)
        return c
        

    @staticmethod
    def decode_vector4f(byteArray, currentIndex):
        v = [0, 0, 0, 0];
        (v[0],c) = Dispatchable.decode_float(byteArray, currentIndex)
        (v[1],c) = Dispatchable.decode_float(byteArray, c)
        (v[2],c) = Dispatchable.decode_float(byteArray, c)
        (v[3],c) = Dispatchable.decode_float(byteArray, c)
        return (v,c)

def packResultString(value):
    result = ''
    for i in range(0,len(value)):                     
        if value[i] == chr(0):
            break
        result = result + value[i]
    return result

class Reader(object):    
    
    def __init__(self, bytes = None):
        if bytes != None:
            self.byteArray = bytes
        else:
            self.byteArray = array.array('c')
        self.currentIndex = 0;

    def read(self, typeName):
        decoder = Dispatchable.dispatch(typeName,False)
        (result, c) = decoder(self.byteArray,self.currentIndex)
        self.currentIndex = c
        return (result, self.currentIndex)

    def eof(self):
        return self.currentIndex >= len(self.byteArray)
    
    def readRange(self, count, typeName = '', typeSize = 1):                
        if type(count) == 'string':
            typeName = count
            count = 0    
        decoder = Dispatchable.dispatch(typeName,False)
        result = []        
        if count == 0:
            count = len(self.byteArray)/typeSize
        for i in range(0, count):
            (r,c) = decoder(self.byteArray,self.currentIndex)
            self.currentIndex = c
            result.append(r)
        if typeName == 'chr':
            result = packResultString(result)
        return (result,self.currentIndex)
        
    def clear(self):
        self.byteArray = array.array('c')
        self.currentIndex = 0;
        pass

    def flush(self):
        pass

class Writer(object):
    
    def __init__(self, bytes = None):
        if bytes != None:
            self.byteArray = bytes
        else:
            self.byteArray = array.array('c')
        self.currentIndex = 0;
    
    def write(self, value, typeName):
        encoder = Dispatchable.dispatch(typeName,True)        
        self.currentIndex = encoder(value, self.byteArray, self.currentIndex)               
        return self.currentIndex

    def writeRange(self, container, count,  typeName):
        
        if typeName == 'chr':
            cnt = len(container)
        else:
            cnt = count
    
        encoder = Dispatchable.dispatch(typeName,True)
        for i in range (0, cnt):
            self.currentIndex = encoder(container[i], self.byteArray, self.currentIndex)
            
        
        if typeName == 'chr' and cnt < count:
            for i in range (0, count - cnt):
                self.currentIndex = encoder(chr(0), self.byteArray, self.currentIndex)


        return self.currentIndex

    def clear(self):
        self.byteArray = array.array('c')
        self.currentIndex = 0;

    def flush(self):
        pass
