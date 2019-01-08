/*
 * --->SQLite3 dataSyncBase table structure.<---
 * --->This class code is automatically generated。<---
 * --->If you need to modify, please place the custom code between <Self Code Begin> and <Self Code End>.
 *                                                                                    --szn
 */

using System.Collections.Generic;
using Framework.Reflection.SQLite3Helper;
using Framework.Reflection.Sync;
using UnityEngine;


namespace SQLite3TableDataTmp
{


    public partial class IPlayerFormation
    {
        public static Dictionary<int, IPlayerFormation> DataMap = new Dictionary<int, IPlayerFormation>();

        public static void Init()
        {
            DataMap = DBManager.instance.LocalSQLite3Operate.SelectDictT<IPlayerFormation>();
        }
        public static void UpdataDataMap()
        {
            foreach (var dataMapValue in DataMap.Values)
            {
                DBManager.instance.LocalSQLite3Operate.UpdateOrInsert(dataMapValue);
            }
        }

        public IPlayerHasCharacters GetHasCharacter()
        {
            return IPlayerHasCharacters.DataMap[itemId];
        }
    }
}
