using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterBuff : UIBase
{
    public Text textRemainsTurns;
    public Image imageRemainsTurnsGage;
    public Image imageIcon;

    public CustomBuff custombuff;//player头上显示buff用
    public Text customBuffText;

    private void Update()
    {
        ShowCustomBuff();
    }


    void ShowCustomBuff()
    {
        if (custombuff == null) return;
        customBuffText.text = custombuff.buffText;
    }
}
