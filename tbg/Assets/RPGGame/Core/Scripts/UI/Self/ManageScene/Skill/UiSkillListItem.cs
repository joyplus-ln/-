using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSkillListItem : MonoBehaviour
{

    private CustomSkill currentSkill;

    public Text skillName;
    // Use this for initialization
    void Start()
    {

    }

    public void SetData(CustomSkill currentSkill)
    {
        this.currentSkill = currentSkill;
        skillName.text = currentSkill.skillName;
    }

}
