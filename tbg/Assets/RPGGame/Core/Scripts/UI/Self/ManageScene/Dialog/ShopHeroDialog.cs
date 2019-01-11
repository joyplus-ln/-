using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;

public class ShopHeroDialog : Dialog
{

    public UIAttributeShow AttributeShow;

    private string heroid;
    // Use this for initialization
    void Start()
    {

    }

    public void SetData(string heroId)
    {
        this.heroid = heroId;
        AttributeShow.SetupInfo(ICharacter.DataMap[heroid].GetAttributes().GetSubAttributes());
    }

    public override void Init()
    {

    }

    public void BuyItem()
    {
        IPlayerHasCharacters.InsertNewCharacter(heroid);
        Close();
    }


}
