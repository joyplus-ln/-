using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterActionSkill : UICharacterAction
{
    public UISkill uiSkill;
    public Text textRemainsTurns;
    public Image imageRemainsTurnsGage;
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

        if (uiSkill != null)
            uiSkill.cskill = cskill;

        if (textRemainsTurns != null)
            textRemainsTurns.text = "0CD";

        if (imageRemainsTurnsGage != null)
            imageRemainsTurnsGage.fillAmount = 1;

    }

    protected override void OnActionSelected()
    {
        ActionManager.ActiveCharacter.SetAction(skillIndex, Const.SkillType.Custom);
    }
}
