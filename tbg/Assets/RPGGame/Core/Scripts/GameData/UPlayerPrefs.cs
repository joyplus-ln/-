using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class UPlayerPrefs
{

    #region New PlayerPref Stuff
    /// <summary>
    /// Returns true if key exists in the preferences.
    /// </summary>
    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    /// <summary>
    ///删除key
    /// </summary>
    public static void DeleteKey(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
        }
    }

    /// <summary>
    /// Removes all keys and values from the preferences. Use with caution.
    /// </summary>
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    /// <summary>
    /// Writes all modified preferences to disk.
    /// </summary>
    public static void Save()
    {
        PlayerPrefs.Save();
    }
    #endregion

    #region New PlayerPref Setters

    public static void SetString(string key, string val)
    {
        PlayerPrefs.SetString(key, val);
    }

    public static void SetInt(string key, int val)
    {
        PlayerPrefs.SetInt(key, val);
    }


    public static void SetBool(string key, bool value)
    {
        SetInt(key, value ? 1 : 0);
    }
    #endregion

    #region New PlayerPref Getters

    public static int GetInt(string key, int defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return defaultValue;
        }
        int storedPref = defaultValue;
        storedPref = PlayerPrefs.GetInt(key);
        return storedPref;
    }

    public static int GetInt(string key)
    {
        return GetInt(key, 0);
    }

    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static string GetString(string key, string defaultstr)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return defaultstr;
        }
        return PlayerPrefs.GetString(key);
    }


    public static bool GetBool(string key, bool defaultValue = false)
    {
        if (!HasKey(key))
            return defaultValue;

        return GetInt(key) == 1;
    }
    #endregion

    #region Double
    public static void SetDouble(string key, double value)
    {
        PlayerPrefs.SetString(key, DoubleToString(value));
    }

    public static double GetDouble(string key, double defaultValue)
    {
        string defaultVal = DoubleToString(defaultValue);
        return StringToDouble(PlayerPrefs.GetString(key, defaultVal));
    }

    public static double GetDouble(string key)
    {
        return GetDouble(key, 0d);
    }

    private static string DoubleToString(double target)
    {
        return target.ToString("R");
    }

    private static double StringToDouble(string target)
    {
        if (string.IsNullOrEmpty(target))
            return 0d;

        return double.Parse(target);
    }
    #endregion

    public static float GetFloat(string key, float defaultValue = 0f)
    {
        string f = GetString(key, defaultValue.ToString());
        float defaultfloat = 0;
        bool ok = float.TryParse(f, out defaultfloat);
        if (ok)
        {
            return defaultfloat;
        }
        else
        {
            return 0;
        }
    }

    public static void SetFloat(string key, float num)
    {
        SetString(key, num + "");
    }

    public static T GetObject<T>(string key, T defVal) where T : new()
    {
        T obj = defVal;

        string objData = UPlayerPrefs.GetString(key, "");
        obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(objData);
        if (obj == null)
            obj = defVal;

        return obj;
    }

    public static void SetObject(string key, object obj)
    {
        if (!string.IsNullOrEmpty(key))
        {
            if (obj != null)
            {
                UPlayerPrefs.SetString(key, Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            }
            else
            {
                UPlayerPrefs.SetString(key, "");
            }
        }
    }

    public static void SetLong(string key, long val)
    {
        UPlayerPrefs.SetString(key, val.ToString());
    }

    public static long GetLong(string key, long val)
    {
        string str = GetString(key, val.ToString());
        long reva = 0;
        long.TryParse(str, out reva);
        return reva;
    }



    public static void SetDate(string key, DateTime date)
    {
        if (!string.IsNullOrEmpty(key))
            UPlayerPrefs.SetString(key, date.ToString());
    }

    #region 写入文件

    private static string savepath = Application.persistentDataPath + "/";





    /// <summary>
    /// 是否存在文件
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static bool HasFile(string filename)
    {
        return File.Exists(savepath + filename);
    }



    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static string LoadFile(string filename)
    {
        string file = "";
        file = File.ReadAllText(savepath + filename);
        return file;

    }

    public static byte[] LoadFileByBytes(string filename)
    {
        return File.ReadAllBytes(savepath + filename);

    }
    #endregion
}

