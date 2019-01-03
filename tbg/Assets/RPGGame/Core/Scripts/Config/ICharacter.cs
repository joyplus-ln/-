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
    public enum ICharacterEnum
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
        customSkill,
        passiveskill,
    }

    public partial class ICharacter : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)ICharacterEnum.id)]
        public int id { get; private set; }  //des

        [Sync((int)ICharacterEnum.guid)]
        public string guid { get; set; }  //des

        [Sync((int)ICharacterEnum.title)]
        public string title { get; set; }  //des

        [Sync((int)ICharacterEnum.description)]
        public string description { get; set; }  //des

        [Sync((int)ICharacterEnum.region)]
        public string region { get; set; }  //des

        [Sync((int)ICharacterEnum.quality)]
        public string quality { get; set; }  //des

        [Sync((int)ICharacterEnum.category)]
        public int category { get; set; }  //des

        [Sync((int)ICharacterEnum.minhp)]
        public int minhp { get; set; }  //des

        [Sync((int)ICharacterEnum.maxhp)]
        public int maxhp { get; set; }  //des

        [Sync((int)ICharacterEnum.hpgrowth)]
        public float hpgrowth { get; set; }  //des

        [Sync((int)ICharacterEnum.minpAtk)]
        public int minpAtk { get; set; }  //des

        [Sync((int)ICharacterEnum.maxpAtk)]
        public int maxpAtk { get; set; }  //des

        [Sync((int)ICharacterEnum.pAtkgrouth)]
        public int pAtkgrouth { get; set; }  //des

        [Sync((int)ICharacterEnum.minpDef)]
        public int minpDef { get; set; }  //des

        [Sync((int)ICharacterEnum.maxpDef)]
        public int maxpDef { get; set; }  //des

        [Sync((int)ICharacterEnum.pDefgrouth)]
        public int pDefgrouth { get; set; }  //des

        [Sync((int)ICharacterEnum.minmAtk)]
        public int minmAtk { get; set; }  //des

        [Sync((int)ICharacterEnum.maxmAtk)]
        public int maxmAtk { get; set; }  //des

        [Sync((int)ICharacterEnum.mAtkgrouth)]
        public int mAtkgrouth { get; set; }  //des

        [Sync((int)ICharacterEnum.minmDef)]
        public int minmDef { get; set; }  //des

        [Sync((int)ICharacterEnum.maxmDef)]
        public int maxmDef { get; set; }  //des

        [Sync((int)ICharacterEnum.mDefgrouth)]
        public int mDefgrouth { get; set; }  //des

        [Sync((int)ICharacterEnum.minspd)]
        public int minspd { get; set; }  //des

        [Sync((int)ICharacterEnum.maxspd)]
        public int maxspd { get; set; }  //des

        [Sync((int)ICharacterEnum.spdgrouth)]
        public int spdgrouth { get; set; }  //des

        [Sync((int)ICharacterEnum.mineva)]
        public int mineva { get; set; }  //des

        [Sync((int)ICharacterEnum.maxeva)]
        public int maxeva { get; set; }  //des

        [Sync((int)ICharacterEnum.evagrouth)]
        public int evagrouth { get; set; }  //des

        [Sync((int)ICharacterEnum.minacc)]
        public int minacc { get; set; }  //des

        [Sync((int)ICharacterEnum.maxacc)]
        public int maxacc { get; set; }  //des

        [Sync((int)ICharacterEnum.accgrouth)]
        public int accgrouth { get; set; }  //des

        [Sync((int)ICharacterEnum.customSkill)]
        public string customSkill { get; set; }  //des

        [Sync((int)ICharacterEnum.passiveskill)]
        public string passiveskill { get; set; }  //des

        public ICharacter()
        {
        }

        public ICharacter(int Inid, string Inguid, string Intitle, string Indescription, string Inregion, string Inquality, int Incategory, int Inminhp, int Inmaxhp, float Inhpgrowth, int InminpAtk, int InmaxpAtk, int InpAtkgrouth, int InminpDef, int InmaxpDef, int InpDefgrouth, int InminmAtk, int InmaxmAtk, int InmAtkgrouth, int InminmDef, int InmaxmDef, int InmDefgrouth, int Inminspd, int Inmaxspd, int Inspdgrouth, int Inmineva, int Inmaxeva, int Inevagrouth, int Inminacc, int Inmaxacc, int Inaccgrouth, string IncustomSkill, string Inpassiveskill)
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
            customSkill = IncustomSkill;
            passiveskill = Inpassiveskill;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "ICharacter : id = " + id + ", guid = " + guid + ", title = " + title + ", description = " + description + ", region = " + region + ", quality = " + quality + ", category = " + category + ", minhp = " + minhp + ", maxhp = " + maxhp + ", hpgrowth = " + hpgrowth + ", minpAtk = " + minpAtk + ", maxpAtk = " + maxpAtk + ", pAtkgrouth = " + pAtkgrouth + ", minpDef = " + minpDef + ", maxpDef = " + maxpDef + ", pDefgrouth = " + pDefgrouth + ", minmAtk = " + minmAtk + ", maxmAtk = " + maxmAtk + ", mAtkgrouth = " + mAtkgrouth + ", minmDef = " + minmDef + ", maxmDef = " + maxmDef + ", mDefgrouth = " + mDefgrouth + ", minspd = " + minspd + ", maxspd = " + maxspd + ", spdgrouth = " + spdgrouth + ", mineva = " + mineva + ", maxeva = " + maxeva + ", evagrouth = " + evagrouth + ", minacc = " + minacc + ", maxacc = " + maxacc + ", accgrouth = " + accgrouth + ", customSkill = " + customSkill + ", passiveskill = " + passiveskill;
        }

    }
}
