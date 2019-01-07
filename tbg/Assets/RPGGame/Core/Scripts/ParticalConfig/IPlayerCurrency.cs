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


    public partial class IPlayerCurrency
    {
        public static Dictionary<string, IPlayerCurrency> DataMap = new Dictionary<string, IPlayerCurrency>();

        public static void Init()
        {
            DataMap = DBManager.instance.LocalSQLite3Operate.SelectDictT_ST<IPlayerCurrency>();
        }
        public static void UpdataDataMap()
        {
            foreach (var dataMapValue in DataMap.Values)
            {
                DBManager.instance.LocalSQLite3Operate.UpdateOrInsert(dataMapValue);
            }
        }
        public static IPlayerCurrency SoftCurrency
        {
            get
            {
                IPlayerCurrency result = null;
                if (GameInstance.GameDatabase != null)
                    TryGetData(GameInstance.GameDatabase.softCurrency.id, out result);
                return result;
            }
        }
        public static IPlayerCurrency HardCurrency
        {
            get
            {
                IPlayerCurrency result = null;
                if (GameInstance.GameDatabase != null)
                    TryGetData(GameInstance.GameDatabase.hardCurrency.id, out result);
                return result;
            }
        }

        public IPlayerCurrency Clone()
        {
            var result = new IPlayerCurrency();
            CloneTo(this, result);
            return result;
        }

        public static void CloneTo(IPlayerCurrency from, IPlayerCurrency to)
        {
            to.id = from.id;
            to.playerId = from.playerId;
            //to. = from.DataId;
            to.amount = from.amount;
            to.purchasedAmount = from.purchasedAmount;
        }

        public IPlayerCurrency SetAmount(int amount, int purchasedAmount)
        {
            amount = amount;
            purchasedAmount = purchasedAmount;
            return this;
        }

        #region Non Serialize Fields
        public Currency CurrencyData
        {
            get
            {
                Currency currency;
                if (GameInstance.GameDatabase != null && !string.IsNullOrEmpty(guid) && GameInstance.GameDatabase.Currencies.TryGetValue(guid, out currency))
                    return currency;
                return null;
            }
        }
        #endregion

        public static bool HaveEnoughSoftCurrency(int amount)
        {
            var data = SoftCurrency;
            if (data != null)
                return data.amount >= amount;
            return false;
        }

        public static bool HaveEnoughHardCurrency(int amount)
        {
            var data = HardCurrency;
            if (data != null)
                return data.amount >= amount;
            return false;
        }

        public static string GetId(string playerId, string dataId)
        {
            return playerId + "_" + dataId;
        }

        public static void SetData(IPlayerCurrency data)
        {
            if (data == null || string.IsNullOrEmpty(data.guid))
                return;
            DataMap[data.guid] = data;
        }

        public static bool TryGetData(string playerId, string dataId, out IPlayerCurrency data)
        {
            return DataMap.TryGetValue(GetId(playerId, dataId), out data);
        }

        public static bool TryGetData(string dataId, out IPlayerCurrency data)
        {
            return TryGetData(IPlayer.CurrentPlayer.guid, dataId, out data);
        }

        public static bool RemoveData(string id)
        {
            return DataMap.Remove(id);
        }

        public static void ClearData()
        {
            DataMap.Clear();
        }

        public static void SetDataRange(IEnumerable<IPlayerCurrency> list)
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
            RemoveDataRange(IPlayer.CurrentPlayer.guid);
        }

    }
}
