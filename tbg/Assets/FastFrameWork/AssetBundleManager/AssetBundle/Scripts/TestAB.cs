using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestAB : MonoBehaviour
{
    FastAssetBundleManager manager;

    void Start()
    {
        manager = gameObject.AddComponent<FastAssetBundleManager>();
        manager.Init();
        LoadObjects();
    }

    void LoadObjects()
    {
        manager.LoadBundle("Assets.FastFrameWork.AssetBundleManager.Assetbundle.Prefabs.Sphere.prefab", (a) =>
        {
            GameObject go = Instantiate(a.mainObject) as GameObject;//a.Instantiate();
            go.transform.localPosition = new Vector3(1, 3, 3);
            Debug.Log(a.bundleName + "-" + a.refCount);
            Destroy(go);
            IEnumerable list = AssetBundle.GetAllLoadedAssetBundles();
            foreach (AssetBundle it in list)
            {
                for (int i = 0; i < it.GetAllAssetNames().Length; i++)
                {
                    Debug.LogError(it.GetAllAssetNames()[i]);
                }
            }
            a.bundle.Unload(true);
            IEnumerable list2 = AssetBundle.GetAllLoadedAssetBundles();
            foreach (AssetBundle it in list2)
            {
                for (int i = 0; i < it.GetAllAssetNames().Length; i++)
                {
                    Debug.Log(it.GetAllAssetNames()[i]);
                }
            }

        });

        manager.LoadBundle("Assets.FastFrameWork.AssetBundleManager.Assetbundle.Prefabs.Sphere.prefab", (a) =>
        {
            GameObject go = Instantiate(a.mainObject) as GameObject;//a.Instantiate();
            go.transform.localPosition = new Vector3(2, 3, 3);
            Debug.Log(a.bundleName + "-" + a.refCount);
        });

        //manager.LoadBundle("Assets.FastFrameWork.AssetBundleManager.Assetbundle.Prefabs.Capsule.prefab", (a) =>
        //{
        //    GameObject go = Instantiate(a.mainObject) as GameObject;//a.Instantiate();
        //    go.transform.localPosition = new Vector3(3, 3, 3);
        //    Debug.Log(a.bundleName + "-" + a.refCount);
        //});

        //manager.LoadBundle("Assets.FastFrameWork.AssetBundleManager.Assetbundle.Prefabs.Cube.prefab", (a) =>
        //{
        //    GameObject go = Instantiate(a.mainObject) as GameObject;//a.Instantiate();
        //    go.transform.localPosition = new Vector3(4, 3, 3);
        //    Debug.Log(a.bundleName + "-" + a.refCount);
        //});

        //manager.Load("Assets.Prefabs.Cube.prefab.ab", (a) =>
        //{
        //    GameObject go = a.Instantiate();
        //    go.transform.localPosition = new Vector3(6, 3, 3);
        //});
        //manager.Load("Assets.Prefabs.Plane.prefab.ab", (a) =>
        //{
        //    GameObject go = a.Instantiate();
        //    go.transform.localPosition = new Vector3(9, 3, 3);
        //});
        //manager.Load("Assets.Prefabs.Capsule.prefab.ab", (a) =>
        //{
        //    GameObject go = a.Instantiate();
        //    go.transform.localPosition = new Vector3(12, 3, 3);
        //});
    }
}