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


        if (self.SelectedCustomSkill == null || !self.SelectedCustomSkill.IsReady())
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
        Dictionary<int, int> actions = new Dictionary<int, int>();
        actions.Add(CharacterEntity.ACTION_ATTACK, 5);
        for (var i = 0; i < self.CustomSkills.Count; ++i)
        {
            var skill = self.CustomSkills[i];
            if (skill == null || !skill.IsReady())
                continue;
            actions.Add(i, 5);
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

        yield return new WaitForSeconds(2);
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
        var selfEnsmys = self.Manager.GetAllies(self);
        var enemyfoes = self.Manager.GetFoes(self);
        self.SelectedCustomSkill.SetNewEntitys(self, selfEnsmys, enemyfoes);
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
