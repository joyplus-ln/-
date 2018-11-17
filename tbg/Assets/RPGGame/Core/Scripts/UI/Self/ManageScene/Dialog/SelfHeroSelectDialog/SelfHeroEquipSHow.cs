using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfHeroEquipSHow : MonoBehaviour
{
    [HideInInspector]
    public string EquipGuid;
    [HideInInspector]
    public string HeroGuid;

    public Text EquipmentNameText;

    // Use this for initialization
    void Start()
    {
    }

    public void SetEquipInfo(string EquipGuid, string HeroGuid)
    {
        this.EquipGuid = EquipGuid;
        this.HeroGuid = HeroGuid;
        EquipmentNameText.text = PlayerItem.equipDataMap[EquipGuid].EquipmentData.title;
    }

    /// <summary>
    /// 点击显示内容
    /// </summary>
    public void ClickShowInfo()
    {
        DialogData data = new DialogData();
        data.dialog = DialogController.instance.SelfHeroEquipSelectDialog;
        SelfHeroEquipSelectData dialogdata = new SelfHeroEquipSelectData();
        dialogdata.equipGuid = this.EquipGuid;
        dialogdata.heroGuid = this.HeroGuid;
        data.obj = dialogdata;
        DialogController.instance.ShowDialog(data, DialogController.DialogType.stack);
    }
}