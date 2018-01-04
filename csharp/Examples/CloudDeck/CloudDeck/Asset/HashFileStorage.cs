using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CloudDeck.Asset
{
    // Persistent data storage on file system.
    public class HashFileStorage
    {
        private string m_storagePath;
        public string StoragePath
        {
            get
            {
                return m_storagePath;
            }
        }

        public HashFileStorage(string storagePath)
        {
            if(storagePath.EndsWith("/"))
            {
                throw new Exception("No tailing / allowed in storage path.");
            }
            m_storagePath = storagePath;
            if(!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }
        }

        public void StoreStream(string key,Stream inputStream)
        {
            string filePath = m_storagePath + "/" + GetPath(key, true);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                int readBytes = 0;
                byte[] buffer = new byte[8096];
                while ((readBytes = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, readBytes);
                }
            }
        }

        public void StoreBytes(string key, byte[] bytes)
        {
            string filePath = m_storagePath + "/" + GetPath(key, true);
            File.WriteAllBytes(filePath, bytes);
        }

        public void StoreAsciiString(string key, string str)
        {
            string filePath = m_storagePath + "/" + GetPath(key, true);
            File.WriteAllBytes(filePath, ASCIIEncoding.ASCII.GetBytes(str));
        }


        public Stream LoadStream(string key)
        {
            string filePath = m_storagePath + "/" + GetPath(key, false);
            if(filePath==null)
            {
                return null;
            }
            return new FileStream(filePath,FileMode.Open, FileAccess.Read, FileShare.Read);            
        }

        public Stream SaveStream(string key)
        {
            string filePath = m_storagePath + "/" + GetPath(key, false);
            if (filePath == null)
            {
                return null;
            }
            return new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        }

        public byte[] LoadBytes(string key)
        {
            string filePath = m_storagePath + "/" + GetPath(key, false);
            if (filePath == null)
            {
                return null;
            }
            return File.ReadAllBytes(filePath);            
        }

        public string LoadAsciiString(string key)
        {
            string filePath = m_storagePath + "/" + GetPath(key, false);
            if (filePath == null)
            {
                return null;
            }
            return ASCIIEncoding.ASCII.GetString(File.ReadAllBytes(filePath));    
        }

        public void DeleteFile(string key)
        {
            string filePath = m_storagePath + "/" + GetPath(key, false);
            if (filePath != null)
            {
                File.Delete(filePath);
            }
        }

        public bool FileExists(string key)
        {
            return GetPath(key, false)!=null;
        }

        public DateTime FileLastModified(string key)
        {
            string filePath = m_storagePath + "/" + GetPath(key, false);
            if (filePath != null)
            {
                return DateTime.MinValue;
            }
            return File.GetLastWriteTime(key);
        }

        public string GetPath(string key, bool isCreate)
        {
            int hash = key.GetHashCode();
            int levelOne = hash%256;
            hash = hash/256;
            int levelTwo = hash % 256;
            hash = hash / 256;
            int levelThree = hash % 256;
            hash = hash / 256;
            int levelFour = hash % 256;
            hash = hash / 256;

            String path = levelOne.ToString();
            if(!Directory.Exists(m_storagePath + "/"+path))
            {
                if (isCreate)
                {
                    Directory.CreateDirectory(m_storagePath + "/" + path);
                }
                else
                {
                    return null;
                }
            }
            path = path + "/" + levelTwo;
            if (!Directory.Exists(m_storagePath + "/" + path))
            {
                if (isCreate)
                {
                    Directory.CreateDirectory(m_storagePath + "/" + path);
                }
                else
                {
                    return null;
                }
            }
            path = path + "/" + levelThree;
            if (!Directory.Exists(m_storagePath + "/" + path))
            {
                if (isCreate)
                {
                    Directory.CreateDirectory(m_storagePath + "/" + path);
                }
                else
                {
                    return null;
                }
            }
            path = path + "/" + levelFour;
            if (!Directory.Exists(m_storagePath + "/" + path))
            {
                if (isCreate)
                {
                    Directory.CreateDirectory(m_storagePath + "/" + path);
                }
                else
                {
                    return null;
                }
            }
            path = path + "/" + Uri.EscapeDataString(key);
            if (!File.Exists(m_storagePath + "/" + path))
            {
                if(!isCreate)
                {
                    return null;
                }
            }
            return path;
        }

    }
}
