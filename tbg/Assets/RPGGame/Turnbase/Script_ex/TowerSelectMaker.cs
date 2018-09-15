using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectMaker : MonoBehaviour
{
    public Text CengText;
    public Text MaxTowerLevelText;
    public Button jia;
    public Button jian;
    public Button Gobutton;

    void Start()
    {
        jia.onClick.AddListener(Cengjia);
        jian.onClick.AddListener(CengJian);
        Gobutton.onClick.AddListener(Go);
        RefreshStates();
    }

    private void OnDestroy()
    {
        jia.onClick.RemoveListener(Cengjia);
        jian.onClick.RemoveListener(CengJian);
        Gobutton.onClick.RemoveListener(Go);
    }

    public void Cengjia()
    {
        PlayerSQLPrefs.yzTowerCurrentLevel++;
        RefreshStates();
    }

    public void CengJian()
    {
        PlayerSQLPrefs.yzTowerCurrentLevel--;
        RefreshStates();
    }

    void RefreshStates()
    {
        if (PlayerSQLPrefs.yzTowerCurrentLevel > PlayerSQLPrefs.yzTowerABSLevel)
            PlayerSQLPrefs.yzTowerCurrentLevel = PlayerSQLPrefs.yzTowerABSLevel;

        jian.interactable = PlayerSQLPrefs.yzTowerCurrentLevel <= 1;
        jia.interactable = PlayerSQLPrefs.yzTowerCurrentLevel >= PlayerSQLPrefs.yzTowerABSLevel;

        MaxTowerLevelText.text = String.Format("Max {0}", PlayerSQLPrefs.yzTowerABSLevel);
        CengText.text = String.Format("当前 {0}", PlayerSQLPrefs.yzTowerCurrentLevel);
    }

    public void Go()
    {
        Debug.Log("开始塔冒险");
        BaseGamePlayManager.StartTowerStage(GameInstance.GameDatabase.towerStages[0], PlayerSQLPrefs.yzTowerCurrentLevel);
    }
}
