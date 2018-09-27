using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSKillItem : MonoBehaviour
{
    private UseSkillManager manager;
    private int skillIndex;
    public void SetData(UseSkillManager manager, int skillIndex)
    {
        this.manager = manager;
        this.skillIndex = skillIndex;
    }
    public void PointDown()
    {
        Debug.Log("point down");
        manager.SelectedTransform.SetParent(transform, false);
        Selected();
    }

    public void Selected()
    {
        manager.ActiveCharacter.SetAction(skillIndex, Const.SkillType.Custom);
    }
}
