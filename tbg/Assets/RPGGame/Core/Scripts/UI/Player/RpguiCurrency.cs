using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.UI;

public class RpguiCurrency : RpguiDataItem<IPlayerCurrency>
{
    public Image imageIcon;
    public Text textAmount;
    public override void UpdateData()
    {
        SetupInfo(data);
    }

    public override void Clear()
    {
        SetupInfo(null);
    }

    private void SetupInfo(IPlayerCurrency data)
    {
        if (data == null)
            data = new IPlayerCurrency();

        var currencyData = data.CurrencyData;

        if (imageIcon != null)
            imageIcon.sprite = currencyData == null ? null : currencyData.icon;

        if (textAmount != null)
            textAmount.text = data.amount.ToString("N0");
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.guid);
    }
}
