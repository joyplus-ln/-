using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite3TableDataTmp;
using UnityEngine;

public class SelfHeroEquipSelectDialog : Dialog
{



    public UIAttributeShow AttributeShow;
    public GameObject equipOn;
    public GameObject equipOff;
    private string equipGuid;
    private string HeroGuid;
    private Const.EquipPosition equipType;
    private Action callBack;


    // Use this for initialization
    void Start()
    {

    }

    public override void Init()
    {

    }

    public void SetData(string equipGuid, string HeroGuid, Const.EquipPosition equipType, Action callBack)
    {
        this.equipGuid = equipGuid;
        this.HeroGuid = HeroGuid;
        this.equipType = equipType;
        this.callBack = callBack;
        RefreshUi();
    }

    void RefreshUi()
    {
        if (!string.IsNullOrEmpty(equipGuid))
        {
            equipOn.SetActive(false);
            equipOff.SetActive(true);
            AttributeShow.gameObject.SetActive(true);
            AttributeShow.SetupInfo(IPlayerHasEquips.DataMap[equipGuid].IEquipment.GetAttributes().GetCreateCalculationAttributes());
        }
        else
        {
            equipOn.SetActive(true);
            equipOff.SetActive(false);
            AttributeShow.gameObject.SetActive(false);
        }
    }

    public override void Close()
    {
        if (callBack != null)
        {
            callBack.Invoke();
        }
        base.Close();
    }

    public void ClickChangeEquip()
    {
        //DialogData openDialogData = new DialogData();
        //openDialogData.dialog = DialogController.instance.selfHeroSelectChangeEquipDialog;
        //SelfHeroSelectChangeEquipData selfHeroSelectChangeEquipData = new SelfHeroSelectChangeEquipData();
        //selfHeroSelectChangeEquipData.heroGuid = equipGuid;
        //selfHeroSelectChangeEquipData.equipType = equipType;
        //openDialogData.obj = selfHeroSelectChangeEquipData;
        //selfHeroSelectChangeEquipData.callBack = CallBack;
        Dialog dialog = DialogController.instance.ShowDialog(DialogController.instance.selfHeroSelectChangeEquipDialog, DialogController.DialogType.stack);
        ((SelfHeroSelectChangeEquipDialog)dialog).SetData(HeroGuid, equipType, CallBack);

    }

    void CallBack(string selectedGuid)
    {
        equipGuid = selectedGuid;
        RefreshUi();
    }

    public void UnEquip()
    {
        //GameInstance.dbBattle.DoUnEquipItem(equipGuid, (result) =>
        //{
        //    //PlayerItem.SetDataRange(result.updateItems);
        //    Close();
        //    if (shopItemData.callback != null)
        //    {
        //        shopItemData.callback.Invoke();
        //    }
        //});
    }


}