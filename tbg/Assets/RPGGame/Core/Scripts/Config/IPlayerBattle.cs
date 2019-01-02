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
    public enum IPlayerBattleEnum
    {
        id,
        playerId,
        guid,
        session,
        battleResult,
        rating,
    }

    public class IPlayerBattle : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)IPlayerBattleEnum.id)]
        public int id { get; private set; }  //des

        [Sync((int)IPlayerBattleEnum.playerId)]
        public string playerId { get; set; }  //des

        [Sync((int)IPlayerBattleEnum.guid)]
        public string guid { get; set; }  //des

        [Sync((int)IPlayerBattleEnum.session)]
        public string session { get; set; }  //des

        [Sync((int)IPlayerBattleEnum.battleResult)]
        public string battleResult { get; set; }  //des

        [Sync((int)IPlayerBattleEnum.rating)]
        public string rating { get; set; }  //des

        public IPlayerBattle()
        {
        }

        public IPlayerBattle(int Inid, string InplayerId, string Inguid, string Insession, string InbattleResult, string Inrating)
        {
            id = Inid;
            playerId = InplayerId;
            guid = Inguid;
            session = Insession;
            battleResult = InbattleResult;
            rating = Inrating;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "IPlayerBattle : id = " + id + ", playerId = " + playerId + ", guid = " + guid + ", session = " + session + ", battleResult = " + battleResult + ", rating = " + rating;
        }

    }
}
