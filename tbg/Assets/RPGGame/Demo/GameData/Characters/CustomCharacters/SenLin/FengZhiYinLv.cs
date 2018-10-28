using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FengZhiYinLv
{


}

public class FengZhiYunSha : CustomSkill
{
    public FengZhiYunSha()
    {
        spendPower = 12;
        des = "对所有敌人造成90%伤害,如果敌人死亡,追加一次技能";
        skillName = "风之陨杀";
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveToTarget();
        List<BaseCharacterEntity> enemys = GetRandomEnemy(99);
        if (enemys.Count > 0)
        {
            bool die = false;
            for (int i = 0; i < enemys.Count; i++)
            {
                AttackInfo info = AttackTarget(new SkillAttackDamage(0, 0.9f, 0), enemys[i] as CharacterEntity);
                if (info.die) die = true;
            }
            if (die)
                yield return DoSkillLogic();
        }
        yield return MoveToSelfPos();
    }
}