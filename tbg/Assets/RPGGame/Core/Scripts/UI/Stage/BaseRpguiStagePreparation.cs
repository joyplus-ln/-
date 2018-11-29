using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseRpguiStagePreparation<TUI, TStage> : RpguiDataItem<TStage>
    where TUI : RpguiDataItem<TStage>
    where TStage : BaseStage
{
    public RpguiItem RpguiFormationSlotPrefab;
    public TUI uiStage;
    public override void Clear()
    {
        // Don't clear
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.Id);
    }

    public override void UpdateData()
    {
        if (uiStage != null)
            uiStage.SetData(data);
    }

    public override void Show()
    {
        base.Show();
    }
}
