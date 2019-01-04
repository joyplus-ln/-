/*
 * --->SQLite3 dataSyncBase table structure.<---
 * --->This class code is automatically generated。<---
 * --->If you need to modify, please place the custom code between <Self Code Begin> and <Self Code End>.
 *                                                                                    --szn
 */

using System.Collections.Generic;
using Framework.Reflection.SQLite3Helper;
using Framework.Reflection.Sync;


namespace SQLite3TableDataTmp
{


    public partial class IPlayerStamina
    {
        public static Dictionary<string, IPlayerStamina> DataMap = new Dictionary<string, IPlayerStamina>();

        public static void Init()
        {
            DataMap = DBManager.instance.LocalSQLite3Operate.SelectDictT_ST<IPlayerStamina>();
        }
        public static void UpdataDataMap()
        {
            foreach (var dataMapValue in DataMap.Values)
            {
                DBManager.instance.LocalSQLite3Operate.UpdateOrInsert(dataMapValue);
            }
        }
        public static void SetData(IPlayerStamina data)
        {
            if (data == null)
                return;
            DataMap[data.Guid] = data;
        }

        public static string GetId(string playerId, string dataId)
        {
            return playerId + "_" + dataId;
        }

    }
}
