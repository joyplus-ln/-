using System;
using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;

public class HeroFormationDialog : Dialog
{
    private string HeroGuid;

    private Action<int> callBack;

    // Use this for initialization
    void Start()
    {

    }

    public void SetData(string HeroGuid, Action<int> CallBack)
    {
        this.HeroGuid = HeroGuid;
        this.callBack = CallBack;
    }



    public void SetFormation(int index)
    {
        if (IPlayerFormation.DataMap.ContainsKey(index))
        {
            foreach (int key in IPlayerFormation.DataMap.Keys)
            {
                if (key >= 1 && key <= 5)
                {
                    if (IPlayerFormation.DataMap[key].itemId == HeroGuid)
                    {
                        IPlayerFormation.DataMap[key].itemId = "";
                    }
                }
            }

            IPlayerFormation.DataMap[index].itemId = HeroGuid;
        }
        else
        {
            foreach (int key in IPlayerFormation.DataMap.Keys)
            {
                if (key >= 1 && key <= 5)
                {
                    if (IPlayerFormation.DataMap[key].itemId == HeroGuid)
                    {
                        IPlayerFormation.DataMap[key].itemId = "";
                    }
                }
            }
            IPlayerFormation iPlayerFormation = new IPlayerFormation(index, "", HeroGuid);
            IPlayerFormation.DataMap.Add(index, iPlayerFormation);
        }
        IPlayerFormation.UpdataDataMap();
        //GameInstance.dbBattle.DoSetFormation(heroFormationData.HeroGuid, PlayerItem.characterDataMap[heroFormationData.HeroGuid].ItemID, "STAGE_FORMATION_A", index,
        //(formationListResult) =>
        //{
        //    PlayerFormation.SetDataRange(formationListResult.list);
        //    if (heroFormationData.callback != null)
        //    {
        //        heroFormationData.callback.Invoke();
        //    }
        //});
        if (callBack != null)
        {
            callBack.Invoke(index);
        }
        Close();
    }

}

