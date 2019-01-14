using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;



    public class ABBuilder
    {
        protected FastAssetBundlePathResolver pathResolver;

        public ABBuilder() : this(new FastAssetBundlePathResolver())
        {

        }

        public ABBuilder(FastAssetBundlePathResolver resolver)
        {
            this.pathResolver = resolver;
            this.InitDirs();
        FastAssetBundleUtils.pathResolver = pathResolver;
        }

        void InitDirs()
        {
            new DirectoryInfo(pathResolver.BundleSavePath).Create();
        }

        public void Begin()
        {
            EditorUtility.DisplayProgressBar("Loading", "Loading...", 0.1f);
        FastAssetBundleUtils.Init();
        }

        public void End()
        {
        FastAssetBundleUtils.ClearCache();
            EditorUtility.ClearProgressBar();
        }

        public virtual void Analyze()
        {
            var all = FastAssetBundleUtils.GetAll();
            foreach (FastAssetTarget target in all)
            {
                target.Analyze();
            }
            all = FastAssetBundleUtils.GetAll();
            foreach (FastAssetTarget target in all)
            {
                target.Merge();
            }
            all = FastAssetBundleUtils.GetAll();
            foreach (FastAssetTarget target in all)
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
                FastAssetTarget target = FastAssetBundleUtils.Load(file);
                    target.exportType = FastAssetBundleExportType.Root;
                }
            }
        }


        /// <summary>
        /// 删除未使用的AB，可能是上次打包出来的，而这一次没生成的
        /// </summary>
        /// <param name="all"></param>
        protected void RemoveUnused(List<FastAssetTarget> all)
        {
            HashSet<string> usedSet = new HashSet<string>();
            for (int i = 0; i < all.Count; i++)
            {
            FastAssetTarget target = all[i];
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

