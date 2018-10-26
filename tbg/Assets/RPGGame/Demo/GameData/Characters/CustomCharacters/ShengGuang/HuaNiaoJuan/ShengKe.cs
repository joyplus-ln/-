using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShengKe
{
    //huaniaojuan

}

public class ShengMingXuYuan : CustomSkill
{
    public ShengMingXuYuan()
    {
        spendPower = 20;
        des = "给全体增加自己生命8%的气血";
        skillName = "生命之光";
        usageScope = SkillUsageScope.Ally;

        //todo
    }
    public override IEnumerator DoSkillLogic()
    {
        yield return MoveOut();
        List<BaseCharacterEntity> list = GetSelfFriends();
        for (int i = 0; i < list.Count; i++)
        {
            (list[i] as CharacterEntity).Custombody.AddFlood((int)(selfOnly.Custombody.GetMaxHP() * 0.08f));
        }
        yield return ApplyBuffLogic();
        yield return MoveToSelfPos();
    }//

    public override IEnumerator ApplyBuffLogic()
    {

        List<BaseCharacterEntity> list = GetSelfFriends();
        for (int i = 0; i < list.Count; i++)
        {
            CustomBuff buff = SkillUtils.MakeCustomBuff("ShengMingXuYuanBuff");
            (list[i] as CharacterEntity).ApplyCustomBuff(buff);
        }
        yield return null;
    }
}

public class ShengMingXuYuanBuff : CustomBuff
{
    public ShengMingXuYuanBuff()
    {
        turns = 2;
        //todo
    }

    public override void BeforeFight()
    {
        selfOnly.Custombody.AddFlood((int)(selfOnly.Custombody.GetMaxHP() * 0.08f));
    }
}
