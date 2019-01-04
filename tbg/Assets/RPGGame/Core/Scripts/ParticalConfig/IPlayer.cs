using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SQLite3TableDataTmp
{
    public partial class IPlayer
    {
        public static string CurrentPlayerId { get; private set; }
        public static Dictionary<string, IPlayer> DataMap = new Dictionary<string, IPlayer>();
        public static IPlayer CurrentPlayer
        {
            get
            {
                if (!string.IsNullOrEmpty(CurrentPlayerId) && DataMap.ContainsKey(CurrentPlayerId))
                    return DataMap[CurrentPlayerId];
                return null;
            }
            set
            {
                if (value == null || string.IsNullOrEmpty(value.guid))
                {
                    CurrentPlayerId = string.Empty;
                    return;
                }
                SetData(value);
                CurrentPlayerId = value.guid;
            }
        }

        public static void Init()
        {
            DataMap = DBManager.instance.LocalSQLite3Operate.SelectDictT_ST<IPlayer>();
        }
        public static void UpdataDataMap()
        {
            foreach (var dataMapValue in DataMap.Values)
            {
                DBManager.instance.LocalSQLite3Operate.UpdateOrInsert(dataMapValue);
            }
        }
        public static void SetData(IPlayer data)
        {
            if (data == null || string.IsNullOrEmpty(data.guid))
                return;
            DataMap[data.guid] = data;
        }

        public static void ClearData()
        {
            DataMap.Clear();
        }

        #region Non Serialize Fields

        private int level = -1;
        private int collectExp = -1;
        private int dirtyExp = -1;  // Exp for dirty check to calculate `Level` and `CollectExp` fields


        public int Level
        {
            get
            {
                CalculateLevelAndRemainExp();
                return level;
            }
        }

        public int CollectExp
        {
            get
            {
                CalculateLevelAndRemainExp();
                return collectExp;
            }
        }

        public int MaxLevel
        {
            get { return GameInstance.GameDatabase == null ? 1 : GameInstance.GameDatabase.playerMaxLevel; }
        }

        public int NextExp
        {
            get { return GameInstance.GameDatabase == null ? 0 : GameInstance.GameDatabase.playerExpTable.Calculate(Level, GameInstance.GameDatabase.playerMaxLevel); }
        }
        #endregion

        private void CalculateLevelAndRemainExp()
        {
            if (GameInstance.GameDatabase == null)
            {
                level = 1;
                collectExp = 0;
                return;
            }
            if (dirtyExp == -1 || dirtyExp != exp)
            {
                dirtyExp = exp;
                var remainExp = exp;
                var maxLevel = GameInstance.GameDatabase.playerMaxLevel;
                for (level = 1; level < maxLevel; ++level)
                {
                    var nextExp = GameInstance.GameDatabase.playerExpTable.Calculate(level, maxLevel);
                    if (remainExp - nextExp < 0)
                        break;
                    remainExp -= nextExp;
                }
                collectExp = remainExp;
            }
        }
    }
}

