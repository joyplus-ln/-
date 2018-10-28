using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeYing
{

}

public class DingXiangZhuiJi : CustomSkill
{

    public DingXiangZhuiJi()
    {
        des = "对敌人定向追击，并造成120%伤害，40%几率追击成功，如果追击成功，额外使用一次技能";
        skillName = "定向追击";
        spendPower = 5;
    }

    public override IEnumerator DoSkillLogic()
    {
        if (selfOnly.ActionTarget.Hp > 0)
        {
            yield return MoveToTarget();
            AttackTarget(new SkillAttackDamage(0, 1.2f, 0));
            yield return MoveToSelfPos();
            if (Random.Range(0, 11) < 5)
            {
                yield return DoSkillLogic();
            }
        }
        else
        {
            List<BaseCharacterEntity> enemy = GetRandomEnemy(0);
            if (enemy.Count > 0)
            {
                selfOnly.ActionTarget = enemy[0] as CharacterEntity;
                yield return DoSkillLogic();
            }

        }
    }
}
