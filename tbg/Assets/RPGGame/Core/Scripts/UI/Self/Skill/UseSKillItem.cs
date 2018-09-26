using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSKillItem : MonoBehaviour
{
    private UseSkillManager manager;
    public void SetData(UseSkillManager manager)
    {
        this.manager = manager;
    }
    public void PointDown()
    {
        Debug.Log("point down");
        manager.SelectedTransform.SetParent(transform, false);
    }
}
