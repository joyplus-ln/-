using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.Events;

public class DBMapItem
{


    public void Init()
    {
        GameInstance.GameDatabase.characters = GetSqliteCharacters();
        GameInstance.GameDatabase.equipments = GetSqliteEquipments();
    }





    public Dictionary<string, CharacterItem> GetSqliteCharacters()
    {
        var reader = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM Character");
        var list = new Dictionary<string, CharacterItem>();
        CharacterItem item = null;
        while (reader.Read())
        {
            item = new CharacterItem();
            item.SqliteId = reader.GetInt32(0);
            item.guid = reader.GetString(1);
            item.title = reader.GetString(2);
            item.description = reader.GetString(3);
            item.region = reader.GetString(4);
            item.quality = reader.GetString(5);
            item.category = reader.GetString(6);
            item.attributes = new Attributes();
            item.attributes.hp.minValue = reader.GetInt32(7);
            item.attributes.hp.maxValue = reader.GetInt32(8);
            item.attributes.hp.growth = reader.GetInt32(9);

            item.attributes.pAtk.minValue = reader.GetInt32(10);
            item.attributes.pAtk.maxValue = reader.GetInt32(11);
            item.attributes.pAtk.growth = reader.GetInt32(12);

            item.attributes.pDef.minValue = reader.GetInt32(13);
            item.attributes.pDef.maxValue = reader.GetInt32(14);
            item.attributes.pDef.growth = reader.GetInt32(15);

            item.attributes.mAtk.minValue = reader.GetInt32(16);
            item.attributes.mAtk.maxValue = reader.GetInt32(17);
            item.attributes.mAtk.growth = reader.GetInt32(18);

            item.attributes.mDef.minValue = reader.GetInt32(19);
            item.attributes.mDef.maxValue = reader.GetInt32(20);
            item.attributes.mDef.growth = reader.GetInt32(21);

            item.attributes.spd.minValue = reader.GetInt32(22);
            item.attributes.spd.maxValue = reader.GetInt32(23);
            item.attributes.spd.growth = reader.GetInt32(24);

            item.attributes.eva.minValue = reader.GetInt32(25);
            item.attributes.eva.maxValue = reader.GetInt32(26);
            item.attributes.eva.growth = reader.GetInt32(27);

            item.attributes.acc.minValue = reader.GetInt32(28);
            item.attributes.acc.maxValue = reader.GetInt32(29);
            item.attributes.acc.growth = reader.GetInt32(30);

            item.customSkill = reader.GetString(31);
            list.Add(item.guid, item);
        }
        return list;
    }

