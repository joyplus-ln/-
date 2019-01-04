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
    public enum IPlayerUnlockItemEnum
    {
        id,
        playerId,
        Guid,
        amount,
    }

    public partial class IPlayerUnlockItem : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)IPlayerUnlockItemEnum.id)]
        public int id { get; private set; }  //id

        [Sync((int)IPlayerUnlockItemEnum.playerId)]
        public string playerId { get; set; }  //playerId

        [Sync((int)IPlayerUnlockItemEnum.Guid)]
        public string Guid { get; set; }  //Guid

        [Sync((int)IPlayerUnlockItemEnum.amount)]
        public string amount { get; set; }  //amount

        public IPlayerUnlockItem()
        {
        }

        public IPlayerUnlockItem(int Inid, string InplayerId, string InGuid, string Inamount)
        {
            id = Inid;
            playerId = InplayerId;
            Guid = InGuid;
            amount = Inamount;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "IPlayerUnlockItem : id = " + id + ", playerId = " + playerId + ", Guid = " + Guid + ", amount = " + amount;
        }

    }
}
