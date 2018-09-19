using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill001 : CustomSkill
{
    public Skill001()
    {
        skillName = "排山倒海";
        des = "攻击敌人,同时增加自己气血上限";
    }

    public override IEnumerator ApplyBuffLogic()
    {
        CustomBuff buff001 = SkillUtils.MakeCustomBuff("001");
        buff001.SetGiver(selfOnly);
        GetSelf().ApplyCustomBuff(buff001);
        Debug.Log("执行了 添加自定义buff 001");
        yield return null;
    }

    public override IEnumerator DoSkillLogic()
    {
        var attackDamage = new SkillAttackDamage();
        selfOnly.Attack(selfOnly.ActionTarget, null, attackDamage.GetPAtkDamageRate(), attackDamage.GetMAtkDamageRate(), attackDamage.hitCount, (int)attackDamage.GetFixDamage());
        yield return ApplyBuffLogic();
        yield return null;
    }
}
