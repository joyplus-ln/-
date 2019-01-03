/*
 * --->SQLite3 dataSyncBase table structure.<---
 * --->This class code is automatically generated。<---
 * --->If you need to modify, please place the custom code between <Self Code Begin> and <Self Code End>.
 *                                                                                    --szn
 */

using Framework.Reflection.SQLite3Helper;
using Framework.Reflection.Sync;


namespace SQLite3TableDataTmp
{
    public enum IEquipmentEnum
    {
        id,
        guid,
        title,
        description,
        region,
        quality,
        category,
        minhp,
        maxhp,
        hpgrowth,
        minpAtk,
        maxpAtk,
        pAtkgrouth,
        minpDef,
        maxpDef,
        pDefgrouth,
        minmAtk,
        maxmAtk,
        mAtkgrouth,
        minmDef,
        maxmDef,
        mDefgrouth,
        minspd,
        maxspd,
        spdgrouth,
        mineva,
        maxeva,
        evagrouth,
        minacc,
        maxacc,
        accgrouth,
        equippablePositions,
        exp_hp,
        exp_patk,
        exp_pdef,
        exp_matk,
        exp_mdef,
        exp_spd,
        exp_eva,
        exp_acc,
        exp_hpRate,
        exp_pAtkRate,
        exp_pDefRate,
        exp_mAtkRate,
        exp_mDefRate,
        exp_spdRate,
        exp_evaRate,
        exp_accRate,
        exp_critChance,
        exp_critDamageRate,
        exp_blockChance,
        exp_blockDamageRate,
    }

