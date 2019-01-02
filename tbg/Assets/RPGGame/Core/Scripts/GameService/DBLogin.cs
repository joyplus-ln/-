using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DBLogin
{
    public const string AUTH_NORMAL = "NORMAL";
    public const string AUTH_GUEST = "GUEST";
    public DBLogin()
    {

    }

    public void SetPrefsLogin(string playerId, string loginToken)
    {
        PlayerPrefs.SetString("PLAYER_ID", playerId);
        PlayerPrefs.SetString("LOGIN_TOKEN", loginToken);
        PlayerPrefs.Save();
    }

    public string GetPrefsPlayerId()
    {
        return PlayerPrefs.GetString("PLAYER_ID");
    }

    public string GetPrefsLoginToken()
    {
        return PlayerPrefs.GetString("LOGIN_TOKEN");
    }

    public void Logout(UnityAction onLogout = null)
    {
        Player.CurrentPlayer = null;
        Player.ClearData();
        PlayerAuth.ClearData();
        PlayerCurrency.ClearData();
        PlayerFormation.ClearData();
        PlayerItem.ClearData();
        PlayerOtherItem.ClearData();
        PlayerStamina.ClearData();
        PlayerUnlockItem.ClearData();
        SetPrefsLogin("", "");
        onLogout();
    }

    public void DoGetAuthList(UnityAction<AuthListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new AuthListResult();
        var player = GameInstance.dbDataUtils.ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerAuth WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
            var list = new List<PlayerAuth>();
            while (reader.Read())
            {
                var entry = new PlayerAuth();
                entry.Id = reader.GetString(0);
                entry.PlayerId = reader.GetString(1);
                entry.Type = reader.GetString(2);
                entry.Username = reader.GetString(3);
                entry.Password = reader.GetString(4);
                list.Add(entry);
            }
            result.list = list;
        }
        onFinish(result);
    }



    #region 登录相关

    public void DoLogin(string username, string password, UnityAction<PlayerResult> onFinish)
    {
        var result = new PlayerResult();
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            result.error = GameServiceErrorCode.EMPTY_USERNMAE_OR_PASSWORD;
        else
        {
            Player player = null;
            if (!TryGetPlayer(AUTH_NORMAL, username, password, out player))
                result.error = GameServiceErrorCode.INVALID_USERNMAE_OR_PASSWORD;
            else
            {
                player = UpdatePlayerLoginToken(player);
                GameInstance.dbPlayerData.UpdatePlayerStamina(player);
                result.player = player;
            }
        }
        onFinish(result);
    }

    public void DoRegisterOrLogin(string username, string password, UnityAction<PlayerResult> onFinish)
    {
        if (IsPlayerWithUsernameFound(AUTH_NORMAL, username))
            DoLogin(username, password, onFinish);
        else
            DoRegister(username, password, onFinish);
    }

    public void DoGuestLogin(string deviceId, UnityAction<PlayerResult> onFinish)
    {
        var result = new PlayerResult();
        if (string.IsNullOrEmpty(deviceId))
            result.error = GameServiceErrorCode.EMPTY_USERNMAE_OR_PASSWORD;
        else if (IsPlayerWithUsernameFound(AUTH_GUEST, deviceId))
        {
            Player player = null;
            if (!TryGetPlayer(AUTH_GUEST, deviceId, deviceId, out player))
                result.error = GameServiceErrorCode.INVALID_USERNMAE_OR_PASSWORD;
            else
            {
                player = UpdatePlayerLoginToken(player);
                GameInstance.dbPlayerData.UpdatePlayerStamina(player);
                result.player = player;
            }
        }
        else
        {
            var player = InsertNewPlayer(AUTH_GUEST, deviceId, deviceId);
            result.player = player;
        }
        onFinish(result);
    }

    public void DoValidateLoginToken(bool refreshToken, UnityAction<PlayerResult> onFinish)
    {
        var playerId = GetPrefsPlayerId();
        var loginToken = GetPrefsLoginToken();

        var result = new PlayerResult();
        var player = GetPlayerByLoginToken(playerId, loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            if (refreshToken)
                player = UpdatePlayerLoginToken(player);
            GameInstance.dbPlayerData.UpdatePlayerStamina(player);
            result.player = player;
        }
        onFinish(result);
    }

    public void DoSetProfileName(string profileName, UnityAction<PlayerResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new PlayerResult();
        var player = GetPlayerByLoginToken(playerId, loginToken);
        var playerWithName = GameInstance.dbDataUtils.ExecuteScalar("SELECT COUNT(*) FROM player WHERE profileName=@profileName", new SqliteParameter("profileName", profileName));
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (string.IsNullOrEmpty(profileName))
            result.error = GameServiceErrorCode.EMPTY_PROFILE_NAME;
        else if (playerWithName != null && (long)playerWithName > 0)
            result.error = GameServiceErrorCode.EXISTED_PROFILE_NAME;
        else
        {
            player.ProfileName = profileName;
            GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE player SET profileName=@profileName WHERE id=@id",
                new SqliteParameter("@profileName", player.ProfileName),
                new SqliteParameter("@id", player.Id));
            result.player = player;
        }
        onFinish(result);
    }

    public void DoRegister(string username, string password, UnityAction<PlayerResult> onFinish)
    {
        var result = new PlayerResult();
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            result.error = GameServiceErrorCode.EMPTY_USERNMAE_OR_PASSWORD;
        else if (IsPlayerWithUsernameFound(AUTH_NORMAL, username))
            result.error = GameServiceErrorCode.EXISTED_USERNAME;
        else
        {
            var player = InsertNewPlayer(AUTH_NORMAL, username, password);
            result.player = player;
        }
        onFinish(result);
    }

    #endregion



    private bool IsPlayerWithUsernameFound(string type, string username)
    {
        var count = GameInstance.dbDataUtils.ExecuteScalar(@"SELECT COUNT(*) FROM playerAuth WHERE type=@type AND username=@username",
            new SqliteParameter("@type", type),
            new SqliteParameter("@username", username));
        return count != null && (long)count > 0;
    }


    #region 帮助类


    private Player SetNewPlayerData(Player player)
    {
        player.LoginToken = System.Guid.NewGuid().ToString();
        player.Exp = 0;

        var gameDb = GameInstance.GameDatabase;
        var softCurrencyTable = gameDb.softCurrency;
        var hardCurrencyTable = gameDb.hardCurrency;

        var formationName = gameDb.stageFormations[0].id;
        player.SelectedFormation = formationName;

        var softCurrency = GameInstance.dbPlayerData.GetCurrency(player.Id, softCurrencyTable.id);
        var hardCurrency = GameInstance.dbPlayerData.GetCurrency(player.Id, hardCurrencyTable.id);
        softCurrency.Amount = softCurrencyTable.startAmount + softCurrency.PurchasedAmount;
        hardCurrency.Amount = hardCurrencyTable.startAmount + hardCurrency.PurchasedAmount;

        GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
            new SqliteParameter("@amount", softCurrency.Amount),
            new SqliteParameter("@id", softCurrency.Id));
        GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
            new SqliteParameter("@amount", hardCurrency.Amount),
            new SqliteParameter("@id", hardCurrency.Id));

        GameInstance.dbDataUtils.ExecuteNonQuery(@"DELETE FROM playerClearStage WHERE playerId=@playerId",
            new SqliteParameter("@playerId", player.Id));
        GameInstance.dbDataUtils.ExecuteNonQuery(@"DELETE FROM playerFormation WHERE playerId=@playerId",
            new SqliteParameter("@playerId", player.Id));
        GameInstance.dbDataUtils.ExecuteNonQuery(@"DELETE FROM playerStamina WHERE playerId=@playerId",
            new SqliteParameter("@playerId", player.Id));
        GameInstance.dbDataUtils.ExecuteNonQuery(@"DELETE FROM playerUnlockItem WHERE playerId=@playerId",
            new SqliteParameter("@playerId", player.Id));
        //todo 插入新玩家数据
        GameInstance.dbPlayerData.InsertStartPlayerCharacter(player);
        GameInstance.dbPlayerData.InsertStartEquiptem(player);
        GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE player SET profileName=@profileName, loginToken=@loginToken, exp=@exp, selectedFormation=@selectedFormation WHERE id=@id",
            new SqliteParameter("@profileName", player.ProfileName),
            new SqliteParameter("@loginToken", player.LoginToken),
            new SqliteParameter("@exp", player.Exp),
            new SqliteParameter("@selectedFormation", player.SelectedFormation),
            new SqliteParameter("@id", player.Id));
        return player;
    }
    /// <summary>
    /// 插入用户
    /// </summary>
    /// <param name="type"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    private Player InsertNewPlayer(string type, string username, string password)
    {
        var playerId = System.Guid.NewGuid().ToString();
        var playerAuth = new PlayerAuth();
        playerAuth.Id = PlayerAuth.GetId(playerId, type);
        playerAuth.PlayerId = playerId;
        playerAuth.Type = type;
        playerAuth.Username = username;
        playerAuth.Password = password;
        GameInstance.dbDataUtils.ExecuteNonQuery(@"INSERT INTO playerAuth (id, playerId, type, username, password) VALUES (@id, @playerId, @type, @username, @password)",
            new SqliteParameter("@id", playerAuth.Id),
            new SqliteParameter("@playerId", playerAuth.PlayerId),
            new SqliteParameter("@type", playerAuth.Type),
            new SqliteParameter("@username", playerAuth.Username),
            new SqliteParameter("@password", playerAuth.Password)
            );
        var player = new Player();
        player.Id = playerId;
        player = SetNewPlayerData(player);
        GameInstance.dbPlayerData.UpdatePlayerStamina(player);
        GameInstance.dbDataUtils.ExecuteNonQuery(@"INSERT INTO player (id, profileName, loginToken, exp, selectedFormation,prefs) VALUES (@id, @profileName, @loginToken, @exp, @selectedFormation,@prefs)",
            new SqliteParameter("@id", player.Id),
            new SqliteParameter("@profileName", player.ProfileName),
            new SqliteParameter("@loginToken", player.LoginToken),
            new SqliteParameter("@exp", player.Exp),
            new SqliteParameter("@selectedFormation", player.SelectedFormation),
            new SqliteParameter("@prefs", "{}"));
        return player;
    }

    private bool TryGetPlayer(string type, string username, string password, out Player player)
    {
        player = null;
        var playerAuths = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM playerAuth WHERE type=@type AND username=@username AND password=@password",
            new SqliteParameter("@type", type),
            new SqliteParameter("@username", username),
            new SqliteParameter("@password", password));
        if (!playerAuths.Read())
            return false;
        var playerAuth = new PlayerAuth();
        playerAuth.Id = playerAuths.GetString(0);
        playerAuth.PlayerId = playerAuths.GetString(1);
        playerAuth.Type = playerAuths.GetString(2);
        playerAuth.Username = playerAuths.GetString(3);
        playerAuth.Password = playerAuths.GetString(4);
        var players = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM player WHERE id=@id",
            new SqliteParameter("@id", playerAuth.PlayerId));
        if (players.Read())
        {
            player = new Player();
            player.Id = players.GetString(0);
            player.ProfileName = players.GetString(1);
            player.LoginToken = players.GetString(2);
            player.Exp = players.GetInt32(3);
            player.SelectedFormation = players.GetString(4);
        }
        if (player == null)
            return false;
        return true;
    }

    private Player UpdatePlayerLoginToken(Player player)
    {
        player.LoginToken = System.Guid.NewGuid().ToString();
        GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE player SET loginToken=@loginToken WHERE id=@id",
            new SqliteParameter("@loginToken", player.loginToken),
            new SqliteParameter("@id", player.Id));
        return player;
    }

    public Player GetPlayerByLoginToken(string playerId, string loginToken)
    {
        Debug.LogError("---" + playerId);
        Player player = null;
        var players = GameInstance.dbDataUtils.ExecuteReader(@"SELECT * FROM player WHERE id=@playerId AND loginToken=@loginToken",
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
    #endregion
}
