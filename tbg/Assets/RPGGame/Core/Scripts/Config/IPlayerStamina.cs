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
    public enum IPlayerStaminaEnum
    {
        id,
        Guid,
        playerId,
        amount,
        recoveredTime,
        dataId,
    }

    public partial class IPlayerStamina : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)IPlayerStaminaEnum.id)]
        public int id { get; private set; }  //id

        [Sync((int)IPlayerStaminaEnum.Guid)]
        public string Guid { get; set; }  //Guid

        [Sync((int)IPlayerStaminaEnum.playerId)]
        public string playerId { get; set; }  //playerId

        [Sync((int)IPlayerStaminaEnum.amount)]
        public int amount { get; set; }  //amount

        [Sync((int)IPlayerStaminaEnum.recoveredTime)]
        public float recoveredTime { get; set; }  //recoveredTime

        [Sync((int)IPlayerStaminaEnum.dataId)]
        public string dataId { get; set; }  //recoveredTime

        public IPlayerStamina()
        {
        }

        public IPlayerStamina(int Inid, string InGuid, string InplayerId, int Inamount, float InrecoveredTime, string IndataId)
        {
            id = Inid;
            Guid = InGuid;
            playerId = InplayerId;
            amount = Inamount;
            recoveredTime = InrecoveredTime;
            dataId = IndataId;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "IPlayerStamina : id = " + id + ", Guid = " + Guid + ", playerId = " + playerId + ", amount = " + amount + ", recoveredTime = " + recoveredTime + ", dataId = " + dataId;
        }

    }
}
