using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaoFaHuoShan
{


}

public class ChiXuPenFa : CustomSkill
{
    public ChiXuPenFa()
    {
        des = "";
        skillName = "";
        spendPower = 0;
        skilltype = SkillType.passive;
    }

    public override void BeforeFight()
    {
        GamePlayManager.Singleton.uiUseSkillManager.sikllPower.AddPower(5);
    }
}

public class PoXueWeiGong : CustomSkill
{
    public PoXueWeiGong()
    {
        spendPower = 0;
        des = "";
        skillName = "";
        usageScope = SkillUsageScope.Self;
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveSkillOB(() =>
        {
            selfOnly.Custombody.DeductBlood((int)(selfOnly.Hp * 0.3), DmgType.Normal);
            GamePlayManager.Singleton.uiUseSkillManager.sikllPower.AddPower(15);
        });
    }
}

