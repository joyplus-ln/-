using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx.Triggers;
using UnityEngine;

public class NormalEnemy
{


}

#region add

public class ShengMingHuiFu : CustomSkill
{
    public ShengMingHuiFu()
    {
        des = "";
        skillName = "";
        spendPower = 0;
        enemyCd = 3;
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveSkillOB(() =>
        {
            List<BaseCharacterEntity> enemys = GetSelfFriends();
            CharacterEntity target = enemys.OrderByDescending(x => x.MaxHp).ToList()[0] as CharacterEntity;
            target.Custombody.AddFlood((int)(target.MaxHp * 0.1f));
        });
    }
}

public class ShengFuZaiJi : CustomSkill
{
    public ShengFuZaiJi()
    {
        des = "";
        skillName = "";
        spendPower = 0;
        enemyCd = 0;
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveSkillOB(() =>
        {
            List<BaseCharacterEntity> enemys = GetRandomEnemy(3);
            for (int i = 0; i < enemys.Count; i++)
            {
                (enemys[i] as CharacterEntity).Custombody.AddFlood((int)(selfOnly.GetTotalAttributes().mAtk));
            }
        });
    }
}


public class XianJiShengMing : CustomSkill
{
    public XianJiShengMing()
    {
        des = "";
        skillName = "";
        spendPower = 0;
        enemyCd = 5;
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveSkillOB(() =>
         {
             selfOnly.Custombody.AddFlood(-(int)(selfOnly.Hp - 1));
             List<BaseCharacterEntity> enemys = GetSelfFriends();
             CharacterEntity target = enemys.OrderByDescending(x => x.MaxHp).ToList()[0] as CharacterEntity;
             target.Custombody.AddFlood((int)(target.MaxHp * 0.5f));
         });
    }
}

#endregion
