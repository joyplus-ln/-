using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastAssetBundleManager : MonoBehaviour
{

    public AssetBundleManifest mainAssetBundleMainfest;

    /// <summary>
    /// 路径管理器
    /// </summary>
    public FastAssetBundlePathResolver pathResolver;

    public delegate void FastLoadAssetCompleteHandler(FastAssetBundleInfo info);
    public delegate void FastLoaderCompleteHandler(FastAssetBundleLoader info);
    public delegate void FastLoadProgressHandler(FastAssetBundleLoadProgress progress);


    /// <summary>
    /// 同时异步加载的bundle总数量，可以根据机型动态设置数量多少
    /// </summary>
    public const int MaxLoadingThread = 10;

    /// <summary>
    /// 正在加载中的loader，后进先出，优先执行后期加入堆的loader，先在家依赖的形式
    /// </summary>
    public Stack<FastAssetBundleLoader> onloadingbundleLoader = new Stack<FastAssetBundleLoader>();

    /// <summary>
    /// 候补队列，当加载的数量大于约定值的时候，放在这里,需要加载的loader，后进先出，优先执行后期加入堆的loader，先在家依赖的形式
    /// </summary>
    public Queue<FastAssetBundleLoader> waitloadingbundleLoader = new Queue<FastAssetBundleLoader>();


    /// <summary>
    /// 所有assetbundleloader，包括加载了的和未加载的，为了给重复调用加载相同对象做缓存，后期添加等级 0 永远不释放，1 切场景释放，2使用完就立刻释放
    /// </summary>
    public Dictionary<string, FastAssetBundleLoader> AllAssetBundleLoaders = new Dictionary<string, FastAssetBundleLoader>();
    /// <summary>
    /// 加载bundle
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    /// <param name="prority"></param>
    public FastAssetBundleLoader LoadBundle(string path, FastLoadAssetCompleteHandler callback = null, int prority = 0)
    {
        Init();
        FastAssetBundleLoader loader = CreateLoader(path + FastContent.BundleSuffix, path);
        loader.AddListener(callback);
        loader.Start();
        return loader;
    }

    private void Update()
    {
        QueueCheck();
    }

    /// <summary>
    /// 最底层的依赖项应该在最优先加载的栈顶
    /// </summary>
    /// <param name="dependLoader"></param>
    public void AddDependBundle(FastAssetBundleLoader dependLoader)
    {
        waitloadingbundleLoader.Enqueue(dependLoader);
    }
    /// <summary>
    /// 检查下载情况，发现有需要下载的就立马跟上
    /// </summary>
    void QueueCheck()
    {
        //如果下载线程没有达到最大量，等待线程又有需要下载项
        if (onloadingbundleLoader.Count < MaxLoadingThread && waitloadingbundleLoader.Count > 0)
        {
            for (int i = 0; i < MaxLoadingThread; i++)
            {
                FastAssetBundleLoader loader = waitloadingbundleLoader.Dequeue();
                onloadingbundleLoader.Push(loader);
                loader.LoadBundle();
                if (waitloadingbundleLoader.Count <= 0)
                    break;
            }
        }
    }

    #region 初始化相关
    public FastAssetBundleManager()
    {
        pathResolver = new FastAssetBundlePathResolver();
    }

    /// <summary>
    /// 最好先执行初始化操作
    /// </summary>
    public void Init()
    {
        if (mainAssetBundleMainfest == null)
        {
            AssetBundle ab = AssetBundle.LoadFromFile(pathResolver.BundleSavePath + pathResolver.BundleName);
            mainAssetBundleMainfest = ab.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
        }

    }
    #endregion






    #region 工具相关


    internal FastAssetBundleLoader CreateLoader(string abFileName, string oriName = null)
    {
        FastAssetBundleLoader loader = null;

        if (AllAssetBundleLoaders.ContainsKey(abFileName))
        {
            loader = AllAssetBundleLoaders[abFileName];
        }
        else
        {


            loader = this.CreateLoader();
            loader.bundleManager = this;
            loader.Main_Manifest = this.mainAssetBundleMainfest;
            loader.bundleName = abFileName;

            AllAssetBundleLoaders.Add(abFileName, loader);
        }

        return loader;
    }

    internal FastAssetBundleInfo CreateBundleInfo(FastAssetBundleLoader loader, FastAssetBundleInfo abi = null, AssetBundle assetBundle = null)
    {
        if (abi == null)
            abi = new FastAssetBundleInfo();
        abi.bundleName = loader.bundleName.ToLower();
        abi.bundle = assetBundle;
        return abi;
    }
    protected virtual FastAssetBundleLoader CreateLoader()
    {
#if UNITY_EDITOR
        return new FastEditorModeAssetBundleLoader();
#elif UNITY_IOS
            return new FastIOSAssetBundleLoader();
#elif UNITY_ANDROID
            return new FastMobileAssetBundleLoader();
#else
            return new FastMobileAssetBundleLoader();
#endif
    }
    #endregion
}
