using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Mono.Data.Sqlite;
using Newtonsoft.Json;

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

public partial class SQLiteGameService : BaseGameService
{
    public string dbPath = "./tbRpgDb.sqlite3";

    public static SqliteConnection connection;


    private void Awake()
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


        GameInstance.GameDatabase.characters = GetSqliteCharacters();
        GameInstance.GameDatabase.equipments = GetSqliteEquipments();
    }


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

    protected override void DoGetAuthList(string playerId, string loginToken, UnityAction<AuthListResult> onFinish)
    {
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


    protected override void DoGetOtherItemList(string playerId, string loginToken, UnityAction<OtherItemListResult> onFinish)
    {
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

    protected override void DoGetCurrencyList(string playerId, string loginToken, UnityAction<CurrencyListResult> onFinish)
    {
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

    protected override void DoGetStaminaList(string playerId, string loginToken, UnityAction<StaminaListResult> onFinish)
    {
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

    protected override void DoGetFormationList(string playerId, string loginToken, UnityAction<FormationListResult> onFinish)
    {
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

    protected override void DoGetUnlockItemList(string playerId, string loginToken, UnityAction<UnlockItemListResult> onFinish)
    {
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

    protected override void DoGetClearStageList(string playerId, string loginToken, UnityAction<ClearStageListResult> onFinish)
    {
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

    public Dictionary<string, CharacterItem> GetSqliteCharacters()
    {
        var reader = ExecuteReader(@"SELECT * FROM Character");
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
            item.passiveskill = reader.GetString(32);
            list.Add(item.guid, item);
        }
        return list;
    }

    public Dictionary<string, EquipmentItem> GetSqliteEquipments()
    {
        var reader = ExecuteReader(@"SELECT * FROM Equipment");
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

    /// <summary>
    /// 取用户存档
    /// </summary>
    protected override void GetPlayerLocalInfo()
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
    protected override void SavePlayerLocalInfo()
    {
        ExecuteNonQuery(@"UPDATE player SET prefs=@prefs WHERE id=@id",
            new SqliteParameter("@prefs", JsonConvert.SerializeObject(PlayerSQLPrefs.localConfig)),
            new SqliteParameter("@id", Player.CurrentPlayerId));
    }
}
