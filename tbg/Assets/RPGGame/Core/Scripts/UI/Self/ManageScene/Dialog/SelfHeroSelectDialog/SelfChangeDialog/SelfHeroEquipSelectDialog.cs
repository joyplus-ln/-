using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelfHeroEquipSelectDialog : Dialog
{



    public UIAttributeShow AttributeShow;
    private SelfHeroEquipSelectData shopItemData;

    public GameObject equipOn;
    public GameObject equipOff;


    // Use this for initialization
    void Start()
    {

    }

    public override void Init(DialogData data)
    {
        base.Init(data);
        shopItemData = (SelfHeroEquipSelectData)data.obj;
        RefreshUi();

    }

    void RefreshUi()
    {
        if (shopItemData.equipGuid.Length > 0)
        {
            equipOn.SetActive(false);
            equipOff.SetActive(true);
            AttributeShow.gameObject.SetActive(true);
            //AttributeShow.SetupInfo(DBManager.instance.GetConfigEquipments()[shopItemData.equipGuid].GetItemAttributes());
        }
        else
        {
            equipOn.SetActive(true);
            equipOff.SetActive(false);
            AttributeShow.gameObject.SetActive(false);
        }
    }

    public void ClickChangeEquip()
    {
        DialogData openDialogData = new DialogData();
        openDialogData.dialog = DialogController.instance.selfHeroSelectChangeEquipDialog;
        SelfHeroSelectChangeEquipData selfHeroSelectChangeEquipData = new SelfHeroSelectChangeEquipData();
        selfHeroSelectChangeEquipData.heroGuid = shopItemData.heroGuid;
        selfHeroSelectChangeEquipData.equipType = shopItemData.equipType;
        openDialogData.obj = selfHeroSelectChangeEquipData;
        selfHeroSelectChangeEquipData.callBack = CallBack;
        DialogController.instance.ShowDialog(openDialogData, DialogController.DialogType.stack);

    }

    void CallBack(string selectedGuid)
    {
        shopItemData.equipGuid = selectedGuid;
        RefreshUi();
    }

    public void UnEquip()
    {
        GameInstance.dbBattle.DoUnEquipItem(shopItemData.equipGuid, (result) =>
        {
            //PlayerItem.SetDataRange(result.updateItems);
            Close();
            if (shopItemData.callback != null)
            {
                shopItemData.callback.Invoke();
            }
        });
    }


}

public class SelfHeroEquipSelectData
{
    public string heroGuid;
    public string equipGuid;
    public Const.EquipPosition equipType;
    public Action callback;
}