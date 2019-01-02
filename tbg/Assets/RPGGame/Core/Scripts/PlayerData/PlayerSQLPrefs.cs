using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Events;

public class PlayerSQLPrefs
{
    public static UnityAction saveAction;
    private static LocalConfig localconfig = null;
    public static LocalConfig localConfig
    {
        get
        {
            return localconfig;
        }
        set
        {
            localconfig = value;
        }
    }

    public static void SavePrefs()
    {
        if (saveAction != null)
        {
            saveAction.Invoke();
        }
    }

    /// <summary>
    /// yongzhe tower ceng
    /// </summary>
    public static int yzTowerABSLevel
    {
        get
        {
            return localConfig.YZTowerABSLevel;
        }
        set
        {
            localConfig.YZTowerABSLevel = value;
            SavePrefs();
        }
    }

    /// <summary>
    /// 当前坐标
    /// </summary>
    public static int yzTowerCurrentLevel
    {
        get
        {
            return localConfig.YZTowerCurrentLevel;
        }
        set
        {
            localConfig.YZTowerCurrentLevel = value;
            SavePrefs();
        }
    }
}

public class LocalConfig
{
    [JsonProperty("YZTowerABSLevel")]
    public int YZTowerABSLevel { get; set; }

    [JsonProperty("YZTowerCurrentLevel")]
    public int YZTowerCurrentLevel { get; set; }

}

