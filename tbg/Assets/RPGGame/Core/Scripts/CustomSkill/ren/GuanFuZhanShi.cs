using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuanFuZhanShi
{


    public class GuanFuZhanShi001 : CustomSkill
    {
        public GuanFuZhanShi001()
        {
            skillName = "GuanFuZhanShi001";
            des = "GuanFuZhanShi001";
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
            var attackDamage0 = new SkillAttackDamage(200, 0.5f, 0);
            selfOnly.Attack(selfOnly.ActionTarget, null, attackDamage0.GetPAtkDamageRate(), attackDamage0.GetMAtkDamageRate(), attackDamage0.hitCount, (int)attackDamage0.GetFixDamage());
            yield return MoveToSelfPos();
            yield return MoveToTarget();
            var attackDamage1 = new SkillAttackDamage(200, 0.5f, 0);
            selfOnly.Attack(selfOnly.ActionTarget, null, attackDamage1.GetPAtkDamageRate(), attackDamage1.GetMAtkDamageRate(), attackDamage1.hitCount, (int)attackDamage1.GetFixDamage());
            yield return MoveToSelfPos();
            yield return MoveToTarget();
            var attackDamage2 = new SkillAttackDamage(200, 0.5f, 0);
            selfOnly.Attack(selfOnly.ActionTarget, null, attackDamage2.GetPAtkDamageRate(), attackDamage2.GetMAtkDamageRate(), attackDamage2.hitCount, (int)attackDamage2.GetFixDamage());
            yield return MoveToSelfPos();
            //yield return ApplyBuffLogic();
            yield return null;
        }
    }
}
