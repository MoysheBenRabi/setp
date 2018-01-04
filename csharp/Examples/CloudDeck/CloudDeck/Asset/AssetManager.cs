using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using MXP.Util;
using System.Xml;
using Horde3DNET;

namespace CloudDeck.Asset
{
    /// <summary>
    /// AssetManager downloads, converts and caches assets used by rendering pipeline.
    /// </summary>
    public class AssetManager
    {

        #region Fields
        
        public AssetDownloadSuccess AssetDownloadSuccess = delegate(string assetUrl) { };
        public AssetDownloadFailure AssetDownloadFailure = delegate(string assetUrl) { };

        private HashSet<string> m_downloadingAssetUrls=new HashSet<string>();

        private Queue<string> m_downloadQueue=new Queue<string>();
        private Queue<string> m_downloadSuccessQueue=new Queue<string>();
        private Queue<string> m_downloadFailureQueue = new Queue<string>();
        
        private HashFileStorage m_assetStorage;
        private HashFileStorage m_urlIdStorage;

        private Thread m_backgroundThread;
        private bool m_isShutdownRequested = false;

        private HashSet<string> m_blackListedAssetUrls = new HashSet<string>();
        public IDictionary<string, int> m_modelUrlHordeIdDictionary = new Dictionary<string, int>();

        #endregion

        #region Constructor
        
        public AssetManager(string storagePath)
        {
            if(!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }
            m_urlIdStorage = new HashFileStorage(storagePath+"/url-ids");
            m_assetStorage = new HashFileStorage(storagePath+"/asset-files");
            m_backgroundThread=new Thread(BackgroundProcess);
        }
        
        #endregion

        #region Startup and Shutdown
        
        public void Startup()
        {
            m_backgroundThread.Start();
        }

        public void Shutdown()
        {
            m_isShutdownRequested = true;
            if (m_backgroundThread.IsAlive)
            {
                Thread.Sleep(100);
            }
        }
        
        #endregion

        #region Processing

        public void Process()
        {
            lock (m_downloadSuccessQueue)
            {
                if (m_downloadSuccessQueue.Count > 0)
                {
                    String url = m_downloadSuccessQueue.Dequeue();
                    lock (m_downloadingAssetUrls)
                    {
                        m_downloadingAssetUrls.Remove(url);
                    }
                    AssetDownloadSuccess(url);
                }
            }
            lock (m_downloadFailureQueue)
            {
                if (m_downloadFailureQueue.Count > 0)
                {
                    String url = m_downloadFailureQueue.Dequeue();
                    lock (m_downloadingAssetUrls)
                    {
                        m_downloadingAssetUrls.Remove(url);
                    }
                    m_blackListedAssetUrls.Add(url);
                    AssetDownloadFailure(url);
                }
            }
        }

        private void BackgroundProcess()
        {
            while (!m_isShutdownRequested)
            {
                string url = null;
                lock (m_downloadQueue)
                {
                    if (m_downloadQueue.Count > 0)
                    {
                        url = m_downloadQueue.Dequeue();
                    }
                }
                if (url != null)
                {
                    try
                    {
                        HttpGetAsset(url);
                        lock (m_downloadSuccessQueue)
                        {
                            m_downloadSuccessQueue.Enqueue(url);
                        }
                    }
                    catch (Exception e)
                    {
                        lock (m_downloadFailureQueue)
                        {
                            m_downloadFailureQueue.Enqueue(url);
                        }
                        LogUtil.Error("Error loading asset " + url + ": " + e.ToString());
                    }
                }
                Thread.Sleep(20);
            }
        }

        #endregion

        #region Public Interface

        public bool IsBlackListed(string url)
        {
            lock (m_blackListedAssetUrls)
            {
                return m_blackListedAssetUrls.Contains(url);
            }
        }

        public bool IsAssetDownloading(string url)
        {
            lock (m_downloadingAssetUrls)
            {
                return m_downloadingAssetUrls.Contains(url);
            }
        }