    public Dictionary<string, EquipmentItem> GetSqliteEquipments()
    {
        var reader = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM Equipment");
        var list = new Dictionary<string, EquipmentItem>();
        EquipmentItem item = null;
        while (reader.Read())
        {
            item = new EquipmentItem();
            item.SqliteId = reader.GetInt32(0);
            item.guid = reader.GetString(1);
            item.title = reader.GetString(2);
            item.description = reader.GetString(3);
            item.region = reader.GetString(4);
            item.quality = reader.GetString(5);
            item.category = reader.GetString(6);
            item.attributes = new Attributes();
            item.attributes.hp.minValue = reader.GetInt32(7);
            item.attributes.hp.maxValue = reader.GetInt32(8);
            item.attributes.hp.growth = reader.GetInt32(9);

            item.attributes.pAtk.minValue = reader.GetInt32(10);
            item.attributes.pAtk.maxValue = reader.GetInt32(11);
            item.attributes.pAtk.growth = reader.GetInt32(12);

            item.attributes.pDef.minValue = reader.GetInt32(13);
            item.attributes.pDef.maxValue = reader.GetInt32(14);
            item.attributes.pDef.growth = reader.GetInt32(15);

            item.attributes.mAtk.minValue = reader.GetInt32(16);
            item.attributes.mAtk.maxValue = reader.GetInt32(17);
            item.attributes.mAtk.growth = reader.GetInt32(18);

            item.attributes.mDef.minValue = reader.GetInt32(19);
            item.attributes.mDef.maxValue = reader.GetInt32(20);
            item.attributes.mDef.growth = reader.GetInt32(21);

            item.attributes.spd.minValue = reader.GetInt32(22);
            item.attributes.spd.maxValue = reader.GetInt32(23);
            item.attributes.spd.growth = reader.GetInt32(24);

            item.attributes.eva.minValue = reader.GetInt32(25);
            item.attributes.eva.maxValue = reader.GetInt32(26);
            item.attributes.eva.growth = reader.GetInt32(27);

            item.attributes.acc.minValue = reader.GetInt32(28);
            item.attributes.acc.maxValue = reader.GetInt32(29);
            item.attributes.acc.growth = reader.GetInt32(30);

            item.extraAttributes = new CalculationAttributes();

            item.equippablePositions = reader.GetString(31).Split(',').ToList();
            item.extraAttributes.hp = reader.GetFloat(32);
            item.extraAttributes.pAtk = reader.GetFloat(33);
            item.extraAttributes.pDef = reader.GetFloat(34);
            item.extraAttributes.mAtk = reader.GetFloat(35);
            item.extraAttributes.mDef = reader.GetFloat(36);
            item.extraAttributes.spd = reader.GetFloat(37);
            item.extraAttributes.eva = reader.GetFloat(38);
            item.extraAttributes.acc = reader.GetFloat(39);
            item.extraAttributes.hpRate = reader.GetFloat(40);
            item.extraAttributes.pAtkRate = reader.GetFloat(42);
            item.extraAttributes.pDefRate = reader.GetFloat(42);
            item.extraAttributes.mAtkRate = reader.GetFloat(43);
            item.extraAttributes.mDefRate = reader.GetFloat(44);
            item.extraAttributes.spdRate = reader.GetFloat(45);
            item.extraAttributes.evaRate = reader.GetFloat(46);
            item.extraAttributes.accRate = reader.GetFloat(47);
            item.extraAttributes.critChance = reader.GetFloat(48);
            item.extraAttributes.critDamageRate = reader.GetFloat(49);
            item.extraAttributes.blockChance = reader.GetFloat(50);
            item.extraAttributes.blockDamageRate = reader.GetFloat(51);

            list.Add(item.guid, item);
        }
        return list;
    }














    #region 其他东西

    public void AddOtherItem(string id, int amount)//Guid
    {
        GameInstance.SqliteUtils.ExecuteNonQuery(@"INSERT INTO playerOtherItem (id,playerId,Guid,amount) VALUES (@id,@playerId,@Guid,@amount)",
            new SqliteParameter("@id", id),
            new SqliteParameter("@playerId", Player.CurrentPlayerId),
            new SqliteParameter("@Guid", "otherItem" + System.Guid.NewGuid()),
            new SqliteParameter("@amount", amount));

    }

    public void DpdateOtherItem(string id, int amount)
    {
        GameInstance.SqliteUtils.ExecuteNonQuery(@"UPDATE playerOtherItem SET amount=@amount WHERE id=@id AND playerId=@playerId",
            new SqliteParameter("@amount", amount),
            new SqliteParameter("@playerId", Player.CurrentPlayerId),
            new SqliteParameter("@id", id));
    }

    public void DeleteOtherItem(string id)
    {
        GameInstance.SqliteUtils.ExecuteNonQuery(@"DELETE FROM playerOtherItem WHERE id=@id",
            new SqliteParameter("@id", id),
            new SqliteParameter("@playerId", Player.CurrentPlayerId));
    }

    #endregion



