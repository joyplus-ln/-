using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : UIDataItem<Player>
{
    public Text textProfileName;
    public UILevel uiLevel;
    public override void UpdateData()
    {
        SetupInfo(data);
    }

    public override void Clear()
    {
        SetupInfo(null);
    }

    private void SetupInfo(Player data)
    {
        if (data == null)
            data = new Player();

        if (textProfileName != null)
            textProfileName.text = data.ProfileName;

        // Stats
        if (uiLevel != null)
        {
            uiLevel.level = data.Level;
            uiLevel.maxLevel = data.MaxLevel;
            uiLevel.collectExp = data.CollectExp;
            uiLevel.nextExp = data.NextExp;
        }
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.Id);
    }
}