    public partial class IEquipment : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)IEquipmentEnum.id)]
        public int id { get; private set; }  //des

        [Sync((int)IEquipmentEnum.guid)]
        public string guid { get; set; }  //des

        [Sync((int)IEquipmentEnum.title)]
        public string title { get; set; }  //des

        [Sync((int)IEquipmentEnum.description)]
        public string description { get; set; }  //des

        [Sync((int)IEquipmentEnum.region)]
        public string region { get; set; }  //des

        [Sync((int)IEquipmentEnum.quality)]
        public string quality { get; set; }  //des

        [Sync((int)IEquipmentEnum.category)]
        public int category { get; set; }  //des

        [Sync((int)IEquipmentEnum.minhp)]
        public int minhp { get; set; }  //des

        [Sync((int)IEquipmentEnum.maxhp)]
        public int maxhp { get; set; }  //des

        [Sync((int)IEquipmentEnum.hpgrowth)]
        public float hpgrowth { get; set; }  //des

        [Sync((int)IEquipmentEnum.minpAtk)]
        public int minpAtk { get; set; }  //des

        [Sync((int)IEquipmentEnum.maxpAtk)]
        public int maxpAtk { get; set; }  //des

        [Sync((int)IEquipmentEnum.pAtkgrouth)]
        public float pAtkgrouth { get; set; }  //des

        [Sync((int)IEquipmentEnum.minpDef)]
        public int minpDef { get; set; }  //des

        [Sync((int)IEquipmentEnum.maxpDef)]
        public int maxpDef { get; set; }  //des

        [Sync((int)IEquipmentEnum.pDefgrouth)]
        public float pDefgrouth { get; set; }  //des

        [Sync((int)IEquipmentEnum.minmAtk)]
        public int minmAtk { get; set; }  //des

        [Sync((int)IEquipmentEnum.maxmAtk)]
        public int maxmAtk { get; set; }  //des

        [Sync((int)IEquipmentEnum.mAtkgrouth)]
        public float mAtkgrouth { get; set; }  //des

        [Sync((int)IEquipmentEnum.minmDef)]
        public int minmDef { get; set; }  //des

        [Sync((int)IEquipmentEnum.maxmDef)]
        public int maxmDef { get; set; }  //des

        [Sync((int)IEquipmentEnum.mDefgrouth)]
        public float mDefgrouth { get; set; }  //des

        [Sync((int)IEquipmentEnum.minspd)]
        public int minspd { get; set; }  //des

        [Sync((int)IEquipmentEnum.maxspd)]
        public int maxspd { get; set; }  //des

        [Sync((int)IEquipmentEnum.spdgrouth)]
        public float spdgrouth { get; set; }  //des

        [Sync((int)IEquipmentEnum.mineva)]
        public int mineva { get; set; }  //des

        [Sync((int)IEquipmentEnum.maxeva)]
        public int maxeva { get; set; }  //des

        [Sync((int)IEquipmentEnum.evagrouth)]
        public float evagrouth { get; set; }  //des

        [Sync((int)IEquipmentEnum.minacc)]
        public int minacc { get; set; }  //des

        [Sync((int)IEquipmentEnum.maxacc)]
        public int maxacc { get; set; }  //des

        [Sync((int)IEquipmentEnum.accgrouth)]
        public float accgrouth { get; set; }  //des

        [Sync((int)IEquipmentEnum.equippablePositions)]
        public int equippablePositions { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_hp)]
        public float exp_hp { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_patk)]
        public float exp_patk { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_pdef)]
        public float exp_pdef { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_matk)]
        public float exp_matk { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_mdef)]
        public float exp_mdef { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_spd)]
        public float exp_spd { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_eva)]
        public float exp_eva { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_acc)]
        public float exp_acc { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_hpRate)]
        public float exp_hpRate { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_pAtkRate)]
        public float exp_pAtkRate { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_pDefRate)]
        public float exp_pDefRate { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_mAtkRate)]
        public float exp_mAtkRate { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_mDefRate)]
        public float exp_mDefRate { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_spdRate)]
        public float exp_spdRate { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_evaRate)]
        public float exp_evaRate { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_accRate)]
        public float exp_accRate { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_critChance)]
        public float exp_critChance { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_critDamageRate)]
        public float exp_critDamageRate { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_blockChance)]
        public float exp_blockChance { get; set; }  //des

        [Sync((int)IEquipmentEnum.exp_blockDamageRate)]
        public float exp_blockDamageRate { get; set; }  //des

        public IEquipment()
        {
        }

        public IEquipment(int Inid, string Inguid, string Intitle, string Indescription, string Inregion, string Inquality, int Incategory, int Inminhp, int Inmaxhp, float Inhpgrowth, int InminpAtk, int InmaxpAtk, float InpAtkgrouth, int InminpDef, int InmaxpDef, float InpDefgrouth, int InminmAtk, int InmaxmAtk, float InmAtkgrouth, int InminmDef, int InmaxmDef, float InmDefgrouth, int Inminspd, int Inmaxspd, float Inspdgrouth, int Inmineva, int Inmaxeva, float Inevagrouth, int Inminacc, int Inmaxacc, float Inaccgrouth, int InequippablePositions, float Inexp_hp, float Inexp_patk, float Inexp_pdef, float Inexp_matk, float Inexp_mdef, float Inexp_spd, float Inexp_eva, float Inexp_acc, float Inexp_hpRate, float Inexp_pAtkRate, float Inexp_pDefRate, float Inexp_mAtkRate, float Inexp_mDefRate, float Inexp_spdRate, float Inexp_evaRate, float Inexp_accRate, float Inexp_critChance, float Inexp_critDamageRate, float Inexp_blockChance, float Inexp_blockDamageRate)
        {
            id = Inid;
            guid = Inguid;
            title = Intitle;
            description = Indescription;
            region = Inregion;
            quality = Inquality;
            category = Incategory;
            minhp = Inminhp;
            maxhp = Inmaxhp;
            hpgrowth = Inhpgrowth;
            minpAtk = InminpAtk;
            maxpAtk = InmaxpAtk;
            pAtkgrouth = InpAtkgrouth;
            minpDef = InminpDef;
            maxpDef = InmaxpDef;
            pDefgrouth = InpDefgrouth;
            minmAtk = InminmAtk;
            maxmAtk = InmaxmAtk;
            mAtkgrouth = InmAtkgrouth;
            minmDef = InminmDef;
            maxmDef = InmaxmDef;
            mDefgrouth = InmDefgrouth;
            minspd = Inminspd;
            maxspd = Inmaxspd;
            spdgrouth = Inspdgrouth;
            mineva = Inmineva;
            maxeva = Inmaxeva;
            evagrouth = Inevagrouth;
            minacc = Inminacc;
            maxacc = Inmaxacc;
            accgrouth = Inaccgrouth;
            equippablePositions = InequippablePositions;
            exp_hp = Inexp_hp;
            exp_patk = Inexp_patk;
            exp_pdef = Inexp_pdef;
            exp_matk = Inexp_matk;
            exp_mdef = Inexp_mdef;
            exp_spd = Inexp_spd;
            exp_eva = Inexp_eva;
            exp_acc = Inexp_acc;
            exp_hpRate = Inexp_hpRate;
            exp_pAtkRate = Inexp_pAtkRate;
            exp_pDefRate = Inexp_pDefRate;
            exp_mAtkRate = Inexp_mAtkRate;
            exp_mDefRate = Inexp_mDefRate;
            exp_spdRate = Inexp_spdRate;
            exp_evaRate = Inexp_evaRate;
            exp_accRate = Inexp_accRate;
            exp_critChance = Inexp_critChance;
            exp_critDamageRate = Inexp_critDamageRate;
            exp_blockChance = Inexp_blockChance;
            exp_blockDamageRate = Inexp_blockDamageRate;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "IEquipment : id = " + id + ", guid = " + guid + ", title = " + title + ", description = " + description + ", region = " + region + ", quality = " + quality + ", category = " + category + ", minhp = " + minhp + ", maxhp = " + maxhp + ", hpgrowth = " + hpgrowth + ", minpAtk = " + minpAtk + ", maxpAtk = " + maxpAtk + ", pAtkgrouth = " + pAtkgrouth + ", minpDef = " + minpDef + ", maxpDef = " + maxpDef + ", pDefgrouth = " + pDefgrouth + ", minmAtk = " + minmAtk + ", maxmAtk = " + maxmAtk + ", mAtkgrouth = " + mAtkgrouth + ", minmDef = " + minmDef + ", maxmDef = " + maxmDef + ", mDefgrouth = " + mDefgrouth + ", minspd = " + minspd + ", maxspd = " + maxspd + ", spdgrouth = " + spdgrouth + ", mineva = " + mineva + ", maxeva = " + maxeva + ", evagrouth = " + evagrouth + ", minacc = " + minacc + ", maxacc = " + maxacc + ", accgrouth = " + accgrouth + ", equippablePositions = " + equippablePositions + ", exp_hp = " + exp_hp + ", exp_patk = " + exp_patk + ", exp_pdef = " + exp_pdef + ", exp_matk = " + exp_matk + ", exp_mdef = " + exp_mdef + ", exp_spd = " + exp_spd + ", exp_eva = " + exp_eva + ", exp_acc = " + exp_acc + ", exp_hpRate = " + exp_hpRate + ", exp_pAtkRate = " + exp_pAtkRate + ", exp_pDefRate = " + exp_pDefRate + ", exp_mAtkRate = " + exp_mAtkRate + ", exp_mDefRate = " + exp_mDefRate + ", exp_spdRate = " + exp_spdRate + ", exp_evaRate = " + exp_evaRate + ", exp_accRate = " + exp_accRate + ", exp_critChance = " + exp_critChance + ", exp_critDamageRate = " + exp_critDamageRate + ", exp_blockChance = " + exp_blockChance + ", exp_blockDamageRate = " + exp_blockDamageRate;
        }

    }
}
