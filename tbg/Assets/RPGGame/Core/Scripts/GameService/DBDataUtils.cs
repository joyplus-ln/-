using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
public class DbRowsReader
{
    private readonly List<List<object>> data = new List<List<object>>();
    private int currentRow = -1;
    public int FieldCount { get; private set; }
    public int VisibleFieldCount { get; private set; }
    public int RowCount { get { return data.Count; } }
    public bool HasRows { get { return RowCount > 0; } }

    public void Init(SqliteDataReader sqliteDataReader)
    {
        FieldCount = sqliteDataReader.FieldCount;
        VisibleFieldCount = sqliteDataReader.VisibleFieldCount;
        while (sqliteDataReader.Read())
        {
            var buffer = new object[sqliteDataReader.FieldCount];
            sqliteDataReader.GetValues(buffer);
            data.Add(new List<object>(buffer));
        }
        ResetReader();
    }

    public bool Read()
    {
        if (currentRow + 1 >= RowCount)
            return false;
        ++currentRow;
        return true;
    }

    public System.DateTime GetDateTime(int index)
    {
        return (System.DateTime)data[currentRow][index];
    }

    public char GetChar(int index)
    {
        return (char)data[currentRow][index];
    }

    public string GetString(int index)
    {
        return (string)data[currentRow][index];
    }

    public bool GetBoolean(int index)
    {
        return (bool)data[currentRow][index];
    }

    public short GetInt16(int index)
    {
        return (short)((long)data[currentRow][index]);
    }

    public int GetInt32(int index)
    {
        return (int)((long)data[currentRow][index]);
    }

    public long GetInt64(int index)
    {
        return (long)data[currentRow][index];
    }

    public decimal GetDecimal(int index)
    {
        return (decimal)((double)data[currentRow][index]);
    }

    public float GetFloat(int index)
    {
        return (float)(data[currentRow][index]);
    }

    public double GetDouble(int index)
    {
        return (double)data[currentRow][index];
    }

    public void ResetReader()
    {
        currentRow = -1;
    }
}
public class DBDataUtils
{

    public string dbPath = "./tbRpgDb.sqlite3";

    public static SqliteConnection connection;
    public SqliteUtils sqliteUtils;

