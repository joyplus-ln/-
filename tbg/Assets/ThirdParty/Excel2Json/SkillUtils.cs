

using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SkillUtils
{


    public static Skill MakeEquipmentItem(Dictionary<string, object> table, Skill equipmentItem)
    {
        SetCommon(table, equipmentItem);
        SetAttacks(table, equipmentItem);
        SetBuffs(table, equipmentItem);
        return equipmentItem;
    }

    static void SetAttacks(Dictionary<string, object> table, Skill character)
    {
        //character.attacks = new SkillAttack[1];
        //character.attacks[0] = new SkillAttack();
        character.attacks[0].attackScope = (AttackScope)int.Parse(table["attacks.attackscope"].ToString());//AttackScope.AllEnemies;
        SetFloatAttribute(ref character.attacks[0].attackDamage.fixDamage, table, "attacks.attackDamage.fixDamage");
        SetFloatAttribute(ref character.attacks[0].attackDamage.fixDamageIncreaseEachLevel, table, "attacks.attackDamage.fixDamageIncreaseEachLevel");
        SetFloatAttribute(ref character.attacks[0].attackDamage.pAtkDamageRate, table, "attacks.attackDamage.pAtkDamageRate");
        SetFloatAttribute(ref character.attacks[0].attackDamage.pAtkDamageRateIncreaseEachLevel, table, "attacks.attackDamage.pAtkDamageRateIncreaseEachLevel");
        SetFloatAttribute(ref character.attacks[0].attackDamage.mAtkDamageRate, table, "attacks.attackDamage.mAtkDamageRate");
        SetFloatAttribute(ref character.attacks[0].attackDamage.mAtkDamageRateIncreaseEachLevel, table, "attacks.attackDamage.mAtkDamageRateIncreaseEachLevel");
        SetIntAttribute(ref character.attacks[0].attackDamage.hitCount, table, "attacks.attackDamage.hitCount");
    }
    static void SetBuffs(Dictionary<string, object> table, Skill character)
    {
        //character.buffs = new SkillBuff[1];
        //character.buffs[0] = new SkillBuff();
        character.buffs[0].type = (BuffType)int.Parse(table["buff.type"].ToString());
        SetFloatAttribute(ref character.buffs[0].applyChance, table, "buff.applyChance");
        SetFloatAttribute(ref character.buffs[0].applyChanceIncreaseEachLevel, table, "buff.applyChanceIncreaseEachLevel");

        SetFloatAttribute(ref character.buffs[0].attributes.hp, table, "buff.attributes.Hp");
        SetFloatAttribute(ref character.buffs[0].attributes.pAtk, table, "buff.attributes.pAtk");
        SetFloatAttribute(ref character.buffs[0].attributes.pDef, table, "buff.attributes.pDef");
        SetFloatAttribute(ref character.buffs[0].attributes.mAtk, table, "buff.attributes.mAtk");
        SetFloatAttribute(ref character.buffs[0].attributes.mDef, table, "buff.attributes.mDef");
        SetFloatAttribute(ref character.buffs[0].attributes.spd, table, "buff.attributes.spd");
        SetFloatAttribute(ref character.buffs[0].attributes.eva, table, "buff.attributes.eva");
        SetFloatAttribute(ref character.buffs[0].attributes.acc, table, "buff.attributes.acc");

        SetFloatAttribute(ref character.buffs[0].attributes.hpRate, table, "buff.attributes.hpRate");
        SetFloatAttribute(ref character.buffs[0].attributes.pAtkRate, table, "buff.attributes.pAtkRate");
        SetFloatAttribute(ref character.buffs[0].attributes.pDefRate, table, "buff.attributes.pDefRate");
        SetFloatAttribute(ref character.buffs[0].attributes.mAtkRate, table, "buff.attributes.mAtkRate");
        SetFloatAttribute(ref character.buffs[0].attributes.mDefRate, table, "buff.attributes.mDefRate");
        SetFloatAttribute(ref character.buffs[0].attributes.spdRate, table, "buff.attributes.spdRate");
        SetFloatAttribute(ref character.buffs[0].attributes.evaRate, table, "buff.attributes.evaRate");
        SetFloatAttribute(ref character.buffs[0].attributes.accRate, table, "buff.attributes.accRate");

        SetFloatAttribute(ref character.buffs[0].attributes.critChance, table, "buff.attributes.CritChance");
        SetFloatAttribute(ref character.buffs[0].attributes.critDamageRate, table, "buff.attributes.critDamageRate");

        SetFloatAttribute(ref character.buffs[0].attributes.blockChance, table, "buff.attributes.BlockChance");
        SetFloatAttribute(ref character.buffs[0].attributes.blockDamageRate, table, "buff.attributes.blockDamageRate");

        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.hp, table, "buff.Attributesincreaseeachlevel.Hp");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.pAtk, table, "buff.Attributesincreaseeachlevel.pAtk");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.pDef, table, "buff.Attributesincreaseeachlevel.pDef");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.mAtk, table, "buff.Attributesincreaseeachlevel.mAtk");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.mDef, table, "buff.Attributesincreaseeachlevel.mDef");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.spd, table, "buff.Attributesincreaseeachlevel.spd");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.eva, table, "buff.Attributesincreaseeachlevel.eva");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.acc, table, "buff.Attributesincreaseeachlevel.acc");

        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.hpRate, table, "buff.Attributesincreaseeachlevel.hpRate");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.pAtkRate, table, "buff.Attributesincreaseeachlevel.pAtkRate");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.pDefRate, table, "buff.Attributesincreaseeachlevel.pDefRate");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.mAtkRate, table, "buff.Attributesincreaseeachlevel.mAtkRate");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.mDefRate, table, "buff.Attributesincreaseeachlevel.mDefRate");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.spdRate, table, "buff.Attributesincreaseeachlevel.spdRate");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.evaRate, table, "buff.Attributesincreaseeachlevel.evaRate");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.accRate, table, "buff.Attributesincreaseeachlevel.accRate");

        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.critChance, table, "buff.Attributesincreaseeachlevel.CritChance");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.critDamageRate, table, "buff.Attributesincreaseeachlevel.critDamageRate");

        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.blockChance, table, "buff.Attributesincreaseeachlevel.BlockChance");
        SetFloatAttribute(ref character.buffs[0].attributesIncreaseEachLevel.blockDamageRate, table, "buff.Attributesincreaseeachlevel.blockDamageRate");


        SetFloatAttribute(ref character.buffs[0].pAtkHealRate, table, "buff.heal.patkhealrate");
        SetFloatAttribute(ref character.buffs[0].pAtkHealRateIncreaseEachLevel, table, "buff.heal.pAtkHealRateIncreaseEachLevel");

        SetFloatAttribute(ref character.buffs[0].mAtkHealRate, table, "buff.heal.mAtkHealRate");
        SetFloatAttribute(ref character.buffs[0].mAtkHealRateIncreaseEachLevel, table, "buff.heal.mAtkHealRateIncreaseEachLevel");

        SetIntAttribute(ref character.buffs[0].clearBuffs, table, "buff.clearbuffs");
        SetIntAttribute(ref character.buffs[0].clearNerfs, table, "buff.clearNerfs");
        character.buffs[0].isStun = bool.Parse(table["buff.isstun"].ToString());
        character.buffs[0].buffScope = (BuffScope)int.Parse(table["buff.buffscope"].ToString());
        SetIntAttribute(ref character.buffs[0].applyTurns, table, "buff.applyturns");
        SetFloatAttribute(ref character.buffs[0].applyTurnsIncreaseEachLevel, table, "buff.applyturnsincreaseeachlevel");

    }



    static void SetCommon(Dictionary<string, object> table, Skill character)
    {
        character.title = table["title"].ToString();
        character.description = table["description"].ToString();
        Debug.Log("==>" + table["usagescope"].ToString());
        character.usageScope = (SkillUsageScope)int.Parse(table["usagescope"].ToString());
        character.coolDownTurns = int.Parse(table["cooldownturns"].ToString());
        character.coolDownIncreaseEachLevel = float.Parse(table["cooldownincreaseeachlevel"].ToString());
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




    public static CustomSkill MakeCustomSkill(string skillId)
    {

        object ect = Assembly.Load("Assembly-CSharp").CreateInstance("Skill" + skillId);//加载程序集，创建程序集里面的 命名空间.类型名 实例
        return (CustomSkill)ect;//类型转换并返回

    }

    public static CustomBuff MakeCustomBuff(string BuffId)
    {

        object ect = Assembly.Load("Assembly-CSharp").CreateInstance("Buff" + BuffId);//加载程序集，创建程序集里面的 命名空间.类型名 实例
        CustomBuff custombuff = (CustomBuff)ect;
        custombuff.guid = System.Guid.NewGuid().ToString();
        custombuff.Init();
        return custombuff;//类型转换并返回

    }
}
