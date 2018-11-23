using System.Collections;
using System.Collections.Generic;
using CatLib;
using UnityEngine;

public class FastLib : FastMonoSingleton<FastLib>
{
    private static Dictionary<string, Manager> managerDic = new Dictionary<string, Manager>();
    static FastLib()
    {
    }
    public override void Init()
    {
        base.Init();
        DontDestroyOnLoad(this);
        Debug.Log("FastLib-Init");
    }

    public void Open()
    {

    }

    private void Update()
    {
        foreach (var manager in managerDic.Values)
        {
            manager.update();
        }
    }



    public static FastTimerManager GetFastTimerManager()
    {
        if (managerDic.ContainsKey(typeof(FastTimerManager).ToString()))
        {
            return managerDic[typeof(FastTimerManager).ToString()] as FastTimerManager;
        }
        FastTimerManager manager = new FastTimerManager();
        managerDic.Add(manager.GetType().ToString(), manager);
        return manager;
    }
}
