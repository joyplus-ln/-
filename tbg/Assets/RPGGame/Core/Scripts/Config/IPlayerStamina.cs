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
        playerId,
        Guid,
        amount,
        recoveredTime,
    }

    public class IPlayerStamina : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)IPlayerStaminaEnum.id)]
        public int id { get; private set; }  //id

        [Sync((int)IPlayerStaminaEnum.playerId)]
        public string playerId { get; set; }  //playerId

        [Sync((int)IPlayerStaminaEnum.Guid)]
        public string Guid { get; set; }  //Guid

        [Sync((int)IPlayerStaminaEnum.amount)]
        public int amount { get; set; }  //amount

        [Sync((int)IPlayerStaminaEnum.recoveredTime)]
        public string recoveredTime { get; set; }  //recoveredTime

        public IPlayerStamina()
        {
        }

        public IPlayerStamina(int Inid, string InplayerId, string InGuid, int Inamount, string InrecoveredTime)
        {
            id = Inid;
            playerId = InplayerId;
            Guid = InGuid;
            amount = Inamount;
            recoveredTime = InrecoveredTime;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "IPlayerStamina : id = " + id + ", playerId = " + playerId + ", Guid = " + Guid + ", amount = " + amount + ", recoveredTime = " + recoveredTime;
        }

    }
}
