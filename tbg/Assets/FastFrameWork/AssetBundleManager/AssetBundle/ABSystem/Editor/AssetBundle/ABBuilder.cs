using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tangzx.ABSystem
{
    public class ABBuilder
    {
        protected AssetBundlePathResolver pathResolver;

        public ABBuilder() : this(new AssetBundlePathResolver())
        {

        }

        public ABBuilder(AssetBundlePathResolver resolver)
        {
            this.pathResolver = resolver;
            this.InitDirs();
            AssetBundleUtils.pathResolver = pathResolver;
        }

        void InitDirs()
        {
            new DirectoryInfo(pathResolver.BundleSavePath).Create();
        }

        public void Begin()
        {
            EditorUtility.DisplayProgressBar("Loading", "Loading...", 0.1f);
            AssetBundleUtils.Init();
        }

        public void End()
        {
            AssetBundleUtils.ClearCache();
            EditorUtility.ClearProgressBar();
        }

        public virtual void Analyze()
        {
            var all = AssetBundleUtils.GetAll();
            foreach (AssetTarget target in all)
            {
                target.Analyze();
            }
            all = AssetBundleUtils.GetAll();
            foreach (AssetTarget target in all)
            {
                target.Merge();
            }
            all = AssetBundleUtils.GetAll();
            foreach (AssetTarget target in all)
            {
                target.BeforeExport();
            }
        }

        public virtual void Export()
        {
            this.Analyze();
        }

        public void AddRootTargets(DirectoryInfo bundleDir, string[] partterns = null, SearchOption searchOption = SearchOption.AllDirectories)
        {
            if (partterns == null)
                partterns = new string[] { "*.*" };
            for (int i = 0; i < partterns.Length; i++)
            {
                FileInfo[] prefabs = bundleDir.GetFiles(partterns[i], searchOption);
                foreach (FileInfo file in prefabs)
                {
                    if (file.Extension.Contains("meta"))
                        continue;
                    AssetTarget target = AssetBundleUtils.Load(file);
                    target.exportType = AssetBundleExportType.Root;
                }
            }
        }


        /// <summary>
        /// 删除未使用的AB，可能是上次打包出来的，而这一次没生成的
        /// </summary>
        /// <param name="all"></param>
        protected void RemoveUnused(List<AssetTarget> all)
        {
            HashSet<string> usedSet = new HashSet<string>();
            for (int i = 0; i < all.Count; i++)
            {
                AssetTarget target = all[i];
                if (target.needSelfExport)
                    usedSet.Add(target.bundleName);
            }

            DirectoryInfo di = new DirectoryInfo(pathResolver.BundleSavePath);
            FileInfo[] abFiles = di.GetFiles("*" + FastContent.BundleSuffix);
            for (int i = 0; i < abFiles.Length; i++)
            {
                FileInfo fi = abFiles[i];
                if (usedSet.Add(fi.Name))
                {
                    Debug.Log("Remove unused AB : " + fi.Name);

                    fi.Delete();
                    //for U5X
                    File.Delete(fi.FullName + ".manifest");
                }
            }
        }
    }
}
