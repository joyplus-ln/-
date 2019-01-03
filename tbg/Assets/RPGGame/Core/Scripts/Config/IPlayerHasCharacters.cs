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
    public enum IPlayerHasCharactersEnum
    {
        id,
        guid,
        playerId,
        amount,
        exp,
        equipItemId,
        equipPosition,
        level,
    }

    public partial class IPlayerHasCharacters : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)IPlayerHasCharactersEnum.id)]
        public int id { get; private set; }  //des

        [Sync((int)IPlayerHasCharactersEnum.guid)]
        public string guid { get; set; }  //des

        [Sync((int)IPlayerHasCharactersEnum.playerId)]
        public string playerId { get; set; }  //des

        [Sync((int)IPlayerHasCharactersEnum.amount)]
        public int amount { get; set; }  //des

        [Sync((int)IPlayerHasCharactersEnum.exp)]
        public int exp { get; set; }  //des

        [Sync((int)IPlayerHasCharactersEnum.equipItemId)]
        public string equipItemId { get; set; }  //des

        [Sync((int)IPlayerHasCharactersEnum.equipPosition)]
        public string equipPosition { get; set; }  //des

        [Sync((int)IPlayerHasCharactersEnum.level)]
        public int level { get; set; }  //des

        public IPlayerHasCharacters()
        {
        }

        public IPlayerHasCharacters(int Inid, string Inguid, string InplayerId, int Inamount, int Inexp, string InequipItemId, string InequipPosition, int Inlevel)
        {
            id = Inid;
            guid = Inguid;
            playerId = InplayerId;
            amount = Inamount;
            exp = Inexp;
            equipItemId = InequipItemId;
            equipPosition = InequipPosition;
            level = Inlevel;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "IPlayerHasCharacters : id = " + id + ", guid = " + guid + ", playerId = " + playerId + ", amount = " + amount + ", exp = " + exp + ", equipItemId = " + equipItemId + ", equipPosition = " + equipPosition + ", level = " + level;
        }

    }
}
