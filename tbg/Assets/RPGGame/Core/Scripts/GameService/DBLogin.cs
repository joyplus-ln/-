using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.Events;

public class DBLogin
{
    public DBLogin()
    {
    }

    #region 登录相关



    public void DoGuestLogin(string deviceId, UnityAction<PlayerResult> onFinish)
    {
        //var result = new PlayerResult();
        //if (string.IsNullOrEmpty(deviceId))
        //{
        //    Debug.LogError("登陆失败，设备号不存在");
        //    result.error = GameServiceErrorCode.EMPTY_USERNMAE_OR_PASSWORD;
        //}
        //else if (IsPlayerWithUsernameFound(AUTH_GUEST, deviceId))
        //{
        //    IPlayer player = null;
        //    if (!TryGetPlayer(AUTH_GUEST, deviceId, deviceId, out player))
        //    {
        //        Debug.LogError("登陆失败");
        //        result.error = GameServiceErrorCode.INVALID_USERNMAE_OR_PASSWORD;
        //    }
        //    else
        //    {
        //        player = UpdatePlayerLoginToken(player);
        //        GameInstance.dbPlayerData.UpdatePlayerStamina(player);
        //        result.player = player;
        //    }
        //}
        //else
        //{
        //    var player = InsertNewPlayer(AUTH_GUEST, deviceId, deviceId);
        //    result.player = player;
        //}
        //onFinish(result);
    }





    #endregion



    private bool IsPlayerWithUsernameFound(string type, string username)
    {
        int count = 0;//IPlayerAuth.DataMap.Values.Where(x => x.username == username && x.type == type).Count();
        //var count = GameInstance.dbDataUtils.ExecuteScalar(@"SELECT COUNT(*) FROM playerAuth WHERE type=@type AND username=@username",
        //    new SqliteParameter("@type", type),
        //    new SqliteParameter("@username", username));
        return count != null && (long)count > 0;
    }


    #region 帮助类


    private IPlayer SetNewPlayerData(IPlayer player)
    {
        //player.loginToken = System.Guid.NewGuid().ToString();
        //player.exp = 0;

        //var gameDb = GameInstance.GameDatabase;
        //var softCurrencyTable = gameDb.softCurrency;
        //var hardCurrencyTable = gameDb.hardCurrency;

        //var formationName = gameDb.stageFormations[0].id;
        //player.selectedFormation = formationName;

        //var softCurrency = GameInstance.dbPlayerData.GetCurrency(player.guid, softCurrencyTable.id);
        //var hardCurrency = GameInstance.dbPlayerData.GetCurrency(player.guid, hardCurrencyTable.id);
        //softCurrency.amount = softCurrencyTable.startAmount + softCurrency.purchasedAmount;
        //hardCurrency.amount = hardCurrencyTable.startAmount + hardCurrency.purchasedAmount;
        //IPlayerCurrency.SetData(softCurrency);
        //IPlayerCurrency.SetData(hardCurrency);
        //IPlayerCurrency.UpdataDataMap();
        ////GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
        ////    new SqliteParameter("@amount", softCurrency.Amount),
        ////    new SqliteParameter("@id", softCurrency.Id));
        ////GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
        ////    new SqliteParameter("@amount", hardCurrency.Amount),
        ////    new SqliteParameter("@id", hardCurrency.Id));

        ////GameInstance.dbDataUtils.ExecuteNonQuery(@"DELETE FROM playerClearStage WHERE playerId=@playerId",
        ////    new SqliteParameter("@playerId", player.guid));
        ////GameInstance.dbDataUtils.ExecuteNonQuery(@"DELETE FROM playerFormation WHERE playerId=@playerId",
        ////    new SqliteParameter("@playerId", player.guid));
        ////GameInstance.dbDataUtils.ExecuteNonQuery(@"DELETE FROM playerStamina WHERE playerId=@playerId",
        ////    new SqliteParameter("@playerId", player.guid));
        ////GameInstance.dbDataUtils.ExecuteNonQuery(@"DELETE FROM playerUnlockItem WHERE playerId=@playerId",
        ////    new SqliteParameter("@playerId", player.guid));
        ////todo 插入新玩家数据
        ////GameInstance.dbPlayerData.InsertStartPlayerCharacter(player);
        ////GameInstance.dbPlayerData.InsertStartEquiptem(player);
        ////GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE player SET profileName=@profileName, loginToken=@loginToken, exp=@exp, selectedFormation=@selectedFormation WHERE id=@id",
        ////    new SqliteParameter("@profileName", player.ProfileName),
        ////    new SqliteParameter("@loginToken", player.LoginToken),
        ////    new SqliteParameter("@exp", player.Exp),
        ////    new SqliteParameter("@selectedFormation", player.SelectedFormation),
        ////    new SqliteParameter("@id", player.Id));
        //return player;
        return null;
    }
    /// <summary>
    /// 插入用户
    /// </summary>
    /// <param name="type"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    private IPlayer InsertNewPlayer(string type, string username, string password)
    {
        //var playerId = System.Guid.NewGuid().ToString();
        //var playerAuth = new IPlayerAuth();
        //playerAuth.guid = IPlayerAuth.GetId(playerId, type);
        //playerAuth.playerId = playerId;
        //playerAuth.type = type;
        //playerAuth.username = username;
        //playerAuth.password = password;
        //IPlayerAuth.SetData(playerAuth);
        //IPlayerAuth.UpdataDataMap();
        ////GameInstance.dbDataUtils.ExecuteNonQuery(@"INSERT INTO playerAuth (id, playerId, type, username, password) VALUES (@id, @playerId, @type, @username, @password)",
        ////    new SqliteParameter("@id", playerAuth.Id),
        ////    new SqliteParameter("@playerId", playerAuth.PlayerId),
        ////    new SqliteParameter("@type", playerAuth.Type),
        ////    new SqliteParameter("@username", playerAuth.Username),
        ////    new SqliteParameter("@password", playerAuth.Password)
        ////    );
        //var player = new IPlayer();
        //player.guid = playerId;
        //player = SetNewPlayerData(player);
        //GameInstance.dbPlayerData.UpdatePlayerStamina(player);
        //IPlayer.SetData(player);
        //IPlayer.UpdataDataMap();
        //GameInstance.dbDataUtils.ExecuteNonQuery(@"INSERT INTO player (id, profileName, loginToken, exp, selectedFormation,prefs) VALUES (@id, @profileName, @loginToken, @exp, @selectedFormation,@prefs)",
        //    new SqliteParameter("@id", player.Id),
        //    new SqliteParameter("@profileName", player.ProfileName),
        //    new SqliteParameter("@loginToken", player.LoginToken),
        //    new SqliteParameter("@exp", player.Exp),
        //    new SqliteParameter("@selectedFormation", player.SelectedFormation),
        //    new SqliteParameter("@prefs", "{}"));
        return null;
    }

