using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
using System.IO;

namespace MXP.Util
{
    public class CompressUtil
    {

        public static byte[] CompressHeightMap(float[] values, float offset, float scale)
        {
            byte[] bytes=new byte[values.Length*2];
            
            for (int i = 0; i < values.Length; i++)
            {
                short shortValue=(short)((values[i] - offset) * scale);
                EncodeUtil.Encode(ref shortValue,bytes,i*2);
            }

            return bytes;
            // Decompression not working with mono for some reason:
            // MXPTests.CompressionTest.HeightMapCompression : System.IO.InvalidDataException : Invalid ZLib data.
            /*using (MemoryStream compressedStream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    using (MemoryStream uncompressedStream=new MemoryStream(bytes))
                    {
                        CopyStream(uncompressedStream, zipStream);
                        return compressedStream.GetBuffer();
                    }
                }
            }*/
        }

        public static float[] DecompressHeightMap(byte[] compressedBytes, float offset, float scale)
        {
            byte[] bytes = compressedBytes;
            
            // Decompression not working with mono for some reason:
            // MXPTests.CompressionTest.HeightMapCompression : System.IO.InvalidDataException : Invalid ZLib data.
            /*using (MemoryStream memoryStream = new MemoryStream(compressedBytes))
            {
                using (GZipStream zipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        CopyStream(zipStream, outputStream);
                        bytes=outputStream.GetBuffer();
                    }
                }
            }*/ 

            float[] values = new float[bytes.Length / 2];
            for (int i = 0; i < bytes.Length; i+=2)
            {
                short shortValue=0;
                EncodeUtil.Decode(ref shortValue, bytes, i); ;
                values[i/2] = shortValue/scale+offset;                
            }
            return values;
        }


        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length); 
                if (read <= 0) return; 
                output.Write(buffer, 0, read);
            }
        }
    }
}
