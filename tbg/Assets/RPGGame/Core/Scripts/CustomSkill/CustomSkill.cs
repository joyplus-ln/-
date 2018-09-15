using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomSkill
{
    private List<BaseCharacterEntity> selfs, enemys;
    public SkillUsageScope usageScope = SkillUsageScope.Enemy;
    public string skillName = "技能名称";
    public string des = "这里是自定义技能的描述!";
    public int id;
    public abstract void DoSkill();

    public abstract void BeforeFight();
    public abstract void Fight();
    public abstract void Afterfight();
    public abstract void ReceiveDamage();
    public abstract void Beibaoji();
    public abstract void Beigedang();
    public abstract void Beimiss();
    public abstract void Gobaoji();
    public abstract void Gogedang();
    public abstract void Gomiss();

    public void Trigger(TriggerType type)
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


    public void SetNewEntitys(List<BaseCharacterEntity> selfs, List<BaseCharacterEntity> enemys)
    {
        this.selfs = selfs;
        this.enemys = enemys;
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
}
