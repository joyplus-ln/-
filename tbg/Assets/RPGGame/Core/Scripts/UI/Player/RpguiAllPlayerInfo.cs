using System.Collections;
using System.Collections.Generic;
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
            RpguiPlayer.SetData(Player.CurrentPlayer);
        if (RpguiSoftCurrency != null)
            RpguiSoftCurrency.SetData(PlayerCurrency.SoftCurrency);
        if (RpguiHardCurrency != null)
            RpguiHardCurrency.SetData(PlayerCurrency.HardCurrency);
        if (RpguiStageStamina != null)
            RpguiStageStamina.SetData(PlayerStamina.StageStamina);
    }
}
