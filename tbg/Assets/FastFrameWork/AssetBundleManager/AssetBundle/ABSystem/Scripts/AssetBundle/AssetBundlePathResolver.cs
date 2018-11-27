﻿using System.IO;
using UnityEngine;

namespace Tangzx.ABSystem
{
    /// <summary>
    /// AB 打包及运行时路径解决器
    /// </summary>
    public class AssetBundlePathResolver
    {
        public static AssetBundlePathResolver instance;

        /// <summary>
        /// bundle文件的名字
        /// </summary>
        public virtual string BundleName
        {
            get
            {
#if UNITY_EDITOR
                return "/Editor_AssetBundles";
#elif UNITY_IOS
                return "/IOS_AssetBundles";
#elif UNITY_ANDROID
                return "/Android_AssetBundles";
#endif
                return "/Normal_AssetBundles";
            }
        }

        public AssetBundlePathResolver()
        {
            instance = this;
        }

        /// <summary>
        /// AB 保存的路径相对于 Assets/StreamingAssets 的名字
        /// </summary>
        public virtual string BundleSaveDirName
        {
            get
            {
#if UNITY_EDITOR
                return "Editor_AssetBundles";
#elif UNITY_IOS
                return "IOS_AssetBundles";
#elif UNITY_ANDROID
                return "Android_AssetBundles";
#endif
                return "Normal_AssetBundles";
            }
        }

        /// <summary>
        /// AB 保存的路径
        /// </summary>
        public string BundleSavePath { get { return "Assets/StreamingAssets/" + BundleSaveDirName; } }
        /// <summary>
        /// AB打包的原文件HashCode要保存到的路径，下次可供增量打包
        /// </summary>
        /// <summary>
        /// 在编辑器模型下将 abName 转为 Assets/... 路径
        /// 这样就可以不用打包直接用了
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public virtual string GetEditorModePath(string abName)
        {
            //将 Assets.AA.BB.prefab 转为 Assets/AA/BB.prefab
            abName = abName.Replace(".", "/");
            int last = abName.LastIndexOf("/");

            if (last == -1)
                return abName;

            string path = string.Format("{0}.{1}", abName.Substring(0, last), abName.Substring(last + 1));
            return path;
        }

        /// <summary>
        /// 获取 AB 源文件路径（打包进安装包的）
        /// </summary>
        /// <param name="path"></param>
        /// <param name="forWWW"></param>
        /// <returns></returns>
        public virtual string GetBundleSourceFile(string path, bool forWWW = true)
        {
            string filePath = null;
#if UNITY_EDITOR
            if (forWWW)
                filePath = string.Format("file://{0}/StreamingAssets/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
            else
                filePath = string.Format("{0}/StreamingAssets/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
#elif UNITY_ANDROID
            if (forWWW)
                filePath = string.Format("jar:file://{0}!/assets/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
            else
                filePath = string.Format("{0}!assets/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
#elif UNITY_IOS
            if (forWWW)
                filePath = string.Format("file://{0}/Raw/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
            else
                filePath = string.Format("{0}/Raw/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
#else
            throw new System.NotImplementedException();
#endif
            return filePath;
        }


        DirectoryInfo cacheDir;

        /// <summary>
        /// 用于缓存AB的目录，要求可写
        /// </summary>
        public virtual string BundleCacheDir
        {
            get
            {
                if (cacheDir == null)
                {
#if UNITY_EDITOR
                    string dir = string.Format("{0}/{1}", Application.streamingAssetsPath, BundleSaveDirName);
#else
					string dir = string.Format("{0}/AssetBundles", Application.persistentDataPath);
#endif
                    cacheDir = new DirectoryInfo(dir);
                    if (!cacheDir.Exists)
                        cacheDir.Create();
                }
                return cacheDir.FullName;
            }
        }
    }
}