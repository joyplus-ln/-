using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SQLite3TableDataTmp
{
    public partial class IPlayerHasEquips
    {
        public static Dictionary<string, IPlayerHasEquips> DataMap = new Dictionary<string, IPlayerHasEquips>();

        public static void Init()
        {
            DataMap = DBManager.instance.LocalSQLite3Operate.SelectDictT_ST<IPlayerHasEquips>();
        }
        public static void UpdataDataMap()
        {
            foreach (var dataMapValue in DataMap.Values)
            {
                DBManager.instance.LocalSQLite3Operate.UpdateOrInsert(dataMapValue);
            }
        }

        public IEquipment IEquipment
        {
            get { return IEquipment.DataMap[dataId]; }
        }


        public static Dictionary<string, IPlayerHasEquips> GetHeroEquipses(string guid)
        {
            Dictionary<string, IPlayerHasEquips> list = new Dictionary<string, IPlayerHasEquips>();
            foreach (var hasEquips in DataMap.Values)
            {
                if (hasEquips.equipItemId.Equals(guid))
                {
                    if (!list.ContainsKey(hasEquips.equipPosition))
                    {
                        list.Add(hasEquips.equipPosition, hasEquips);
                    }
                    else
                    {
                        Debug.LogError("出错了，一个英雄身上穿了两件装备");
                    }
                }

            }
            return list;
        }

        public static void InsertNewEquips(string dataId)
        {
            if (IEquipment.DataMap.ContainsKey(dataId))
            {
                string guid = Guid.NewGuid().ToString();
                DataMap.Add(guid, new IPlayerHasEquips(DataMap.Count + 1, guid, dataId, 1, 0, "", "", 1));
                UpdataDataMap();
            }
            else
            {
                Debug.LogError("插入失败");
            }
        }


    }
}

