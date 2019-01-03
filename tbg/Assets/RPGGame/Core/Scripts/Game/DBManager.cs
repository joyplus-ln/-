using System.Collections;
using System.Collections.Generic;
using Framework.Reflection.SQLite3Helper;
using SQLite3TableDataTmp;
using UnityEngine;

public class DBManager
{

    private static DBManager _instance = null;

    public static DBManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DBManager();
            }

            return _instance;
        }
    }
    public SQLite3Operate ConfigSQLite3Operate { get; private set; }
    public SQLite3Operate LocalSQLite3Operate { get; private set; }

    private Dictionary<string, ICharacter> characters = new Dictionary<string, ICharacter>();
    private Dictionary<string, IEquipment> equipments = new Dictionary<string, IEquipment>();
    private Dictionary<string, IPlayerHasCharacters> hasCharacterses = new Dictionary<string, IPlayerHasCharacters>();
    private Dictionary<string, IPlayerHasEquips> hasEquipses = new Dictionary<string, IPlayerHasEquips>();

    public void Init()
    {
        ConfigSQLite3Operate = SQLite3Factory.OpenToRead("Database.db");
        LocalSQLite3Operate = SQLite3Factory.OpenToWrite("Dynamic.db");
        CheckTable();
        characters = ConfigSQLite3Operate.SelectDictT_ST<ICharacter>();
        equipments = ConfigSQLite3Operate.SelectDictT_ST<IEquipment>();
        hasCharacterses = LocalSQLite3Operate.SelectDictT_ST<IPlayerHasCharacters>();
        hasEquipses = LocalSQLite3Operate.SelectDictT_ST<IPlayerHasEquips>();
        Debug.LogError("characters" + characters.Count);
        Debug.LogError("equipments" + equipments.Count);
    }

    void CheckTable()
    {
        if (!LocalSQLite3Operate.TableExists("IPlayerHasCharacters"))
        {
            LocalSQLite3Operate.CreateTable<IPlayerHasCharacters>();
        }
        if (!LocalSQLite3Operate.TableExists("IPlayerHasEquips"))
        {
            LocalSQLite3Operate.CreateTable<IPlayerHasEquips>();
        }
    }

    public Dictionary<string, ICharacter> GetConfigCharacters()
    {
        return characters;
    }

    public Dictionary<string, IEquipment> GetConfigEquipments()
    {
        return equipments;
    }

    public Dictionary<string, IPlayerHasCharacters> GetHasCharacterses()
    {
        return hasCharacterses;
    }

    public Dictionary<string, IPlayerHasEquips> GetHasEquipses()
    {
        return hasEquipses;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="InPropData"></param>
    public void UpdateData(IDictionary<string, IPlayerHasCharacters> InPropData)
    {
        foreach (KeyValuePair<string, IPlayerHasCharacters> itor in InPropData)
        {
            LocalSQLite3Operate.UpdateOrInsert(itor.Value);
        }
    }

    public void UpdateData(IDictionary<string, IPlayerHasEquips> InPropData)
    {
        foreach (KeyValuePair<string, IPlayerHasEquips> itor in InPropData)
        {
            LocalSQLite3Operate.UpdateOrInsert(itor.Value);
        }
    }
}
