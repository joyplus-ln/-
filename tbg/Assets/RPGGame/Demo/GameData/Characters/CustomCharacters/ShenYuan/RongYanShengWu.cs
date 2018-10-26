using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RongYanShengWu
{


}

public class RongYanZhiNu : CustomSkill
{
    public RongYanZhiNu()
    {
        skillName = "";
        des = "";
        spendPower = 16;
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveToTarget();
        yield return MoveSkillOB(() =>
        {

            SkillAttackDamage damage = new SkillAttackDamage(0, 3, 0);
            AttackInfo info = AttackTarget(damage);
            if (info.die)
            {
                int overHP = (int)(info.totalDamage - info.lastHP);
                List<BaseCharacterEntity> allEntities = GetRandomEnemy(9);
                for (int i = 0; i < allEntities.Count; i++)
                {
                    SkillAttackDamage damageOver = new SkillAttackDamage(overHP, 0, 0);
                    AttackTarget(damageOver, allEntities[i] as CharacterEntity);
                }
            }
        });
        yield return MoveToSelfPos();
    }
}