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

    private void OnEnable()
    {
        if (shopItemData != null)
            RefreshUI();
    }


    public override void Init(DialogData data)
    {
        base.Init(data);
        shopItemData = (SelfHeroSelectData)data.obj;
        RefreshUI();
    }

    void RefreshUI()
    {
        AttributeShow.SetupInfo(PlayerItem.characterDataMap[shopItemData.heroGuid].GetItemAttributes());
        List<PlayerItem> equipments = PlayerItem.equipDataMap.Values.ToList()
            .FindAll(x => x.EquipItemGuid == shopItemData.heroGuid);
        List<PlayerItem> equipments_weapon = equipments.FindAll(x => x.EquipPosition == "weapon");
        if (equipments_weapon.Count > 0)
        {
            equip1.SetEquipInfo(equipments_weapon[0].GUID, shopItemData.heroGuid);
        }
        else
        {
            equip1.SetEquipInfo("", shopItemData.heroGuid);
        }

        List<PlayerItem> equipments_cloth = equipments.FindAll(x => x.EquipPosition == "cloth");
        if (equipments_cloth.Count > 0)
        {
            equip2.SetEquipInfo(equipments_cloth[0].GUID, shopItemData.heroGuid);
        }
        else
        {
            equip2.SetEquipInfo("", shopItemData.heroGuid);
        }

        List<PlayerItem> equipments_shoot = equipments.FindAll(x => x.EquipPosition == "shoot");
        if (equipments_shoot.Count > 0)
        {
            equip3.SetEquipInfo(equipments_shoot[0].GUID, shopItemData.heroGuid);
        }
        else
        {
            equip3.SetEquipInfo("", shopItemData.heroGuid);
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