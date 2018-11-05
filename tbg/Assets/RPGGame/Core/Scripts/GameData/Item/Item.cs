using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class BaseItemAmount<T> where T : BaseItem
{
    public T item;
    public int amount;

    public string Id
    {
        get { return item == null ? "" : item.itemid; }
    }

    public virtual string ToJson()
    {
        if (string.IsNullOrEmpty(Id) || amount <= 0)
            return "";
        return "{\"id\":\"" + Id + "\"," +
            "\"amount\":" + amount + "}";
    }
}

public abstract class BaseItemDrop<T> : BaseItemAmount<T> where T : BaseItem
{
    [Range(0, 1)]
    public float randomRate;

    public override string ToJson()
    {
        if (string.IsNullOrEmpty(Id) || amount <= 0)
            return "";
        return "{\"id\":\"" + Id + "\"," +
            "\"amount\":" + amount + "," +
            "\"randomRate\":" + randomRate + "}";
    }
}

[System.Serializable]
public class ItemAmount : BaseItemAmount<BaseItem> { }

[System.Serializable]
public class ItemDrop : BaseItemDrop<BaseItem> { }

[System.Serializable]
public class GenericItemEvolve
{
    [Tooltip("Price to evolve to next level")]
    public int price;

    [Tooltip("This will be multiplied by `GameDatabase.itemExpTable` to calculate item level up exp at current evolve level")]
    [Range(1f, 10f)]
    public float expRate = 1;

    [Tooltip("This will be multiplied by `GameDatabase.itemSellPriceTable` to calculate item sell price at current evolve level")]
    [Range(1f, 10f)]
    public float sellPriceRate = 1;

    [Tooltip("This will be multiplied by `GameDatabase.itemLevelUpPriceTable` to calculate item level up price at current evolve level")]
    [Range(1f, 10f)]
    public float levelUpPriceRate = 1;

    [Tooltip("This will be multiplied by `GameDatabase.itemRewardExpTable` to calculate item reward exp at current evolve level")]
    [Range(1f, 10f)]
    public float rewardExpRate = 1;
}

public abstract class SpecificItemEvolve
{
    [Tooltip("Required items for evolving")]
    public ItemAmount[] requiredMaterials;

    public abstract BaseItem GetEvolveItem();

    public virtual string ToJson()
    {
        // Lootbox rewards
        var jsonRequiredMaterials = "";
        foreach (var entry in requiredMaterials)
        {
            if (!string.IsNullOrEmpty(jsonRequiredMaterials))
                jsonRequiredMaterials += ",";
            jsonRequiredMaterials += entry.ToJson();
        }
        jsonRequiredMaterials = "[" + jsonRequiredMaterials + "]";

        var evolveItem = GetEvolveItem();
        var evolveItemId = "";
        if (evolveItem != null)
            evolveItemId = evolveItem.itemid;

        return "{\"requiredMaterials\":" + jsonRequiredMaterials + "," +
            "\"evolveItem\":\"" + evolveItemId + "\"}";
    }
}

public abstract class SpecificItemEvolve<T> : SpecificItemEvolve where T : BaseItem
{
    public T evolveItem;

    public override BaseItem GetEvolveItem()
    {
        return evolveItem;
    }

    public SpecificItemEvolve Clone()
    {
        var result = Create();
        result.requiredMaterials = new ItemAmount[requiredMaterials.Length];
        for (var i = 0; i < result.requiredMaterials.Length; ++i)
        {
            var cloningData = requiredMaterials[i];
            var newData = new ItemAmount();
            if (cloningData != null)
            {
                newData.item = cloningData.item;
                newData.amount = cloningData.amount;
            }
            result.requiredMaterials[i] = newData;
        }
        result.evolveItem = evolveItem;
        return result;
    }
    public abstract SpecificItemEvolve<T> Create();
}

[System.Serializable]
public class CreateEvolveItemData
{
    public string title;
    public string description;
    public Sprite icon;
    [Range(1f, 10f)]
    public float attributesRate;
}

public abstract class BaseItem
{
    public string itemid;
    public string title;
    [Multiline]
    public string description;

    // public string characterGuid { get { return name; } }

    [Header("General information")]
    //public Sprite icon;
    public string category;


    [Tooltip("0 is not limit stack"), Range(0, 1000)]
    public int maxStack = 1;

    private string type;
    public string Type
    {
        get
        {
            if (string.IsNullOrEmpty(type))
                type = GetType().ToString();
            return type;
        }
    }

    public virtual int MaxStack
    {
        get { return maxStack; }
    }

    public virtual string ToJson()
    {
        return "{\"SqliteId\":\""  + "\"," +
               "\"itemid\":\"" + itemid + "\"," +
            "\"category\":\"" + category + "\"," +
            "\"type\":\"" + Type + "\"," +
            "\"maxStack\":" + MaxStack + "}";
    }
}

public abstract class BaseActorItem : BaseItem
{
    [Tooltip("Max values of these attributes are max values of `GameDatabase.itemMaxLevel` level")]
    public Attributes attributes;


    public override int MaxStack
    {
        get { return 1; }
    }

    public override string ToJson()
    {
        return "{\"SqliteId\":\"" + "\"," +
               "\"itemid\":\"" + itemid + "\"," +
            "\"category\":\"" + category + "\"," +
            "\"type\":\"" + Type + "\"," +
            "\"maxStack\":" + MaxStack;
    }
    public CalculationAttributes GetTotalAttributes()
    {
        var result = new CalculationAttributes();
        result += attributes.CreateCalculationAttributes(1,50);

        // If this is character item, applies rate attributes
        result.hp += Mathf.CeilToInt(result.hpRate * result.hp);
        result.pAtk += Mathf.CeilToInt(result.pAtkRate * result.pAtk);
        result.pDef += Mathf.CeilToInt(result.pDefRate * result.pDef);
        result.mAtk += Mathf.CeilToInt(result.mAtkRate * result.mAtk);
        result.mDef += Mathf.CeilToInt(result.mDefRate * result.mDef);
        result.spd += Mathf.CeilToInt(result.spdRate * result.spd);
        result.eva += Mathf.CeilToInt(result.evaRate * result.eva);
        result.acc += Mathf.CeilToInt(result.accRate * result.acc);

        result.hp += Mathf.CeilToInt(result._hpRate * result.hp);
        result.pAtk += Mathf.CeilToInt(result._pAtkRate * result.pAtk);
        result.pDef += Mathf.CeilToInt(result._pDefRate * result.pDef);
        result.mAtk += Mathf.CeilToInt(result._mAtkRate * result.mAtk);
        result.mDef += Mathf.CeilToInt(result._mDefRate * result.mDef);
        result.spd += Mathf.CeilToInt(result._spdRate * result.spd);
        result.eva += Mathf.CeilToInt(result._evaRate * result.eva);
        result.acc += Mathf.CeilToInt(result._accRate * result.acc);
        return result;
    }
}
