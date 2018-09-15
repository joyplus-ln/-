using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterBuff : UIBase
{
    public Text textRemainsTurns;
    public Image imageRemainsTurnsGage;
    public Image imageIcon;
    public BaseCharacterBuff buff;

    public CustomBuff custombuff;//player头上显示buff用
    public Text customBuffText;

    private void Update()
    {
        ShowNormalBuff();
        ShowCustomBuff();
    }

    void ShowNormalBuff()
    {
        if (buff == null)
            return;

        var rate = buff.GetRemainsDurationRate();

        if (imageIcon != null)
            imageIcon.sprite = buff.Buff.icon;

        if (textRemainsTurns != null)
            textRemainsTurns.text = buff.GetRemainsDuration() <= 0 ? "" : buff.GetRemainsDuration().ToString("N2");

        if (imageRemainsTurnsGage != null)
            imageRemainsTurnsGage.fillAmount = rate;
    }

    void ShowCustomBuff()
    {
        if (custombuff == null) return;
        customBuffText.text = custombuff.buffText;
    }
}
