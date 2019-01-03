using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DBBattle
{

    public const ushort BATTLE_RESULT_NONE = 0;
    public const ushort BATTLE_RESULT_LOSE = 1;
    public const ushort BATTLE_RESULT_WIN = 2;
    private void HelperSetFormation(string itemguid,string playerId, string characterId, string formationName, int position)
    {
        PlayerFormation oldFormation = null;
        if (!string.IsNullOrEmpty(characterId))
        {
            var oldFormations = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerFormation WHERE playerId=@playerId AND Guid=@Guid AND position=@position LIMIT 1",
                new SqliteParameter("@playerId", playerId),
                new SqliteParameter("@Guid", formationName),
                new SqliteParameter("@position", position));
            if (oldFormations.Read())
            {
                oldFormation = new PlayerFormation();
                oldFormation.PlayerId = oldFormations.GetString(1);
                oldFormation.formationId = oldFormations.GetString(2);
                oldFormation.Position = oldFormations.GetInt32(3);
                oldFormation.ItemId = oldFormations.GetString(4);
                oldFormation.characterGuid = oldFormations.GetString(5);
            }
            if (oldFormation != null)
            {
                GameInstance.dbDataUtils.ExecuteNonQuery(@"DELETE FROM playerFormation WHERE playerId=@playerId AND Guid=@Guid AND position=@position",
                new SqliteParameter("@playerId", playerId),
                new SqliteParameter("@Guid", formationName),
                new SqliteParameter("@position", position));
            }
        }
        PlayerFormation formation = null;
        var targetFormations = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerFormation WHERE playerId=@playerId AND Guid=@Guid AND itemguid=@itemguid LIMIT 1",
            new SqliteParameter("@itemguid", itemguid),
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@Guid", formationName));
        if (targetFormations.Read())
        {
            formation = new PlayerFormation();
            formation.PlayerId = targetFormations.GetString(1);
            formation.formationId = targetFormations.GetString(2);
            formation.Position = targetFormations.GetInt32(3);
            formation.ItemId = targetFormations.GetString(4);
            formation.characterGuid = targetFormations.GetString(5);
        }
        if (formation == null)
        {
            formation = new PlayerFormation();
            formation.characterGuid = itemguid;
            formation.PlayerId = playerId;
            formation.formationId = formationName;
            formation.Position = position;
            formation.ItemId = characterId;
            GameInstance.dbDataUtils.ExecuteNonQuery(@"INSERT INTO playerFormation (itemguid, playerId, Guid, position, itemId,id)
                VALUES (@itemguid, @playerId, @Guid, @position, @itemId,@id)",
                new SqliteParameter("@itemguid", formation.characterGuid),
                new SqliteParameter("@playerId", formation.PlayerId),
                new SqliteParameter("@Guid", formation.formationId),
                new SqliteParameter("@position", formation.Position),
                new SqliteParameter("@itemId", formation.ItemId),
                new SqliteParameter("@id", 0));
        }
        else
        {

            Debug.LogError("find this item" + formation.characterGuid);
            formation.Position = position;
            GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerFormation SET position=@position WHERE itemguid=@itemguid",
                new SqliteParameter("@position", formation.Position),
                new SqliteParameter("@itemguid", formation.characterGuid));
        }
    }

    private PlayerClearStage HelperClearStage(string playerId, string dataId, int grade)
    {
        PlayerClearStage clearStage = null;
        var clearStages = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerClearStage WHERE playerId=@playerId AND Guid=@Guid LIMIT 1",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@Guid", dataId));
        if (!clearStages.Read())
        {
            clearStage = new PlayerClearStage();
            clearStage.Id = PlayerClearStage.GetId(playerId, dataId);
            clearStage.PlayerId = playerId;
            clearStage.DataId = dataId;
            clearStage.BestRating = grade;
            GameInstance.dbDataUtils.ExecuteNonQuery(@"INSERT INTO playerClearStage (id, playerId, Guid, bestRating)
                VALUES (@id, @playerId, @Guid, @bestRating)",
                new SqliteParameter("@id", clearStage.Id),
                new SqliteParameter("@playerId", clearStage.PlayerId),
                new SqliteParameter("@Guid", clearStage.DataId),
                new SqliteParameter("@bestRating", clearStage.BestRating));
        }
        else
        {
            clearStage = new PlayerClearStage();
            clearStage.Id = clearStages.GetString(0);
            clearStage.PlayerId = clearStages.GetString(1);
            clearStage.DataId = clearStages.GetString(2);
            clearStage.BestRating = clearStages.GetInt32(3);
            if (clearStage.BestRating < grade)
            {
                clearStage.BestRating = grade;
                GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerClearStage SET bestRating=@bestRating WHERE id=@id",
                    new SqliteParameter("@bestRating", clearStage.BestRating),
                    new SqliteParameter("@id", clearStage.Id));
            }
        }
        return clearStage;
    }

    private PlayerBattle GetPlayerBattleBySession(string playerId, string session)
    {
        PlayerBattle playerBattle = null;
        var playerBattles = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerBattle WHERE playerId=@playerId AND session=@session",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@session", session));
        if (playerBattles.Read())
        {
            playerBattle = new PlayerBattle();
            playerBattle.Id = playerBattles.GetString(0);
            playerBattle.PlayerId = playerBattles.GetString(1);
            playerBattle.DataId = playerBattles.GetString(2);
            playerBattle.Session = playerBattles.GetString(3);
            playerBattle.BattleResult = (uint)playerBattles.GetInt32(4);
            playerBattle.Rating = playerBattles.GetInt32(5);
        }
        return playerBattle;
    }

    #region 战斗
    public void DoSelectFormation(string formationName, UnityAction<PlayerResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new PlayerResult();
        var gameDb = GameInstance.GameDatabase;
        var player = GameInstance.dbLogin.GetPlayerByLoginToken(playerId, loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (!gameDb.Formations.ContainsKey(formationName))
            result.error = GameServiceErrorCode.INVALID_FORMATION_DATA;
        else
        {
            player.SelectedFormation = formationName;
            GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE player SET selectedFormation=@selectedFormation WHERE id=@id",
                new SqliteParameter("@selectedFormation", player.SelectedFormation),
                new SqliteParameter("@id", player.Id));
            result.player = player;
        }
        onFinish(result);
    }

    /// <summary>
    /// itemguid   heroGuid  
    /// characterid   heroItemid
    /// </summary>
    /// <param name="itemguid"></param>
    /// <param name="characterId"></param>
    /// <param name="formationName"></param>
    /// <param name="position"></param>
    /// <param name="onFinish"></param>
    public void DoSetFormation(string itemguid,string characterId, string formationName, int position, UnityAction<FormationListResult> onFinish)
    {
        Debug.LogError("111:" + itemguid);

        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new FormationListResult();
        var player = GameInstance.dbLogin.GetPlayerByLoginToken(playerId, loginToken);
        PlayerItem character = null;
        if (!string.IsNullOrEmpty(characterId))
            character = GameInstance.dbPlayerData.GetPlayerCharacterItemById(characterId);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (character != null && character.CharacterData == null)
            result.error = GameServiceErrorCode.INVALID_ITEM_DATA;
        else
        {
            HelperSetFormation(itemguid,playerId, characterId, formationName, position);
            var reader = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerFormation WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
            var list = new List<PlayerFormation>();
            while (reader.Read())
            {
                var entry = new PlayerFormation();
                entry.PlayerId = reader.GetString(1);
                entry.formationId = reader.GetString(2);
                entry.Position = reader.GetInt32(3);
                entry.ItemId = reader.GetString(4);
                entry.characterGuid = reader.GetString(5);
                list.Add(entry);
            }
            result.list = list;
        }
        onFinish(result);
    }

    public void DoStartStage(string stageDataId, UnityAction<StartStageResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new StartStageResult();
        var gameDb = GameInstance.GameDatabase;



        var player = GameInstance.dbLogin.GetPlayerByLoginToken(playerId, loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (!gameDb.Stages.ContainsKey(stageDataId))
            result.error = GameServiceErrorCode.INVALID_STAGE_DATA;
        else
        {
            GameInstance.dbDataUtils.ExecuteNonQuery(@"DELETE FROM playerBattle WHERE playerId=@playerId AND battleResult=@battleResult",
                new SqliteParameter("@playerId", playerId),
                new SqliteParameter("@battleResult", BATTLE_RESULT_NONE));
            var stage = gameDb.Stages[stageDataId];
            var stageStaminaTable = gameDb.stageStamina;
            if (!GameInstance.dbPlayerData.DecreasePlayerStamina(player, stageStaminaTable, stage.requireStamina))
                result.error = GameServiceErrorCode.NOT_ENOUGH_STAGE_STAMINA;
            else
            {
                var playerBattle = new PlayerBattle();
                playerBattle.Id = System.Guid.NewGuid().ToString();
                playerBattle.PlayerId = playerId;
                playerBattle.DataId = stageDataId;
                playerBattle.Session = System.Guid.NewGuid().ToString();
                playerBattle.BattleResult = BATTLE_RESULT_NONE;
                GameInstance.dbDataUtils.ExecuteNonQuery(@"INSERT INTO playerBattle (id, playerId, Guid, session, battleResult, rating) VALUES (@id, @playerId, @Guid, @session, @battleResult, @rating)",
                    new SqliteParameter("@id", playerBattle.Id),
                    new SqliteParameter("@playerId", playerBattle.PlayerId),
                    new SqliteParameter("@Guid", playerBattle.DataId),
                    new SqliteParameter("@session", playerBattle.Session),
                    new SqliteParameter("@battleResult", playerBattle.BattleResult),
                    new SqliteParameter("@rating", playerBattle.Rating));

                var stamina = GameInstance.dbPlayerData.GetStamina(player.Id, stageStaminaTable.id);
                result.stamina = stamina;
                result.session = playerBattle.Session;
            }
        }
        onFinish(result);
    }

    public void DoStartTowerStage(string stageDataId, UnityAction<StartStageResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new StartStageResult();
        var gameDb = GameInstance.GameDatabase;



        var player = GameInstance.dbLogin.GetPlayerByLoginToken(playerId, loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            GameInstance.dbDataUtils.ExecuteNonQuery(@"DELETE FROM playerBattle WHERE playerId=@playerId AND battleResult=@battleResult",
                new SqliteParameter("@playerId", playerId),
                new SqliteParameter("@battleResult", BATTLE_RESULT_NONE));
            var stageStaminaTable = gameDb.stageStamina;

            var playerBattle = new PlayerBattle();
            playerBattle.Id = System.Guid.NewGuid().ToString();
            playerBattle.PlayerId = playerId;
            playerBattle.DataId = stageDataId;
            playerBattle.Session = System.Guid.NewGuid().ToString();
            playerBattle.BattleResult = BATTLE_RESULT_NONE;
            GameInstance.dbDataUtils.ExecuteNonQuery(@"INSERT INTO playerBattle (id, playerId, Guid, session, battleResult, rating) VALUES (@id, @playerId, @Guid, @session, @battleResult, @rating)",
                new SqliteParameter("@id", playerBattle.Id),
                new SqliteParameter("@playerId", playerBattle.PlayerId),
                new SqliteParameter("@Guid", playerBattle.DataId),
                new SqliteParameter("@session", playerBattle.Session),
                new SqliteParameter("@battleResult", playerBattle.BattleResult),
                new SqliteParameter("@rating", playerBattle.Rating));

            var stamina = GameInstance.dbPlayerData.GetStamina(player.Id, stageStaminaTable.id);
            result.stamina = stamina;
            result.session = playerBattle.Session;

        }
        onFinish(result);
    }

    public void DoFinishStage(Const.StageType stagetype, string session, ushort battleResult, int deadCharacters, UnityAction<FinishStageResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;

        var result = new FinishStageResult();
        var gameDb = GameInstance.GameDatabase;
        var player = GameInstance.dbLogin.GetPlayerByLoginToken(playerId, loginToken);
        var battle = GetPlayerBattleBySession(playerId, session);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (battle == null)
            result.error = GameServiceErrorCode.INVALID_BATTLE_SESSION;
        else
        {
            var rating = 0;
            battle.BattleResult = battleResult;
            if (battleResult == BATTLE_RESULT_WIN)
            {
                rating = 3 - deadCharacters;
                if (rating <= 0)
                    rating = 1;
            }
            battle.Rating = rating;
            result.rating = rating;
            GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerBattle SET battleResult=@battleResult, rating=@rating WHERE id=@id",
                new SqliteParameter("@battleResult", battle.BattleResult),
                new SqliteParameter("@rating", battle.Rating),
                new SqliteParameter("@id", battle.Id));
            if (battleResult == BATTLE_RESULT_WIN)
            {
                BaseStage stage = null;
                switch (stagetype)
                {
                    case Const.StageType.Normal:
                        stage = gameDb.Stages[battle.DataId];
                        break;
                    case Const.StageType.Tower:
                        stage = gameDb.towerStages[0];
                        break;
                }
                //var stage = gameDb.Stages[battle.GUID];
                var rewardPlayerExp = stage.rewardPlayerExp;
                result.rewardPlayerExp = rewardPlayerExp;
                // Player exp
                player.Exp += rewardPlayerExp;
                GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE player SET exp=@exp WHERE id=@playerId",
                    new SqliteParameter("@exp", player.Exp),
                    new SqliteParameter("@playerId", playerId));
                result.player = player;
                // Character exp
                var countFormation = GameInstance.dbDataUtils.ExecuteScalar(@"SELECT COUNT(*) FROM playerFormation WHERE playerId=@playerId AND Guid=@Guid",
                    new SqliteParameter("@playerId", playerId),
                    new SqliteParameter("@Guid", player.SelectedFormation));
                if (countFormation != null && (long)countFormation > 0)
                {
                    var devivedExp = (int)(stage.rewardCharacterExp / (long)countFormation);
                    result.rewardCharacterExp = devivedExp;

                    var formations = GameInstance.dbDataUtils.ExecuteReader(@"SELECT itemId FROM playerFormation WHERE playerId=@playerId AND Guid=@Guid",
                        new SqliteParameter("@playerId", playerId),
                        new SqliteParameter("@Guid", player.SelectedFormation));
                    while (formations.Read())
                    {
                        var itemId = formations.GetString(0);
                        var character = GameInstance.dbPlayerData.GetPlayerCharacterItemById(itemId);
                        if (character != null)
                        {
                            character.Exp += devivedExp;
                            GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerHasCharacters SET exp=@exp WHERE Guid=@Guid",
                                new SqliteParameter("@exp", character.Exp),
                                new SqliteParameter("@Guid", character.GUID));
                            result.updateItems.Add(character);
                        }
                    }
                }
                // Soft currency
                var softCurrency = GameInstance.dbPlayerData.GetCurrency(playerId, gameDb.softCurrency.id);
                var rewardSoftCurrency = Random.Range(stage.randomSoftCurrencyMinAmount, stage.randomSoftCurrencyMaxAmount);
                result.rewardSoftCurrency = rewardSoftCurrency;
                softCurrency.Amount += rewardSoftCurrency;
                GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                    new SqliteParameter("@amount", softCurrency.Amount),
                    new SqliteParameter("@id", softCurrency.Id));
                result.updateCurrencies.Add(softCurrency);
                // Items
                for (var i = 0; i < stage.rewardItems.Length; ++i)
                {
                    var rewardItem = stage.rewardItems[i];
                    if (rewardItem == null || rewardItem.item == null || Random.value > rewardItem.randomRate)
                        continue;
                    var createItems = new List<PlayerItem>();
                    var updateItems = new List<PlayerItem>();
                    //todo 战斗胜利后的结算
                    //if (AddItems(player.characterGuid, rewardItem.characterGuid, rewardItem.amount, out createItems, out updateItems))
                    //{
                    //    foreach (var createEntry in createItems)
                    //    {
                    //        createEntry.characterGuid = System.Guid.NewGuid().ToString();
                    //        ExecuteNonQuery(@"INSERT INTO playerItem (id, playerId, Guid, amount, exp, equipItemGuid, equipPosition) VALUES (@id, @playerId, @Guid, @amount, @exp, @equipItemId, @equipPosition)",
                    //            new SqliteParameter("@id", createEntry.characterGuid),
                    //            new SqliteParameter("@playerId", createEntry.PlayerId),
                    //            new SqliteParameter("@Guid", createEntry.GUID),
                    //            new SqliteParameter("@amount", createEntry.Amount),
                    //            new SqliteParameter("@exp", createEntry.Exp),
                    //            new SqliteParameter("@equipItemGuid", createEntry.equipItemId),
                    //            new SqliteParameter("@equipPosition", createEntry.EquipPosition));
                    //        result.rewardItems.Add(createEntry);
                    //        result.createItems.Add(createEntry);
                    //        HelperUnlockItem(player.characterGuid, rewardItem.characterGuid);
                    //    }
                    //    foreach (var updateEntry in updateItems)
                    //    {
                    //        ExecuteNonQuery(@"UPDATE playerItem SET playerId=@playerId, Guid=@Guid, amount=@amount, exp=@exp, equipItemGuid=@equipItemGuid, equipPosition=@equipPosition WHERE id=@id",
                    //            new SqliteParameter("@playerId", updateEntry.PlayerId),
                    //            new SqliteParameter("@Guid", updateEntry.GUID),
                    //            new SqliteParameter("@amount", updateEntry.Amount),
                    //            new SqliteParameter("@exp", updateEntry.Exp),
                    //            new SqliteParameter("@equipItemGuid", updateEntry.equipItemGuid),
                    //            new SqliteParameter("@equipPosition", updateEntry.EquipPosition),
                    //            new SqliteParameter("@id", updateEntry.characterGuid));
                    //        result.rewardItems.Add(updateEntry);
                    //        result.updateItems.Add(updateEntry);
                    //    }
                    //}
                    // End add item condition
                }
                // End reward items loop
                var clearedStage = HelperClearStage(playerId, stage.Id, rating);
                result.clearStage = clearedStage;
            }
        }
        onFinish(result);
    }
    #endregion


    public void DoUnEquipItem(string equipmentId, UnityAction<ItemResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new ItemResult();
        var player = GameInstance.dbLogin.GetPlayerByLoginToken(playerId, loginToken);
        var unEquipItem = GameInstance.dbPlayerData.GetPlayerEquipmentItemById(equipmentId);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (unEquipItem == null || unEquipItem.PlayerId != playerId)
            result.error = GameServiceErrorCode.INVALID_PLAYER_ITEM_DATA;
        else
        {
            result.updateItems = new List<PlayerItem>();
            unEquipItem.EquipItemGuid = "";
            unEquipItem.EquipPosition = "";
            GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerHasEquips SET equipItemGuid=@equipItemGuid, equipPosition=@equipPosition WHERE Guid=@Guid",
                new SqliteParameter("@equipItemGuid", unEquipItem.EquipItemGuid),
                new SqliteParameter("@equipPosition", unEquipItem.EquipPosition),
                new SqliteParameter("@Guid", unEquipItem.GUID));
            result.updateItems.Add(unEquipItem);
        }
        onFinish(result);
    }

    public void DoEquipItem(string characterId, string equipmentId, string equipPosition, UnityAction<ItemResult> onFinish)
    {
        var player = Player.CurrentPlayer;
        var playerId = player.Id;
        var loginToken = player.LoginToken;
        var result = new ItemResult();
        var foundPlayer = GameInstance.dbLogin.GetPlayerByLoginToken(playerId, loginToken);
        var foundCharacter = GameInstance.dbPlayerData.GetPlayerCharacterItemById(characterId);
        var foundEquipment = GameInstance.dbPlayerData.GetPlayerEquipmentItemById(equipmentId);
        CharacterItem characterData = null;
        EquipmentItem equipmentData = null;
        //todo
        //if (foundCharacter != null)
        //    characterData = foundCharacter.CharacterData;
        //if (foundEquipment != null)
        //    equipmentData = foundEquipment.EquipmentData;
        if (foundPlayer == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (foundCharacter == null || foundCharacter.PlayerId != playerId || foundEquipment == null || foundEquipment.PlayerId != playerId)
            result.error = GameServiceErrorCode.INVALID_PLAYER_ITEM_DATA;
        else if (characterData == null || equipmentData == null)
            result.error = GameServiceErrorCode.INVALID_ITEM_DATA;
        else if (equipmentData.equippablePosition != equipPosition)
            result.error = GameServiceErrorCode.INVALID_EQUIP_POSITION;
        else
        {
            result.updateItems = new List<PlayerItem>();
            //寻找装备了这个装备的角色，卸下来
            var unEquipItem = GameInstance.dbPlayerData.GetPlayerItemByEquipper(playerId, characterId, equipPosition);
            if (unEquipItem != null)
            {
                unEquipItem.EquipItemGuid = "";
                unEquipItem.EquipPosition = "";
                GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerHasEquips SET equipItemGuid=@equipItemGuid, equipPosition=@equipPosition WHERE Guid=@Guid",
                    new SqliteParameter("@equipItemGuid", unEquipItem.EquipItemGuid),
                    new SqliteParameter("@equipPosition", unEquipItem.EquipPosition),
                    new SqliteParameter("@Guid", unEquipItem.GUID));
                result.updateItems.Add(unEquipItem);
            }
            foundEquipment.EquipItemGuid = characterId;
            foundEquipment.EquipPosition = equipPosition;
            GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerHasEquips SET equipItemGuid=@equipItemGuid, equipPosition=@equipPosition WHERE Guid=@Guid",
                new SqliteParameter("@equipItemGuid", foundEquipment.EquipItemGuid),
                new SqliteParameter("@equipPosition", foundEquipment.EquipPosition),
                new SqliteParameter("@Guid", foundEquipment.GUID));
            result.updateItems.Add(foundEquipment);
        }
        if (result.error.Length > 1)
        {
            Debug.LogError(result.error);
        }
        onFinish(result);
    }

    public void DoGetClearStageList(UnityAction<ClearStageListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new ClearStageListResult();
        var player = GameInstance.dbDataUtils.ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerClearStage WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
            var list = new List<PlayerClearStage>();
            while (reader.Read())
            {
                var entry = new PlayerClearStage();
                entry.Id = reader.GetString(0);
                entry.PlayerId = reader.GetString(1);
                entry.DataId = reader.GetString(2);
                entry.BestRating = reader.GetInt32(3);
                list.Add(entry);
            }
            result.list = list;
        }
        onFinish(result);
    }

    public void DoGetFormationList(UnityAction<FormationListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new FormationListResult();
        var player = GameInstance.dbDataUtils.ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerFormation WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
            var list = new List<PlayerFormation>();
            while (reader.Read())
            {
                var entry = new PlayerFormation();
                entry.PlayerId = reader.GetString(1);
                entry.formationId = reader.GetString(2);
                entry.Position = reader.GetInt32(3);
                entry.ItemId = reader.GetString(4);
                entry.characterGuid = reader.GetString(5);
                list.Add(entry);
            }
            result.list = list;
        }
        onFinish(result);
    }

    public void DoGetUnlockItemList(UnityAction<UnlockItemListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new UnlockItemListResult();
        var player = GameInstance.dbDataUtils.ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerUnlockItem WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
            var list = new List<PlayerUnlockItem>();
            while (reader.Read())
            {
                var entry = new PlayerUnlockItem();
                entry.Id = reader.GetString(0);
                entry.PlayerId = reader.GetString(1);
                entry.DataId = reader.GetString(2);
                entry.Amount = reader.GetInt32(3);
                list.Add(entry);
            }
            result.list = list;
        }
        onFinish(result);
    }
}
