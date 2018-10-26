using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MuSong
{

}

public class YuHouChunSun : CustomSkill
{
    private int hintCount = 1;
    private int MaxHintCount = 5;
    public YuHouChunSun()
    {
        spendPower = 0;
        skillName = "";
        des = String.Format("{0}", hintCount);
    }

    public override void Afterfight()
    {
        int id = Random.Range(0, 11);
        if (id <= 5)
        {
            hintCount++;
            if (hintCount > MaxHintCount)
                hintCount = MaxHintCount;
            des = String.Format("{0}", hintCount);
        }
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveToTarget();
        AttackInfo info = null;
        for (int i = 0; i < hintCount; i++)
        {

            yield return MoveSkillOB(() =>
            {
                SkillAttackDamage damage = new SkillAttackDamage(0, 1, 0);
                info = AttackTarget(damage);

            });
            if (info.die)
                break;
        }
        yield return MoveToSelfPos();
    }
}
