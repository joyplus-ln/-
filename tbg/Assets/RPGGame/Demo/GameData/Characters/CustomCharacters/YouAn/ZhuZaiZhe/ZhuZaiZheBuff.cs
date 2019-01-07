using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhuZaiZheBuff : MonoBehaviour
{


}

public class WangLingYiZhiBuff : CustomBuff
{
    public override void Init()
    {
        des = "增加40%气血，增加20%命中";
        buffText = "强";
        turns = -99;
    }
    public override void BattleStart()
    {
        SelfAttributes.exp_hpRate = 0.4f;
        selfOnly.Custombody.CustomText("气血增加40%");
        SelfAttributes.acc = 0.2f;
        selfOnly.Custombody.CustomText("命中增加20%");
        selfOnly.Hp = selfOnly.MaxHp;
    }


}
