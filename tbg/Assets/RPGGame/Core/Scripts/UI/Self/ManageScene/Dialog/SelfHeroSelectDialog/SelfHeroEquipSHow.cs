using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
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
            EquipmentNameText.text = IPlayerHasEquips.DataMap[this.EquipGuid].IEquipment.title;
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
        //DialogData data = new DialogData();
        //data.dialog = DialogController.instance.SelfHeroEquipSelectDialog;
        //SelfHeroEquipSelectData dialogdata = new SelfHeroEquipSelectData();
        //dialogdata.equipGuid = this.EquipGuid;
        //dialogdata.heroGuid = this.HeroGuid;
        //dialogdata.equipType = this.equipType;
        //data.obj = dialogdata;
        Dialog data = DialogController.instance.ShowDialog(DialogController.instance.SelfHeroEquipSelectDialog, DialogController.DialogType.stack);
        ((SelfHeroEquipSelectDialog)data).SetData(EquipGuid, HeroGuid, this.equipType);
    }
}