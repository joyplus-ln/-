using System.Collections;
using System.Collections.Generic;
using CatLib;
using UnityEngine;

public class FastLib : FastMonoSingleton<FastLib>
{
    public override void Init()
    {
        base.Init();
        DontDestroyOnLoad(this);
        Debug.Log("FastLib-Init");
    }

    public void Open()
    {
        
    }
}
