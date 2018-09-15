using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Data.Sqlite;
using UnityEngine;

public class DBMapItem
{
    private string dbPath = "./tbRpgDbTable.sqlite3";

    private SqliteConnection connection;


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
        if (!File.Exists(dbPath))
            SqliteConnection.CreateFile(dbPath);

        // open connection
        connection = new SqliteConnection("URI=file:" + dbPath);

        GameInstance.GameDatabase.characters = GetSqliteCharacters();
        GameInstance.GameDatabase.equipments = GetSqliteEquipments();
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

}
