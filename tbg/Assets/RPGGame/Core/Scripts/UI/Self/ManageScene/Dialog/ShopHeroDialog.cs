using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;

public class ShopHeroDialog : Dialog
{

    public UIAttributeShow AttributeShow;
    private ShopItemData shopItemData;
    // Use this for initialization
    void Start()
    {

    }

    public override void Init()
    {
        //AttributeShow.SetupInfo(ICharacter.DataMap[shopItemData.heroid].GetTotalAttributes());
    }

    public void BuyItem()
    {
        GameInstance.dbPlayerData.InsertCharacter(shopItemData.heroid);
    }


}
