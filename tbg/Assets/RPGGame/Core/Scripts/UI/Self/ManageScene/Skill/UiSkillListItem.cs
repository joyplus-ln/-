using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UiSkillListItem : MonoBehaviour
{

    private CustomSkill currentSkill;

    public Text skillName;

    public Action<string> clickCallBack;
    // Use this for initialization
    void Start()
    {

    }

    public void SetData(CustomSkill currentSkill, Action<string> clickCallBack)
    {
        this.currentSkill = currentSkill;
        skillName.text = currentSkill.skillName;
        this.clickCallBack = clickCallBack;
    }

    public void Click()
    {
        StringBuilder build = new StringBuilder();

        build.AppendLine("技能：" + currentSkill.skillName);
        build.AppendLine("类型：" + currentSkill.skilltype.ToString());
        build.AppendLine("能量消耗:" + currentSkill.spendPower.ToString());
        build.AppendLine("效果:" + currentSkill.des);
        if (clickCallBack != null)
        {
            clickCallBack.Invoke(build.ToString());
        }
    }
}
