using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UseSKillItem : MonoBehaviour
{
    private UseSkillManager manager;
    private int skillIndex;

    /// <summary>
    /// 技能类型描述
    /// </summary>
    public Text SkillText;

    public Text SkillDesText;
    private CustomSkill skill;
    public void SetData(UseSkillManager manager, int skillIndex, CustomSkill skill)
    {
        this.manager = manager;
        this.skillIndex = skillIndex;
        this.skill = skill;
        SkillText.text = skill.skilltype == SkillType.passive ? "被动" : "主动";
    }
    public void PointDown()
    {
        Debug.Log("point down");
        SkillDesText.gameObject.SetActive(true);
        SkillDesText.text = GetDesString();
        if (skill.skilltype == SkillType.passive) return;
        Selected();
    }

    public void PointUp()
    {
        SkillDesText.gameObject.SetActive(false);
    }

    public void Selected()
    {
        manager.ActiveCharacter.SetAction(skillIndex, Const.SkillType.Custom);
        manager.SelectedTransform.SetParent(transform, false);
    }

    private string GetDesString()
    {
        StringBuilder text = new StringBuilder();
        text.Append(skill.skillName + "\n");
        text.Append(skill.des + "\n");
        text.Append(skill.spengPower + "\n");
        return text.ToString();
    }
}