    /// <summary>
    /// 装备升级
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="loginToken"></param>
    /// <param name="itemId"></param>
    /// <param name="materials"></param>
    /// <param name="onFinish"></param>
    public void DoEquipmentLevelUpItem(string itemId, Dictionary<string, int> materials, UnityAction<ItemResult> onFinish)
    {
        var player = Player.CurrentPlayer;
        var playerId = player.Id;
        var loginToken = player.LoginToken;
        var result = new ItemResult();
        var foundPlayer = GameInstance.dbLogin.GetPlayerByLoginToken(playerId, loginToken);
        var foundItem = GameInstance.dbPlayerData.GetPlayerEquipmentItemById(itemId);
        if (foundPlayer == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (foundItem == null || foundItem.PlayerId != playerId)
            result.error = GameServiceErrorCode.INVALID_PLAYER_ITEM_DATA;
        else
        {
            var softCurrency = GameInstance.dbPlayerData.GetCurrency(playerId, GameInstance.GameDatabase.softCurrency.id);
            var levelUpPrice = foundItem.LevelUpPrice;
            var requireCurrency = 0;
            var increasingExp = 0;
            var updateItems = new List<PlayerItem>();
            var deleteItemIds = new Dictionary<string, PlayerItem.ItemType>();
            var materialItemIds = materials.Keys;
            var materialItems = new List<PlayerItem>();
            foreach (var materialItemId in materialItemIds)
            {
                var foundMaterial = GameInstance.dbPlayerData.GetPlayerEquipmentItemById(materialItemId);
                if (foundMaterial == null || foundMaterial.PlayerId != playerId)
                    continue;

                if (foundMaterial.CanBeMaterial)
                    materialItems.Add(foundMaterial);
            }
            foreach (var materialItem in materialItems)
            {
                var usingAmount = materials[materialItem.SqLiteIndex];
                if (usingAmount > materialItem.Amount)
                    usingAmount = materialItem.Amount;
                requireCurrency += levelUpPrice * usingAmount;
                increasingExp += materialItem.RewardExp * usingAmount;
                materialItem.Amount -= usingAmount;
                if (materialItem.Amount > 0)
                    updateItems.Add(materialItem);
                else
                    deleteItemIds.Add(materialItem.SqLiteIndex, PlayerItem.ItemType.equip);
            }
            if (requireCurrency > softCurrency.Amount)
                result.error = GameServiceErrorCode.NOT_ENOUGH_SOFT_CURRENCY;
            else
            {
                softCurrency.Amount -= requireCurrency;
                GameInstance.SqliteUtils.ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                    new SqliteParameter("@amount", softCurrency.Amount),
                    new SqliteParameter("@id", softCurrency.Id));

                foundItem = foundItem.CreateLevelUpItem(increasingExp);
                updateItems.Add(foundItem);
                foreach (var updateItem in updateItems)
                {
                    GameInstance.SqliteUtils.ExecuteNonQuery(@"UPDATE playerHasEquips SET playerId=@playerId, Guid=@Guid, amount=@amount, exp=@exp, equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
                        new SqliteParameter("@playerId", updateItem.PlayerId),
                        new SqliteParameter("@Guid", updateItem.GUID),
                        new SqliteParameter("@amount", updateItem.Amount),
                        new SqliteParameter("@exp", updateItem.Exp),
                        new SqliteParameter("@equipItemId", updateItem.EquipItemId),
                        new SqliteParameter("@equipPosition", updateItem.EquipPosition),
                        new SqliteParameter("@id", updateItem.SqLiteIndex));
                }
                foreach (var deleteItemId in deleteItemIds)
                {
                    GameInstance.SqliteUtils.ExecuteNonQuery(@"DELETE FROM playerHasEquips WHERE id=@id", new SqliteParameter("@id", deleteItemId));
                }
                result.updateCurrencies.Add(softCurrency);
                result.updateItems = updateItems;
                result.deleteItemIds = deleteItemIds;
            }
        }
        onFinish(result);
    }

