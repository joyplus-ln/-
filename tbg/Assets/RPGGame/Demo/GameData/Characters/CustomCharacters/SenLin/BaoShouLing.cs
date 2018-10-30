using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaoShouLing
{

}

public class ShiJianZhuanYi : CustomSkill
{
    public ShiJianZhuanYi()
    {
        des = "";
        skillName = "";
        spendPower = 10;
        usageScope = SkillUsageScope.Ally;
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveSkillOB(() =>
        {
            selfOnly.ActionTarget.currentTimeCount = 99999;
        });
    }
}
