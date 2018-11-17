using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelfHeroSelectDialog : Dialog
{
    public UIAttributeShow AttributeShow;
    private SelfHeroSelectData shopItemData;

    public SelfHeroEquipSHow equip1;
    public SelfHeroEquipSHow equip2;

    public SelfHeroEquipSHow equip3;

    // Use this for initialization
    void Start()
    {
    }

    public override void Init(DialogData data)
    {
        base.Init(data);
        shopItemData = (SelfHeroSelectData) data.obj;
        AttributeShow.SetupInfo(PlayerItem.characterDataMap[shopItemData.heroGuid].GetItemAttributes());
        List<PlayerItem> equipments = PlayerItem.equipDataMap.Values.ToList()
            .FindAll(x => x.EquipItemGuid == shopItemData.heroGuid);
        for (int i = 0; i < equipments.Count; i++)
        {
            if (equipments[i].equipPosition == "weapon")
            {
                equip1.SetEquipInfo(equipments[i].GUID,shopItemData.heroGuid);
            }else if(equipments[i].equipPosition == "cloth")
            {
                equip2.SetEquipInfo(equipments[i].GUID, shopItemData.heroGuid);
            }
            else if (equipments[i].equipPosition == "shoot")
            {
                equip3.SetEquipInfo(equipments[i].GUID, shopItemData.heroGuid);
            }
        }
    }

    public void ClickEquip(string pos)
    {
    }
}

public class SelfHeroSelectData
{
    public string heroGuid;
}