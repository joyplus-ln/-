using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UseSKillItem : MonoBehaviour
{
    private UseSkillManager manager;
    private string skillId;

    /// <summary>
    /// 技能类型描述
    /// </summary>
    public Text SkillText;

    public Text SkillDesText;
    private CustomSkill skill;

    public Text PowerText;
    public void SetData(UseSkillManager manager, string skillId, CustomSkill skill)
    {
        this.manager = manager;
        this.skillId = skillId;
        this.skill = skill;
        SkillText.text = skill.skilltype == SkillType.passive ? "被动" : "主动";
        if (!skill.PowerEnough())
        {
            PowerText.text = "能量不足";
        }
        else
        {
            PowerText.text = "";
        }
    }
    public void PointDown()
    {
        Debug.Log("point down");
        SkillDesText.gameObject.SetActive(true);
        SkillDesText.text = GetDesString();
        if (skill.skilltype == SkillType.passive || !skill.CanUse()) return;
        Selected();
    }

    public void PointUp()
    {
        SkillDesText.gameObject.SetActive(false);
    }

    public void Selected()
    {
        manager.ActiveCharacter.SetAction(skillId, Const.SkillType.Custom);
        manager.SelectedTransform.SetParent(transform, false);
    }

    private string GetDesString()
    {
        StringBuilder text = new StringBuilder();
        text.Append(skill.skillName + "\n");
        text.Append(skill.des + "\n");
        text.Append(skill.spendPower + "\n");
        return text.ToString();
    }

    /// <summary>
    /// 当前开始，并被选中的状态
    /// </summary>
    public void RestartAndSelected()
    {
        manager.ActiveCharacter.SetAction(skillId, Const.SkillType.Custom);
        manager.SelectedTransform.SetParent(transform, false);
    }
}
