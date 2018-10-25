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
        //todo
    }
    public override IEnumerator DoSkillLogic()
    {
        yield return MoveOut();
        List<BaseCharacterEntity> list = GetSelfFriends();
        for (int i = 0; i < list.Count; i++)
        {
            (list[i] as CharacterEntity).Custombody.AddFlood((int)(selfOnly.Custombody.GetMaxHP() * 0.8f));
        }
        yield return ApplyBuffLogic();
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
        selfOnly.Custombody.AddFlood((int)(selfOnly.Custombody.GetMaxHP() * 0.8f));
    }
}
