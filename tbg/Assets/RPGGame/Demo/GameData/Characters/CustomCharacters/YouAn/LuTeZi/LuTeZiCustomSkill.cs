using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuTeZiCustomSkill : CustomSkill
{

}

public class DianNengJiDang : CustomSkill
{

    public DianNengJiDang()
    {
        skillName = "电能激荡";
        des = "对随机4名敌人造成180%伤害";
        spendPower = 10;
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveToTarget();
        List<BaseCharacterEntity> randomFour = GetRandomEnemy(4);
        SkillAttackDamage damage = new SkillAttackDamage(0, 1.8f, 0);
        for (int i = 0; i < randomFour.Count; i++)
        {
            yield return PatkImmediate(damage, randomFour[i] as CharacterEntity);

        }
        yield return MoveToSelfPos();
    }
}

public class PoZhan : CustomSkill
{

    public PoZhan()
    {
        skilltype = SkillType.passive;
        skillName = "破绽";
        des = "攻击增加25%,生命增加15%";
        spendPower = 0;
        SelfAttributes.exp_pAtkRate = 0.25f;
        SelfAttributes.exp_hpRate = 0.15f;
    }


}
