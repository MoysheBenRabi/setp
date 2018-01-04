using System;
using System.Collections.Generic;

using System.Text;

namespace MXP.Util
{
    /// <summary>
    /// Handles conversions between primitives and byte array presentations.
    /// NOTE: to handle endianness: if (BitConverter.IsLittleEndian) Array.Reverse(intBytes);
    /// 
    /// </summary>
    public class EncodeUtil
    {

        public static int Encode(ref String str, byte[] bytes, int index, int maxLength)
        {
            // TODO This may still cause too long string. Figure out better way to cut the string at correct character.
            if (str != null)
            {
                if (str.Length > maxLength)
                {
                    str = str.Substring(0, maxLength);
                }
                int bytesEncodedCount = Encoding.UTF8.GetBytes(str, 0, str.Length, bytes, index);
                // Ensure that the string is ended with zero
                if (bytesEncodedCount < maxLength)
                {
                    bytes[index + bytesEncodedCount] = 0;
                }
            }
            else
            {
                bytes[index] = 0;
            }
            return index + maxLength;
        }

        public static int Decode(ref String str, byte[] bytes, int index, int maxLength)
        {
            // TODO Figure out efficient way to find the end of string before decoding.
            str = Encoding.UTF8.GetString(bytes, index, maxLength);
            int terminationIndex = str.IndexOf('\0');
            if (terminationIndex > -1)
            {
                str = str.Substring(0, str.IndexOf('\0'));
            }
            return index + maxLength;
        }

        public static int Encode(ref byte[] fieldBytes, byte[] packetBytes, int packetIndex, int maxLength)
        {
            Array.Copy(fieldBytes, 0, packetBytes, packetIndex, maxLength);
            return packetIndex + maxLength;
        }

        public static int Decode(ref byte[] fieldBytes, byte[] packetBytes, int packetIndex, int maxLength)
        {
            Array.Copy(packetBytes, packetIndex, fieldBytes, 0, maxLength);
            return packetIndex + maxLength;
        }

        public static int Encode(ref byte[] fieldBytes, int fieldIndex, byte[] packetBytes, int packetIndex, int maxLength)
        {
            Array.Copy(fieldBytes, fieldIndex, packetBytes, packetIndex, maxLength);
            return packetIndex + maxLength;
        }

        public static int Decode(ref byte[] fieldBytes, int fieldIndex, byte[] packetBytes, int packetIndex, int maxLength)
        {
            try
            {
                Array.Copy(packetBytes, packetIndex, fieldBytes, fieldIndex, maxLength);
            }
            catch (ArgumentException e)
            {
                throw e;
            }        
            return packetIndex + maxLength;
        }

        # region 64 bit integer
        public static int Encode(ref long value, byte[] bytes, int index)
        {
            BitConverter.GetBytes(value).CopyTo(bytes, index);
            return index + 8;
        }

        public static int Decode(ref long value, byte[] bytes, int index)
        {
            value=BitConverter.ToInt64(bytes, index);
            return index + 8;
        }

        public static int Encode(ref ulong value, byte[] bytes, int index)
        {
            BitConverter.GetBytes(value).CopyTo(bytes, index);
            return index + 8;
        }

        public static int Decode(ref ulong value, byte[] bytes, int index)
        {
            value = BitConverter.ToUInt64(bytes, index);
            return index + 8;
        }
        # endregion

        # region 32 bit integer
        public static int Encode(ref int value, byte[] bytes, int index)
        {
            BitConverter.GetBytes(value).CopyTo(bytes,index);
            return index + 4;
        }

        public static int Decode(ref int value, byte[] bytes, int index)
        {
            value=BitConverter.ToInt32(bytes, index);
            return index + 4;
        }

        public static int Encode(ref uint value, byte[] bytes, int index)
        {
            BitConverter.GetBytes(value).CopyTo(bytes, index);
            return index + 4;
        }

        public static int Decode(ref uint value, byte[] bytes, int index)
        {
            value = BitConverter.ToUInt32(bytes, index);
            return index + 4;
        }
        # endregion

        # region 16 bit integer
        public static int Encode(ref short value, byte[] bytes, int index)
        {
            BitConverter.GetBytes(value).CopyTo(bytes, index);
            return index + 2;
        }

        public static int Decode(ref short value, byte[] bytes, int index)
        {
            value = BitConverter.ToInt16(bytes, index);
            return index + 2;
        }

        public static int Encode(ref ushort value, byte[] bytes, int index)
        {
            BitConverter.GetBytes(value).CopyTo(bytes, index);
            return index + 2;
        }

        public static int Decode(ref ushort value, byte[] bytes, int index)
        {
            value = BitConverter.ToUInt16(bytes, index);
            return index + 2;
        }
        # endregion

        public static int Encode(ref float value, byte[] bytes, int index)
        {
            BitConverter.GetBytes(value).CopyTo(bytes, index);
            return index + 4;
        }

        public static int Encode(float value, byte[] bytes, int index)
        {
            BitConverter.GetBytes(value).CopyTo(bytes, index);
            return index + 4;
        }

        public static int Decode(ref float value, byte[] bytes, int index)
        {
            value = BitConverter.ToSingle(bytes, index);
            return index + 4;
        }

        public static int Encode(ref byte b, byte[] bytes, int index)
        {
            bytes[index] = b;
            return index + 1;
        }

        public static int Decode(ref byte b, byte[] bytes, int index)
        {
            b = bytes[index];
            return index + 1;
        }

        public static int Encode(ref bool b, byte[] bytes, int index)
        {
            bytes[index] = b?(byte)1:(byte)0;
            return index + 1;
        }

        public static int Decode(ref bool b, byte[] bytes, int index)
        {
            b = bytes[index]==1?true:false;
            return index + 1;
        }

        public static int Encode(ref Guid guid, byte[] bytes, int index)
        {
            byte[] guidBytes = guid.ToByteArray();
            guidBytes.CopyTo(bytes, index);
            return index + 16;
        }

        public static int Decode(ref Guid guid, byte[] bytes, int index)
        {
            byte[] guidBytes = new byte[16];
            Array.Copy(bytes, index, guidBytes, 0, 16);
            guid = new Guid(guidBytes);
            return index + guidBytes.Length;
        }
    
    }
}
