using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelfHeroEquipSelectDialog : Dialog
{



    public UIAttributeShow AttributeShow;
    private SelfHeroEquipSelectData shopItemData;


    // Use this for initialization
    void Start()
    {

    }

    public override void Init(DialogData data)
    {
        base.Init(data);
        shopItemData = (SelfHeroEquipSelectData)data.obj;

        AttributeShow.SetupInfo(PlayerItem.equipDataMap[shopItemData.equipGuid].GetItemAttributes());

    }

    public void ClickEquip(string pos)
    {

    }

    public void UnEquip()
    {
        
    }
}

public class SelfHeroEquipSelectData
{
    public string heroGuid;
    public string equipGuid;
}