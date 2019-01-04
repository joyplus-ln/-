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
    public enum IPlayerAuthEnum
    {
        id,
        guid,
        playerId,
        type,
        username,
        password,
    }

    public partial class IPlayerAuth : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)IPlayerAuthEnum.id)]
        public int id { get; private set; }  //des

        [Sync((int)IPlayerAuthEnum.guid)]
        public string guid { get; set; }  //des

        [Sync((int)IPlayerAuthEnum.playerId)]
        public string playerId { get; set; }  //des

        [Sync((int)IPlayerAuthEnum.type)]
        public string type { get; set; }  //des

        [Sync((int)IPlayerAuthEnum.username)]
        public string username { get; set; }  //des

        [Sync((int)IPlayerAuthEnum.password)]
        public string password { get; set; }  //des

        public IPlayerAuth()
        {
        }

        public IPlayerAuth(int Inid, string Inguid, string InplayerId, string Intype, string Inusername, string Inpassword)
        {
            id = Inid;
            guid = Inguid;
            playerId = InplayerId;
            type = Intype;
            username = Inusername;
            password = Inpassword;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "IPlayerAuth : id = " + id + ", guid = " + guid + ", playerId = " + playerId + ", type = " + type + ", username = " + username + ", password = " + password;
        }

    }
}
