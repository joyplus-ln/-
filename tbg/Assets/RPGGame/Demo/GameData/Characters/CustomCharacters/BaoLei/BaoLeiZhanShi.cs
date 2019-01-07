using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaoLeiZhanShi : MonoBehaviour
{

}

public class YanShiJia : CustomSkill
{
    public YanShiJia()
    {
        skilltype = SkillType.passive;
        skillName = "";
        des = "";
        spendPower = 0;
        SelfAttributes.exp_blockChance = 0.5f;
    }

    public override void Beigedang()
    {
        SelfAttributes.pDef += 20;
    }
}


public class YanShiBao : CustomSkill
{
    public YanShiBao()
    {
        des = "";
        skillName = "";
        spendPower = 11;
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveSkillOB(() =>
        {
            CustomBuff buff = SkillUtils.MakeCustomBuff("YanShiBaoBuff");
            selfOnly.ActionTarget.ApplyCustomBuff(buff);
            AttackTarget(new SkillAttackDamage(20, 0.5f, 0));
        });
    }
}

public class YanShiBaoBuff : CustomBuff
{
    private CharacterEntity buffGiver = null;
    public YanShiBaoBuff()
    {
        buffText = "";
        SelfAttributes.exp_pDefRate = -0.2f;
        SelfAttributes.exp_mDefRate = -0.2f;
        guid = "YanShiBaoBuff";
        turns = 3;
    }

    public override void ReceiveDamage()
    {
        if (buffGiver == null && selfOnly.Custombody.lastAttacker != null)
        {
            buffGiver = selfOnly.Custombody.lastAttacker;
        }
        if (buffGiver == selfOnly.Custombody.lastAttacker)
        {
            SelfAttributes.exp_pDefRate += -0.02f;
            SelfAttributes.exp_mDefRate += -0.02f;
        }
    }
}
