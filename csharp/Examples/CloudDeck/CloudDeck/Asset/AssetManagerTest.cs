using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace CloudDeck.Asset
{
    [TestFixture]
    public class AssetManagerTest
    {
        [Test]
        public void TestColladaModelLoading()
        {
            String tempDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"/Cache";
            AssetManager assetManager = new AssetManager(tempDirectory+"/temp/"+Guid.NewGuid());

            const string assetUrl = "http://assets.bubblecloud.org/Collada/Duck/duck_triangulate.dae";
            Guid assetId = Guid.Empty;
            string readyAssetUrl=null;
            assetManager.AssetDownloadSuccess = delegate(string loadedAssetUrl) { readyAssetUrl = loadedAssetUrl; };

            assetManager.Startup();

            assetId=assetManager.DownloadAsset(assetUrl);

            int i = 0;
            while (readyAssetUrl == null&&i<200)
            {
                Thread.Sleep(10);
                i++;
            }

            assetManager.Process();

            Assert.IsFalse(assetManager.IsAssetDownloading(assetUrl));
            Assert.IsTrue(assetManager.IsAssetDownloaded(assetUrl));

            Assert.IsNotNull(readyAssetUrl);
            Assert.AreEqual(assetUrl, readyAssetUrl);

            byte[] assetBytes = assetManager.GetAssetBytes(assetUrl);
            Assert.IsTrue(assetBytes.Length>0,"Asset bytes was empty.");

            assetManager.Shutdown();
        }

    }
}
