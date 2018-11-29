using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.UI;

public abstract class BaseRpguiSkill<CSkill> : RpguiDataItem<CSkill> where CSkill : class
{
    public Text textTitle;
    public Text textDescription;
    public Text skillName;

    private CustomSkill _cskill;

    public CustomSkill cskill
    {
        get { return _cskill; }
        set
        {
            _cskill = value;
            ShowOnceCustomSkill = true;
        }
    }

    public override void Clear()
    {
        SetupInfo(null);
    }

    public override void UpdateData()
    {
        SetupInfo(data as CustomSkill);
        SetupCustomSkillInfo();
    }

    private void SetupInfo(CustomSkill data)
    {
        if (data == null) return;
        if (textTitle != null)
            textTitle.text = data == null ? "" : data.id.ToString();

        if (textDescription != null)
            textDescription.text = data == null ? "" : data.des;

        if (skillName != null)
            skillName.text = data == null ? null : data.skillName;
    }

    private void SetupCustomSkillInfo()
    {
        if (cskill == null) return;

        if (textTitle != null)
            textTitle.text = cskill.skillName;

        if (textDescription != null)
            textDescription.text = cskill.des;

        if (skillName != null)
            skillName.text = cskill.skillName;
    }

    public override bool IsEmpty()
    {
        return data == null;
    }

    public void ShowDataOnMessageDialog()
    {
        GameInstance.Singleton.ShowMessageDialog((data as CustomSkill).skillName, (data as CustomSkill).des);
    }
}
