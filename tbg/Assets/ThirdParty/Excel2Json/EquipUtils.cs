using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUtils
{

    public static EquipmentItem MakeEquipmentItem(Dictionary<string, object> table, EquipmentItem equipmentItem)
    {
        SetCommon(table, equipmentItem);
        SetAttributes(table, equipmentItem);
        SetExtraAttributes(table, equipmentItem);
        return equipmentItem;
    }

    static void SetExtraAttributes(Dictionary<string, object> table, EquipmentItem character)
    {
        SetFloatAttribute(ref character.extraAttributes.hp, table, "extraAttributes.hp");
        SetFloatAttribute(ref character.extraAttributes.pAtk, table, "extraAttributes.pAtk");
        SetFloatAttribute(ref character.extraAttributes.pDef, table, "extraAttributes.pDef");
        SetFloatAttribute(ref character.extraAttributes.mAtk, table, "extraAttributes.mAtk");
        SetFloatAttribute(ref character.extraAttributes.mDef, table, "extraAttributes.mDef");
        SetFloatAttribute(ref character.extraAttributes.spd, table, "extraAttributes.spd");
        SetFloatAttribute(ref character.extraAttributes.eva, table, "extraAttributes.eva");
        SetFloatAttribute(ref character.extraAttributes.acc, table, "extraAttributes.acc");

        SetFloatAttribute(ref character.extraAttributes.hpRate, table, "extraAttributes.hpRate");
        SetFloatAttribute(ref character.extraAttributes.pAtkRate, table, "extraAttributes.pAtkRate");
        SetFloatAttribute(ref character.extraAttributes.pDefRate, table, "extraAttributes.pDefRate");
        SetFloatAttribute(ref character.extraAttributes.mAtkRate, table, "extraAttributes.mAtkRate");
        SetFloatAttribute(ref character.extraAttributes.mDefRate, table, "extraAttributes.mDefRate");
        SetFloatAttribute(ref character.extraAttributes.spdRate, table, "extraAttributes.spdRate");
        SetFloatAttribute(ref character.extraAttributes.evaRate, table, "extraAttributes.evaRate");
        SetFloatAttribute(ref character.extraAttributes.accRate, table, "extraAttributes.accRate");

        SetFloatAttribute(ref character.extraAttributes.critChance, table, "extraAttributes.critChance");
        SetFloatAttribute(ref character.extraAttributes.critDamageRate, table, "extraAttributes.critDamageRate");

        SetFloatAttribute(ref character.extraAttributes.blockChance, table, "extraAttributes.blockChance");
        SetFloatAttribute(ref character.extraAttributes.blockDamageRate, table, "extraAttributes.blockDamageRate");
    }

    static void SetCommon(Dictionary<string, object> table, EquipmentItem character)
    {
        character.title = table["title"].ToString();
        character.description = table["description"].ToString();
        character.category = table["category"].ToString();
    }
    static void SetAttributes(Dictionary<string, object> table, EquipmentItem character)
    {
        SetFloatAttribute(ref character.attributes.acc.growth, table, "acc.growth");
        SetIntAttribute(ref character.attributes.acc.maxValue, table, "acc.maxValue");
        SetIntAttribute(ref character.attributes.acc.minValue, table, "acc.minValue");

        SetFloatAttribute(ref character.attributes.eva.growth, table, "eva.growth");
        SetIntAttribute(ref character.attributes.eva.maxValue, table, "eva.maxValue");
        SetIntAttribute(ref character.attributes.eva.minValue, table, "eva.minValue");

        SetFloatAttribute(ref character.attributes.hp.growth, table, "hp.growth");
        SetIntAttribute(ref character.attributes.hp.maxValue, table, "hp.maxValue");
        SetIntAttribute(ref character.attributes.hp.minValue, table, "hp.minValue");

        SetFloatAttribute(ref character.attributes.mAtk.growth, table, "mAtk.growth");
        SetIntAttribute(ref character.attributes.mAtk.maxValue, table, "mAtk.maxValue");
        SetIntAttribute(ref character.attributes.mAtk.minValue, table, "mAtk.minValue");

        SetFloatAttribute(ref character.attributes.mDef.growth, table, "mDef.growth");
        SetIntAttribute(ref character.attributes.mDef.maxValue, table, "mDef.maxValue");
        SetIntAttribute(ref character.attributes.mDef.minValue, table, "mDef.minValue");

        SetFloatAttribute(ref character.attributes.pAtk.growth, table, "pAtk.growth");
        SetIntAttribute(ref character.attributes.pAtk.maxValue, table, "pAtk.maxValue");
        SetIntAttribute(ref character.attributes.pAtk.minValue, table, "pAtk.minValue");

        SetFloatAttribute(ref character.attributes.pDef.growth, table, "pDef.growth");
        SetIntAttribute(ref character.attributes.pDef.maxValue, table, "pDef.maxValue");
        SetIntAttribute(ref character.attributes.pDef.minValue, table, "pDef.minValue");

        SetFloatAttribute(ref character.attributes.spd.growth, table, "spd.growth");
        SetIntAttribute(ref character.attributes.spd.maxValue, table, "spd.maxValue");
        SetIntAttribute(ref character.attributes.spd.minValue, table, "spd.minValue");
    }
    static void SetFloatAttribute(ref float def, Dictionary<string, object> table, string key)
    {
        key = key.ToLower();
        if (table.ContainsKey(key))
        {
            if (table[key] == null)
            {
                Debug.Log("table null key:" + key);
                return;
            }
            float temp = 0;
            if (float.TryParse(table[key].ToString(), out temp))
            {
                def = temp;
            }
            else
            {
                Debug.Log("table wrong key:" + key);
            }
        }
        else
        {
            Debug.Log("table dont contain key:" + key);
        }
    }

    static void SetIntAttribute(ref int def, Dictionary<string, object> table, string key)
    {
        key = key.ToLower();
        if (table.ContainsKey(key))
        {
            if (table[key] == null)
            {
                Debug.Log("table null key:" + key);
                return;
            }
            int temp = 0;
            if (int.TryParse(table[key].ToString(), out temp))
            {
                def = temp;
            }
            else
            {
                Debug.Log("table wrong key:" + key);
            }
        }
        else
        {
            Debug.Log("table dont contain key:" + key);
        }
    }
}
