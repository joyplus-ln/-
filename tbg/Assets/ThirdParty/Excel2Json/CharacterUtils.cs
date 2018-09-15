using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUtils
{


    public static CharacterItem MakeCharacterItem(Dictionary<string, object> table, CharacterItem character)
    {
        SetCommon(table, character);
        SetAttributes(table, character);
        return character;
    }



    static void SetCommon(Dictionary<string, object> table, CharacterItem character)
    {
        character.title = table["title"].ToString();
        character.description = table["description"].ToString();
        character.category = table["category"].ToString();
    }
    static void SetAttributes(Dictionary<string, object> table, CharacterItem character)
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
