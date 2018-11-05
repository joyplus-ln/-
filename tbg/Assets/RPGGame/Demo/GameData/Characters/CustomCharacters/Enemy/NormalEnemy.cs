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


#region damage

public class SuiJiDaJi : CustomSkill
{

    public SuiJiDaJi()
    {
        des = "";
        skillName = "";
        spendPower = 0;
        enemyCd = 2;
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveSkillOB(() =>
        {
            int random = Random.Range(1, 6);
            List<BaseCharacterEntity> randomHint = GetRandomEnemy(random);
            for (int i = 0; i < randomHint.Count; i++)
            {
                AttackTarget(new SkillAttackDamage(10, 0.5f, 0), randomHint[i] as CharacterEntity);
            }
        });

    }
}

public class ZhiMingYiJi : CustomSkill
{

    public ZhiMingYiJi()
    {
        des = "";
        skillName = "";
        spendPower = 0;
        enemyCd = 2;
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveSkillOB(() =>
        {
            List<BaseCharacterEntity> randomHint = GetRandomEnemy(1);
            for (int i = 0; i < randomHint.Count; i++)
            {
                AttackTarget(new SkillAttackDamage(10, 1.2f, 0), randomHint[i] as CharacterEntity);
            }
        });

    }
}

#endregion