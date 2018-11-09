using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelfHeroSelectDialog : Dialog
{



    public UIAttributeShow AttributeShow;
    private SelfHeroSelectData shopItemData;


    // Use this for initialization
    void Start()
    {

    }

    public override void Init(DialogData data)
    {
        base.Init(data);
        shopItemData = (SelfHeroSelectData)data.obj;
        AttributeShow.SetupInfo(PlayerItem.characterDataMap[shopItemData.heroGuid].GetItemAttributes());
        List<PlayerItem> equipments = PlayerItem.equipDataMap.Values.ToList().FindAll(x => x.EquipItemGuid == shopItemData.heroGuid);

    }

    public void ClickEquip(string pos)
    {

    }


}

public class SelfHeroSelectData
{
    public string heroGuid;
}