using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YanDaoShiZhe
{

}

public class YaoDaoLianZhan : CustomSkill
{
    public YaoDaoLianZhan()
    {
        skillName = "";
        spendPower = 12;
        des = "";
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveToTarget();
        bool allDie = false;
        for (int i = 0; i < 6; i++)
        {
            yield return MoveSkillOB(() =>
            {
                SkillAttackDamage damage = new SkillAttackDamage(0, 0.5f, 0);
                AttackInfo info = AttackTarget(damage);
                if (info.die)
                {
                    List<BaseCharacterEntity> list = GetRandomEnemy(1);
                    if (list.Count > 0)
                    {
                        selfOnly.ActionTarget = list[0] as CharacterEntity;
                    }
                    else
                    {
                        allDie = true;
                    }

                }
            });
            if (allDie) break;
        }
        yield return MoveToSelfPos();
    }
}
