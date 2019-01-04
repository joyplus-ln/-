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
    public enum IPlayerCurrencyEnum
    {
        id,
        guid,
        playerId,
        amount,
        purchasedAmount,
    }

    public partial class IPlayerCurrency : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)IPlayerCurrencyEnum.id)]
        public int id { get; private set; }  //des

        [Sync((int)IPlayerCurrencyEnum.guid)]
        public string guid { get; set; }  //des

        [Sync((int)IPlayerCurrencyEnum.playerId)]
        public string playerId { get; set; }  //des

        [Sync((int)IPlayerCurrencyEnum.amount)]
        public int amount { get; set; }  //des

        [Sync((int)IPlayerCurrencyEnum.purchasedAmount)]
        public int purchasedAmount { get; set; }  //des

        public IPlayerCurrency()
        {
        }

        public IPlayerCurrency(int Inid, string Inguid, string InplayerId, int Inamount, int InpurchasedAmount)
        {
            id = Inid;
            guid = Inguid;
            playerId = InplayerId;
            amount = Inamount;
            purchasedAmount = InpurchasedAmount;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "IPlayerCurrency : id = " + id + ", guid = " + guid + ", playerId = " + playerId + ", amount = " + amount + ", purchasedAmount = " + purchasedAmount;
        }

    }
}
