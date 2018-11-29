using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum FastLoadState
{
    State_None,
    State_Error,
    State_Complete,
}
/// <summary>
/// Loader 父类
/// </summary>
[Serializable]
public abstract class FastAssetBundleLoader
{
    static int idCounter = 0;

    protected FastAssetBundleManager.FastLoadAssetCompleteHandler onComplete;
    public string bundleName;
    public FastAssetBundleInfo bundleInfo;
    public FastAssetBundleManager bundleManager;
    public FastLoadState state = FastLoadState.State_None;

    protected FastAssetBundleLoader[] depLoaders;



    /// <summary>
    /// 加载完成后赋值给bundleInfo 然后置空
    /// </summary>
    protected AssetBundle _bundle;
    protected bool _hasError;
    protected string _assetBundleCachedFile;
    protected string _assetBundleSourceFile;

    private bool loading = false;


    /// <summary>
    /// 当前总体bundle的mainfest文件
    /// </summary>
    public AssetBundleManifest Main_Manifest;

    public FastAssetBundleLoader()
    {
        id = idCounter++;
    }



    /// <summary>
    /// 其它都准备好了，加载AssetBundle
    /// 注意：这个方法只能被 AssetBundleManager 调用
    /// 由 Manager 统一分配加载时机，防止加载过卡
    /// </summary>
    public void LoadBundle()
    {
        _assetBundleCachedFile = string.Format("{0}/{1}", bundleManager.pathResolver.BundleCacheDir, bundleName);
        _assetBundleSourceFile = bundleManager.pathResolver.GetBundleSourceFile(bundleName);

        if (File.Exists(_assetBundleCachedFile))
            bundleManager.StartCoroutine(LoadFromCachedFile());
        else
            bundleManager.StartCoroutine(LoadFromPackage());
    }

    public int id
    {
        get; private set;
    }


    /// <summary>
    /// 从已缓存的文件里加载
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator LoadFromCachedFile()
    {
        if (state != FastLoadState.State_Error)
        {

            AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(_assetBundleCachedFile);
            yield return req;
            _bundle = req.assetBundle;

            this.Complete();
        }
    }

    /// <summary>
    /// 从源文件(安装包里)加载
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator LoadFromPackage()
    {
        if (state != FastLoadState.State_Error)
        {
            //加载主体
            WWW www = new WWW(_assetBundleSourceFile);
            yield return www;

            //加载完缓存一份，便于下次快速加载
            if (www.error == null)
            {
                File.WriteAllBytes(_assetBundleCachedFile, www.bytes);

                _bundle = www.assetBundle;

                Complete();
            }
            else
            {
                Error();
            }

            www.Dispose();
            www = null;
        }
    }

    /// <summary>
    /// 检查是否已经完成了所有的依赖项加载，如果完成了所有的依赖项加载就开始加载自己本身，否则就继续等待其他依赖项加载
    /// </summary>
    /// <param name="abi"></param>
    void OnDepComplete(FastAssetBundleInfo abi)
    {
        //依赖项都加在完了,开始加载自己
        if (CheckDepComplete())
        {
            StartLoadSelfBundle();
        }
    }

    /// <summary>
    /// 检查所有依赖项是否都已经加载完毕了
    /// </summary>
    bool CheckDepComplete()
    {
        if (depLoaders == null)
        {
            //为null可能是没有依赖项
            Debug.Log("为null可能是没有依赖项");
            return true;
        }
        bool allComplete = true;
        for (int i = 0; i < depLoaders.Length; i++)
        {
            if (!depLoaders[i].isComplete)
            {
                allComplete = false;
            }
        }
        return allComplete;
    }



    private void OnBundleUnload(FastAssetBundleInfo abi)
    {
        this.bundleInfo = null;
        this.state = FastLoadState.State_None;
    }



    public virtual bool isComplete
    {
        get
        {
            return state == FastLoadState.State_Error || state == FastLoadState.State_Complete;
        }
    }



    protected virtual void Complete()
    {
        if (bundleInfo == null)
        {
            this.state = FastLoadState.State_Complete;

            this.bundleInfo = bundleManager.CreateBundleInfo(this, null, _bundle);
            this.bundleInfo.isReady = true;
            this.bundleInfo.onUnloaded = OnBundleUnload;

            _bundle = null;
        }
        FireEvent();
    }

    protected virtual void Error()
    {
        _hasError = true;
        this.state = FastLoadState.State_Error;
        this.bundleInfo = null;
        FireEvent();
    }

    public void AlreadyComplete()
    {
        
    }

    public void FireEvent()
    {
        if (onComplete != null)
        {
            var handler = onComplete;
            onComplete = null;
            handler(bundleInfo);
        }
    }

    /// <summary>
    /// 加载完成的回调
    /// </summary>
    public void AddListener(FastAssetBundleManager.FastLoadAssetCompleteHandler handler)
    {
        //如果已经下载完成了直接返回bundle完毕事件，否则就去加载并监听
        if (isComplete)
        {
            if (handler != null)
                handler.Invoke(bundleInfo);
        }
        else
        {
            onComplete += handler;
        }
    }


    ///// <summary>
    ///// 开始加载依赖项，如果有依赖项的话
    ///// </summary>
    public virtual void Start()
    {
        if (loading) return;
        loading = true;
        if (!isComplete)
        {
            if (HasDepensd())
            {
                this.LoadDepends();
            }
            else
            {
                StartLoadSelfBundle();
            }
        }
    }
    /// <summary>
    /// 依赖项加载完毕或者没有依赖项的情况下
    /// </summary>
    protected void StartLoadSelfBundle()
    {
        bundleManager.AddDependBundle(this);
    }
    /// <summary>
    /// 先加载依赖项
    /// </summary>
    void LoadDepends()
    {
        if (depLoaders == null)
        {
            string[] dependencies = GetDepensdList();
            depLoaders = new FastAssetBundleLoader[dependencies.Length];
            for (int i = 0; i < dependencies.Length; i++)
            {
                depLoaders[i] = bundleManager.CreateLoader(dependencies[i]);
            }
        }

        for (int i = 0; i < depLoaders.Length; i++)
        {
            FastAssetBundleLoader depLoader = depLoaders[i];
            if (!depLoader.isComplete)
            {
                depLoader.AddListener(OnDepComplete);
                depLoader.Start();
            }
        }
    }

    /// <summary>
    /// 获取当前文件的所有依赖项
    /// </summary>
    /// <returns></returns>
    public string[] GetDepensdList()
    {
        return Main_Manifest.GetAllDependencies(bundleName);
    }

    /// <summary>
    /// 如果有依赖项
    /// </summary>
    /// <returns></returns>
    public bool HasDepensd()
    {
        if (GetDepensdList().Length > 0)
        {
            return true;
        }
        return false;
    }
}

