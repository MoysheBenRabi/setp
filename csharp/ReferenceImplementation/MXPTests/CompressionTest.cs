using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP;
using MXP.Messages;
using MXP.Util;
using System.Diagnostics;

namespace MXPTests
{
    /// <summary>
    /// Summary description for TerrainCompressTest
    /// </summary>
    [TestFixture]
    public class CompressionTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void HeightMapCompression()
        {
            int width=256;
            int height=256;
            float[] heightmap=new float[width*height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    heightmap[x + y * width] = -x + y;
                }
            }

            byte[] compressedBytes = CompressUtil.CompressHeightMap(heightmap, 0, 10);
            float[] decompressedHeightMap = CompressUtil.DecompressHeightMap(compressedBytes, 0, 10);

            LogUtil.Debug("Original byte size: "+heightmap.Length*4);
            LogUtil.Debug("Compressed byte size: " + compressedBytes.Length);

            LogUtil.Debug("Compression ratio: " + (compressedBytes.Length * 100 / (heightmap.Length * 4))+"%");

            Assert.AreEqual(heightmap.Length, decompressedHeightMap.Length);
            for (int i = 0; i < heightmap.Length; i++)
            {
                Assert.AreEqual(heightmap[i], decompressedHeightMap[i]);
            }
        }

    }
}
