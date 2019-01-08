
using SQLite3TableDataTmp;
using UnityEngine;

public class FormulaUtils
{


    public static float FightFormula(int pAtk, int mAtk, int acc, float critChance, float critDamageRate, CalculationAttributes ReceiverAttributes, int hitCount = 1, int fixDamage = 0)
    {
        if (hitCount <= 0)
            hitCount = 1;
        var gameDb = GameInstance.GameDatabase;
        var pDmg = pAtk - ReceiverAttributes.pDef;
        var mDmg = mAtk - ReceiverAttributes.mDef;
        if (pDmg < 0)
            pDmg = 0;
        if (mDmg < 0)
            mDmg = 0;
        var totalDmg = pDmg + mDmg;
        var isCritical = false;
        var isBlock = false;
        totalDmg += Mathf.CeilToInt(totalDmg * Random.Range(gameDb.minAtkVaryRate, gameDb.maxAtkVaryRate)) + fixDamage;
        return totalDmg;
    }

    public static CalculationAttributes GetTowerExtraAttributes(bool IsBos)
    {

        CalculationAttributes extraAttributes = new CalculationAttributes();
        int playerlevel = IPlayer.CurrentPlayer.Level;
        int towerLevel = IPlayer.CurrentPlayer.TowerCurrentLevel;
        int totalWeight = (playerlevel * 10 + towerLevel * 15 + 5) * 50;
        extraAttributes.SetExtraAtt(totalWeight);

        return extraAttributes;
    }
}
