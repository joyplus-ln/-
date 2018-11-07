using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBuff
{
    public string des;
    /// <summary>
    /// 如果buff是不可叠加的 需要制定guid
    /// </summary>
    public string guid;
    public string buffText;

    protected CharacterEntity selfOnly;
    protected CharacterEntity giver;
    public CalculationAttributes SelfAttributes = new CalculationAttributes();

    /// <summary>
    /// 沉默状态
    /// </summary>
    protected int CMState = 0;

    /// <summary>
    /// 眩晕
    /// </summary>
    protected int XYState = 0;


    /// <summary>
    /// buff剩余回合数，如果是-99 则代表永远带着的buff ,如果buff是不可叠加的 需要在init中赋值guid
    /// </summary>
    protected int turns = 0;

    protected CharacterEntity MustCharacterEntity = null;

    public virtual void Init()
    {
        des = "自定义技能 ID 001";
        buffText = "A";
    }

    public virtual void SetSelf(CharacterEntity selfOnly)
    {
        this.selfOnly = selfOnly;
    }

    public virtual void SetGiver(CharacterEntity giver)
    {
        this.giver = giver;
    }

    #region 各种方法

    /// <summary>
    /// 战斗之前的触发，每次战斗只触发一次
    /// </summary>
    public virtual void BattleStart()
    {

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

    public virtual void FriendsReceiveDamage()
    {

    }

    public virtual void FriendsAttack()
    {

    }
    #endregion



    /// <summary>
    /// 减少技能回合
    /// </summary>
    public virtual void ReduceTurnsCount()
    {
        if (turns == -99) return;
        if (turns > 0)
            turns--;
        if (CMState > 0)
            CMState--;
        if (XYState > 0)
            XYState--;
    }

    //增加回合
    public virtual void IncreaseTurnsCount()
    {
        if (turns == -99) return;
        if (turns > 0)
            turns++;
    }

    /// <summary>
    /// 是否可以使用skill cm中
    /// </summary>
    /// <returns></returns>
    public virtual bool CanUseSkill()
    {
        if (CMState > 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 是否可以行动，xy中
    /// </summary>
    /// <returns></returns>
    public virtual bool CanDoAction()
    {
        if (XYState > 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 强制目标
    /// </summary>
    /// <returns></returns>
    public virtual CharacterEntity MustTargetCharacterEntity()
    {
        return MustCharacterEntity;
    }
    //是否结束了
    public virtual bool IsEnd()
    {
        if (turns == -99) return false;
        return turns <= 0;
    }

    /// <summary>
    /// 是否在buff条目中显示出来
    /// </summary>
    /// <returns></returns>
    public virtual bool ShowInGUI()
    {
        return true;
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
            case CustomSkill.TriggerType.friendsReceiveDamage:
                FriendsReceiveDamage();
                break;
            case CustomSkill.TriggerType.friendsAttack:
                FriendsAttack();
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