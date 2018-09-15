using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.UI;

public abstract class BaseUISkill<TSkill> : UIDataItem<TSkill>
    where TSkill : BaseSkill
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
        SetupInfo(data);
        SetupCustomSkillInfo();
    }

    private void SetupInfo(TSkill data)
    {
        if (data == null) return;
        if (textTitle != null)
            textTitle.text = data == null ? "" : data.title;

        if (textDescription != null)
            textDescription.text = data == null ? "" : data.description;

        if (skillName != null)
            skillName.text = data == null ? null : data.title;
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
        return data == null || string.IsNullOrEmpty(data.Id);
    }

    public void ShowDataOnMessageDialog()
    {
        GameInstance.Singleton.ShowMessageDialog(data.title, data.description);
    }
}