    public void Init()
    {
        if (Application.isMobilePlatform)
        {
            if (dbPath.StartsWith("./"))
                dbPath = dbPath.Substring(1);
            if (!dbPath.StartsWith("/"))
                dbPath = "/" + dbPath;
            dbPath = Application.persistentDataPath + dbPath;
        }
        sqliteUtils = new SqliteUtils();
        if (!File.Exists(dbPath))
            SqliteConnection.CreateFile(dbPath);

        // open connection
        connection = new SqliteConnection("URI=file:" + dbPath);

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS player (
            id TEXT NOT NULL PRIMARY KEY,
            profileName TEXT NOT NULL,
            loginToken TEXT NOT NULL,
            exp INTEGER NOT NULL,
            selectedFormation TEXT NOT NULL,
            prefs TEXT NOT NULL
            )");

        //ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerItem (
        //    id TEXT NOT NULL PRIMARY KEY,
        //    playerId TEXT NOT NULL,
        //    Guid TEXT NOT NULL,
        //    amount INTEGER NOT NULL,
        //    exp INTEGER NOT NULL,
        //    equipItemId TEXT NOT NULL,
        //    equipPosition TEXT NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerAuth (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            type TEXT NOT NULL,
            username TEXT NOT NULL,
            password TEXT NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerCurrency (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            amount INTEGER NOT NULL,
            purchasedAmount INTEGER NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerStamina (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            amount INTEGER NOT NULL,
            recoveredTime INTEGER NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerFormation (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            position INTEGER NOT NULL,
            itemId TEXT NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerUnlockItem (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            amount INTEGER NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerClearStage (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            bestRating INTEGER NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerBattle (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            session TEXT NOT NULL,
            battleResult INTEGER NOT NULL,
            rating INTEGER NOT NULL)");


        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerOtherItem (
            id TEXT NOT NULL PRIMARY KEY,
            Guid TEXT NOT NULL,
            playerId TEXT NOT NULL,
            amount INTEGER NOT NULL)");


        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerHasCharacters (
            id TEXT PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            amount INTEGER NOT NULL,
            exp INTEGER NOT NULL,
            equipItemId TEXT NOT NULL,
            equipPosition TEXT NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerHasEquips (
            id TEXT PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            amount INTEGER NOT NULL,
            exp INTEGER NOT NULL,
            equipItemId TEXT NOT NULL,
            equipPosition TEXT NOT NULL)");

    }

    public const string AUTH_NORMAL = "NORMAL";
    public const string AUTH_GUEST = "GUEST";
    public const ushort BATTLE_RESULT_NONE = 0;
    public const ushort BATTLE_RESULT_LOSE = 1;
    public const ushort BATTLE_RESULT_WIN = 2;



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
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = ExecuteReader(@"SELECT * FROM playerAuth WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
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


    public void DoGetOtherItemList(UnityAction<OtherItemListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new OtherItemListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = ExecuteReader(@"SELECT * FROM playerOtherItem WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
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


    public void DoGetCurrencyList(UnityAction<CurrencyListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new CurrencyListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = ExecuteReader(@"SELECT * FROM playerCurrency WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
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
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = ExecuteReader(@"SELECT * FROM playerStamina WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
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

    public void DoGetFormationList(UnityAction<FormationListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new FormationListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
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

    public void DoGetUnlockItemList(UnityAction<UnlockItemListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new UnlockItemListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = ExecuteReader(@"SELECT * FROM playerUnlockItem WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
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

    public void DoGetClearStageList(UnityAction<ClearStageListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new ClearStageListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
        {
            var reader = ExecuteReader(@"SELECT * FROM playerClearStage WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
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



    /// <summary>
    /// 取用户存档
    /// </summary>
    public void GetPlayerLocalInfo()
    {
        var reader = ExecuteReader(@"SELECT * FROM player WHERE id=@playerId", new SqliteParameter("@playerId", Player.CurrentPlayer.Id));
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
        ExecuteNonQuery(@"UPDATE player SET prefs=@prefs WHERE id=@id",
            new SqliteParameter("@prefs", JsonConvert.SerializeObject(PlayerSQLPrefs.localConfig)),
            new SqliteParameter("@id", Player.CurrentPlayerId));
    }


    #region 数据库操作

    public void ExecuteNonQuery(string sql, params SqliteParameter[] args)
    {
        connection.Open();
        using (var cmd = new SqliteCommand(sql, connection))
        {
            foreach (var arg in args)
            {
                cmd.Parameters.Add(arg);
            }
            cmd.ExecuteNonQuery();
        }
        connection.Close();
    }

    public object ExecuteScalar(string sql, params SqliteParameter[] args)
    {
        object result;
        connection.Open();
        using (var cmd = new SqliteCommand(sql, connection))
        {
            foreach (var arg in args)
            {
                cmd.Parameters.Add(arg);
            }
            result = cmd.ExecuteScalar();
        }
        connection.Close();
        return result;
    }

    public DbRowsReader ExecuteReader(string sql, params SqliteParameter[] args)
    {
        DbRowsReader result = new DbRowsReader();
        connection.Open();
        using (var cmd = new SqliteCommand(sql, connection))
        {
            foreach (var arg in args)
            {
                cmd.Parameters.Add(arg);
            }
            result.Init(cmd.ExecuteReader());
        }
        connection.Close();
        return result;
    }

    #endregion


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
                UpdatePlayerStamina(player);
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
                UpdatePlayerStamina(player);
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
            UpdatePlayerStamina(player);
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
        var playerWithName = ExecuteScalar("SELECT COUNT(*) FROM player WHERE profileName=@profileName", new SqliteParameter("profileName", profileName));
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (string.IsNullOrEmpty(profileName))
            result.error = GameServiceErrorCode.EMPTY_PROFILE_NAME;
        else if (playerWithName != null && (long)playerWithName > 0)
            result.error = GameServiceErrorCode.EXISTED_PROFILE_NAME;
        else
        {
            player.ProfileName = profileName;
            ExecuteNonQuery(@"UPDATE player SET profileName=@profileName WHERE id=@id",
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

    #region 帮助类

    private bool IsPlayerWithUsernameFound(string type, string username)
    {
        var count = ExecuteScalar(@"SELECT COUNT(*) FROM playerAuth WHERE type=@type AND username=@username",
            new SqliteParameter("@type", type),
            new SqliteParameter("@username", username));
        return count != null && (long)count > 0;
    }

    private Player SetNewPlayerData(Player player)
    {
        player.LoginToken = System.Guid.NewGuid().ToString();
        player.Exp = 0;

        var gameDb = GameInstance.GameDatabase;
        var softCurrencyTable = gameDb.softCurrency;
        var hardCurrencyTable = gameDb.hardCurrency;

        var formationName = gameDb.stageFormations[0].id;
        player.SelectedFormation = formationName;

        var softCurrency = GetCurrency(player.Id, softCurrencyTable.id);
        var hardCurrency = GetCurrency(player.Id, hardCurrencyTable.id);
        softCurrency.Amount = softCurrencyTable.startAmount + softCurrency.PurchasedAmount;
        hardCurrency.Amount = hardCurrencyTable.startAmount + hardCurrency.PurchasedAmount;

        ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
            new SqliteParameter("@amount", softCurrency.Amount),
            new SqliteParameter("@id", softCurrency.Id));
        ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
            new SqliteParameter("@amount", hardCurrency.Amount),
            new SqliteParameter("@id", hardCurrency.Id));

        ExecuteNonQuery(@"DELETE FROM playerClearStage WHERE playerId=@playerId",
            new SqliteParameter("@playerId", player.Id));
        ExecuteNonQuery(@"DELETE FROM playerFormation WHERE playerId=@playerId",
            new SqliteParameter("@playerId", player.Id));
        ExecuteNonQuery(@"DELETE FROM playerStamina WHERE playerId=@playerId",
            new SqliteParameter("@playerId", player.Id));
        ExecuteNonQuery(@"DELETE FROM playerUnlockItem WHERE playerId=@playerId",
            new SqliteParameter("@playerId", player.Id));
        //todo 插入新玩家数据
        InsertStartPlayerCharacter(player);
        //for (var i = 0; i < gameDb.startCharacters.Count; ++i)
        //{
        //    var startCharacter = gameDb.startCharacters[i];
        //    if (startCharacter == null)
        //        continue;
        //    var createItems = new List<PlayerItem>();
        //    var updateItems = new List<PlayerItem>();
        //    if (AddItems(player.Id, startCharacter.guid, 1, out createItems, out updateItems))
        //    {
        //        foreach (var createEntry in createItems)
        //        {
        //            createEntry.Id = System.Guid.NewGuid().ToString();
        //            ExecuteNonQuery(@"INSERT INTO playerItem (id, playerId, Guid, amount, exp, equipItemId, equipPosition) VALUES (@id, @playerId, @Guid, @amount, @exp, @equipItemId, @equipPosition)",
        //                new SqliteParameter("@id", createEntry.Id),
        //                new SqliteParameter("@playerId", createEntry.PlayerId),
        //                new SqliteParameter("@Guid", createEntry.GUID),
        //                new SqliteParameter("@amount", createEntry.Amount),
        //                new SqliteParameter("@exp", createEntry.Exp),
        //                new SqliteParameter("@equipItemId", createEntry.EquipItemId),
        //                new SqliteParameter("@equipPosition", createEntry.EquipPosition));
        //            HelperUnlockItem(player.Id, startCharacter.guid);
        //            HelperSetFormation(player.Id, createEntry.Id, formationName, i);
        //        }
        //        foreach (var updateEntry in updateItems)
        //        {
        //            ExecuteNonQuery(@"UPDATE playerItem SET playerId=@playerId, Guid=@Guid, amount=@amount, exp=@exp, equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
        //                new SqliteParameter("@playerId", updateEntry.PlayerId),
        //                new SqliteParameter("@Guid", updateEntry.GUID),
        //                new SqliteParameter("@amount", updateEntry.Amount),
        //                new SqliteParameter("@exp", updateEntry.Exp),
        //                new SqliteParameter("@equipItemId", updateEntry.EquipItemId),
        //                new SqliteParameter("@equipPosition", updateEntry.EquipPosition),
        //                new SqliteParameter("@id", updateEntry.Id));
        //        }
        //    }
        //}
        ExecuteNonQuery(@"UPDATE player SET profileName=@profileName, loginToken=@loginToken, exp=@exp, selectedFormation=@selectedFormation WHERE id=@id",
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
        ExecuteNonQuery(@"INSERT INTO playerAuth (id, playerId, type, username, password) VALUES (@id, @playerId, @type, @username, @password)",
            new SqliteParameter("@id", playerAuth.Id),
            new SqliteParameter("@playerId", playerAuth.PlayerId),
            new SqliteParameter("@type", playerAuth.Type),
            new SqliteParameter("@username", playerAuth.Username),
            new SqliteParameter("@password", playerAuth.Password)
            );
        var player = new Player();
        player.Id = playerId;
        player = SetNewPlayerData(player);
        UpdatePlayerStamina(player);
        ExecuteNonQuery(@"INSERT INTO player (id, profileName, loginToken, exp, selectedFormation,prefs) VALUES (@id, @profileName, @loginToken, @exp, @selectedFormation,@prefs)",
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
        var playerAuths = ExecuteReader(@"SELECT * FROM playerAuth WHERE type=@type AND username=@username AND password=@password",
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
        var players = ExecuteReader(@"SELECT * FROM player WHERE id=@id",
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
        ExecuteNonQuery(@"UPDATE player SET loginToken=@loginToken WHERE id=@id",
            new SqliteParameter("@loginToken", player.loginToken),
            new SqliteParameter("@id", player.Id));
        return player;
    }

    private bool DecreasePlayerStamina(Player player, Stamina staminaTable, int decreaseAmount)
    {
        var stamina = GetStamina(player.Id, staminaTable.id);
        if (stamina.Amount >= decreaseAmount)
        {
            stamina.Amount -= decreaseAmount;
            ExecuteNonQuery(@"UPDATE playerStamina SET amount=@amount WHERE id=@id",
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
            ExecuteNonQuery(@"UPDATE playerStamina SET amount=@amount, recoveredTime=@recoveredTime WHERE id=@id",
                new SqliteParameter("@amount", stamina.Amount),
                new SqliteParameter("@recoveredTime", stamina.RecoveredTime),
                new SqliteParameter("@id", stamina.Id));
        }
    }

    private void UpdatePlayerStamina(Player player)
    {
        var gameDb = GameInstance.GameDatabase;
        var stageStaminaTable = gameDb.stageStamina;
        UpdatePlayerStamina(player, stageStaminaTable);
    }


    private PlayerStamina GetStamina(string playerId, string dataId)
    {
        var staminas = ExecuteReader(@"SELECT * FROM playerStamina WHERE playerId=@playerId AND Guid=@Guid LIMIT 1",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@Guid", dataId));
        PlayerStamina stamina = null;
        if (!staminas.Read())
        {
            stamina = new PlayerStamina();
            stamina.Id = PlayerStamina.GetId(playerId, dataId);
            stamina.PlayerId = playerId;
            stamina.DataId = dataId;
            ExecuteNonQuery(@"INSERT INTO playerStamina (id, playerId, Guid, amount, recoveredTime) VALUES (@id, @playerId, @Guid, @amount, @recoveredTime)",
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

    //playerHasCharacters
    //private bool AddItems1(string playerId,
    //    string Guid,
    //    int amount,
    //    out List<PlayerItem> createItems,
    //    out List<PlayerItem> updateItems)
    //{
    //    createItems = new List<PlayerItem>();
    //    updateItems = new List<PlayerItem>();
    //    CharacterItem itemData = null;
    //    if (!GameInstance.GameDatabase.characters.TryGetValue(Guid, out itemData))
    //        return false;
    //    var maxStack = itemData.MaxStack;
    //    var oldEntries = ExecuteReader(@"SELECT * FROM playerItem WHERE playerId=@playerId AND Guid=@Guid AND amount<@amount",
    //        new SqliteParameter("@playerId", playerId),
    //        new SqliteParameter("@Guid", Guid),
    //        new SqliteParameter("@amount", maxStack));
    //    while (oldEntries.Read())
    //    {
    //        var entry = new PlayerItem();
    //        entry.Id = oldEntries.GetString(0);
    //        entry.PlayerId = oldEntries.GetString(1);
    //        entry.GUID = oldEntries.GetString(2);
    //        entry.Amount = oldEntries.GetInt32(3);
    //        entry.Exp = oldEntries.GetInt32(4);
    //        entry.EquipItemId = oldEntries.GetString(5);
    //        entry.EquipPosition = oldEntries.GetString(6);
    //        var sumAmount = entry.Amount + amount;
    //        if (sumAmount > maxStack)
    //        {
    //            entry.Amount = maxStack;
    //            amount = sumAmount - maxStack;
    //        }
    //        else
    //        {
    //            entry.Amount += amount;
    //            amount = 0;
    //        }
    //        updateItems.Add(entry);

    //        if (amount == 0)
    //            break;
    //    }
    //    while (amount > 0)
    //    {
    //        var newEntry = new PlayerItem();
    //        newEntry.PlayerId = playerId;
    //        newEntry.GUID = Guid;
    //        if (amount > maxStack)
    //        {
    //            newEntry.Amount = maxStack;
    //            amount -= maxStack;
    //        }
    //        else
    //        {
    //            newEntry.Amount = amount;
    //            amount = 0;
    //        }
    //        createItems.Add(newEntry);
    //    }
    //    return true;
    //}

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
        var materials = ExecuteReader(@"SELECT * FROM playerItem WHERE playerId=@playerId AND Guid=@Guid",
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

    private void HelperSetFormation(string playerId, string characterId, string formationName, int position)
    {
        PlayerFormation oldFormation = null;
        if (!string.IsNullOrEmpty(characterId))
        {
            var oldFormations = ExecuteReader(@"SELECT * FROM playerFormation WHERE playerId=@playerId AND Guid=@Guid AND itemId=@itemId LIMIT 1",
                new SqliteParameter("@playerId", playerId),
                new SqliteParameter("@Guid", formationName),
                new SqliteParameter("@itemId", characterId));
            if (oldFormations.Read())
            {
                oldFormation = new PlayerFormation();
                oldFormation.Id = oldFormations.GetString(0);
                oldFormation.PlayerId = oldFormations.GetString(1);
                oldFormation.DataId = oldFormations.GetString(2);
                oldFormation.Position = oldFormations.GetInt32(3);
                oldFormation.ItemId = oldFormations.GetString(4);
            }
            if (oldFormation != null)
            {
                ExecuteNonQuery(@"UPDATE playerFormation SET itemId=@itemId WHERE id=@id",
                    new SqliteParameter("@itemId", oldFormation.ItemId),
                    new SqliteParameter("@id", oldFormation.Id));
            }
        }
        PlayerFormation formation = null;
        var targetFormations = ExecuteReader(@"SELECT * FROM playerFormation WHERE playerId=@playerId AND Guid=@Guid AND position=@position LIMIT 1",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@Guid", formationName),
            new SqliteParameter("@position", position));
        if (targetFormations.Read())
        {
            formation = new PlayerFormation();
            formation.Id = targetFormations.GetString(0);
            formation.PlayerId = targetFormations.GetString(1);
            formation.DataId = targetFormations.GetString(2);
            formation.Position = targetFormations.GetInt32(3);
            formation.ItemId = targetFormations.GetString(4);
        }
        if (formation == null)
        {
            formation = new PlayerFormation();
            formation.Id = PlayerFormation.GetId(playerId, formationName, position);
            formation.PlayerId = playerId;
            formation.DataId = formationName;
            formation.Position = position;
            formation.ItemId = characterId;
            ExecuteNonQuery(@"INSERT INTO playerFormation (id, playerId, Guid, position, itemId)
                VALUES (@id, @playerId, @Guid, @position, @itemId)",
                new SqliteParameter("@id", formation.Id),
                new SqliteParameter("@playerId", formation.PlayerId),
                new SqliteParameter("@Guid", formation.DataId),
                new SqliteParameter("@position", formation.Position),
                new SqliteParameter("@itemId", formation.ItemId));
        }
        else
        {
            if (oldFormation != null)
            {
                oldFormation.ItemId = formation.ItemId;
                ExecuteNonQuery(@"UPDATE playerFormation SET itemId=@itemId WHERE id=@id",
                    new SqliteParameter("@itemId", oldFormation.ItemId),
                    new SqliteParameter("@id", oldFormation.Id));
            }
            formation.ItemId = characterId;
            ExecuteNonQuery(@"UPDATE playerFormation SET itemId=@itemId WHERE id=@id",
                new SqliteParameter("@itemId", formation.ItemId),
                new SqliteParameter("@id", formation.Id));
        }
    }

    //private void HelperUnlockItem(string playerId, string dataId)
    //{
    //    PlayerUnlockItem unlockItem = null;
    //    var oldUnlockItems = ExecuteReader(@"SELECT * FROM playerUnlockItem WHERE playerId=@playerId AND Guid=@Guid LIMIT 1",
    //        new SqliteParameter("@playerId", playerId),
    //        new SqliteParameter("@Guid", dataId));
    //    if (!oldUnlockItems.Read())
    //    {
    //        unlockItem = new PlayerUnlockItem();
    //        unlockItem.Id = PlayerUnlockItem.GetId(playerId, dataId);
    //        unlockItem.PlayerId = playerId;
    //        unlockItem.DataId = dataId;
    //        unlockItem.Amount = 0;
    //        ExecuteNonQuery(@"INSERT INTO playerUnlockItem (id, playerId, Guid, amount)
    //            VALUES (@id, @playerId, @Guid, @amount)",
    //            new SqliteParameter("@id", unlockItem.Id),
    //            new SqliteParameter("@playerId", unlockItem.PlayerId),
    //            new SqliteParameter("@Guid", unlockItem.DataId),
    //            new SqliteParameter("@amount", unlockItem.Amount));
    //    }
    //}

    private PlayerClearStage HelperClearStage(string playerId, string dataId, int grade)
    {
        PlayerClearStage clearStage = null;
        var clearStages = ExecuteReader(@"SELECT * FROM playerClearStage WHERE playerId=@playerId AND Guid=@Guid LIMIT 1",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@Guid", dataId));
        if (!clearStages.Read())
        {
            clearStage = new PlayerClearStage();
            clearStage.Id = PlayerClearStage.GetId(playerId, dataId);
            clearStage.PlayerId = playerId;
            clearStage.DataId = dataId;
            clearStage.BestRating = grade;
            ExecuteNonQuery(@"INSERT INTO playerClearStage (id, playerId, Guid, bestRating)
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
                ExecuteNonQuery(@"UPDATE playerClearStage SET bestRating=@bestRating WHERE id=@id",
                    new SqliteParameter("@bestRating", clearStage.BestRating),
                    new SqliteParameter("@id", clearStage.Id));
            }
        }
        return clearStage;
    }

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

    private PlayerBattle GetPlayerBattleBySession(string playerId, string session)
    {
        PlayerBattle playerBattle = null;
        var playerBattles = ExecuteReader(@"SELECT * FROM playerBattle WHERE playerId=@playerId AND session=@session",
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

    //private PlayerItem GetPlayerItemById(string id)
    //{
    //    PlayerItem playerItem = null;
    //    var playerItems = ExecuteReader(@"SELECT * FROM playerItem WHERE id=@id",
    //        new SqliteParameter("@id", id));
    //    if (playerItems.Read())
    //    {
    //        playerItem = new PlayerItem();
    //        playerItem.Id = playerItems.GetString(0);
    //        playerItem.PlayerId = playerItems.GetString(1);
    //        playerItem.GUID = playerItems.GetString(2);
    //        playerItem.Amount = playerItems.GetInt32(3);
    //        playerItem.Exp = playerItems.GetInt32(4);
    //        playerItem.EquipItemId = playerItems.GetString(5);
    //        playerItem.EquipPosition = playerItems.GetString(6);
    //    }
    //    return playerItem;
    //}

    private PlayerItem GetPlayerItemByEquipper(string playerId, string equipItemId, string equipPosition)
    {
        PlayerItem playerItem = null;
        var playerItems = ExecuteReader(@"SELECT * FROM playerHasEquips WHERE playerId=@playerId AND equipItemId=@equipItemId AND equipPosition=@equipPosition",
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

    #region 战斗
    public void DoSelectFormation(string formationName, UnityAction<PlayerResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
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

    public void DoSetFormation(string characterId, string formationName, int position, UnityAction<FormationListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new FormationListResult();
        var player = GetPlayerByLoginToken(playerId, loginToken);
        PlayerItem character = null;
        if (!string.IsNullOrEmpty(characterId))
            character = GetPlayerCharacterItemById(characterId);
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

    public void DoStartStage(string stageDataId, UnityAction<StartStageResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
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

    public void DoStartTowerStage(string stageDataId, UnityAction<StartStageResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
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

    public void DoFinishStage(Const.StageType stagetype, string session, ushort battleResult, int deadCharacters, UnityAction<FinishStageResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;

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
                        var character = GetPlayerCharacterItemById(itemId);
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
    #endregion

    #region 商城

    public void DoEquipItem(string characterId, string equipmentId, string equipPosition, UnityAction<ItemResult> onFinish)
    {
        var player = Player.CurrentPlayer;
        var playerId = player.Id;
        var loginToken = player.LoginToken;
        var result = new ItemResult();
        var foundPlayer = GetPlayerByLoginToken(playerId, loginToken);
        var foundCharacter = GetPlayerCharacterItemById(characterId);
        var foundEquipment = GetPlayerEquipmentItemById(equipmentId);
        CharacterItem characterData = null;
        EquipmentItem equipmentData = null;
        if (foundCharacter != null)
            characterData = foundCharacter.CharacterData;
        if (foundEquipment != null)
            equipmentData = foundEquipment.EquipmentData;
        if (foundPlayer == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (foundCharacter == null || foundCharacter.PlayerId != playerId || foundEquipment == null || foundEquipment.PlayerId != playerId)
            result.error = GameServiceErrorCode.INVALID_PLAYER_ITEM_DATA;
        else if (characterData == null || equipmentData == null)
            result.error = GameServiceErrorCode.INVALID_ITEM_DATA;
        else if (!equipmentData.equippablePositions.Contains(equipPosition))
            result.error = GameServiceErrorCode.INVALID_EQUIP_POSITION;
        else
        {
            result.updateItems = new List<PlayerItem>();
            var unEquipItem = GetPlayerItemByEquipper(playerId, characterId, equipPosition);
            if (unEquipItem != null)
            {
                unEquipItem.EquipItemId = "";
                unEquipItem.EquipPosition = "";
                ExecuteNonQuery(@"UPDATE playerItem SET equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
                    new SqliteParameter("@equipItemId", unEquipItem.EquipItemId),
                    new SqliteParameter("@equipPosition", unEquipItem.EquipPosition),
                    new SqliteParameter("@id", unEquipItem.SqLiteIndex));
                result.updateItems.Add(unEquipItem);
            }
            foundEquipment.EquipItemId = characterId;
            foundEquipment.EquipPosition = equipPosition;
            ExecuteNonQuery(@"UPDATE playerItem SET equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
                new SqliteParameter("@equipItemId", foundEquipment.EquipItemId),
                new SqliteParameter("@equipPosition", foundEquipment.EquipPosition),
                new SqliteParameter("@id", foundEquipment.SqLiteIndex));
            result.updateItems.Add(foundEquipment);
        }
        onFinish(result);
    }

    public void DoUnEquipItem(string equipmentId, UnityAction<ItemResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new ItemResult();
        var player = GetPlayerByLoginToken(playerId, loginToken);
        var unEquipItem = GetPlayerEquipmentItemById(equipmentId);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (unEquipItem == null || unEquipItem.PlayerId != playerId)
            result.error = GameServiceErrorCode.INVALID_PLAYER_ITEM_DATA;
        else
        {
            result.updateItems = new List<PlayerItem>();
            unEquipItem.EquipItemId = "";
            unEquipItem.EquipPosition = "";
            ExecuteNonQuery(@"UPDATE playerItem SET equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
                new SqliteParameter("@equipItemId", unEquipItem.EquipItemId),
                new SqliteParameter("@equipPosition", unEquipItem.EquipPosition),
                new SqliteParameter("@id", unEquipItem.SqLiteIndex));
            result.updateItems.Add(unEquipItem);
        }
        onFinish(result);
    }

    public void DoGetAvailableLootBoxList(UnityAction<AvailableLootBoxListResult> onFinish)
    {
        var result = new AvailableLootBoxListResult();
        var gameDb = GameInstance.GameDatabase;
        result.list.AddRange(gameDb.LootBoxes.Keys);
        onFinish(result);
    }

    public void DoOpenLootBox(string lootBoxDataId, int packIndex, UnityAction<ItemResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
        var result = new ItemResult();
        var gameDb = GameInstance.GameDatabase;
        var player = GetPlayerByLoginToken(playerId, loginToken);
        LootBox lootBox;
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (!gameDb.LootBoxes.TryGetValue(lootBoxDataId, out lootBox))
            result.error = GameServiceErrorCode.INVALID_LOOT_BOX_DATA;
        else
        {
            var softCurrency = GetCurrency(playerId, gameDb.softCurrency.id);
            var hardCurrency = GetCurrency(playerId, gameDb.hardCurrency.id);
            var requirementType = lootBox.requirementType;
            if (packIndex > lootBox.lootboxPacks.Length - 1)
                packIndex = 0;
            var pack = lootBox.lootboxPacks[packIndex];
            var price = pack.price;
            var openAmount = pack.openAmount;
            if (requirementType == LootBoxRequirementType.RequireSoftCurrency && price > softCurrency.Amount)
                result.error = GameServiceErrorCode.NOT_ENOUGH_SOFT_CURRENCY;
            else if (requirementType == LootBoxRequirementType.RequireHardCurrency && price > hardCurrency.Amount)
                result.error = GameServiceErrorCode.NOT_ENOUGH_HARD_CURRENCY;
            else
            {
                switch (requirementType)
                {
                    case LootBoxRequirementType.RequireSoftCurrency:
                        softCurrency.Amount -= price;
                        ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                            new SqliteParameter("@amount", softCurrency.Amount),
                            new SqliteParameter("@id", softCurrency.Id));
                        result.updateCurrencies.Add(softCurrency);
                        break;
                    case LootBoxRequirementType.RequireHardCurrency:
                        hardCurrency.Amount -= price;
                        ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                            new SqliteParameter("@amount", hardCurrency.Amount),
                            new SqliteParameter("@id", hardCurrency.Id));
                        result.updateCurrencies.Add(hardCurrency);
                        break;
                }

                for (var i = 0; i < openAmount; ++i)
                {
                    var rewardItem = lootBox.RandomReward().rewardItem;
                    var createItems = new List<PlayerItem>();
                    var updateItems = new List<PlayerItem>();
                    //todo 开商城的数据
                    //if (AddItems(playerId, rewardItem.Id, rewardItem.amount, out createItems, out updateItems))
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
                    //        result.updateItems.Add(updateEntry);
                    //    }
                    //}
                }
            }
        }
        onFinish(result);
    }

    #endregion


    #region 其他东西

    public void AddOtherItem(string id, int amount)//Guid
    {
        ExecuteNonQuery(@"INSERT INTO playerOtherItem (id,playerId,Guid,amount) VALUES (@id,@playerId,@Guid,@amount)",
            new SqliteParameter("@id", id),
            new SqliteParameter("@playerId", Player.CurrentPlayerId),
            new SqliteParameter("@Guid", "otherItem" + System.Guid.NewGuid()),
            new SqliteParameter("@amount", amount));

    }

    public void DpdateOtherItem(string id, int amount)
    {
        ExecuteNonQuery(@"UPDATE playerOtherItem SET amount=@amount WHERE id=@id AND playerId=@playerId",
            new SqliteParameter("@amount", amount),
            new SqliteParameter("@playerId", Player.CurrentPlayerId),
            new SqliteParameter("@id", id));
    }

    public void DeleteOtherItem(string id)
    {
        ExecuteNonQuery(@"DELETE FROM playerOtherItem WHERE id=@id",
            new SqliteParameter("@id", id),
            new SqliteParameter("@playerId", Player.CurrentPlayerId));
    }

    #endregion

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
    public void DoCharacterLevelUpItem(string itemId, Dictionary<string, int> materials, UnityAction<ItemResult> onFinish)
    {
        var player = Player.CurrentPlayer;
        var playerId = player.Id;
        var loginToken = player.LoginToken;
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
    public void DoEquipmentLevelUpItem(string itemId, Dictionary<string, int> materials, UnityAction<ItemResult> onFinish)
    {
        var player = Player.CurrentPlayer;
        var playerId = player.Id;
        var loginToken = player.LoginToken;
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

    public void DoSellCharacterItems(Dictionary<string, int> items, UnityAction<ItemResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
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
    public void DoGetItemList(UnityAction<ItemListResult> onFinish)
    {
        var cplayer = Player.CurrentPlayer;
        var playerId = cplayer.Id;
        var loginToken = cplayer.LoginToken;
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


    #endregion
}
