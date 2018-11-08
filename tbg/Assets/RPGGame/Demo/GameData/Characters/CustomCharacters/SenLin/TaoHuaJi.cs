using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaoHuaJi
{


}

public class TaoHuaZhuoZhuo : CustomSkill
{
    public TaoHuaZhuoZhuo()
    {
        skillName = "";
        des = "";
        spendPower = 16;
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveSkillOB(() =>
        {
            selfOnly.ActionTarget.Custombody.AddFlood((int)(selfOnly.MaxHp * 0.2f));
        });
    }
}
