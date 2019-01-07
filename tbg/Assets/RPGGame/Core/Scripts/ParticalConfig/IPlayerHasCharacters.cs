using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SQLite3TableDataTmp
{
    public partial class IPlayerHasCharacters
    {
        public static Dictionary<string, IPlayerHasCharacters> DataMap = new Dictionary<string, IPlayerHasCharacters>();

        public static void Init()
        {
            DataMap = DBManager.instance.LocalSQLite3Operate.SelectDictT_ST<IPlayerHasCharacters>();
        }
        public static void UpdataDataMap()
        {
            foreach (var dataMapValue in DataMap.Values)
            {
                DBManager.instance.LocalSQLite3Operate.UpdateOrInsert(dataMapValue);
            }
        }
        public static CalculationAttributes GetTowerExtraAttributes(bool IsBos)
        {

            CalculationAttributes extraAttributes = new CalculationAttributes();
            int playerlevel = IPlayer.CurrentPlayer.Level;
            int towerLevel = PlayerSQLPrefs.yzTowerCurrentLevel;
            int totalWeight = (playerlevel * 10 + towerLevel * 15 + 5) * 50;
            extraAttributes.SetExtraAtt(totalWeight);

            return extraAttributes;
        }

        //public static PlayerItem CreateActorItemWithLevel(ICharacter itemData, int level)
        //{
        //    if (level <= 0)
        //        level = 1;
        //    //var itemTier = itemData.itemTier;
        //    var sumExp = 0;
        //    var result = new PlayerItem(PlayerItem.ItemType.character);
        //    result.itemType = PlayerItem.ItemType.character;
        //    for (var i = 1; i < level; ++i)
        //    {
        //        //sumExp += itemTier.expTable.Calculate(i + 1, itemTier.maxLevel);
        //        sumExp += Const.NextEXP;
        //    }
        //    result.ItemID = itemData.guid;
        //    //if(!isplayer)
        //    //result.GUID = itemData.;
        //    result.Exp = sumExp;
        //    result.ExtrAttributesData = GetExtraAttributes(type);
        //    return result;
        //}
        static CalculationAttributes GetExtraAttributes(Const.StageType type)
        {
            switch (type)
            {
                case Const.StageType.Normal:
                    return null;
                    break;
                case Const.StageType.Tower:
                    return FormulaUtils.GetTowerExtraAttributes(false);
                    break;
            }
            return null;
        }

        public ICharacter Character
        {
            get
            {
                return ICharacter.DataMap[dataId];
            }
        }

        /// <summary>
        /// 获取基数属性
        /// </summary>
        /// <returns></returns>
        public Attributes GetAttributes()
        {
            Attributes result = Character.GetAttributes(level);
            return result;
        }

        public static void InsertNewCharacter(string dataId)
        {
            if (ICharacter.DataMap.ContainsKey(dataId))
            {
                string guid = Guid.NewGuid().ToString();
                DataMap.Add(guid, new IPlayerHasCharacters(DataMap.Count + 1, guid, dataId, 1, 0, "", "", 1));
                UpdataDataMap();
            }
            else
            {
                Debug.LogError("插入失败");
            }
        }

    }
}