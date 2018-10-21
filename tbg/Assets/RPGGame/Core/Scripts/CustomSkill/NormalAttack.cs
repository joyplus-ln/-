using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : CustomSkill
{

    public NormalAttack()
    {
        skillName = "普通攻击";
        des = "普通攻击，对地方造成100%伤害,并回复8点能量";
        spendPower = 0;
    }

    public override IEnumerator DoSkillLogic()
    {
        selfOnly.Manager.SpawnCombatCustomText(selfOnly, skillName);
        yield return MoveToTarget();
        var attackDamage = new SkillAttackDamage(0, 1, 0);
        selfOnly.Attack(selfOnly.ActionTarget, attackDamage.GetPAtkDamageRate(), attackDamage.GetMAtkDamageRate(), attackDamage.hitCount, (int)attackDamage.GetFixDamage());
        //消耗0能量，增加8能量就可了，不需要修改那个值，在这里灵活一点,可以根据情况决定恢复程度
        GamePlayManager.Singleton.uiUseSkillManager.sikllPower.AddPower(8);
        yield return MoveToSelfPos();

    }


}
