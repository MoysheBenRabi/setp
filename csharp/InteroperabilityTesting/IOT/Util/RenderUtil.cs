using System;
using System.Collections.Generic;
using System.Text;

namespace IOT.Util
{
    public class RenderUtil
    {
        public static string FixedWrapString(string str,int partLength)
        {
            char[] stringCharacters = str.ToCharArray(); 
            int totalLength = str.Length; 
            int partCount = (totalLength / partLength) + ((totalLength % partLength == 0) ? 0 : 1); 
            int index = 0; 
            char[] partCharacters = new char[partLength]; 
            string[] partStrings = new string[partCount]; 
            for (int i = 0; i < partCount; i++) { 
                int actualPartLength = Math.Min(partLength, totalLength - index); 
                Array.Copy(stringCharacters, index, partCharacters, 0, actualPartLength); 
                partStrings[i] = new string(partCharacters, 0, actualPartLength); 
                index += partLength; 
            }
            return string.Join("<br/>", partStrings) + "<br/>";
        }

        private const string HexCharacters = "0123456789ABCDEF";

        public static string RenderByteArray(byte[] bytes, List<int> colorIndexes, int rowLength)
        {
            StringBuilder stringBuilder=new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                byte b=bytes[i];

                if (i != 0 && i % rowLength == 0)
                {
                    stringBuilder.Append("<br/>");
                }

                if (i % rowLength != 0)
                {
                    stringBuilder.Append("-");
                }

                if (colorIndexes.Contains(i))
                {
                    stringBuilder.Append("<font color=\"#cc0000\">");
                }
                stringBuilder.Append(HexCharacters[b / 16]);
                stringBuilder.Append(HexCharacters[b % 16]);
                if (colorIndexes.Contains(i))
                {
                    stringBuilder.Append("</font>");
                }

            }

            return stringBuilder.ToString()+"</br> (Length: "+bytes.Length+")";
        }
    }
}
