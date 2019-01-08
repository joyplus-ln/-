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
    public enum IPlayerFormationEnum
    {
        id,
        guid,
        itemId,
    }

    public partial class IPlayerFormation : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)IPlayerFormationEnum.id)]
        public int id { get; private set; }  //des

        [Sync((int)IPlayerFormationEnum.guid)]
        public string guid { get; set; }  //des

        [Sync((int)IPlayerFormationEnum.itemId)]
        public string itemId { get; set; }  //des

        public IPlayerFormation()
        {
        }

        public IPlayerFormation(int Inid, string Inguid, string InitemId)
        {
            id = Inid;
            guid = Inguid;
            itemId = InitemId;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "IPlayerFormation : id = " + id + ", guid = " + guid + ", itemId = " + itemId;
        }

    }
}
