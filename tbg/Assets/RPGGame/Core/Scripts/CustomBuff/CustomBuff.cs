using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBuff
{
    public string des;
    public string guid;
    public string buffText;

    protected CharacterEntity selfOnly;
    protected CharacterEntity giver;
    public CalculationAttributes SelfAttributes = new CalculationAttributes();

    public virtual void Init()
    {
    }

    public virtual void SetSelf(CharacterEntity selfOnly)
    {
        this.selfOnly = selfOnly;
    }

    public virtual void SetGiver(CharacterEntity giver)
    {
        this.giver = giver;
    }

    public virtual void BeforeFight()
    {

    }

    public virtual void Fight()
    {
    }

    public virtual void Afterfight()
    {
    }

    public virtual void ReceiveDamage()
    {
    }

    public virtual void Beibaoji()
    {
    }

    public virtual void Beigedang()
    {
    }

    public virtual void Beimiss()
    {
    }

    public virtual void Gobaoji()
    {
    }

    public virtual void Gogedang()
    {
    }

    public virtual void Gomiss()
    {
    }

    public virtual void BuffRemove()
    {

    }

    //是否是眩晕
    public bool GetIsStun()
    {
        return false;
    }

    //增加回合
    public void IncreaseTurnsCount()
    {

    }

    //是否结束了
    public bool IsEnd()
    {
        return false;
    }

    public void Trigger(CustomSkill.TriggerType type)
    {
        switch (type)
        {
            case CustomSkill.TriggerType.beforeFight:
                BeforeFight();
                break;
            case CustomSkill.TriggerType.fight:
                Fight();
                break;
            case CustomSkill.TriggerType.afterfight:
                Afterfight();
                break;
            case CustomSkill.TriggerType.receiveDamage:
                ReceiveDamage();
                break;
            case CustomSkill.TriggerType.beibaoji:
                Beibaoji();
                break;
            case CustomSkill.TriggerType.beigedang:
                Beigedang();
                break;
            case CustomSkill.TriggerType.beimiss:
                Beimiss();
                break;
            case CustomSkill.TriggerType.gobaoji:
                Gobaoji();
                break;
            case CustomSkill.TriggerType.gogedang:
                Gogedang();
                break;
            case CustomSkill.TriggerType.gomiss:
                Gomiss();
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public struct SkillAttackDamage
{
    public float fixDamage;
    public float fixDamageIncreaseEachLevel;
    public float pAtkDamageRate;
    public float pAtkDamageRateIncreaseEachLevel;
    public float mAtkDamageRate;
    public float mAtkDamageRateIncreaseEachLevel;
    [Tooltip("This will devide with calculated damage to show damage number text")]
    public int hitCount;

    public float GetFixDamage(int level = 1)
    {
        return fixDamage + (fixDamageIncreaseEachLevel * level);
    }

    public float GetPAtkDamageRate(int level = 1)
    {
        return pAtkDamageRate + (pAtkDamageRateIncreaseEachLevel * level);
    }

    public float GetMAtkDamageRate(int level = 1)
    {
        return mAtkDamageRate + (mAtkDamageRateIncreaseEachLevel * level);
    }

    public SkillAttackDamage(int level = 1)
    {
        fixDamage = 0;
        fixDamageIncreaseEachLevel = 0;
        pAtkDamageRate = 0;
        pAtkDamageRateIncreaseEachLevel = 0;
        mAtkDamageRate = 0;
        mAtkDamageRateIncreaseEachLevel = 0;
        hitCount = 1;
    }

    public SkillAttackDamage(float fixDamage, float pAtkDamageRate, float mAtkDamageRate, float fixDamageIncreaseEachLevel = 0, float pAtkDamageRateIncreaseEachLevel = 0, float mAtkDamageRateIncreaseEachLevel = 0)
    {
        this.fixDamage = fixDamage;
        this.pAtkDamageRate = pAtkDamageRate;
        this.mAtkDamageRate = mAtkDamageRate;
        this.fixDamageIncreaseEachLevel = 0;
        this.pAtkDamageRateIncreaseEachLevel = 0;
        this.mAtkDamageRateIncreaseEachLevel = 0;
        this.hitCount = 1;
    }
}

[System.Serializable]
public class SkillAttack
{
    public CustomSkill.AttackScope attackScope;
    [Tooltip("Skill damage formula = `a.fixDamage` + ((`a.pAtkDamageRate` * `a.pAtk`) - `b.pDef`) + ((`a.mAtkDamageRate` * `a.mAtk`) - `b.mDef`)")]
    public SkillAttackDamage attackDamage;
}