using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhuZaiZheCustomSkill : CustomSkill
{




}

public class HaoHanHongXi : CustomSkill
{
    public HaoHanHongXi()
    {
        skillName = "浩瀚虹吸";
        des = "对随机4名敌人造成210%攻击，恢复自己600%攻击等量生命";
    }

    public override IEnumerator ApplyBuffLogic()
    {
        //CustomBuff buff001 = SkillUtils.MakeCustomBuff("Buff001");
        //buff001.SetGiver(selfOnly);
        //GetSelf().ApplyCustomBuff(buff001);
        yield return null;
    }

    public override IEnumerator DoSkillLogic()
    {
        selfOnly.Manager.SpawnCombatCustomText(selfOnly, skillName);
        yield return MoveToTarget();
        List<BaseCharacterEntity> targets = GetRandomEnemy(4);
        var attackDamage0 = new SkillAttackDamage(200, 2.1f, 0);
        for (int i = 0; i < targets.Count; i++)
        {
            yield return PatkImmediate(attackDamage0, targets[i] as CharacterEntity);
        }
        selfOnly.Custombody.AddFlood((int)selfOnly.GetTotalAttributes().pAtk * 6);
        yield return MoveToSelfPos();
        //yield return ApplyBuffLogic();
        yield return null;
    }
}

public class WangLingYiZhi : CustomSkill
{
    public WangLingYiZhi()
    {
        skilltype = SkillType.passive;
        skillName = "亡灵意志";
        des = "生命增加40%，命中增加20%";
    }

    public override IEnumerator ApplyBuffLogic()
    {
        CustomBuff buff001 = SkillUtils.MakeCustomBuff("WangLingYiZhiBuff");
        buff001.SetGiver(selfOnly);
        GetSelf().ApplyCustomBuff(buff001);
        Debug.Log("添加buff 亡灵意志  触发");
        yield return null;
    }

    public override void BattleStart()
    {
        Debug.Log("技能 亡灵意志  触发");
        selfOnly.StartCoroutine(ApplyBuffLogic());
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return null;
    }
}
