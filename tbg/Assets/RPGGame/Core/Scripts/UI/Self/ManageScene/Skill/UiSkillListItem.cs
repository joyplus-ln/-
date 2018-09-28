using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSkillListItem : MonoBehaviour
{

    private CustomSkill currentSkill;
    // Use this for initialization
    void Start()
    {

    }

    public void SetData(CustomSkill currentSkill)
    {
        this.currentSkill = currentSkill;
    }

}
