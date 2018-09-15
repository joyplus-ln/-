using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStamina : UIDataItem<PlayerStamina>
{
    public Image imageIcon;
    public Text textAmount;
    public Text recoveryingTime;
    public override void UpdateData()
    {
        SetupInfo(data);
    }

    public override void Clear()
    {
        SetupInfo(null);
    }

    private void SetupInfo(PlayerStamina data)
    {
        if (data == null)
            data = new PlayerStamina();

        var staminaData = data.StaminaData;

        if (imageIcon != null)
            imageIcon.sprite = staminaData == null ? null : staminaData.icon;

        if (textAmount != null)
            textAmount.text = data.Amount.ToString("N0");

        if (recoveryingTime != null)
            recoveryingTime.text = "";
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.DataId);
    }
}
