using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfHeroEquipSHow : MonoBehaviour
{
    [HideInInspector] public string EquipGuid;
    [HideInInspector] public string HeroGuid;

    // Use this for initialization
    void Start()
    {
    }

    public void SetEquipInfo(string EquipGuid, string HeroGuid)
    {
        this.EquipGuid = EquipGuid;
        this.HeroGuid = HeroGuid;
    }

    void ShowEquipMentInfo()
    {
        EquipmentItem thisItem = GameInstance.GameDatabase.equipments[EquipGuid];
    }

    public void ClickShowInfo()
    {
        if (EquipGuid.Length == 0 || HeroGuid.Length == 0) return;
        DialogData data = new DialogData();
        data.dialog = DialogController.instance.SelfHeroEquipSelectDialog;
        SelfHeroEquipSelectData dialogdata = new SelfHeroEquipSelectData();
        dialogdata.equipGuid = this.EquipGuid;
        dialogdata.heroGuid = this.HeroGuid;
        data.obj = dialogdata;
        DialogController.instance.ShowDialog(data, DialogController.DialogType.stack);
    }
}