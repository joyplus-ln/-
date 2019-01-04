using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SQLite3TableDataTmp
{

    public partial class IPlayerAuth
    {
        public static Dictionary<string, IPlayerAuth> DataMap = new Dictionary<string, IPlayerAuth>();

        public static void Init()
        {
            DataMap = DBManager.instance.LocalSQLite3Operate.SelectDictT_ST<IPlayerAuth>();
        }

        public static void UpdataDataMap()
        {
            foreach (var dataMapValue in DataMap.Values)
            {
                DBManager.instance.LocalSQLite3Operate.UpdateOrInsert(dataMapValue);
            }
        }

        public static string GetId(string playerId, string type)
        {
            return playerId + "_" + type;
        }

        public static void SetData(IPlayerAuth data)
        {
            if (data == null || string.IsNullOrEmpty(data.guid))
                return;
            DataMap[data.guid] = data;
        }

        public static bool TryGetData(string playerId, string type, out IPlayerAuth data)
        {
            return DataMap.TryGetValue(GetId(playerId, type), out data);
        }

        public static bool TryGetData(string type, out IPlayerAuth data)
        {
            return TryGetData(IPlayer.CurrentPlayerId, type, out data);
        }

        public static bool RemoveData(string id)
        {
            return DataMap.Remove(id);
        }

        public static void ClearData()
        {
            DataMap.Clear();
        }

        public static void SetDataRange(IEnumerable<IPlayerAuth> list)
        {
            foreach (var data in list)
            {
                SetData(data);
            }
        }

        public static void RemoveDataRange(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                RemoveData(id);
            }
        }

        public static void RemoveDataRange(string playerId)
        {
            var values = DataMap.Values;
            foreach (var value in values)
            {
                if (value.playerId == playerId)
                    RemoveData(value.guid);
            }
        }

        public static void RemoveDataRange()
        {
            RemoveDataRange(IPlayer.CurrentPlayerId);
        }
    }
}
