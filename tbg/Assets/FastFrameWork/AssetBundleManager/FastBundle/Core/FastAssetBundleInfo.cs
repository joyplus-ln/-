using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


[Serializable]
public class FastAssetBundleInfo
{
    public delegate void OnUnloadedHandler(FastAssetBundleInfo abi);
    public OnUnloadedHandler onUnloaded;

    internal AssetBundle bundle;

    public string bundleName;


    /// <summary>
    /// 准备完毕时的时间
    /// </summary>
    private float _readyTime;

    /// <summary>
    /// 标记当前是否准备完毕
    /// </summary>
    private bool _isReady;

    private Object _mainObject;

    /// <summary>
    /// 强制的引用计数
    /// </summary>
    public int refCount { get; private set; }


    public FastAssetBundleInfo()
    {
        
    }



    public void ResetLifeTime()
    {
        if (_isReady)
        {
            _readyTime = Time.time;
        }
    }

    /// <summary>
    /// 引用计数增一
    /// </summary>
    public void Retain()
    {
        refCount++;
        Debug.Log(bundleName + "-引用-" + refCount);
    }

    /// <summary>
    /// 引用计数减一
    /// </summary>
    public void Release()
    {
        refCount--;
    }





    /// <summary>
    /// 实例化对象
    /// </summary>
    /// <param name="user">增加引用的对象</param>
    /// <returns></returns>
    public virtual GameObject Instantiate()
    {
        return Instantiate(true);
    }

    public virtual GameObject Instantiate(bool enable)
    {
        if (mainObject != null)
        {
            //只有GameObject才可以Instantiate
            if (mainObject is GameObject)
            {
                GameObject prefab = mainObject as GameObject;
                prefab.SetActive(enable);
                Object inst = Object.Instantiate(prefab);
                inst.name = prefab.name;
                return (GameObject)inst;
            }
        }
        return null;
    }

    public virtual GameObject Instantiate(Vector3 position, Quaternion rotation, bool enable = true)
    {
        if (mainObject != null)
        {
            //只有GameObject才可以Instantiate
            if (mainObject is GameObject)
            {
                GameObject prefab = mainObject as GameObject;
                prefab.SetActive(enable);
                Object inst = Object.Instantiate(prefab, position, rotation);
                inst.name = prefab.name;
                return (GameObject)inst;
            }
        }
        return null;
    }

    public T LoadAsset<T>(Object user, string name) where T : Object
    {
        if (bundle)
        {
            T asset = bundle.LoadAsset<T>(name);

            return asset;
        }
        return null;
    }



    /// <summary>
    /// 这个资源是否不用了
    /// </summary>
    /// <returns></returns>
    public bool isUnused
    {
        get { return _isReady && refCount <= 0  ; }
    }



    public bool isReady
    {
        get { return _isReady; }

        set { _isReady = value; }
    }

    public virtual Object mainObject
    {
        get
        {
            if (_mainObject == null && _isReady)
            {

                string[] names = bundle.GetAllAssetNames();
                _mainObject = bundle.LoadAsset(names[0]);

                //优化：如果是根，则可以 unload(false) 以节省内存
                //if (data.compositeType == AssetBundleExportType.Root)
                // UnloadBundle();
            }
            return _mainObject;
        }
    }

}
