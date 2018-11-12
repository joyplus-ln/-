using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfHeroEquipSHow : MonoBehaviour
{
    public string EquipGuid;
    public string HeroGuid;

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
        DialogData data = new DialogData();
        data.dialog = DialogController.instance.SelfHeroEquipSelectDialog;
        SelfHeroEquipSelectData dialogdata = new SelfHeroEquipSelectData();
        dialogdata.equipGuid = this.EquipGuid;
        dialogdata.heroGuid = this.HeroGuid;
        DialogController.instance.ShowDialog(data, DialogController.DialogType.stack);
    }
}