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

    public Const.EquipPosition equipType;

    public Text EquipmentNameText;

    // Use this for initialization
    void Start()
    {
    }

    public void SetEquipInfo(string EquipGuid, string HeroGuid)
    {
        this.EquipGuid = EquipGuid;
        this.HeroGuid = HeroGuid;
        if (EquipGuid.Length > 0)
        {
            EquipmentNameText.text = PlayerItem.equipDataMap[EquipGuid].EquipmentData.title;
        }
        else
        {
            EquipmentNameText.text = "无";
        }
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
        dialogdata.equipType = this.equipType;
        data.obj = dialogdata;
        DialogController.instance.ShowDialog(data, DialogController.DialogType.stack);
    }
}