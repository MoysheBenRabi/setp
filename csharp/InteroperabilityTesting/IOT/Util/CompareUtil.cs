using System;
using System.Collections.Generic;
using System.Text;

namespace IOT.Util
{
    public class CompareUtil
    {
        public static List<int> FindFirstDifference(byte[] bytes1, byte[] bytes2)
        {
            int maxLength = Math.Max(bytes1.Length, bytes2.Length);
            List<int> diffIndexes = new List<int>();
            for (int i = 0; i < maxLength; i++)
            {
                if (i >= bytes1.Length)
                {
                    diffIndexes.Add(i);
                    continue;
                }
                if (i >= bytes2.Length)
                {
                    diffIndexes.Add(i);
                    continue;
                }
                if (bytes1[i] != bytes2[i])
                {
                    diffIndexes.Add(i);
                }
            }
            return diffIndexes;
        }
    }
}