        public void EnsureModelIsLoadedToRenderer(string modelUrl)
        {
            lock (m_modelUrlHordeIdDictionary)
            {
                if (!m_modelUrlHordeIdDictionary.ContainsKey(modelUrl))
                {
                    string modelFolder;
                    string scenePath;
                    GetHordePaths(modelUrl, out modelFolder, out scenePath);
                    int modelHordeId = h3d.addResource((int)h3d.H3DResTypes.SceneGraph, scenePath, 0);
                    Horde3DNET.Utils.Horde3DUtils.loadResourcesFromDisk(modelFolder);
                    m_modelUrlHordeIdDictionary.Add(modelUrl, modelHordeId);
                }
            }
        }

        public int GetAssetRendererId(string modelUrl)
        {
            lock (m_modelUrlHordeIdDictionary)
            {
                return m_modelUrlHordeIdDictionary[modelUrl];
            }
        }

        public bool IsAssetDownloaded(string url)
        {
            lock (m_downloadingAssetUrls)
            {
                if(m_downloadingAssetUrls.Contains(url))
                {
                    return false;
                }
            }
            return m_urlIdStorage.FileExists(url)&&m_assetStorage.FileExists(m_urlIdStorage.LoadAsciiString(url)+GetFileExtension(url));
        }

        public Guid DownloadAsset(String url)
        {
            lock (m_downloadingAssetUrls)
            {
                if (m_downloadingAssetUrls.Contains(url))
                {
                    return new Guid(m_urlIdStorage.LoadAsciiString(url));
                }

                Guid fileId = Guid.NewGuid();
                lock (m_downloadQueue)
                {
                    m_downloadQueue.Enqueue(url);
                    m_urlIdStorage.StoreAsciiString(url, fileId.ToString());
                }
                m_downloadingAssetUrls.Add(url);
                return fileId;                
            }
        }

        public Stream GetAssetStream(string url)
        {
            string assetId = m_urlIdStorage.LoadAsciiString(url);
            return m_assetStorage.LoadStream(assetId + GetFileExtension(url));
        }

        public byte[] GetAssetBytes(string url)
        {
            string assetId = m_urlIdStorage.LoadAsciiString(url);
            return m_assetStorage.LoadBytes(assetId + GetFileExtension(url));
        }

        public void GetHordePaths(string modelUrl, out string modelDirectory, out string scenePath)
        {
            string modelId = m_urlIdStorage.LoadAsciiString(modelUrl);
            string modifiedModelPath =m_assetStorage.StoragePath+"/"+m_assetStorage.GetPath(modelId.ToString() + GetFileExtension(modelUrl), false);
            modelDirectory = modifiedModelPath.Substring(0, modifiedModelPath.LastIndexOf('/')) + "/" + modelId;
            scenePath = "models/" + modelId + "/" + modelId + ".scene.xml";
        }

        #endregion

        #region Asset Downloading

