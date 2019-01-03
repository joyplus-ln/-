using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SQLite3TableDataTmp
{
    public partial class ICharacter
    {
        public int Calculate(int maxValue, int minValue, float growth, int currentLevel, int maxLevel)
        {
            if (currentLevel <= 0)
                currentLevel = 1;
            if (maxLevel <= 0)
                maxLevel = 1;
            if (currentLevel == 1)
                return minValue;
            if (currentLevel == maxLevel)
                return maxValue;
            return minValue + Mathf.RoundToInt((maxValue - minValue) * Mathf.Pow((float)(currentLevel - 1) / (float)(maxLevel - 1), growth));
        }
        public CalculationAttributes CreateCalculationAttributes(int currentLevel, int maxLevel)
        {
            CalculationAttributes result = new CalculationAttributes();
            result.hp = Calculate(maxhp, minhp, hpgrowth, currentLevel, maxLevel);// hp.Calculate(currentLevel, maxLevel);
            result.pAtk = Calculate(maxpAtk, minpAtk, pAtkgrouth, currentLevel, maxLevel);//pAtk.Calculate(currentLevel, maxLevel);
            result.pDef = Calculate(maxpDef, minpDef, pDefgrouth, currentLevel, maxLevel);//pDef.Calculate(currentLevel, maxLevel);
            result.mAtk = Calculate(maxmAtk, minmAtk, mAtkgrouth, currentLevel, maxLevel);//mAtk.Calculate(currentLevel, maxLevel);
            result.mDef = Calculate(maxmDef, minmDef, mDefgrouth, currentLevel, maxLevel);//mDef.Calculate(currentLevel, maxLevel);
            result.spd = Calculate(maxspd, minspd, spdgrouth, currentLevel, maxLevel);//pd.Calculate(currentLevel, maxLevel);
            result.eva = Calculate(maxeva, mineva, evagrouth, currentLevel, maxLevel);//eva.Calculate(currentLevel, maxLevel);
            result.acc = Calculate(maxacc, minacc, accgrouth, currentLevel, maxLevel);//acc.Calculate(currentLevel, maxLevel);
            return result;
        }

        public CalculationAttributes GetTotalAttributes()
        {
            var result = new CalculationAttributes();
            result += CreateCalculationAttributes(1, Const.MaxLevel);

            // If this is character item, applies rate attributes
            result.hp += Mathf.CeilToInt(result.hpRate * result.hp);
            result.pAtk += Mathf.CeilToInt(result.pAtkRate * result.pAtk);
            result.pDef += Mathf.CeilToInt(result.pDefRate * result.pDef);
            result.mAtk += Mathf.CeilToInt(result.mAtkRate * result.mAtk);
            result.mDef += Mathf.CeilToInt(result.mDefRate * result.mDef);
            result.spd += Mathf.CeilToInt(result.spdRate * result.spd);
            result.eva += Mathf.CeilToInt(result.evaRate * result.eva);
            result.acc += Mathf.CeilToInt(result.accRate * result.acc);

            result.hp += Mathf.CeilToInt(result._hpRate * result.hp);
            result.pAtk += Mathf.CeilToInt(result._pAtkRate * result.pAtk);
            result.pDef += Mathf.CeilToInt(result._pDefRate * result.pDef);
            result.mAtk += Mathf.CeilToInt(result._mAtkRate * result.mAtk);
            result.mDef += Mathf.CeilToInt(result._mDefRate * result.mDef);
            result.spd += Mathf.CeilToInt(result._spdRate * result.spd);
            result.eva += Mathf.CeilToInt(result._evaRate * result.eva);
            result.acc += Mathf.CeilToInt(result._accRate * result.acc);
            return result;
        }

        public List<CustomSkill> GetBattleCustomSkills()
        {
            return new List<CustomSkill>();
        }

        public PlayerItem GetPlayerItem()
        {
            return new PlayerItem(PlayerItem.ItemType.character);
        }
    }
}