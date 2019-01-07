using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SQLite3TableDataTmp
{
    public partial class ICharacter
    {
        public static Dictionary<string, ICharacter> DataMap = new Dictionary<string, ICharacter>();

        public static void Init()
        {
            DataMap = DBManager.instance.ConfigSQLite3Operate.SelectDictT_ST<ICharacter>();
        }
        public static void UpdataDataMap()
        {
            foreach (var dataMapValue in DataMap.Values)
            {
                DBManager.instance.LocalSQLite3Operate.UpdateOrInsert(dataMapValue);
            }
        }

        /// <summary>
        /// 获取这个英雄的基数属性,白板属性
        /// </summary>
        /// <returns></returns>
        public Attributes GetAttributes(int level = 1)
        {
            Attributes result = new Attributes(level);
            result.hp.SetData(minhp, maxhp, hpgrowth);
            result.pAtk.SetData(minpAtk, maxpAtk, pAtkgrouth);
            result.pDef.SetData(minpDef, maxpDef, pDefgrouth);
            result.mAtk.SetData(minmAtk, maxmAtk, mAtkgrouth);
            result.mDef.SetData(minmDef, maxmDef, mDefgrouth);
            result.spd.SetData(minspd, maxspd, spdgrouth);
            result.eva.SetData(mineva, maxeva, evagrouth);
            result.acc.SetData(minacc, maxacc, accgrouth);
            result.exp_hp = 0;
            result.exp_patk = 0;
            result.exp_pdef = 0;
            result.exp_matk = 0;
            result.exp_mdef = 0;
            result.exp_spd = 0;
            result.exp_eva = 0;
            result.exp_acc = 0;
            result.exp_hpRate = 0;
            result.exp_pAtkRate = 0;
            result.exp_pDefRate = 0;
            result.exp_mAtkRate = 0;
            result.exp_mDefRate = 0;
            result.exp_spdRate = 0;
            result.exp_evaRate = 0;
            result.exp_accRate = 0;
            result.exp_critChance = 0;
            result.exp_critDamageRate = 0;
            result.exp_blockChance = 0;
            result.exp_blockDamageRate = 0;
            return result;
        }



        public List<CustomSkill> GetBattleCustomSkills()
        {
            return new List<CustomSkill>();
        }

    }
}