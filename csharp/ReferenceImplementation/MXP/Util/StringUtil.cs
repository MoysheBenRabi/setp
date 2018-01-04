using System;
using System.Collections.Generic;
using System.Text;

namespace MXP.Util
{
    public class StringUtil
    {
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
