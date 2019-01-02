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
    public enum IPlayerOtherItemEnum
    {
        id,
        Guid,
        playerId,
        amount,
    }

    public class IPlayerOtherItem : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)IPlayerOtherItemEnum.id)]
        public int id { get; private set; }  //id

        [Sync((int)IPlayerOtherItemEnum.Guid)]
        public string Guid { get; set; }  //Guid

        [Sync((int)IPlayerOtherItemEnum.playerId)]
        public string playerId { get; set; }  //playerId

        [Sync((int)IPlayerOtherItemEnum.amount)]
        public string amount { get; set; }  //amount

        public IPlayerOtherItem()
        {
        }

        public IPlayerOtherItem(int Inid, string InGuid, string InplayerId, string Inamount)
        {
            id = Inid;
            Guid = InGuid;
            playerId = InplayerId;
            amount = Inamount;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "IPlayerOtherItem : id = " + id + ", Guid = " + Guid + ", playerId = " + playerId + ", amount = " + amount;
        }

    }
}
