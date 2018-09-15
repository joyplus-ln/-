using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class DBPlayerData {


    public void DoGetCurrencyList(UnityAction<CurrencyListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new CurrencyListResult();
        var player = GameInstance.SqliteUtils.ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM playerCurrency WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
            var list = new List<PlayerCurrency>();
            while (reader.Read())
            {
                var entry = new PlayerCurrency();
                entry.Id = reader.GetString(0);
                entry.PlayerId = reader.GetString(1);
                entry.DataId = reader.GetString(2);
                entry.Amount = reader.GetInt32(3);
                entry.PurchasedAmount = reader.GetInt32(4);
                list.Add(entry);
            }
            result.list = list;
        }
        onFinish(result);
    }

    public void DoGetStaminaList(UnityAction<StaminaListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new StaminaListResult();
        var player = GameInstance.SqliteUtils.ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM playerStamina WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
            var list = new List<PlayerStamina>();
            while (reader.Read())
            {
                var entry = new PlayerStamina();
                entry.Id = reader.GetString(0);
                entry.PlayerId = reader.GetString(1);
                entry.DataId = reader.GetString(2);
                entry.Amount = reader.GetInt32(3);
                entry.RecoveredTime = reader.GetInt64(4);
                list.Add(entry);
            }
            result.list = list;
        }
        onFinish(result);
    }


    /// <summary>
    /// 取用户存档
    /// </summary>
    public void GetPlayerLocalInfo()
    {
        var reader = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM player WHERE id=@playerId", new SqliteParameter("@playerId", Player.CurrentPlayer.Id));
        var list = new List<PlayerItem>();
        string localConfig = "";
        while (reader.Read())
        {
            localConfig = reader.GetString(5);
        }
        PlayerSQLPrefs.localConfig = JsonConvert.DeserializeObject<LocalConfig>(localConfig);
        PlayerSQLPrefs.saveAction += SavePlayerLocalInfo;
    }
    /// <summary>
    /// 保存用户存档
    /// </summary>
    public void SavePlayerLocalInfo()
    {
        GameInstance.SqliteUtils.ExecuteNonQuery(@"UPDATE player SET prefs=@prefs WHERE id=@id",
            new SqliteParameter("@prefs", JsonConvert.SerializeObject(PlayerSQLPrefs.localConfig)),
            new SqliteParameter("@id", Player.CurrentPlayerId));
    }




    public bool DecreasePlayerStamina(Player player, Stamina staminaTable, int decreaseAmount)
    {
        var stamina = GetStamina(player.Id, staminaTable.id);
        if (stamina.Amount >= decreaseAmount)
        {
            stamina.Amount -= decreaseAmount;
            GameInstance.SqliteUtils.ExecuteNonQuery(@"UPDATE playerStamina SET amount=@amount WHERE id=@id",
                new SqliteParameter("@amount", stamina.Amount),
                new SqliteParameter("@id", stamina.Id));
            UpdatePlayerStamina(player, staminaTable);
            return true;
        }
        return false;
    }

    private void UpdatePlayerStamina(Player player, Stamina staminaTable)
    {
        var gameDb = GameInstance.GameDatabase;

        var stamina = GetStamina(player.Id, staminaTable.id);
        var maxStamina = staminaTable.maxAmountTable.Calculate(player.Level, gameDb.playerMaxLevel);
        if (stamina.Amount < maxStamina)
        {
            var currentTimeInMillisecond = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond;
            var diffTimeInMillisecond = currentTimeInMillisecond - stamina.RecoveredTime;
            var devideAmount = 1;
            switch (staminaTable.recoverUnit)
            {
                case StaminaUnit.Days:
                    devideAmount = 1000 * 60 * 60 * 24;
                    break;
                case StaminaUnit.Hours:
                    devideAmount = 1000 * 60 * 60;
                    break;
                case StaminaUnit.Minutes:
                    devideAmount = 1000 * 60;
                    break;
                case StaminaUnit.Seconds:
                    devideAmount = 1000;
                    break;
            }
            var recoveryAmount = (int)(diffTimeInMillisecond / devideAmount) / staminaTable.recoverDuration;
            stamina.Amount += recoveryAmount;
            if (stamina.Amount > maxStamina)
                stamina.Amount = maxStamina;
            stamina.RecoveredTime = currentTimeInMillisecond;
            GameInstance.SqliteUtils.ExecuteNonQuery(@"UPDATE playerStamina SET amount=@amount, recoveredTime=@recoveredTime WHERE id=@id",
                new SqliteParameter("@amount", stamina.Amount),
                new SqliteParameter("@recoveredTime", stamina.RecoveredTime),
                new SqliteParameter("@id", stamina.Id));
        }
    }

    public void UpdatePlayerStamina(Player player)
    {
        var gameDb = GameInstance.GameDatabase;
        var stageStaminaTable = gameDb.stageStamina;
        UpdatePlayerStamina(player, stageStaminaTable);
    }


    public PlayerStamina GetStamina(string playerId, string dataId)
    {
        var staminas = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM playerStamina WHERE playerId=@playerId AND Guid=@Guid LIMIT 1",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@Guid", dataId));
        PlayerStamina stamina = null;
        if (!staminas.Read())
        {
            stamina = new PlayerStamina();
            stamina.Id = PlayerStamina.GetId(playerId, dataId);
            stamina.PlayerId = playerId;
            stamina.DataId = dataId;
            GameInstance.SqliteUtils.ExecuteNonQuery(@"INSERT INTO playerStamina (id, playerId, Guid, amount, recoveredTime) VALUES (@id, @playerId, @Guid, @amount, @recoveredTime)",
                new SqliteParameter("@id", stamina.Id),
                new SqliteParameter("@playerId", stamina.PlayerId),
                new SqliteParameter("@Guid", stamina.DataId),
                new SqliteParameter("@amount", stamina.Amount),
                new SqliteParameter("@recoveredTime", stamina.RecoveredTime));
        }
        else
        {
            stamina = new PlayerStamina();
            stamina.Id = staminas.GetString(0);
            stamina.PlayerId = staminas.GetString(1);
            stamina.DataId = staminas.GetString(2);
            stamina.Amount = staminas.GetInt32(3);
            stamina.RecoveredTime = staminas.GetInt64(4);
        }
        return stamina;
    }


    #region utils
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
                GameInstance.SqliteUtils.ExecuteNonQuery(@"INSERT INTO playerHasCharacters (id,playerId, Guid, amount, exp, equipItemId, equipPosition) VALUES ( @id,@playerId, @Guid, @amount, @exp, @equipItemId, @equipPosition)",
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
    public void DoCharacterLevelUpItem(string itemId, Dictionary<string, int> materials, UnityAction<ItemResult> onFinish)
    {
        var player = Player.CurrentPlayer;
        var playerId = player.Id;
        var loginToken = player.LoginToken;
        var result = new ItemResult();
        var foundPlayer = GameInstance.dbLogin.GetPlayerByLoginToken(playerId, loginToken);
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
                GameInstance.SqliteUtils.ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                    new SqliteParameter("@amount", softCurrency.Amount),
                    new SqliteParameter("@id", softCurrency.Id));

                foundItem = foundItem.CreateLevelUpItem(increasingExp);
                updateItems.Add(foundItem);
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
        }
        onFinish(result);
    }


    /// <summary>
    /// 获取数据库中的角色,通过id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public PlayerItem GetPlayerCharacterItemById(string id)
    {
        PlayerItem playerItem = null;
        var playerItems = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM playerHasCharacters WHERE id=@id",
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
        var playerItems = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM playerHasEquips WHERE id=@id",
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

    /// <summary>
    /// 获取金币
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="dataId"></param>
    /// <returns></returns>
    public PlayerCurrency GetCurrency(string playerId, string dataId)
    {
        var currencies = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM playerCurrency WHERE playerId=@playerId AND Guid=@Guid LIMIT 1",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@Guid", dataId));
        PlayerCurrency currency = null;
        if (!currencies.Read())
        {
            currency = new PlayerCurrency();
            currency.Id = PlayerCurrency.GetId(playerId, dataId);
            currency.PlayerId = playerId;
            currency.DataId = dataId;
            GameInstance.SqliteUtils.ExecuteNonQuery(@"INSERT INTO playerCurrency (id, playerId, Guid, amount, purchasedAmount) VALUES (@id, @playerId, @Guid, @amount, @purchasedAmount)",
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


    public PlayerItem GetPlayerItemByEquipper(string playerId, string equipItemId, string equipPosition)
    {
        PlayerItem playerItem = null;
        var playerItems = GameInstance.SqliteUtils.ExecuteReader(@"SELECT * FROM playerHasEquips WHERE playerId=@playerId AND equipItemId=@equipItemId AND equipPosition=@equipPosition",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@equipItemId", equipItemId),
            new SqliteParameter("@equipPosition", equipPosition));
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
#endregion
}
