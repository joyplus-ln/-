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
    public enum IPlayerClearStageEnum
    {
        id,
        playerId,
        Guid,
        bestRating,
    }

    public class IPlayerClearStage : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)IPlayerClearStageEnum.id)]
        public int id { get; private set; }  //des

        [Sync((int)IPlayerClearStageEnum.playerId)]
        public string playerId { get; set; }  //des

        [Sync((int)IPlayerClearStageEnum.Guid)]
        public string Guid { get; set; }  //des

        [Sync((int)IPlayerClearStageEnum.bestRating)]
        public string bestRating { get; set; }  //des

        public IPlayerClearStage()
        {
        }

        public IPlayerClearStage(int Inid, string InplayerId, string InGuid, string InbestRating)
        {
            id = Inid;
            playerId = InplayerId;
            Guid = InGuid;
            bestRating = InbestRating;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "IPlayerClearStage : id = " + id + ", playerId = " + playerId + ", Guid = " + Guid + ", bestRating = " + bestRating;
        }

    }
}
