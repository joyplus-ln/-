﻿using UnityEngine;
using UnityEditor;

public class TestUtils
{

    [MenuItem("Test/Option1")]
    public static void SaveString()
    {

        //PlayerSQLPrefs.yzTowerLevel = 10;
        //PlayerSQLPrefs.yzTowerABSLevel = 10;
        Debug.Log(Player.CurrentPlayerId);
        PlayerOtherItem.AddOneItem(GameInstance.GameDatabase.otherItem[0].Id, 10);
    }

    [MenuItem("Test/Option2")]
    public static void SaveString2()
    {
        TextAsset test = Resources.Load<TextAsset>("code/Skill001.lua");
        XLua.LuaEnv luaenv = new XLua.LuaEnv();
        //luaenv.DoString("CS.UnityEngine.Debug.Log('hello world')");
        luaenv.DoString(test.text);
        luaenv.Dispose();
        Debug.Log(test);
        return;
        CustomSkill skill = SkillUtils.MakeCustomSkill("001");
        skill.id = 001;
        skill.Trigger(TriggerType.fight);
        return;
        FormulaUtils.GetTowerExtraAttributes(false);
        return;
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(UnityEngine.Random.Range(0, 0.5f));
        }
        return;
        //PlayerSQLPrefs.yzTowerLevel = 10;
        //PlayerSQLPrefs.yzTowerABSLevel = 10;
        PlayerOtherItem.ReduceItem(GameInstance.GameDatabase.otherItem[0].Id, 10);
    }

    [MenuItem("Test/Option3")]
    public static void SaveString3()
    {
        GameObject.Find("HomePrefab").GetComponent<CanvasGroup>().alpha = 0.7f;

    }
}