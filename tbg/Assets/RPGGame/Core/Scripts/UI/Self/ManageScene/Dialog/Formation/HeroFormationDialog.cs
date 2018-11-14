using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFormationDialog : Dialog
{
    private HeroFormationData heroFormationData;
    // Use this for initialization
    void Start()
    {

    }

    public override void Init(DialogData data)
    {
        base.Init(data);
        heroFormationData = (HeroFormationData)data.obj;

    }

    public void SetFormation(int index)
    {
        GameInstance.dbBattle.DoSetFormation(heroFormationData.HeroGuid, PlayerItem.characterDataMap[heroFormationData.HeroGuid].ItemID, "STAGE_FORMATION_A", index,
            (formationListResult) =>
            {
                PlayerFormation.SetDataRange(formationListResult.list);
                if (heroFormationData.callback != null)
                {
                    heroFormationData.callback.Invoke();
                }
            });
        Close();
    }
}

public class HeroFormationData
{
    public string HeroGuid;
    public Action callback;
}
