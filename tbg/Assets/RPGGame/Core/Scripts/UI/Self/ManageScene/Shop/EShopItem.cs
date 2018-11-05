using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EShopItem : MonoBehaviour
{

    public Text NameText;

    private string equipId;
    // Use this for initialization
    void Start()
    {

    }

    public void SetItemData(EquipmentItem item)
    {
        NameText.text = item.title;
        equipId = item.itemid;
    }

    public void Click()
    {
        DialogData data = new DialogData();
        data.dialog = DialogController.instance.ShopEquipDialog;
        EShopItemData heroData = new EShopItemData();
        heroData.equipId = equipId;
        data.obj = heroData;
        DialogController.instance.ShowDialog(data, DialogController.DialogType.stack);
    }
}

public class EShopItemData
{
    public string equipId;
}
