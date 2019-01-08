using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;

public class BattleItem
{
    private ICharacter icharacter;
    private IPlayerHasCharacters ihascharacter;
    private bool isEnemy = false;

    private Dictionary<string, CustomSkill> skills = new Dictionary<string, CustomSkill>();
    private Dictionary<string, CustomBuff> buffs = new Dictionary<string, CustomBuff>();

    public Dictionary<string, CustomSkill> GetCustomSkills()
    {
        return skills;
    }

    public Dictionary<string, CustomBuff> GetBuffs()
    {
        return buffs;
    }

    public BattleItem(ICharacter icharacter, int level, Const.StageType type)
    {
        this.icharacter = icharacter;
        skills = icharacter.GetCloneCustomSkills();
        isEnemy = true;
    }

    public BattleItem(IPlayerHasCharacters ihascharacter, Const.StageType type)
    {
        this.ihascharacter = ihascharacter;
        skills = ihascharacter.Character.GetCloneCustomSkills();
        isEnemy = false;
    }
    public CalculationAttributes GetCalculationAttributes()
    {
        if (isEnemy)
            return icharacter.GetAttributes().GetSubAttributes();
        else
        {
            return ihascharacter.GetCalculationAttributesWithProp();
        }
    }
    public CalculationAttributes GetAllCalculationAttributes()
    {
        CalculationAttributes skillCalculationAttributes = GetCalculationAttributes();
        foreach (var buff in buffs.Values)
        {
            skillCalculationAttributes += buff.SelfAttributes;
        }

        foreach (var cskill in skills.Values)
        {
            skillCalculationAttributes += cskill.SelfAttributes;
        }

        return skillCalculationAttributes;
    }
}
