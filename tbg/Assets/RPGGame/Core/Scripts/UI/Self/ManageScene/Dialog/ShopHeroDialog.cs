using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHeroDialog : Dialog
{

    public UIAttributeShow AttributeShow;
	// Use this for initialization
	void Start () {
		
	}

    public override void Init(DialogData data)
    {
        base.Init(data);
        ShopItemData shopItemData = (ShopItemData)data.obj;
        AttributeShow.SetupInfo(GameInstance.GameDatabase.characters[shopItemData.heroid].GetTotalAttributes());
    }
}
