using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RpguiCharacterActionSkill : RpguiCharacterAction
{
    public RpguiSkill RpguiSkill;
    public Text textRemainsTurns;
    //public Image imageRemainsTurnsGage;
    public int skillIndex;
    public CustomSkill cskill;

    private void Update()
    {
        CustomSkill();
    }


    void CustomSkill()
    {
        if (cskill == null)
            return;

        if (RpguiSkill != null)
            RpguiSkill.cskill = cskill;

        if (textRemainsTurns != null)
            textRemainsTurns.text = cskill.CanUse() == true ? "可用" : ("不可用:能量" + cskill.spendPower);

        //if (imageRemainsTurnsGage != null)
        //    imageRemainsTurnsGage.fillAmount = cskill.GetCDFloat();

    }

    protected override void OnActionSelected()
    {
        ActionManager.ActiveCharacter.SetAction(skillIndex, Const.SkillType.Custom);
    }
}
