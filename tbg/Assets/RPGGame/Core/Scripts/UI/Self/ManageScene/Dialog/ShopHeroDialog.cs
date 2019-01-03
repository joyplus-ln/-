using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHeroDialog : Dialog
{

    public UIAttributeShow AttributeShow;
    private ShopItemData shopItemData;
    // Use this for initialization
    void Start()
    {

    }

    public override void Init(DialogData data)
    {
        base.Init(data);
        shopItemData = (ShopItemData)data.obj;
        AttributeShow.SetupInfo(DBManager.instance.GetConfigCharacters()[shopItemData.heroid].GetTotalAttributes());
    }

    public void BuyItem()
    {
        GameInstance.dbPlayerData.InsertCharacter(shopItemData.heroid);
    }


}
