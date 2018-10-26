using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SkillType
{
    active,
    passive
}

/// <summary>
/// 123
/// 复写 Init   
/// 赋值 skillName   des   spendPower
/// 复写 DoSkillLogic   ApplyBuffLogic
/// </summary>
public class CustomSkill
{


    #region 怪物技能情况

    public int enemyCd = 3;

    public int enemyCurrentCd = 0;


    #endregion

    #region 面板中显示的
    public string skillName = "技能名称";
    public string des = "这里是自定义技能的描述!";
    //技能消耗的能量
    public int spendPower = 10;
    #endregion
    protected List<BaseCharacterEntity> selfs, enemys;
    protected CharacterEntity selfOnly;
    public SkillUsageScope usageScope = SkillUsageScope.Enemy;


    public int id;

    //主动技能，被动技能,默认主动
    public SkillType skilltype = SkillType.active;
    //被动技能的属性加成
    public CalculationAttributes SelfAttributes = new CalculationAttributes();


    private bool inited = false;

    #region 一堆方法
    /// <summary>
    /// 开场触发的效果也在init里面执行
    /// </summary>
    public virtual void Init()
    {
    }
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

    /// <summary>
    /// 随机获取几个地方单位
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    protected List<BaseCharacterEntity> GetRandomEnemy(int random)
    {
        List<BaseCharacterEntity> allEnemys = GamePlayManager.Singleton.GetFoes(selfOnly);
        List<BaseCharacterEntity> givCharacterEntities = new List<BaseCharacterEntity>();
        if (allEnemys.Count >= random)
        {
            List<int> randomIndex = new List<int>();
            Random.InitState(DateTime.Now.Millisecond);
            int index = UnityEngine.Random.Range(0, allEnemys.Count);

            int seed = 1;
            while (randomIndex.Count < random)
            {
                seed++;
                Random.InitState(DateTime.Now.Millisecond + seed);
                index = UnityEngine.Random.Range(0, allEnemys.Count);
                if (!randomIndex.Contains(index))
                {
                    randomIndex.Add(index);
                }
            }
            for (int i = 0; i < randomIndex.Count; i++)
            {
                givCharacterEntities.Add(allEnemys[randomIndex[i]]);
            }
            return givCharacterEntities;
        }
        else
        {
            return allEnemys;
        }

    }

    protected List<BaseCharacterEntity> GetSelfFriends()
    {
        List<BaseCharacterEntity> allFriends = GamePlayManager.Singleton.GetAllies(selfOnly);
        return allFriends;
    }

