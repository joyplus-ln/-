using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuangZhanShi
{

}

public class ShiXue : CustomSkill
{
    public ShiXue()
    {
        des = "";
        skillName = "";
        skilltype = SkillType.passive;
        spendPower = 0;
    }

    public override void BeforeFight()
    {
        SelfAttributes._mAtkRate = (1 - (selfOnly.Hp / selfOnly.MaxHp));
    }
}
