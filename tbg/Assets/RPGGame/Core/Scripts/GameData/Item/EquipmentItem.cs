using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : BaseActorItem
{
    public string region;
    public string quality;
    [Header("Equipment Data")]
    public CalculationAttributes extraAttributes;
    public string equippablePosition;


    public override string ToJson()
    {
        return null;
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
