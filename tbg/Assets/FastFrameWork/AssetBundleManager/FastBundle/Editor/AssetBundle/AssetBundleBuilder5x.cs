﻿#if UNITY_5 || UNITY_2017_1_OR_NEWER
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


    public class AssetBundleBuilder5x : ABBuilder
    {
        public AssetBundleBuilder5x(FastAssetBundlePathResolver resolver)
            : base(resolver)
        {

        }

        public override void Export()
        {
            base.Export();

            List<AssetBundleBuild> list = new List<AssetBundleBuild>();
            //标记所有 asset bundle name
            var all = FastAssetBundleUtils.GetAll();
            for (int i = 0; i < all.Count; i++)
            {
            FastAssetTarget target = all[i];
                if (target.needSelfExport)
                {
                    AssetBundleBuild build = new AssetBundleBuild();
                    build.assetBundleName = target.bundleName;
                    build.assetNames = new string[] { target.assetPath };
                    list.Add(build);
                }
            }

            //开始打包
            BuildPipeline.BuildAssetBundles(pathResolver.BundleSavePath, list.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);

            AssetBundle ab = AssetBundle.LoadFromFile(pathResolver.BundleSavePath + pathResolver.BundleName);

            AssetBundleManifest manifest = ab.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            //hash
            for (int i = 0; i < all.Count; i++)
            {
            FastAssetTarget target = all[i];
                if (target.needSelfExport)
                {
                    Hash128 hash = manifest.GetAssetBundleHash(target.bundleName);
                    target.bundleCrc = hash.ToString();
                }
            }
            ab.Unload(true);
            this.RemoveUnused(all);

            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetDatabase.Refresh();
        }
    }
#endif