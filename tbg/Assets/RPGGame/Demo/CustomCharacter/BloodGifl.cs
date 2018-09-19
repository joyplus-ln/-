using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodGirl
{

    public class BGS001 : CustomSkill
    {
        public BGS001()
        {
            skillName = "嗜血反击";
            des = "消耗当前10%的气血来反击敌人";
        }

        public override IEnumerator DoSkillLogic()
        {
            yield return null;

        }
    }

    public class BGB001 : CustomBuff
    {

    }
}
