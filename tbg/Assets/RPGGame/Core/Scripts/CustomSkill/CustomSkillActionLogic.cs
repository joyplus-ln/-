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
            case SkillUsageScope.Self:
                if (target != self)
                    return false;
                break;
            case SkillUsageScope.Enemy:
                if (target == self || IsSameTeamWith(target))
                    return false;
                break;
            case SkillUsageScope.Ally:
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
    IEnumerator SkillAttackRoutine()
    {
        var skill = self.SelectedCustomSkill;

        var foes = self.Manager.GetFoes(self);
        var isAlreadyReachedTarget = false;

        var attackDamage = new SkillAttackDamage();

        
        // Move to target character
        //yield return self.MoveTo(self.ActionTarget, self.Manager.doActionMoveSpeed);
        //isAlreadyReachedTarget = true;

        self.Attack(self.ActionTarget, null, attackDamage.GetPAtkDamageRate(), attackDamage.GetMAtkDamageRate(), attackDamage.hitCount, (int)attackDamage.GetFixDamage());
        self.ClearActionState();
        yield return 0;

        // End attack loop
        // Wait damages done
        while (self.Damages.Count > 0)
        {
            yield return 0;
        }

    }

    public void RandomAction()
    {
        // Random Action
        // Dictionary of actionId, weight
        Dictionary<int, int> actions = new Dictionary<int, int>();
        actions.Add(CharacterEntity.ACTION_ATTACK, 5);
        for (var i = 0; i < self.Skills.Count; ++i)
        {
            var skill = self.Skills[i] as CharacterSkill;
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
            switch (self.SelectedSkill.CastedSkill.usageScope)
            {
                case SkillUsageScope.Enemy:
                    var foes = self.Manager.GetFoes(self);
                    Random.InitState(System.DateTime.Now.Millisecond);
                    self.ActionTarget = foes[Random.Range(0, foes.Count)] as CharacterEntity;
                    break;
                case SkillUsageScope.Ally:
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
        self.Attack(self.ActionTarget, null);
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
        var skill = self.SelectedCustomSkill;
        // Cast
        yield return self.MoveTo(self.Manager.MapCenterPosition, self.Manager.doActionMoveSpeed);
        self.ClearActionState();
        // Buffs
        yield return self.StartCoroutine(ApplyBuffsRoutine());
        // Attacks
        yield return self.StartCoroutine(SkillAttackRoutine());
        // Move back to formation
        yield return self.MoveTo(self.Container.position, self.Manager.actionDoneMoveSpeed);
        self.NotifyEndAction();
        self.IsDoingAction = false;
    }

    //custom buff
    IEnumerator ApplyBuffsRoutine()
    {
        var allies = self.Manager.GetAllies(self);
        var foes = self.Manager.GetFoes(self);
        self.SelectedCustomSkill.SetNewEntitys(allies, foes);
        yield return self.SelectedCustomSkill.ApplyBuffLogic();
        yield return 0;
    }
}
