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
    public enum IPlayerHasEquipsEnum
    {
        id,
        playerId,
        Guid,
        amount,
        exp,
        equipItemId,
        equipPosition,
    }

    public partial class IPlayerHasEquips : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)IPlayerHasEquipsEnum.id)]
        public int id { get; private set; }  //des

        [Sync((int)IPlayerHasEquipsEnum.playerId)]
        public string playerId { get; set; }  //des

        [Sync((int)IPlayerHasEquipsEnum.Guid)]
        public string Guid { get; set; }  //des

        [Sync((int)IPlayerHasEquipsEnum.amount)]
        public int amount { get; set; }  //des

        [Sync((int)IPlayerHasEquipsEnum.exp)]
        public int exp { get; set; }  //des

        [Sync((int)IPlayerHasEquipsEnum.equipItemId)]
        public string equipItemId { get; set; }  //des

        [Sync((int)IPlayerHasEquipsEnum.equipPosition)]
        public string equipPosition { get; set; }  //des

        public IPlayerHasEquips()
        {
        }

        public IPlayerHasEquips(int Inid, string InplayerId, string InGuid, int Inamount, int Inexp, string InequipItemId, string InequipPosition)
        {
            id = Inid;
            playerId = InplayerId;
            Guid = InGuid;
            amount = Inamount;
            exp = Inexp;
            equipItemId = InequipItemId;
            equipPosition = InequipPosition;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "IPlayerHasEquips : id = " + id + ", playerId = " + playerId + ", Guid = " + Guid + ", amount = " + amount + ", exp = " + exp + ", equipItemId = " + equipItemId + ", equipPosition = " + equipPosition;
        }

    }
}
