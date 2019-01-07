using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuShiWaLong
{

}

public class JuShiHuDun : CustomSkill
{

    public JuShiHuDun()
    {
        spendPower = 8;
        des = "";
        skillName = "";
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveSkillOB(() =>
        {
            List<BaseCharacterEntity> selfList = GetSelfFriends();
            for (int i = 0; i < selfList.Count; i++)
            {
                CustomBuff buff = SkillUtils.MakeCustomBuff("JuShiHuDunBuff");
                (selfList[i] as CharacterEntity).ApplyCustomBuff(buff);
            }
        });
    }
}

public class JuShiHuDunBuff : CustomBuff
{
    public JuShiHuDunBuff()
    {
        turns = 3;
        buffText = "防";
    }

    public override void Init()
    {
        SelfAttributes.exp_pDefRate = 0.2f;
        SelfAttributes.exp_mDefRate = 0.2f;
    }
}
