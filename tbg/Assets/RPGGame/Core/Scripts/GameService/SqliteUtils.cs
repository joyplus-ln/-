using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.Events;

public class SqliteUtils
{

    /// <summary>
    /// 插入初始角色
    /// </summary>
    public void InsertStartPlayerCharacter(Player player)
    {
        for (var i = 0; i < GameInstance.Singleton.gameDatabase.startCharacterItems.Count; ++i)
        {
            if (!GameInstance.Singleton.gameDatabase.characters.ContainsKey(GameInstance.Singleton.gameDatabase.startCharacterItems[i]))
                continue;
            var startItem = GameInstance.Singleton.gameDatabase.characters[GameInstance.Singleton.gameDatabase.startCharacterItems[i]];

            if (true)
            {
                PlayerItem currentItem = new PlayerItem();
                currentItem.GUID = GameInstance.Singleton.gameDatabase.startCharacterItems[i];//id,@id,
                ExecuteNonQuery(@"INSERT INTO playerHasCharacters (id,playerId, Guid, amount, exp, equipItemId, equipPosition) VALUES ( @id,@playerId, @Guid, @amount, @exp, @equipItemId, @equipPosition)",
                    new SqliteParameter("@id", System.Guid.NewGuid().ToString()),
                    new SqliteParameter("@playerId", player.Id),
                    new SqliteParameter("@Guid", currentItem.GUID),
                    new SqliteParameter("@amount", currentItem.Amount),
                    new SqliteParameter("@exp", currentItem.Exp),
                    new SqliteParameter("@equipItemId", currentItem.EquipItemId),
                    new SqliteParameter("@equipPosition", currentItem.EquipPosition));
                //HelperUnlockItem(player.Id, startItem.guid);

            }
        }
    }
    /// <summary>
    /// 角色升级
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="loginToken"></param>
    /// <param name="itemId"></param>
    /// <param name="materials"></param>
    /// <param name="onFinish"></param>
    public void DoCharacterLevelUpItem(string playerId, string loginToken, string itemId, Dictionary<string, int> materials, UnityAction<ItemResult> onFinish)
    {
        var result = new ItemResult();
        var foundPlayer = GetPlayerByLoginToken(playerId, loginToken);
        var foundItem = GetPlayerCharacterItemById(itemId);
        if (foundPlayer == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (foundItem == null || foundItem.PlayerId != playerId)
            result.error = GameServiceErrorCode.INVALID_PLAYER_ITEM_DATA;
        else
        {
            var softCurrency = GetCurrency(playerId, GameInstance.GameDatabase.softCurrency.id);
            var levelUpPrice = foundItem.LevelUpPrice;
            var requireCurrency = 0;
            var increasingExp = 0;
            var updateItems = new List<PlayerItem>();
            var deleteItemIds = new Dictionary<string, PlayerItem.ItemType>();
            var materialItemIds = materials.Keys;
            var materialItems = new List<PlayerItem>();
            foreach (var materialItemId in materialItemIds)
            {
                var foundMaterial = GetPlayerCharacterItemById(materialItemId);
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
                    deleteItemIds.Add(materialItem.SqLiteIndex, PlayerItem.ItemType.character);
            }
            if (requireCurrency > softCurrency.Amount)
                result.error = GameServiceErrorCode.NOT_ENOUGH_SOFT_CURRENCY;
            else
            {
                softCurrency.Amount -= requireCurrency;
                ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                    new SqliteParameter("@amount", softCurrency.Amount),
                    new SqliteParameter("@id", softCurrency.Id));

                foundItem = foundItem.CreateLevelUpItem(increasingExp);
                updateItems.Add(foundItem);
                foreach (var updateItem in updateItems)
                {
                    ExecuteNonQuery(@"UPDATE playerHasCharacters SET playerId=@playerId, Guid=@Guid, amount=@amount, exp=@exp, equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
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
                    ExecuteNonQuery(@"DELETE FROM playerHasCharacters WHERE id=@id", new SqliteParameter("@id", deleteItemId));
                }
                result.updateCurrencies.Add(softCurrency);
                result.updateItems = updateItems;
                result.deleteItemIds = deleteItemIds;
            }
        }
        onFinish(result);
    }

    /// <summary>
    /// 装备升级
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="loginToken"></param>
    /// <param name="itemId"></param>
    /// <param name="materials"></param>
    /// <param name="onFinish"></param>
    public void DoEquipmentLevelUpItem(string playerId, string loginToken, string itemId, Dictionary<string, int> materials, UnityAction<ItemResult> onFinish)
    {
        var result = new ItemResult();
        var foundPlayer = GetPlayerByLoginToken(playerId, loginToken);
        var foundItem = GetPlayerEquipmentItemById(itemId);
        if (foundPlayer == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (foundItem == null || foundItem.PlayerId != playerId)
            result.error = GameServiceErrorCode.INVALID_PLAYER_ITEM_DATA;
        else
        {
            var softCurrency = GetCurrency(playerId, GameInstance.GameDatabase.softCurrency.id);
            var levelUpPrice = foundItem.LevelUpPrice;
            var requireCurrency = 0;
            var increasingExp = 0;
            var updateItems = new List<PlayerItem>();
            var deleteItemIds = new Dictionary<string, PlayerItem.ItemType>();
            var materialItemIds = materials.Keys;
            var materialItems = new List<PlayerItem>();
            foreach (var materialItemId in materialItemIds)
            {
                var foundMaterial = GetPlayerEquipmentItemById(materialItemId);
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
                ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                    new SqliteParameter("@amount", softCurrency.Amount),
                    new SqliteParameter("@id", softCurrency.Id));

                foundItem = foundItem.CreateLevelUpItem(increasingExp);
                updateItems.Add(foundItem);
                foreach (var updateItem in updateItems)
                {
                    ExecuteNonQuery(@"UPDATE playerHasEquips SET playerId=@playerId, Guid=@Guid, amount=@amount, exp=@exp, equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
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
                    ExecuteNonQuery(@"DELETE FROM playerHasEquips WHERE id=@id", new SqliteParameter("@id", deleteItemId));
                }
                result.updateCurrencies.Add(softCurrency);
                result.updateItems = updateItems;
                result.deleteItemIds = deleteItemIds;
            }
        }
        onFinish(result);
    }

    /// <summary>
    /// 通过id , token获取角色
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="loginToken"></param>
    /// <returns></returns>
    private Player GetPlayerByLoginToken(string playerId, string loginToken)
    {
        Player player = null;
        var players = ExecuteReader(@"SELECT * FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (players.Read())
        {
            player = new Player();
            player.Id = players.GetString(0);
            player.ProfileName = players.GetString(1);
            player.LoginToken = players.GetString(2);
            player.Exp = players.GetInt32(3);
            player.SelectedFormation = players.GetString(4);
        }
        return player;
    }

    /// <summary>
    /// 获取数据库中的角色,通过id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public PlayerItem GetPlayerCharacterItemById(string id)
    {
        PlayerItem playerItem = null;
        var playerItems = ExecuteReader(@"SELECT * FROM playerHasCharacters WHERE id=@id",
            new SqliteParameter("@id", id));
        if (playerItems.Read())
        {
            playerItem = new PlayerItem();
            playerItem.SqLiteIndex = playerItems.GetString(0);
            playerItem.PlayerId = playerItems.GetString(1);
            playerItem.GUID = playerItems.GetString(2);
            playerItem.Amount = playerItems.GetInt32(3);
            playerItem.Exp = playerItems.GetInt32(4);
            playerItem.EquipItemId = playerItems.GetString(5);
            playerItem.EquipPosition = playerItems.GetString(6);
        }
        return playerItem;
    }

    public PlayerItem GetPlayerEquipmentItemById(string id)
    {
        PlayerItem playerItem = null;
        var playerItems = ExecuteReader(@"SELECT * FROM playerHasEquips WHERE id=@id",
            new SqliteParameter("@id", id));
        if (playerItems.Read())
        {
            playerItem = new PlayerItem();
            playerItem.SqLiteIndex = playerItems.GetString(0);
            playerItem.PlayerId = playerItems.GetString(1);
            playerItem.GUID = playerItems.GetString(2);
            playerItem.Amount = playerItems.GetInt32(3);
            playerItem.Exp = playerItems.GetInt32(4);
            playerItem.EquipItemId = playerItems.GetString(5);
            playerItem.EquipPosition = playerItems.GetString(6);
        }
        return playerItem;
    }

    public void DoSellCharacterItems(string playerId, string loginToken, Dictionary<string, int> items, UnityAction<ItemResult> onFinish)
    {
        var result = new ItemResult();
        var player = GetPlayerByLoginToken(playerId, loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var softCurrency = GetCurrency(playerId, GameInstance.GameDatabase.softCurrency.id);
            var returnCurrency = 0;
            var updateItems = new List<PlayerItem>();
            var deleteItemIds = new Dictionary<string, PlayerItem.ItemType>();
            var sellingItemIds = items.Keys;
            var sellingItems = new List<PlayerItem>();
            foreach (var sellingItemId in sellingItemIds)
            {
                var foundItem = GetPlayerCharacterItemById(sellingItemId);
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
            ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                new SqliteParameter("@amount", softCurrency.Amount),
                new SqliteParameter("@id", softCurrency.Id));
            foreach (var updateItem in updateItems)
            {
                ExecuteNonQuery(@"UPDATE playerHasCharacters SET playerId=@playerId, Guid=@Guid, amount=@amount, exp=@exp, equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
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
                ExecuteNonQuery(@"DELETE FROM playerHasCharacters WHERE id=@id", new SqliteParameter("@id", deleteItemId));
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
        var player = GetPlayerByLoginToken(playerId, loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var softCurrency = GetCurrency(playerId, GameInstance.GameDatabase.softCurrency.id);
            var returnCurrency = 0;
            var updateItems = new List<PlayerItem>();
            var deleteItemIds = new Dictionary<string, PlayerItem.ItemType>();
            var sellingItemIds = items.Keys;
            var sellingItems = new List<PlayerItem>();
            foreach (var sellingItemId in sellingItemIds)
            {
                var foundItem = GetPlayerEquipmentItemById(sellingItemId);
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
            ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                new SqliteParameter("@amount", softCurrency.Amount),
                new SqliteParameter("@id", softCurrency.Id));
            foreach (var updateItem in updateItems)
            {
                ExecuteNonQuery(@"UPDATE playerHasEquips SET playerId=@playerId, Guid=@Guid, amount=@amount, exp=@exp, equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
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
                ExecuteNonQuery(@"DELETE FROM playerHasEquips WHERE id=@id", new SqliteParameter("@id", deleteItemId));
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
    public void DoGetItemList(string playerId, string loginToken, UnityAction<ItemListResult> onFinish)
    {
        var result = new ItemListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = ExecuteReader(@"SELECT * FROM playerHasCharacters WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
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


            var equipmentreader = ExecuteReader(@"SELECT * FROM playerHasEquips WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
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




    /// <summary>
    /// 获取金币
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="dataId"></param>
    /// <returns></returns>
    private PlayerCurrency GetCurrency(string playerId, string dataId)
    {
        var currencies = ExecuteReader(@"SELECT * FROM playerCurrency WHERE playerId=@playerId AND Guid=@Guid LIMIT 1",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@Guid", dataId));
        PlayerCurrency currency = null;
        if (!currencies.Read())
        {
            currency = new PlayerCurrency();
            currency.Id = PlayerCurrency.GetId(playerId, dataId);
            currency.PlayerId = playerId;
            currency.DataId = dataId;
            ExecuteNonQuery(@"INSERT INTO playerCurrency (id, playerId, Guid, amount, purchasedAmount) VALUES (@id, @playerId, @Guid, @amount, @purchasedAmount)",
                new SqliteParameter("@id", currency.Id),
                new SqliteParameter("@playerId", currency.PlayerId),
                new SqliteParameter("@Guid", currency.DataId),
                new SqliteParameter("@amount", currency.Amount),
                new SqliteParameter("@purchasedAmount", currency.PurchasedAmount));
        }
        else
        {
            currency = new PlayerCurrency();
            currency.Id = currencies.GetString(0);
            currency.PlayerId = currencies.GetString(1);
            currency.DataId = currencies.GetString(2);
            currency.Amount = currencies.GetInt32(3);
            currency.PurchasedAmount = currencies.GetInt32(4);
        }
        return currency;
    }


    #region MyRegion

    public void ExecuteNonQuery(string sql, params SqliteParameter[] args)
    {
        SQLiteGameService.connection.Open();
        using (var cmd = new SqliteCommand(sql, SQLiteGameService.connection))
        {
            foreach (var arg in args)
            {
                cmd.Parameters.Add(arg);
            }
            cmd.ExecuteNonQuery();
        }
        SQLiteGameService.connection.Close();
    }

    public object ExecuteScalar(string sql, params SqliteParameter[] args)
    {
        object result;
        SQLiteGameService.connection.Open();
        using (var cmd = new SqliteCommand(sql, SQLiteGameService.connection))
        {
            foreach (var arg in args)
            {
                cmd.Parameters.Add(arg);
            }
            result = cmd.ExecuteScalar();
        }
        SQLiteGameService.connection.Close();
        return result;
    }

    public DbRowsReader ExecuteReader(string sql, params SqliteParameter[] args)
    {
        DbRowsReader result = new DbRowsReader();
        SQLiteGameService.connection.Open();
        using (var cmd = new SqliteCommand(sql, SQLiteGameService.connection))
        {
            foreach (var arg in args)
            {
                cmd.Parameters.Add(arg);
            }
            result.Init(cmd.ExecuteReader());
        }
        SQLiteGameService.connection.Close();
        return result;
    }

    #endregion


}
