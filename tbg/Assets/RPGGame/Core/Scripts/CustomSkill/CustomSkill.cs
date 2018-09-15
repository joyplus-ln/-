using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CustomSkill
{
    #region 面板中显示的
#endregion
    private List<BaseCharacterEntity> selfs, enemys;
    public SkillUsageScope usageScope = SkillUsageScope.Enemy;
    public string skillName = "技能名称";
    public string des = "这里是自定义技能的描述!";
    public int id;
    //被动技能的属性加成
    public CalculationAttributes SelfAttributes = new CalculationAttributes();
    /// <summary>
    /// 部分增加属性的被动技能使用
    /// </summary>
    public virtual void BattleStart()
    {

    }

    public virtual void DoSkill()
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

    public virtual void Trigger(TriggerType type)
    {
        switch (type)
        {
            case TriggerType.beforeFight:
                BeforeFight();
                break;
            case TriggerType.fight:
                Fight();
                break;
            case TriggerType.afterfight:
                Afterfight();
                break;
            case TriggerType.receiveDamage:
                ReceiveDamage();
                break;
            case TriggerType.beibaoji:
                Beibaoji();
                break;
            case TriggerType.beigedang:
                Beigedang();
                break;
            case TriggerType.beimiss:
                Beimiss();
                break;
            case TriggerType.gobaoji:
                Gobaoji();
                break;
            case TriggerType.gogedang:
                Gogedang();
                break;
            case TriggerType.gomiss:
                Gomiss();
                break;
            default:
                break;
        }


    }


    public int TurnsCount;
    public int CoolDownTurns;
    public bool IsReady()
    {
        return TurnsCount >= CoolDownTurns;
    }
    public void OnUseSkill()
    {
        TurnsCount = 0;
    }

    public int GetCoolDownDuration()
    {
        return 10;
    }


    public void SetNewEntitys(List<BaseCharacterEntity> selfs, List<BaseCharacterEntity> enemys)
    {
        this.selfs = selfs;
        this.enemys = enemys;
    }

    //增加回合
    public void IncreaseTurnsCount()
    {

    }


    public virtual IEnumerator ApplyBuffLogic()
    {
        yield return 0;
    }
    public virtual IEnumerator DoSkillLogic()
    {
        yield return 0;
    }

    protected CharacterEntity GetSelf()
    {
        return GamePlayManager.Singleton.ActiveCharacter;
    }

    public enum SkillUsageScope
    {
        Self,
        Ally,
        Enemy,
        All,
    }

    public enum AttackScope
    {
        SelectedTarget,
        SelectedAndOneRandomTargets,
        SelectedAndTwoRandomTargets,
        SelectedAndThreeRandomTargets,
        OneRandomEnemy,
        TwoRandomEnemies,
        ThreeRandomEnemies,
        FourRandomEnemies,
        AllEnemies,
    }

    public enum BuffScope
    {
        Self,
        SelectedTarget,
        SelectedAndOneRandomTargets,
        SelectedAndTwoRandomTargets,
        SelectedAndThreeRandomTargets,
        OneRandomAlly,
        TwoRandomAllies,
        ThreeRandomAllies,
        FourRandomAllies,
        AllAllies,
        OneRandomEnemy,
        TwoRandomEnemies,
        ThreeRandomEnemies,
        FourRandomEnemies,
        AllEnemies,
        All,
    }

    public enum TriggerType
    {
        beforeFight,
        fight,
        afterfight,
        receiveDamage,
        beibaoji,
        beigedang,
        beimiss,
        gobaoji,
        gogedang,
        gomiss,

    }
 
}

