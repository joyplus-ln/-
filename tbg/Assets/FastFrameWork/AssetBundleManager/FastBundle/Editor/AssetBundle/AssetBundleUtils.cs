using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;



    class AssetCacheInfo
    {
        /// <summary>
        /// 源文件的hash，比较变化
        /// </summary>
        public string fileHash;
        /// <summary>
        /// 源文件meta文件的hash，部分类型的素材需要结合这个来判断变化
        /// 如：Texture
        /// </summary>
        public string metaHash;
        /// <summary>
        /// 上次打好的AB的CRC值，用于增量判断
        /// </summary>
        public string bundleCrc;
        /// <summary>
        /// 所依赖的那些文件
        /// </summary>
        public string[] depNames;
    }

    class FastAssetBundleUtils
{
        public static FastAssetBundlePathResolver pathResolver;
        public static DirectoryInfo AssetDir = new DirectoryInfo(Application.dataPath);
        public static string AssetPath = AssetDir.FullName;
        public static DirectoryInfo ProjectDir = AssetDir.Parent;
        public static string ProjectPath = ProjectDir.FullName;

        static Dictionary<int, FastAssetTarget> _object2target;
        static Dictionary<string, FastAssetTarget> _assetPath2target;
        static Dictionary<string, string> _fileHashCache;
        static Dictionary<string, AssetCacheInfo> _fileHashOld;

        public static void Init()
        {
            _object2target = new Dictionary<int, FastAssetTarget>();
            _assetPath2target = new Dictionary<string, FastAssetTarget>();
            _fileHashCache = new Dictionary<string, string>();
            _fileHashOld = new Dictionary<string, AssetCacheInfo>();
        }

        public static void ClearCache()
        {
            _object2target = null;
            _assetPath2target = null;
            _fileHashCache = null;
            _fileHashOld = null;
        }


        public static List<FastAssetTarget> GetAll()
        {
            return new List<FastAssetTarget>(_object2target.Values);
        }


        public static FastAssetTarget Load(FileInfo file, System.Type t)
        {
        FastAssetTarget target = null;
            string fullPath = file.FullName;
            int index = fullPath.IndexOf("Assets");
            if (index != -1)
            {
                string assetPath = fullPath.Substring(index);
                if (_assetPath2target.ContainsKey(assetPath))
                {
                    target = _assetPath2target[assetPath];
                }
                else
                {
                    Object o = null;
                    if (t == null)
                        o = AssetDatabase.LoadMainAssetAtPath(assetPath);
                    else
                        o = AssetDatabase.LoadAssetAtPath(assetPath, t);

                    if (o != null)
                    {
                        int instanceId = o.GetInstanceID();

                        if (_object2target.ContainsKey(instanceId))
                        {
                            target = _object2target[instanceId];
                        }
                        else
                        {
                            target = new FastAssetTarget(o, file, assetPath);
                            string key = string.Format("{0}/{1}", assetPath, instanceId);
                            _assetPath2target[key] = target;
                            _object2target[instanceId] = target;
                        }
                    }
                }
            }

            return target;
        }

        public static FastAssetTarget Load(FileInfo file)
        {
            return Load(file, null);
        }

        public static string ConvertToABName(string assetPath)
        {
            string bn = assetPath
                .Replace(AssetPath, "")
                .Replace('\\', '.')
                .Replace('/', '.')
                .Replace(" ", "_")
                .ToLower();
            return bn;
        }

        public static string GetFileHash(string path, bool force = false)
        {
            string _hexStr = null;
            if (_fileHashCache.ContainsKey(path) && !force)
            {
                _hexStr = _fileHashCache[path];
            }
            else if (File.Exists(path) == false)
            {
                _hexStr = "FileNotExists";
            }
            else
            {
                FileStream fs = new FileStream(path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read);

                _hexStr = FastHashUtil.Get(fs);
                _fileHashCache[path] = _hexStr;
                fs.Close();
            }
            
            return _hexStr;
        }

        public static AssetCacheInfo GetCacheInfo(string path)
        {
            if (_fileHashOld.ContainsKey(path))
                return _fileHashOld[path];
            return null;
        }
    }