    private bool TryGetPlayer(string type, string username, string password, out IPlayer player)
    {
        player = null;
        //IPlayerAuth auther = IPlayerAuth.DataMap.Values.ToList()
        //    .Find(x => x.type == type && x.username == username && x.password == password);
        //if (auther == null)
        //{
        //    return false;
        //}
        //player = IPlayer.DataMap.Values.ToList().Find(x => x.guid == auther.playerId);
        //if (player == null)
        //{
        //    return false;
        //}
        //player = null;
        //var playerAuths = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerAuth WHERE type=@type AND username=@username AND password=@password",
        //    new SqliteParameter("@type", type),
        //    new SqliteParameter("@username", username),
        //    new SqliteParameter("@password", password));
        //if (!playerAuths.Read())
        //    return false;
        //var playerAuth = new PlayerAuth();
        //playerAuth.Id = playerAuths.GetString(0);
        //playerAuth.PlayerId = playerAuths.GetString(1);
        //playerAuth.Type = playerAuths.GetString(2);
        //playerAuth.Username = playerAuths.GetString(3);
        //playerAuth.Password = playerAuths.GetString(4);
        //var players = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM player WHERE id=@id",
        //    new SqliteParameter("@id", playerAuth.PlayerId));
        //if (players.Read())
        //{
        //    player = new Player();
        //    player.Id = players.GetString(0);
        //    player.ProfileName = players.GetString(1);
        //    player.LoginToken = players.GetString(2);
        //    player.Exp = players.GetInt32(3);
        //    player.SelectedFormation = players.GetString(4);
        //}
        //if (player == null)
        //    return false;
        return true;
    }

    //private IPlayer UpdatePlayerLoginToken(IPlayer player)
    //{
    //    //player.loginToken = System.Guid.NewGuid().ToString();
    //    //GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE player SET loginToken=@loginToken WHERE id=@id",
    //    //    new SqliteParameter("@loginToken", player.loginToken),
    //    //    new SqliteParameter("@id", player.guid));
    //    //return player;
    //}

    public IPlayer GetPlayerByLoginToken(string playerId, string loginToken)
    {
        IPlayer player = null;
        //player = IPlayer.DataMap.Values.ToList().Find(x => x.loginToken == loginToken && x.guid == playerId);
        return player;
    }
    #endregion
}