        private void DownloadAndMapColladaAssets(String modelUrl, Guid modelId)
        {

            LogUtil.Debug("Loading collada assets for: " + modelUrl);

            XmlDocument document = new XmlDocument();
            IDictionary<string, Guid> images = new Dictionary<string, Guid>();
            using (
                Stream documentInputStream = m_assetStorage.LoadStream(modelId.ToString() + GetFileExtension(modelUrl)))
            {
                document.Load(documentInputStream);
                foreach (XmlNode libraryImages in document.GetElementsByTagName("library_images"))
                {
                    foreach (XmlNode image in libraryImages.ChildNodes)
                    {
                        foreach (XmlNode node in image.ChildNodes)
                        {
                            if (node.Name == "init_from")
                            {
                                String assetRelativeUrl = node.InnerXml;
                                Uri assetUri = new Uri(new Uri(modelUrl), assetRelativeUrl);
                                String assetAbsoluteUrl = assetUri.AbsoluteUri;
                                Guid assetId = DownloadAssetSynchronously(assetAbsoluteUrl);
                                String assetLocalPath = GetAssetPath(assetAbsoluteUrl, assetId);
                                images.Add(assetAbsoluteUrl, assetId);
                                node.InnerXml = "../../../../../" + assetLocalPath;
                                LogUtil.Debug(assetRelativeUrl + "->" + assetAbsoluteUrl + "->" + assetLocalPath);
                            }
                        }
                    }
                }
            }

            // Saving collada files with mapped image locations
            string modifiedModelPath = GetMappedColladaModelPath(modelUrl, modelId);
            document.Save(modifiedModelPath);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = "\"" + modifiedModelPath + "\"";
            startInfo.FileName = "ColladaConv.exe";
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;

            // Executing collada to horde3d conversion
            System.Diagnostics.Process conversionProcess = System.Diagnostics.Process.Start(startInfo);
            conversionProcess.WaitForExit();

            // Copying images to conversion output because conversion does not allow referencing images from original location
            string convertedModelDirectory = Directory.GetParent(modifiedModelPath).ToString();
            convertedModelDirectory += "/models/" + modelId;

            foreach (string imageUrl in images.Keys)
            {
                Guid imageId = images[imageUrl];
                String sourcePath = m_assetStorage.StoragePath + "/" + GetAssetPath(imageUrl, imageId);
                String targetPath = convertedModelDirectory + "/" + imageId + GetFileExtension(imageUrl);
                File.Copy(sourcePath, targetPath, true);
            }

        }

        private void HttpGetAsset(String url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            WebResponse webResponse = webRequest.GetResponse();
            Guid assetId = new Guid(m_urlIdStorage.LoadAsciiString(url));
            using (Stream responseStream = webResponse.GetResponseStream())
            {
                m_assetStorage.StoreStream(assetId.ToString() + GetFileExtension(url), responseStream);
            }
            if (url.EndsWith(".dae"))
            {
                DownloadAndMapColladaAssets(url, assetId);
            }
        }

        private Guid DownloadAssetSynchronously(String url)
        {
            lock (m_downloadingAssetUrls)
            {
                if (m_downloadingAssetUrls.Contains(url))
                {
                    return new Guid(m_urlIdStorage.LoadAsciiString(url));
                }
                m_downloadingAssetUrls.Add(url);
            }

            Guid fileId = Guid.NewGuid();
            m_urlIdStorage.StoreAsciiString(url, fileId.ToString());

            try
            {
                HttpGetAsset(url);
            }
            catch (Exception e)
            {
                LogUtil.Error("Error loading asset " + url + ": " + e.ToString());
            }

            lock (m_downloadingAssetUrls)
            {
                m_downloadingAssetUrls.Remove(url);
            }
            return fileId;
        }

        #endregion

        #region Path Forming

        private string GetAssetPath(string url, Guid assetId)
        {
            return m_assetStorage.GetPath(assetId.ToString() + GetFileExtension(url), true);
        }

        private string GetMappedColladaModelPath(string modelUrl, Guid modelId)
        {
            string modifiedModelPath = m_assetStorage.StoragePath + "/" + m_assetStorage.GetPath(modelId.ToString() + GetFileExtension(modelUrl), false);
            modifiedModelPath = Directory.GetParent(modifiedModelPath).ToString() + "/" + modelId;
            if (!Directory.Exists(modifiedModelPath))
            {
                Directory.CreateDirectory(modifiedModelPath);
            }
            modifiedModelPath += "/" + modelId + ".dae";
            return modifiedModelPath;
        }

        private string GetFileExtension(string url)
        {
            Uri uri=new Uri(url);
            string path = uri.AbsolutePath;
            if (path.Contains("."))
            {
                return path.Substring(path.LastIndexOf('.'));
            }
            else
            {
                return "";
            }
        }

        #endregion

    }
}
