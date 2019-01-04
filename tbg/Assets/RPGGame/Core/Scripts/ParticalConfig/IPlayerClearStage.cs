using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SQLite3TableDataTmp
{

    public partial class IPlayerClearStage
    {
        public static Dictionary<string, IPlayerClearStage> DataMap = new Dictionary<string, IPlayerClearStage>();

        public static void Init()
        {
            DataMap = DBManager.instance.LocalSQLite3Operate.SelectDictT_ST<IPlayerClearStage>();
        }
public static void UpdataDataMap()
        {
            foreach (var dataMapValue in DataMap.Values)
            {
                DBManager.instance.LocalSQLite3Operate.UpdateOrInsert(dataMapValue);
            }
        }
    }
}
