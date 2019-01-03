using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class DBPlayerData
{


    public void DoGetCurrencyList(UnityAction<CurrencyListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new CurrencyListResult();
        var player = GameInstance.dbDataUtils.ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerCurrency WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
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
        var player = GameInstance.dbDataUtils.ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerStamina WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
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
        var reader = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM player WHERE id=@playerId", new SqliteParameter("@playerId", Player.CurrentPlayer.Id));
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
        GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE player SET prefs=@prefs WHERE id=@id",
            new SqliteParameter("@prefs", JsonConvert.SerializeObject(PlayerSQLPrefs.localConfig)),
            new SqliteParameter("@id", Player.CurrentPlayerId));
    }




    public bool DecreasePlayerStamina(Player player, Stamina staminaTable, int decreaseAmount)
    {
        var stamina = GetStamina(player.Id, staminaTable.id);
        if (stamina.Amount >= decreaseAmount)
        {
            stamina.Amount -= decreaseAmount;
            GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerStamina SET amount=@amount WHERE id=@id",
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
            GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerStamina SET amount=@amount, recoveredTime=@recoveredTime WHERE id=@id",
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
        var staminas = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerStamina WHERE playerId=@playerId AND Guid=@Guid LIMIT 1",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@Guid", dataId));
        PlayerStamina stamina = null;
        if (!staminas.Read())
        {
            stamina = new PlayerStamina();
            stamina.Id = PlayerStamina.GetId(playerId, dataId);
            stamina.PlayerId = playerId;
            stamina.DataId = dataId;
            GameInstance.dbDataUtils.ExecuteNonQuery(@"INSERT INTO playerStamina (id, playerId, Guid, amount, recoveredTime) VALUES (@id, @playerId, @Guid, @amount, @recoveredTime)",
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
            if (!DBManager.instance.GetConfigCharacters().ContainsKey(GameInstance.Singleton.gameDatabase.startCharacterItems[i]))
                continue;
            var startItem = DBManager.instance.GetConfigCharacters()[GameInstance.Singleton.gameDatabase.startCharacterItems[i]];

            if (true)
            {
                PlayerItem currentItem = new PlayerItem(PlayerItem.ItemType.character);
                currentItem.ItemID = GameInstance.Singleton.gameDatabase.startCharacterItems[i];//id,@id,
                GameInstance.dbDataUtils.ExecuteNonQuery(@"INSERT INTO playerHasCharacters (itemid,playerId, Guid, amount, exp) VALUES ( @itemid,@playerId, @Guid, @amount, @exp)",
                    new SqliteParameter("@itemid", currentItem.ItemID),
                    new SqliteParameter("@playerId", player.Id),
                    new SqliteParameter("@Guid", System.Guid.NewGuid().ToString()),
                    new SqliteParameter("@amount", currentItem.Amount),
                    new SqliteParameter("@exp", currentItem.Exp)
                    );
                //HelperUnlockItem(player.characterGuid, startItem.guid);

            }
        }
    }

    /// <summary>
    /// 增加角色
    /// </summary>
    public void InsertCharacter(string itemid)
    {
        {
            if (!DBManager.instance.GetConfigCharacters().ContainsKey(itemid)) return;

            {
                PlayerItem currentItem = new PlayerItem(PlayerItem.ItemType.character);
                currentItem.ItemID = itemid;//id,@id,
                currentItem.GUID = System.Guid.NewGuid().ToString();
                GameInstance.dbDataUtils.ExecuteNonQuery(@"INSERT INTO playerHasCharacters (itemid,playerId, Guid, amount, exp) VALUES ( @itemid,@playerId, @Guid, @amount, @exp)",
                    new SqliteParameter("@itemid", currentItem.ItemID),
                    new SqliteParameter("@playerId", Player.CurrentPlayer.Id),
                    new SqliteParameter("@Guid", currentItem.GUID),
                    new SqliteParameter("@amount", currentItem.Amount),
                    new SqliteParameter("@exp", currentItem.Exp)
                    );
                var result = new ItemResult();
                result.createItems = new List<PlayerItem>();
                result.createItems.Add(currentItem);
                //PlayerItem.SetDataRange(result.createItems);
                //HelperUnlockItem(player.characterGuid, startItem.guid);

            }
        }
    }

    /// <summary>
    /// 插入初始otheritem
    /// </summary>
    public void InsertStartEquiptem(Player player)
    {
        for (var i = 0; i < GameInstance.Singleton.gameDatabase.startEquipsItems.Count; ++i)
        {
            if (!GameInstance.Singleton.gameDatabase.equipments.ContainsKey(GameInstance.Singleton.gameDatabase.startEquipsItems[i]))
                continue;
            var startItem = GameInstance.Singleton.gameDatabase.equipments[GameInstance.Singleton.gameDatabase.startEquipsItems[i]];

            if (true)
            {
                PlayerItem currentItem = new PlayerItem(PlayerItem.ItemType.equip);
                currentItem.ItemID = GameInstance.Singleton.gameDatabase.startEquipsItems[i];//id,@id,
                GameInstance.dbDataUtils.ExecuteNonQuery(@"INSERT INTO playerHasEquips (itemid,playerId, Guid, amount, exp, equipItemGuid, equipPosition) VALUES ( @itemid,@playerId, @Guid, @amount, @exp, @equipItemGuid, @equipPosition)",
                    new SqliteParameter("@itemid", currentItem.ItemID),
                    new SqliteParameter("@playerId", player.Id),
                    new SqliteParameter("@Guid", System.Guid.NewGuid().ToString()),
                    new SqliteParameter("@amount", currentItem.Amount),
                    new SqliteParameter("@exp", currentItem.Exp),
                    new SqliteParameter("@equipItemGuid", currentItem.EquipItemGuid),
                    new SqliteParameter("@equipPosition", currentItem.EquipPosition));
                //HelperUnlockItem(player.characterGuid, startItem.guid);

            }
        }
    }
    /// <summary>
    /// 角色升级
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="loginToken"></param>
    /// <param name="itemGuid"></param>
    /// <param name="materials"></param>
    /// <param name="ExIncreasingExp"> 额外加的经验</param>
    /// <param name="onFinish"></param>
    public void DoCharacterLevelUpItem(string itemGuid, Dictionary<string, int> materials, int ExIncreasingExp, UnityAction<ItemResult> onFinish)
    {
        var player = Player.CurrentPlayer;
        var playerId = player.Id;
        var loginToken = player.LoginToken;
        var result = new ItemResult();
        var foundPlayer = GameInstance.dbLogin.GetPlayerByLoginToken(playerId, loginToken);
        var foundItem = GetPlayerCharacterItemById(itemGuid);
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
                var usingAmount = materials[materialItem.ItemID];
                if (usingAmount > materialItem.Amount)
                    usingAmount = materialItem.Amount;
                requireCurrency += levelUpPrice * usingAmount;
                increasingExp += materialItem.RewardExp * usingAmount;
                materialItem.Amount -= usingAmount;
                if (materialItem.Amount > 0)
                    updateItems.Add(materialItem);
                else
                    deleteItemIds.Add(materialItem.GUID, PlayerItem.ItemType.character);
            }
            if (requireCurrency > softCurrency.Amount)
                result.error = GameServiceErrorCode.NOT_ENOUGH_SOFT_CURRENCY;
            else
            {
                softCurrency.Amount -= requireCurrency;
                GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                    new SqliteParameter("@amount", softCurrency.Amount),
                    new SqliteParameter("@id", softCurrency.Id));

                foundItem = foundItem.CreateLevelUpItem(increasingExp + ExIncreasingExp);
                updateItems.Add(foundItem);
                foreach (var updateItem in updateItems)
                {
                    GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerHasCharacters SET playerId=@playerId, itemid=@itemid, amount=@amount, exp=@exp WHERE Guid=@Guid",
                        new SqliteParameter("@playerId", updateItem.PlayerId),
                        new SqliteParameter("@itemid", updateItem.ItemID),
                        new SqliteParameter("@amount", updateItem.Amount),
                        new SqliteParameter("@exp", updateItem.Exp),
                        new SqliteParameter("@Guid", updateItem.GUID));
                }
                foreach (var deleteItemId in deleteItemIds)
                {
                    GameInstance.dbDataUtils.ExecuteNonQuery(@"DELETE FROM playerHasCharacters WHERE Guid=@Guid", new SqliteParameter("@Guid", deleteItemId));
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
    public PlayerItem GetPlayerCharacterItemById(string Guid)
    {
        PlayerItem playerItem = null;
        var playerItems = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerHasCharacters WHERE Guid=@Guid",
            new SqliteParameter("@Guid", Guid));
        if (playerItems.Read())
        {
            playerItem = new PlayerItem(PlayerItem.ItemType.character);
            playerItem.itemType = PlayerItem.ItemType.character;
            playerItem.ItemID = playerItems.GetString(0);
            playerItem.PlayerId = playerItems.GetString(1);
            playerItem.GUID = playerItems.GetString(2);
            playerItem.Amount = playerItems.GetInt32(3);
            playerItem.Exp = playerItems.GetInt32(4);
        }
        return playerItem;
    }

    public PlayerItem GetPlayerEquipmentItemById(string Guid)
    {
        PlayerItem playerItem = null;
        var playerItems = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerHasEquips WHERE Guid=@Guid",
            new SqliteParameter("@Guid", Guid));
        if (playerItems.Read())
        {
            playerItem = new PlayerItem(PlayerItem.ItemType.equip);
            playerItem.itemType = PlayerItem.ItemType.equip;
            playerItem.ItemID = playerItems.GetString(0);
            playerItem.PlayerId = playerItems.GetString(1);
            playerItem.GUID = playerItems.GetString(2);
            playerItem.Amount = playerItems.GetInt32(3);
            playerItem.Exp = playerItems.GetInt32(4);
            playerItem.EquipItemGuid = playerItems.GetString(5);
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
        var currencies = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerCurrency WHERE playerId=@playerId AND Guid=@Guid LIMIT 1",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@Guid", dataId));
        PlayerCurrency currency = null;
        if (!currencies.Read())
        {
            currency = new PlayerCurrency();
            currency.Id = PlayerCurrency.GetId(playerId, dataId);
            currency.PlayerId = playerId;
            currency.DataId = dataId;
            GameInstance.dbDataUtils.ExecuteNonQuery(@"INSERT INTO playerCurrency (id, playerId, Guid, amount, purchasedAmount) VALUES (@id, @playerId, @Guid, @amount, @purchasedAmount)",
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


    /// <summary>
    /// 从用户表中查找这件装备装在哪个角色身上
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="equipItemId"></param>
    /// <param name="equipPosition"></param>
    /// <returns></returns>
    public PlayerItem GetPlayerItemByEquipper(string playerId, string equipItemGuid, string equipPosition)
    {
        PlayerItem playerItem = null;
        var playerItems = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerHasEquips WHERE playerId=@playerId AND equipItemGuid=@equipItemGuid AND equipPosition=@equipPosition",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@equipItemGuid", equipItemGuid),
            new SqliteParameter("@equipPosition", equipPosition));
        if (playerItems.Read())
        {
            playerItem = new PlayerItem(PlayerItem.ItemType.equip);
            playerItem.ItemID = playerItems.GetString(0);
            playerItem.PlayerId = playerItems.GetString(1);
            playerItem.GUID = playerItems.GetString(2);
            playerItem.Amount = playerItems.GetInt32(3);
            playerItem.Exp = playerItems.GetInt32(4);
            playerItem.EquipItemGuid = playerItems.GetString(5);
            playerItem.EquipPosition = playerItems.GetString(6);
        }
        return playerItem;
    }
    #endregion
}
