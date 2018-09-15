using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mono.Data.Sqlite;

public partial class SQLiteGameService
{
    protected override void DoStartStage(string playerId, string loginToken, string stageDataId, UnityAction<StartStageResult> onFinish)
    {
        var result = new StartStageResult();
        var gameDb = GameInstance.GameDatabase;



        var player = GetPlayerByLoginToken(playerId, loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (!gameDb.Stages.ContainsKey(stageDataId))
            result.error = GameServiceErrorCode.INVALID_STAGE_DATA;
        else
        {
            ExecuteNonQuery(@"DELETE FROM playerBattle WHERE playerId=@playerId AND battleResult=@battleResult",
                new SqliteParameter("@playerId", playerId),
                new SqliteParameter("@battleResult", BATTLE_RESULT_NONE));
            var stage = gameDb.Stages[stageDataId];
            var stageStaminaTable = gameDb.stageStamina;
            if (!DecreasePlayerStamina(player, stageStaminaTable, stage.requireStamina))
                result.error = GameServiceErrorCode.NOT_ENOUGH_STAGE_STAMINA;
            else
            {
                var playerBattle = new PlayerBattle();
                playerBattle.Id = System.Guid.NewGuid().ToString();
                playerBattle.PlayerId = playerId;
                playerBattle.DataId = stageDataId;
                playerBattle.Session = System.Guid.NewGuid().ToString();
                playerBattle.BattleResult = BATTLE_RESULT_NONE;
                ExecuteNonQuery(@"INSERT INTO playerBattle (id, playerId, Guid, session, battleResult, rating) VALUES (@id, @playerId, @Guid, @session, @battleResult, @rating)",
                    new SqliteParameter("@id", playerBattle.Id),
                    new SqliteParameter("@playerId", playerBattle.PlayerId),
                    new SqliteParameter("@Guid", playerBattle.DataId),
                    new SqliteParameter("@session", playerBattle.Session),
                    new SqliteParameter("@battleResult", playerBattle.BattleResult),
                    new SqliteParameter("@rating", playerBattle.Rating));

                var stamina = GetStamina(player.Id, stageStaminaTable.id);
                result.stamina = stamina;
                result.session = playerBattle.Session;
            }
        }
        onFinish(result);
    }

    protected override void DoStartTowerStage(string playerId, string loginToken, string stageDataId, UnityAction<StartStageResult> onFinish)
    {
        var result = new StartStageResult();
        var gameDb = GameInstance.GameDatabase;



        var player = GetPlayerByLoginToken(playerId, loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            ExecuteNonQuery(@"DELETE FROM playerBattle WHERE playerId=@playerId AND battleResult=@battleResult",
                new SqliteParameter("@playerId", playerId),
                new SqliteParameter("@battleResult", BATTLE_RESULT_NONE));
            var stageStaminaTable = gameDb.stageStamina;

            var playerBattle = new PlayerBattle();
            playerBattle.Id = System.Guid.NewGuid().ToString();
            playerBattle.PlayerId = playerId;
            playerBattle.DataId = stageDataId;
            playerBattle.Session = System.Guid.NewGuid().ToString();
            playerBattle.BattleResult = BATTLE_RESULT_NONE;
            ExecuteNonQuery(@"INSERT INTO playerBattle (id, playerId, Guid, session, battleResult, rating) VALUES (@id, @playerId, @Guid, @session, @battleResult, @rating)",
                new SqliteParameter("@id", playerBattle.Id),
                new SqliteParameter("@playerId", playerBattle.PlayerId),
                new SqliteParameter("@Guid", playerBattle.DataId),
                new SqliteParameter("@session", playerBattle.Session),
                new SqliteParameter("@battleResult", playerBattle.BattleResult),
                new SqliteParameter("@rating", playerBattle.Rating));

            var stamina = GetStamina(player.Id, stageStaminaTable.id);
            result.stamina = stamina;
            result.session = playerBattle.Session;

        }
        onFinish(result);
    }

    protected override void DoFinishStage(Const.StageType stagetype, string playerId, string loginToken, string session, ushort battleResult, int deadCharacters, UnityAction<FinishStageResult> onFinish)
    {
        var result = new FinishStageResult();
        var gameDb = GameInstance.GameDatabase;
        var player = GetPlayerByLoginToken(playerId, loginToken);
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
            ExecuteNonQuery(@"UPDATE playerBattle SET battleResult=@battleResult, rating=@rating WHERE id=@id",
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
                ExecuteNonQuery(@"UPDATE player SET exp=@exp WHERE id=@playerId",
                    new SqliteParameter("@exp", player.Exp),
                    new SqliteParameter("@playerId", playerId));
                result.player = player;
                // Character exp
                var countFormation = ExecuteScalar(@"SELECT COUNT(*) FROM playerFormation WHERE playerId=@playerId AND Guid=@Guid",
                    new SqliteParameter("@playerId", playerId),
                    new SqliteParameter("@Guid", player.SelectedFormation));
                if (countFormation != null && (long)countFormation > 0)
                {
                    var devivedExp = (int)(stage.rewardCharacterExp / (long)countFormation);
                    result.rewardCharacterExp = devivedExp;

                    var formations = ExecuteReader(@"SELECT itemId FROM playerFormation WHERE playerId=@playerId AND Guid=@Guid",
                        new SqliteParameter("@playerId", playerId),
                        new SqliteParameter("@Guid", player.SelectedFormation));
                    while (formations.Read())
                    {
                        var itemId = formations.GetInt32(0).ToString();
                        var character = sqliteUtils.GetPlayerCharacterItemById(itemId);
                        if (character != null)
                        {
                            character.Exp += devivedExp;
                            ExecuteNonQuery(@"UPDATE playerHasCharacters SET exp=@exp WHERE id=@id",
                                new SqliteParameter("@exp", character.Exp),
                                new SqliteParameter("@id", character.SqLiteIndex));
                            result.updateItems.Add(character);
                        }
                    }
                }
                // Soft currency
                var softCurrency = GetCurrency(playerId, gameDb.softCurrency.id);
                var rewardSoftCurrency = Random.Range(stage.randomSoftCurrencyMinAmount, stage.randomSoftCurrencyMaxAmount);
                result.rewardSoftCurrency = rewardSoftCurrency;
                softCurrency.Amount += rewardSoftCurrency;
                ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
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
                    //if (AddItems(player.Id, rewardItem.Id, rewardItem.amount, out createItems, out updateItems))
                    //{
                    //    foreach (var createEntry in createItems)
                    //    {
                    //        createEntry.Id = System.Guid.NewGuid().ToString();
                    //        ExecuteNonQuery(@"INSERT INTO playerItem (id, playerId, Guid, amount, exp, equipItemId, equipPosition) VALUES (@id, @playerId, @Guid, @amount, @exp, @equipItemId, @equipPosition)",
                    //            new SqliteParameter("@id", createEntry.Id),
                    //            new SqliteParameter("@playerId", createEntry.PlayerId),
                    //            new SqliteParameter("@Guid", createEntry.GUID),
                    //            new SqliteParameter("@amount", createEntry.Amount),
                    //            new SqliteParameter("@exp", createEntry.Exp),
                    //            new SqliteParameter("@equipItemId", createEntry.EquipItemId),
                    //            new SqliteParameter("@equipPosition", createEntry.EquipPosition));
                    //        result.rewardItems.Add(createEntry);
                    //        result.createItems.Add(createEntry);
                    //        HelperUnlockItem(player.Id, rewardItem.Id);
                    //    }
                    //    foreach (var updateEntry in updateItems)
                    //    {
                    //        ExecuteNonQuery(@"UPDATE playerItem SET playerId=@playerId, Guid=@Guid, amount=@amount, exp=@exp, equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
                    //            new SqliteParameter("@playerId", updateEntry.PlayerId),
                    //            new SqliteParameter("@Guid", updateEntry.GUID),
                    //            new SqliteParameter("@amount", updateEntry.Amount),
                    //            new SqliteParameter("@exp", updateEntry.Exp),
                    //            new SqliteParameter("@equipItemId", updateEntry.EquipItemId),
                    //            new SqliteParameter("@equipPosition", updateEntry.EquipPosition),
                    //            new SqliteParameter("@id", updateEntry.Id));
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

    protected override void DoReviveCharacters(string playerId, string loginToken, UnityAction<CurrencyResult> onFinish)
    {
        var result = new CurrencyResult();
        var gameDb = GameInstance.GameDatabase;
        var player = GetPlayerByLoginToken(playerId, loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var hardCurrency = GetCurrency(playerId, gameDb.hardCurrency.id);
            var revivePrice = gameDb.revivePrice;
            if (revivePrice > hardCurrency.Amount)
                result.error = GameServiceErrorCode.NOT_ENOUGH_HARD_CURRENCY;
            else
            {
                hardCurrency.Amount -= revivePrice;
                ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                    new SqliteParameter("@amount", hardCurrency.Amount),
                    new SqliteParameter("@id", hardCurrency.Id));
                result.updateCurrencies.Add(hardCurrency);
            }
        }
        onFinish(result);
    }

    protected override void DoSelectFormation(string playerId, string loginToken, string formationName, UnityAction<PlayerResult> onFinish)
    {
        var result = new PlayerResult();
        var gameDb = GameInstance.GameDatabase;
        var player = GetPlayerByLoginToken(playerId, loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (!gameDb.Formations.ContainsKey(formationName))
            result.error = GameServiceErrorCode.INVALID_FORMATION_DATA;
        else
        {
            player.SelectedFormation = formationName;
            ExecuteNonQuery(@"UPDATE player SET selectedFormation=@selectedFormation WHERE id=@id",
                new SqliteParameter("@selectedFormation", player.SelectedFormation),
                new SqliteParameter("@id", player.Id));
            result.player = player;
        }
        onFinish(result);
    }

    protected override void DoSetFormation(string playerId, string loginToken, string characterId, string formationName, int position, UnityAction<FormationListResult> onFinish)
    {
        var result = new FormationListResult();
        var player = GetPlayerByLoginToken(playerId, loginToken);
        PlayerItem character = null;
        if (!string.IsNullOrEmpty(characterId))
            character = sqliteUtils.GetPlayerCharacterItemById(characterId);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (character != null && character.CharacterData == null)
            result.error = GameServiceErrorCode.INVALID_ITEM_DATA;
        else
        {
            HelperSetFormation(playerId, characterId, formationName, position);
            var reader = ExecuteReader(@"SELECT * FROM playerFormation WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
            var list = new List<PlayerFormation>();
            while (reader.Read())
            {
                var entry = new PlayerFormation();
                entry.Id = reader.GetString(0);
                entry.PlayerId = reader.GetString(1);
                entry.DataId = reader.GetString(2);
                entry.Position = reader.GetInt32(3);
                entry.ItemId = reader.GetString(4);
                list.Add(entry);
            }
            result.list = list;
        }
        onFinish(result);
    }
}
