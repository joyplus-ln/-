using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    public Text NameText;

    private string heruid;
    // Use this for initialization
    void Start()
    {

    }

    public void SetItemData(ICharacter item)
    {
        NameText.text = item.title;
        heruid = item.guid;
    }

    public void Click()
    {
        DialogData data = new DialogData();
        data.dialog = DialogController.instance.ShopHeroDialog;
        ShopItemData heroData = new ShopItemData();
        heroData.heroid = heruid;
        data.obj = heroData;
        DialogController.instance.ShowDialog(DialogController.instance.ShopHeroDialog, DialogController.DialogType.stack);
    }
}

public class ShopItemData
{
    public string heroid;
}
