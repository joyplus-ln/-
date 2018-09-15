using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSkillLogic
{

    private CharacterEntity self;

    public NormalSkillLogic(CharacterEntity self)
    {
        this.self = self;
    }

    public bool DoAction(CharacterEntity target)
    {

        if (self.SelectedSkill == null || !self.SelectedSkill.IsReady())
            return false;

        switch (self.SelectedSkill.CastedSkill.usageScope)
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

    IEnumerator SkillAttackRoutine()
    {
        var level = self.SelectedSkill.Level;
        var skill = self.SelectedSkill.CastedSkill;
        if (skill.attacks.Length > 0)
        {
            var foes = self.Manager.GetFoes(self);
            var isAlreadyReachedTarget = false;
            foreach (var attack in skill.attacks)
            {
                if (attack == null)
                    continue;
                var attackDamage = attack.attackDamage;
                
                    // Move to target character
                    yield return self.MoveTo(self.ActionTarget, self.Manager.doActionMoveSpeed);
                    isAlreadyReachedTarget = true;
                

                yield return new WaitForSeconds(2);
                // Apply damage
                // Attack to selected target
                switch (attack.attackScope)
                {
                    case AttackScope.SelectedTarget:
                    case AttackScope.SelectedAndOneRandomTargets:
                    case AttackScope.SelectedAndTwoRandomTargets:
                    case AttackScope.SelectedAndThreeRandomTargets:
                        self.Attack(self.ActionTarget, null, attackDamage.GetPAtkDamageRate(level), attackDamage.GetMAtkDamageRate(level), attackDamage.hitCount, (int)attackDamage.GetFixDamage(level));
                        break;
                }
                // Attack to random targets
                int randomFoeCount = 0;
                // Attack scope
                switch (attack.attackScope)
                {
                    case AttackScope.SelectedAndOneRandomTargets:
                    case AttackScope.OneRandomEnemy:
                        randomFoeCount = 1;
                        break;
                    case AttackScope.SelectedAndTwoRandomTargets:
                    case AttackScope.TwoRandomEnemies:
                        randomFoeCount = 2;
                        break;
                    case AttackScope.SelectedAndThreeRandomTargets:
                    case AttackScope.ThreeRandomEnemies:
                        randomFoeCount = 3;
                        break;
                    case AttackScope.FourRandomEnemies:
                        randomFoeCount = 4;
                        break;
                    case AttackScope.AllEnemies:
                        randomFoeCount = foes.Count;
                        break;
                }
                // End attack scope
                while (foes.Count > 0 && randomFoeCount > 0)
                {
                    Random.InitState(System.DateTime.Now.Millisecond);
                    var randomIndex = Random.Range(0, foes.Count - 1);
                    var attackingTarget = foes[randomIndex] as CharacterEntity;
                    self.Attack(attackingTarget, null, attackDamage.GetPAtkDamageRate(level), attackDamage.GetMAtkDamageRate(level), attackDamage.hitCount, (int)attackDamage.GetFixDamage(level));
                    foes.RemoveAt(randomIndex);
                    --randomFoeCount;
                }

                yield return new WaitForSeconds(2);
                self.ClearActionState();
                yield return 0;
            }
            // End attack loop
            // Wait damages done
            while (self.Damages.Count > 0)
            {
                yield return 0;
            }
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

    public void DoAction()
    {
        if (self.IsDoingAction)
            return;

        if (self.Action == CharacterEntity.ACTION_ATTACK)
            self.StartCoroutine(DoAttackActionRoutine());
        else
        {
            self.SelectedSkill.OnUseSkill();
            self.StartCoroutine(DoSkillActionRoutine());
        }
    }

    public IEnumerator DoAttackActionRoutine()
    {
        self.IsDoingAction = true;
        var manager = self.Manager;

            // Move to target character
            yield return self.MoveTo(self.ActionTarget, self.Manager.doActionMoveSpeed);
        

        yield return new WaitForSeconds(2);
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
        var skill = self.SelectedSkill.CastedSkill;
        var manager = self.Manager;
        // Cast
            yield return self.MoveTo(self.Manager.MapCenterPosition, self.Manager.doActionMoveSpeed);

        yield return new WaitForSeconds(2);
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

    IEnumerator ApplyBuffsRoutine()
    {
        var level = self.SelectedSkill.Level;
        var skill = self.SelectedSkill.CastedSkill;
        for (var i = 0; i < skill.buffs.Length; ++i)
        {
            var buff = skill.buffs[i];
            if (buff == null)
                continue;

            var allies = self.Manager.GetAllies(self);
            var foes = self.Manager.GetFoes(self);
            if (buff.RandomToApply(level))
            {
                // Apply buffs to selected targets
                switch (buff.buffScope)
                {
                    case BuffScope.SelectedTarget:
                    case BuffScope.SelectedAndOneRandomTargets:
                    case BuffScope.SelectedAndTwoRandomTargets:
                    case BuffScope.SelectedAndThreeRandomTargets:
                        self.ActionTarget.ApplyBuff(self, level, skill, i);
                        break;
                }

                int randomAllyCount = 0;
                int randomFoeCount = 0;
                // Buff scope
                switch (buff.buffScope)
                {
                    case BuffScope.Self:
                        self.ApplyBuff(self, level, skill, i);
                        continue;
                    case BuffScope.SelectedAndOneRandomTargets:
                        if (self.ActionTarget.IsSameTeamWith(self))
                            randomAllyCount = 1;
                        else if (!self.ActionTarget.IsSameTeamWith(self))
                            randomFoeCount = 1;
                        break;
                    case BuffScope.SelectedAndTwoRandomTargets:
                        if (self.ActionTarget.IsSameTeamWith(self))
                            randomAllyCount = 2;
                        else if (!self.ActionTarget.IsSameTeamWith(self))
                            randomFoeCount = 2;
                        break;
                    case BuffScope.SelectedAndThreeRandomTargets:
                        if (self.ActionTarget.IsSameTeamWith(self))
                            randomAllyCount = 3;
                        else if (!self.ActionTarget.IsSameTeamWith(self))
                            randomFoeCount = 3;
                        break;
                    case BuffScope.OneRandomAlly:
                        randomAllyCount = 1;
                        break;
                    case BuffScope.TwoRandomAllies:
                        randomAllyCount = 2;
                        break;
                    case BuffScope.ThreeRandomAllies:
                        randomAllyCount = 3;
                        break;
                    case BuffScope.FourRandomAllies:
                        randomAllyCount = 4;
                        break;
                    case BuffScope.AllAllies:
                        randomAllyCount = allies.Count;
                        break;
                    case BuffScope.OneRandomEnemy:
                        randomFoeCount = 1;
                        break;
                    case BuffScope.TwoRandomEnemies:
                        randomFoeCount = 2;
                        break;
                    case BuffScope.ThreeRandomEnemies:
                        randomFoeCount = 3;
                        break;
                    case BuffScope.FourRandomEnemies:
                        randomFoeCount = 4;
                        break;
                    case BuffScope.AllEnemies:
                        randomFoeCount = foes.Count;
                        break;
                    case BuffScope.All:
                        randomAllyCount = allies.Count;
                        randomFoeCount = foes.Count;
                        break;
                }
                // End buff scope
                // Don't apply buffs to character that already applied
                if (randomAllyCount > 0)
                {
                    allies.Remove(self.ActionTarget);
                    while (allies.Count > 0 && randomAllyCount > 0)
                    {
                        Random.InitState(System.DateTime.Now.Millisecond);
                        var randomIndex = Random.Range(0, allies.Count - 1);
                        var applyBuffTarget = allies[randomIndex];
                        applyBuffTarget.ApplyBuff(self, level, skill, i);
                        allies.RemoveAt(randomIndex);
                        --randomAllyCount;
                    }
                }
                // Don't apply buffs to character that already applied
                if (randomFoeCount > 0)
                {
                    foes.Remove(self.ActionTarget);
                    while (foes.Count > 0 && randomFoeCount > 0)
                    {
                        Random.InitState(System.DateTime.Now.Millisecond);
                        var randomIndex = Random.Range(0, foes.Count - 1);
                        var applyBuffTarget = foes[randomIndex];
                        applyBuffTarget.ApplyBuff(self, level, skill, i);
                        foes.RemoveAt(randomIndex);
                        --randomFoeCount;
                    }
                }
            }
        }
        yield return 0;
    }
}
