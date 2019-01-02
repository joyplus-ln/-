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
    public enum IPlayerEnum
    {
        id,
        guid,
        profileName,
        loginToken,
        exp,
        selectedFormation,
        prefs,
    }

    public class IPlayer : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)IPlayerEnum.id)]
        public int id { get; private set; }  //des

        [Sync((int)IPlayerEnum.guid)]
        public string guid { get; set; }  //des

        [Sync((int)IPlayerEnum.profileName)]
        public string profileName { get; set; }  //des

        [Sync((int)IPlayerEnum.loginToken)]
        public string loginToken { get; set; }  //des

        [Sync((int)IPlayerEnum.exp)]
        public int exp { get; set; }  //des

        [Sync((int)IPlayerEnum.selectedFormation)]
        public string selectedFormation { get; set; }  //des

        [Sync((int)IPlayerEnum.prefs)]
        public string prefs { get; set; }  //des

        public IPlayer()
        {
        }

        public IPlayer(int Inid, string Inguid, string InprofileName, string InloginToken, int Inexp, string InselectedFormation, string Inprefs)
        {
            id = Inid;
            guid = Inguid;
            profileName = InprofileName;
            loginToken = InloginToken;
            exp = Inexp;
            selectedFormation = InselectedFormation;
            prefs = Inprefs;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "IPlayer : id = " + id + ", guid = " + guid + ", profileName = " + profileName + ", loginToken = " + loginToken + ", exp = " + exp + ", selectedFormation = " + selectedFormation + ", prefs = " + prefs;
        }

    }
}
