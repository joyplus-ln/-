using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : BaseActorItem
{
    public string region;
    public string quality;
    [Header("Equipment Data")]
    public CalculationAttributes extraAttributes;
    public List<string> equippablePositions;


    public override string ToJson()
    {
        var jsonEquippablePositions = "";
        foreach (var entry in equippablePositions)
        {
            if (!string.IsNullOrEmpty(jsonEquippablePositions))
                jsonEquippablePositions += ",";
            jsonEquippablePositions += "\"" + entry + "\"";
        }
        jsonEquippablePositions = "[" + jsonEquippablePositions + "]";
        return "{\"SqliteId\":\""  + "\"," +
            "\"category\":\"" + category + "\"," +
               "\"itemid\":\"" + itemid + "\"," +
            "\"type\":\"" + Type + "\"," +
            "\"maxStack\":" + MaxStack + "," +
            "\"equippablePositions\":" + jsonEquippablePositions + "}";
    }
}

[System.Serializable]
public class EquipmentItemAmount : BaseItemAmount<EquipmentItem> { }

[System.Serializable]
public class EquipmentItemDrop : BaseItemDrop<EquipmentItem> { }

[System.Serializable]
public class EquipmentItemEvolve : SpecificItemEvolve<EquipmentItem>
{
    public override SpecificItemEvolve<EquipmentItem> Create()
    {
        return new EquipmentItemEvolve();
    }
}
