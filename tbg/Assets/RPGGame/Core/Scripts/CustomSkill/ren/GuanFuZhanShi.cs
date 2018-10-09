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
            yield return Patk(attackDamage0);
            var attackDamage1 = new SkillAttackDamage(200, 0.5f, 0);
            yield return Patk(attackDamage1);
            var attackDamage2 = new SkillAttackDamage(200, 0.5f, 0);
            yield return Patk(attackDamage2);
            yield return MoveToSelfPos();
            //yield return ApplyBuffLogic();
            yield return null;
        }
    }
}