    /// <summary>
    /// 是否可以使用技能
    /// </summary>
    /// <returns></returns>
    public virtual bool CanUse()
    {
        if (selfOnly.IsPlayerCharacter)
        {
            if (GamePlayManager.Singleton.uiUseSkillManager.sikllPower.GetCurrentPower() < spendPower) return false;
            return true;
        }
        else
        {
            //怪物
            if (enemyCurrentCd >= enemyCd)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 能量是否充足
    /// </summary>
    /// <returns></returns>
    public virtual bool PowerEnough()
    {
        if (selfOnly.IsPlayerCharacter)
        {
            if (skilltype == SkillType.passive) return true;
            if (GamePlayManager.Singleton.uiUseSkillManager.sikllPower.GetCurrentPower() < spendPower) return false;
            return true;
        }
        else
        {
            //怪物
            if (enemyCurrentCd >= enemyCd)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 新的一轮 
    /// </summary>
    public virtual void IncreaseTurnsCount()
    {
        if (selfOnly.IsPlayerCharacter)
        {

        }
        else
        {
            enemyCurrentCd++;
        }
    }

    public virtual void Trigger(TriggerType type)
    {
        switch (type)
        {
            case TriggerType.beforeFight:
                if (inited == false)
                {
                    BattleStart();
                    inited = true;
                }
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

    #endregion

    /// <summary>
    /// 使用技能，技能内部逻辑
    /// </summary>
    public virtual void OnUseSkill()
    {
        if (selfOnly.IsPlayerCharacter)
        {
            GamePlayManager.Singleton.uiUseSkillManager.sikllPower.UsePower(spendPower);
        }
        else
        {
            enemyCurrentCd = 0;
        }

    }


    /// <summary>
    /// 初始化技能，开局和每次使用技能都初始化
    /// </summary>
    /// <param name="selfOnly"></param>
    /// <param name="selfs"></param>
    /// <param name="enemys"></param>
    public void SetNewEntitys(CharacterEntity selfOnly, List<BaseCharacterEntity> selfs, List<BaseCharacterEntity> enemys)
    {
        this.selfOnly = selfOnly;
        this.selfs = selfs;
        this.enemys = enemys;
    }



    public virtual IEnumerator ApplyBuffLogic()
    {
        yield return null;
    }

    public virtual void ApplyBuffLogicM()
    {
    }
    public virtual IEnumerator DoSkillLogic()
    {
        yield return null;
    }

    protected CharacterEntity GetSelf()
    {
        return GamePlayManager.Singleton.ActiveCharacter;
    }

    protected IEnumerator MoveToTarget()
    {
        yield return selfOnly.MoveTo(selfOnly.Manager.MapCenterPosition, selfOnly.Manager.doActionMoveSpeed);
    }

    protected IEnumerator MoveOut()
    {
        yield return selfOnly.MoveTo(selfOnly.Container.position + new Vector3(5, 0, 0), selfOnly.Manager.actionDoneMoveSpeed);
    }
    protected IEnumerator MoveToSelfPos()
    {
        yield return selfOnly.MoveTo(selfOnly.Container.position, selfOnly.Manager.actionDoneMoveSpeed);
    }

    protected IEnumerator MoveSkillOB(Action logic)
    {
        yield return selfOnly.MoveTo(selfOnly.Container.position + new Vector3(5, 0, 0), selfOnly.Manager.actionDoneMoveSpeed);
        if (logic != null) logic.Invoke();
        yield return selfOnly.MoveTo(selfOnly.Container.position - new Vector3(5, 0, 0), selfOnly.Manager.actionDoneMoveSpeed);
    }

    /// <summary>
    /// 移动到指定目标旁边并攻击
    /// </summary>
    /// <param name="atkAttackDamage"></param>
    /// <param name="targetEntity"></param>
    /// <returns></returns>
    public virtual IEnumerator Patk(SkillAttackDamage atkAttackDamage, CharacterEntity targetEntity = null)
    {
        if (targetEntity == null)
            targetEntity = selfOnly.ActionTarget;
        yield return selfOnly.MoveTo(targetEntity.CastedFormation.GetTarget2Position(targetEntity.Position), 1);
        selfOnly.Attack(targetEntity, atkAttackDamage.GetPAtkDamageRate(), atkAttackDamage.GetMAtkDamageRate(), atkAttackDamage.hitCount, (int)atkAttackDamage.GetFixDamage());
        yield return selfOnly.MoveTo(targetEntity.CastedFormation.GetTarget1Position(targetEntity.Position), 1);
    }

    /// <summary>
    /// 直接攻击目标，不需要移动过去
    /// </summary>
    /// <param name="atkAttackDamage"></param>
    /// <param name="targetEntity"></param>
    /// <returns></returns>
    public virtual IEnumerator PatkImmediate(SkillAttackDamage atkAttackDamage, CharacterEntity targetEntity = null)
    {
        if (targetEntity == null)
            targetEntity = selfOnly.ActionTarget;
        selfOnly.Attack(targetEntity, atkAttackDamage.GetPAtkDamageRate(), atkAttackDamage.GetMAtkDamageRate(), atkAttackDamage.hitCount, (int)atkAttackDamage.GetFixDamage());
        yield return null;
    }

    public virtual void AttackTarget(SkillAttackDamage atkAttackDamage, CharacterEntity targetEntity = null)
    {
        if (targetEntity == null)
            targetEntity = selfOnly.ActionTarget;
        selfOnly.Attack(targetEntity, atkAttackDamage.GetPAtkDamageRate(), atkAttackDamage.GetMAtkDamageRate(), atkAttackDamage.hitCount, (int)atkAttackDamage.GetFixDamage());
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

