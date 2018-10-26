using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnRenShaShou
{


}

public class AnRen : CustomSkill
{
    public AnRen()
    {
        skillName = "暗刃";
        des = "沉默2回合并造成300%伤害";
        spendPower = 15;
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveSkillOB(() =>
        {
            SkillAttackDamage damage = new SkillAttackDamage(0, 1.5f, 1.5f);
            AttackTarget(damage);
        });

    }

    public override void ApplyBuffLogicM()
    {
        CustomBuff buff = SkillUtils.MakeCustomBuff("AnRenBuff");
        selfOnly.ActionTarget.ApplyCustomBuff(buff);
    }
}

public class AnRenBuff : CustomBuff
{
    public AnRenBuff()
    {
        turns = 2;
        des = "沉默2回合";
        buffText = "禁";
        CMState = 2;

    }


}
