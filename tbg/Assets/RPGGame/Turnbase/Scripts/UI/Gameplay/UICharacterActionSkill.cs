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
    public CharacterSkill skill;
    public CustomSkill cskill;

    private void Update()
    {
        NormalSkill();
        CustomSkill();
    }

    void NormalSkill()
    {
        if (skill == null)
            return;

        var rate = 1 - skill.GetCoolDownDurationRate();

        if (uiSkill != null)
            uiSkill.data = skill.Skill as Skill;

        if (textRemainsTurns != null)
            textRemainsTurns.text = skill.GetCoolDownDuration() <= 0 ? "" : skill.GetCoolDownDuration().ToString("N0");

        if (imageRemainsTurnsGage != null)
            imageRemainsTurnsGage.fillAmount = rate;
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
        if (cskill != null)
        {
            ActionManager.ActiveCharacter.SetAction(skillIndex,Const.SkillType.Custom);
        }
        else
        {
            ActionManager.ActiveCharacter.SetAction(skillIndex, Const.SkillType.Normal);
        }
        

    }
}
