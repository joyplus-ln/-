using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SQLite3TableDataTmp
{

    public partial class IEquipment
    {
        public static Dictionary<string, IEquipment> DataMap = new Dictionary<string, IEquipment>();

        public static void Init()
        {
            DataMap = DBManager.instance.ConfigSQLite3Operate.SelectDictT_ST<IEquipment>();
        }
        public static void UpdataDataMap()
        {
            foreach (var dataMapValue in DataMap.Values)
            {
                DBManager.instance.LocalSQLite3Operate.UpdateOrInsert(dataMapValue);
            }
        }

        /// <summary>
        /// 获取这个装备的基数属性,白板属性,,,成长信息
        /// </summary>
        /// <returns></returns>
        public Attributes GetAttributes()
        {
            Attributes result = new Attributes();
            result.hp.SetData(minhp, maxhp, hpgrowth);
            result.pAtk.SetData(minpAtk, maxpAtk, pAtkgrouth);
            result.pDef.SetData(minpDef, maxpDef, pDefgrouth);
            result.mAtk.SetData(minmAtk, maxmAtk, mAtkgrouth);
            result.mDef.SetData(minmDef, maxmDef, mDefgrouth);
            result.spd.SetData(minspd, maxspd, spdgrouth);
            result.eva.SetData(mineva, maxeva, evagrouth);
            result.acc.SetData(minacc, maxacc, accgrouth);
            result.exp_hp = exp_hp;
            result.exp_patk = exp_patk;
            result.exp_pdef = exp_pdef;
            result.exp_matk = exp_matk;
            result.exp_mdef = exp_mdef;
            result.exp_spd = exp_spd;
            result.exp_eva = exp_eva;
            result.exp_acc = exp_acc;
            result.exp_hpRate = exp_hpRate;
            result.exp_pAtkRate = exp_pAtkRate;
            result.exp_pDefRate = exp_pDefRate;
            result.exp_mAtkRate = exp_mAtkRate;
            result.exp_mDefRate = exp_mDefRate;
            result.exp_spdRate = exp_spdRate;
            result.exp_evaRate = exp_evaRate;
            result.exp_accRate = exp_accRate;
            result.exp_critChance = exp_critChance;
            result.exp_critDamageRate = exp_critDamageRate;
            result.exp_blockChance = exp_blockChance;
            result.exp_blockDamageRate = exp_blockDamageRate;
            return result;
        }

    }
}
