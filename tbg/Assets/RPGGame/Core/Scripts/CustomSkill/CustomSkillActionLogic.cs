using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSkillActionLogic
{

    private CharacterEntity self;


    public CustomSkillActionLogic(CharacterEntity self)
    {
        this.self = self;
    }

    public bool DoAction(CharacterEntity target)
    {
        if (!self.Custombody.CanUseKill())
        {
            // cmstate 只能使用 normal attack
            self.Action = CharacterEntity.ACTION_ATTACK;
        }

        if (self.SelectedCustomSkill == null || !self.SelectedCustomSkill.CanUse())
            return false;
        switch (self.SelectedCustomSkill.usageScope)
        {
            case CustomSkill.SkillUsageScope.Self:
                if (target != self)
                    return false;
                break;
            case CustomSkill.SkillUsageScope.Enemy:
                if (target == self || IsSameTeamWith(target))
                    return false;
                break;
            case CustomSkill.SkillUsageScope.Ally:
                if (!IsSameTeamWith(target))
                    return false;
                break;
        }
        self.ActionTarget = target;
        GamePlayManager.Singleton.uiUseSkillManager.Hide();
        DoAction();
        return true;
    }


    public bool IsSameTeamWith(CharacterEntity target)
    {
        return target != null && self.Formation == target.Formation;
    }

    //custom skill logic


    public void RandomAction()
    {
        // Random Action
        // Dictionary of actionId, weight
        Dictionary<string, int> actions = new Dictionary<string, int>();
        actions.Add(CharacterEntity.ACTION_ATTACK, 5);
        foreach (string key in self.Item.GetCustomSkills().Keys)
        {
            var skill = self.Item.GetCustomSkills()[key];
            if (skill == null || !skill.CanUse())
                continue;
            actions.Add(key, 5);
        }
        self.Action = WeightedRandomizer.From(actions).TakeOne();
        // Random Target
        if (self.Action == CharacterEntity.ACTION_ATTACK)
        {
            var foes = self.Manager.GetFoes(self);
            Random.InitState(System.DateTime.Now.Millisecond);
            self.ActionTarget = foes[Random.Range(0, foes.Count - 1)] as CharacterEntity;
        }
        else
        {
            switch (self.SelectedCustomSkill.usageScope)
            {
                case CustomSkill.SkillUsageScope.Enemy:
                    var foes = self.Manager.GetFoes(self);
                    Random.InitState(System.DateTime.Now.Millisecond);
                    self.ActionTarget = foes[Random.Range(0, foes.Count)] as CharacterEntity;
                    break;
                case CustomSkill.SkillUsageScope.Ally:
                    var allies = self.Manager.GetAllies(self);
                    Random.InitState(System.DateTime.Now.Millisecond);
                    self.ActionTarget = allies[Random.Range(0, allies.Count)] as CharacterEntity;
                    break;
                default:
                    self.ActionTarget = null;
                    break;
            }
        }
        DoAction();
    }

    private void DoAction()
    {
        HandleSkill();

        if (self.IsDoingAction)
            return;

        if (self.Action == CharacterEntity.ACTION_ATTACK)
            self.StartCoroutine(DoAttackActionRoutine());
        else
        {
            self.SelectedCustomSkill.OnUseSkill();
            self.StartCoroutine(DoSkillActionRoutine());
        }
    }

    /// <summary>
    /// 处理嘲讽，沉默，等
    /// </summary>
    void HandleSkill()
    {
        bool canUseSkill = true;
        CharacterEntity mustTarget = null;
        foreach (var buff in self.Item.GetBuffs().Values)
        {
            if (!buff.CanUseSkill())
                canUseSkill = false;
            if (buff.MustTargetCharacterEntity() != null)
                mustTarget = buff.MustTargetCharacterEntity();
        }
        if (!canUseSkill)
        {
            self.Action = CharacterEntity.ACTION_ATTACK;
        }
        if (mustTarget != null)
        {
            self.ActionTarget = mustTarget;
        }
    }

    IEnumerator DoAttackActionRoutine()
    {
        self.IsDoingAction = true;
        var manager = self.Manager;

        // Move to target character
        yield return self.MoveTo(self.ActionTarget, self.Manager.doActionMoveSpeed);

        // Apply damage
        self.Attack(self.ActionTarget);
        // Wait damages done
        while (self.Damages.Count > 0)
        {
            yield return 0;
        }

        yield return new WaitForSeconds(0.2f);
        self.ClearActionState();
        yield return self.MoveTo(self.Container.position, self.Manager.actionDoneMoveSpeed);
        self.NotifyEndAction();
        self.IsDoingAction = false;
    }

    IEnumerator DoSkillActionRoutine()
    {
        self.IsDoingAction = true;
        // Cast

        self.ClearActionState();
        // Buffs
        yield return self.StartCoroutine(SkillAttackRoutine());
        // Attacks
        //yield return self.StartCoroutine(ApplyBuffsRoutine());
        // Move back to formation

        self.NotifyEndAction();
        self.IsDoingAction = false;
    }
    IEnumerator SkillAttackRoutine()
    {
        //var isAlreadyReachedTarget = false;
        yield return self.SelectedCustomSkill.DoSkillLogic();


        // Move to target character
        //yield return self.MoveTo(self.ActionTarget, self.Manager.doActionMoveSpeed);
        //isAlreadyReachedTarget = true;
        self.ClearActionState();
        yield return 0;

        // End attack loop
        // Wait damages done
        while (self.Damages.Count > 0)
        {
            yield return 0;
        }

    }

    /// <summary>
    /// 初始化一些局内战斗属性，在局内调用
    /// </summary>
    public void InitBattleCustomSkill()
    {
        if (self.Manager == null)
        {
            Debug.Log("不在局内  不需要初始化技能");
            return;
        }
        var selfEnsmys = self.Manager.GetAllies(self);
        var enemyfoes = self.Manager.GetFoes(self);
        foreach (string key in self.Item.GetCustomSkills().Keys)
        {
            self.Item.GetCustomSkills()[key].SetNewEntitys(self, selfEnsmys, enemyfoes);
        }
    }
    //buff放到技能里边放
    ////custom buff
    //IEnumerator ApplyBuffsRoutine()
    //{
    //    var selfEnsmys = self.Manager.GetAllies(self);
    //    var enemyfoes = self.Manager.GetFoes(self);
    //    self.SelectedCustomSkill.SetNewEntitys(selfEnsmys, enemyfoes);
    //    yield return self.SelectedCustomSkill.ApplyBuffLogic();
    //    yield return 0;
    //}
}
