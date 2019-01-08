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
        IPlayer.CurrentPlayer.TowerCurrentLevel++;
        RefreshStates();
    }

    public void CengJian()
    {
        IPlayer.CurrentPlayer.TowerCurrentLevel--;
        RefreshStates();
    }

    void RefreshStates()
    {
        if (IPlayer.CurrentPlayer.TowerCurrentLevel > IPlayer.CurrentPlayer.TowerAbsLevel)
            IPlayer.CurrentPlayer.TowerCurrentLevel = IPlayer.CurrentPlayer.TowerAbsLevel;

        jian.interactable = IPlayer.CurrentPlayer.TowerCurrentLevel <= 1;
        jia.interactable = IPlayer.CurrentPlayer.TowerCurrentLevel >= IPlayer.CurrentPlayer.TowerAbsLevel;

        MaxTowerLevelText.text = String.Format("Max {0}", IPlayer.CurrentPlayer.TowerAbsLevel);
        CengText.text = String.Format("当前 {0}", IPlayer.CurrentPlayer.TowerCurrentLevel);
    }

    public void Go()
    {
        Debug.Log("开始塔冒险");
        BaseGamePlayManager.StartTowerStage(GameInstance.GameDatabase.towerStages[0], IPlayer.CurrentPlayer.TowerCurrentLevel);
    }
}
