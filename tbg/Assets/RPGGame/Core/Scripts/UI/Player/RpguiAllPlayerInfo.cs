using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;

public class RpguiAllPlayerInfo : RPGUIBase
{
    public RpguiPlayer RpguiPlayer;
    public RpguiCurrency RpguiSoftCurrency;
    public RpguiCurrency RpguiHardCurrency;
    public RpguiStamina RpguiStageStamina;
    
	void Update ()
    {
        if (RpguiPlayer != null)
            RpguiPlayer.SetData(IPlayer.CurrentPlayer);
        if (RpguiSoftCurrency != null)
            RpguiSoftCurrency.SetData(IPlayerCurrency.SoftCurrency);
        if (RpguiHardCurrency != null)
            RpguiHardCurrency.SetData(IPlayerCurrency.HardCurrency);
        if (RpguiStageStamina != null)
            RpguiStageStamina.SetData(PlayerStamina.StageStamina);
    }
}