    public void DoSellCharacterItems(Dictionary<string, int> items, UnityAction<ItemResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new ItemResult();
        var player = GameInstance.dbLogin.GetPlayerByLoginToken(playerId, loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var softCurrency = GameInstance.dbPlayerData.GetCurrency(playerId, GameInstance.GameDatabase.softCurrency.id);
            var returnCurrency = 0;
            var updateItems = new List<PlayerItem>();
            var deleteItemIds = new Dictionary<string, PlayerItem.ItemType>();
            var sellingItemIds = items.Keys;
            var sellingItems = new List<PlayerItem>();
            foreach (var sellingItemId in sellingItemIds)
            {
                var foundItem = GameInstance.dbPlayerData.GetPlayerCharacterItemById(sellingItemId);
                if (foundItem == null || foundItem.PlayerId != playerId)
                    continue;

                if (foundItem.CanSell)
                    sellingItems.Add(foundItem);
            }
            foreach (var sellingItem in sellingItems)
            {
                var usingAmount = items[sellingItem.SqLiteIndex];
                if (usingAmount > sellingItem.Amount)
                    usingAmount = sellingItem.Amount;
                returnCurrency += sellingItem.SellPrice * usingAmount;
                sellingItem.Amount -= usingAmount;
                if (sellingItem.Amount > 0)
                    updateItems.Add(sellingItem);
                else
                    deleteItemIds.Add(sellingItem.SqLiteIndex, PlayerItem.ItemType.character);
            }
            softCurrency.Amount += returnCurrency;
            GameInstance.SqliteUtils.ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                new SqliteParameter("@amount", softCurrency.Amount),
                new SqliteParameter("@id", softCurrency.Id));
            foreach (var updateItem in updateItems)
            {
                GameInstance.SqliteUtils.ExecuteNonQuery(@"UPDATE playerHasCharacters SET playerId=@playerId, Guid=@Guid, amount=@amount, exp=@exp, equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
                    new SqliteParameter("@playerId", updateItem.PlayerId),
                    new SqliteParameter("@Guid", updateItem.GUID),
                    new SqliteParameter("@amount", updateItem.Amount),
                    new SqliteParameter("@exp", updateItem.Exp),
                    new SqliteParameter("@equipItemId", updateItem.EquipItemId),
                    new SqliteParameter("@equipPosition", updateItem.EquipPosition),
                    new SqliteParameter("@id", updateItem.SqLiteIndex));
            }
            foreach (var deleteItemId in deleteItemIds)
            {
                GameInstance.SqliteUtils.ExecuteNonQuery(@"DELETE FROM playerHasCharacters WHERE id=@id", new SqliteParameter("@id", deleteItemId));
            }
            result.updateCurrencies.Add(softCurrency);
            result.updateItems = updateItems;
            result.deleteItemIds = deleteItemIds;
        }
        onFinish(result);
    }

    public void DoSellEquipmentItems(string playerId, string loginToken, Dictionary<string, int> items, UnityAction<ItemResult> onFinish)
    {
        var result = new ItemResult();
        var player = GameInstance.dbLogin.GetPlayerByLoginToken(playerId, loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var softCurrency = GameInstance.dbPlayerData.GetCurrency(playerId, GameInstance.GameDatabase.softCurrency.id);
            var returnCurrency = 0;
            var updateItems = new List<PlayerItem>();
            var deleteItemIds = new Dictionary<string, PlayerItem.ItemType>();
            var sellingItemIds = items.Keys;
            var sellingItems = new List<PlayerItem>();
            foreach (var sellingItemId in sellingItemIds)
            {
                var foundItem = GameInstance.dbPlayerData.GetPlayerEquipmentItemById(sellingItemId);
                if (foundItem == null || foundItem.PlayerId != playerId)
                    continue;

                if (foundItem.CanSell)
                    sellingItems.Add(foundItem);
            }
            foreach (var sellingItem in sellingItems)
            {
                var usingAmount = items[sellingItem.SqLiteIndex];
                if (usingAmount > sellingItem.Amount)
                    usingAmount = sellingItem.Amount;
                returnCurrency += sellingItem.SellPrice * usingAmount;
                sellingItem.Amount -= usingAmount;
                if (sellingItem.Amount > 0)
                    updateItems.Add(sellingItem);
                else
                    deleteItemIds.Add(sellingItem.SqLiteIndex, PlayerItem.ItemType.character);
            }
            softCurrency.Amount += returnCurrency;
            GameInstance.SqliteUtils.ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                new SqliteParameter("@amount", softCurrency.Amount),
                new SqliteParameter("@id", softCurrency.Id));
            foreach (var updateItem in updateItems)
            {
                GameInstance.SqliteUtils.ExecuteNonQuery(@"UPDATE playerHasEquips SET playerId=@playerId, Guid=@Guid, amount=@amount, exp=@exp, equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
                    new SqliteParameter("@playerId", updateItem.PlayerId),
                    new SqliteParameter("@Guid", updateItem.GUID),
                    new SqliteParameter("@amount", updateItem.Amount),
                    new SqliteParameter("@exp", updateItem.Exp),
                    new SqliteParameter("@equipItemId", updateItem.EquipItemId),
                    new SqliteParameter("@equipPosition", updateItem.EquipPosition),
                    new SqliteParameter("@id", updateItem.SqLiteIndex));
            }
            foreach (var deleteItemId in deleteItemIds)
            {
                GameInstance.SqliteUtils.ExecuteNonQuery(@"DELETE FROM playerHasEquips WHERE id=@id", new SqliteParameter("@id", deleteItemId));
            }
            result.updateCurrencies.Add(softCurrency);
            result.updateItems = updateItems;
            result.deleteItemIds = deleteItemIds;
        }
        onFinish(result);
    }

    /// <summary>
    /// 获取所有item
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="loginToken"></param>
    /// <param name="onFinish"></param>
    public void DoGetItemList(UnityAction<ItemListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new ItemListResult();
        var player = GameInstance.SqliteUtils.ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM playerHasCharacters WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
            var list = new List<PlayerItem>();
            while (reader.Read())
            {
                var entry = new PlayerItem();
                entry.itemType = PlayerItem.ItemType.character;
                entry.SqLiteIndex = reader.GetString(0);
                entry.PlayerId = reader.GetString(1);
                entry.GUID = reader.GetString(2);
                entry.Amount = reader.GetInt32(3);
                entry.Exp = reader.GetInt32(4);
                entry.EquipItemId = reader.GetString(5);
                entry.EquipPosition = reader.GetString(6);
                list.Add(entry);
            }


            var equipmentreader = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM playerHasEquips WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
            var equipmentlist = new List<PlayerItem>();
            while (equipmentreader.Read())
            {
                var entry = new PlayerItem();
                entry.itemType = PlayerItem.ItemType.equip;
                entry.SqLiteIndex = equipmentreader.GetString(0);
                entry.PlayerId = equipmentreader.GetString(1);
                entry.GUID = equipmentreader.GetString(2);
                entry.Amount = equipmentreader.GetInt32(3);
                entry.Exp = equipmentreader.GetInt32(4);
                entry.EquipItemId = equipmentreader.GetString(5);
                entry.EquipPosition = equipmentreader.GetString(6);
                equipmentlist.Add(entry);
            }
            result.characterlist = list;
            result.equipmentlist = equipmentlist;
        }
        onFinish(result);
    }

    private bool UseItems(string playerId,
    string dataId,
    int amount,
    out List<PlayerItem> updateItem,
    out List<string> deleteItemIds,
    bool conditionCanLevelUp = false,
    bool conditionCanEvolve = false,
    bool conditionCanSell = false,
    bool conditionCanBeMaterial = false,
    bool conditionCanBeEquipped = false)
    {
        updateItem = new List<PlayerItem>();
        deleteItemIds = new List<string>();
        if (!GameInstance.GameDatabase.characters.ContainsKey(dataId))
            return false;
        var materials = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM playerItem WHERE playerId=@playerId AND Guid=@Guid",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@Guid", dataId));
        while (materials.Read())
        {
            var material = new PlayerItem();
            material.SqLiteIndex = materials.GetInt32(0).ToString();
            material.PlayerId = materials.GetString(1);
            material.GUID = materials.GetString(2);
            material.Amount = materials.GetInt32(3);
            material.Exp = materials.GetInt32(4);
            material.EquipItemId = materials.GetString(5);
            material.EquipPosition = materials.GetString(6);

            if ((!conditionCanLevelUp || material.CanLevelUp) &&
                (!conditionCanEvolve || false) &&  //(!conditionCanEvolve || material.CanEvolve) &&
                (!conditionCanSell || material.CanSell) &&
                (!conditionCanBeMaterial || material.CanBeMaterial) &&
                (!conditionCanBeEquipped || material.CanBeEquipped))
            {
                if (material.Amount >= amount)
                {
                    material.Amount -= amount;
                    amount = 0;
                }
                else
                {
                    amount -= material.Amount;
                    material.Amount = 0;
                }

                if (material.Amount > 0)
                    updateItem.Add(material);
                else
                    deleteItemIds.Add(material.SqLiteIndex);

                if (amount == 0)
                    break;
            }
        }
        if (amount > 0)
            return false;
        return true;
    }

    public void DoGetOtherItemList(UnityAction<OtherItemListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new OtherItemListResult();
        var player = GameInstance.SqliteUtils.ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM playerOtherItem WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
            var list = new List<PlayerOtherItem>();
            while (reader.Read())
            {
                var entry = new PlayerOtherItem();
                entry.SqLiteIndex = reader.GetString(0);
                entry.DataId = reader.GetString(1);
                entry.PlayerId = reader.GetString(2);
                entry.Amount = reader.GetInt32(3);
                list.Add(entry);
            }
            result.list = list;
        }
        onFinish(result);
    }
}
