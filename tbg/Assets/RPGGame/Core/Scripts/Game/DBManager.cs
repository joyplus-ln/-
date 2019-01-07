using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void Init()
    {
        ConfigSQLite3Operate = SQLite3Factory.OpenToRead("Database.db");
        LocalSQLite3Operate = SQLite3Factory.OpenToWrite("Dynamic.db");
        CheckTable();
        IPlayer.Init();
        IPlayerBattle.Init();
        ICharacter.Init();
        IEquipment.Init();
        IPlayerBattle.Init();
        IPlayerClearStage.Init();
        IPlayerCurrency.Init();
        IPlayerFormation.Init();
        IPlayerHasCharacters.Init();
        IPlayerHasEquips.Init();
        IPlayerOtherItem.Init();
        IPlayerStamina.Init();
        IPlayerUnlockItem.Init();
        IPlayerHasEquips.InsertNewEquips("2001");
        IPlayerHasCharacters.InsertNewCharacter("1001");
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
        if (!LocalSQLite3Operate.TableExists("IPlayerBattle"))
        {
            LocalSQLite3Operate.CreateTable<IPlayerBattle>();
        }
        if (!LocalSQLite3Operate.TableExists("IPlayerClearStage"))
        {
            LocalSQLite3Operate.CreateTable<IPlayerClearStage>();
        }
        if (!LocalSQLite3Operate.TableExists("IPlayerCurrency"))
        {
            LocalSQLite3Operate.CreateTable<IPlayerCurrency>();
        }

        if (!LocalSQLite3Operate.TableExists("IPlayerFormation"))
        {
            LocalSQLite3Operate.CreateTable<IPlayerFormation>();
        }
        if (!LocalSQLite3Operate.TableExists("IPlayerHasCharacters"))
        {
            LocalSQLite3Operate.CreateTable<IPlayerHasCharacters>();
        }
        if (!LocalSQLite3Operate.TableExists("IPlayerHasEquips"))
        {
            LocalSQLite3Operate.CreateTable<IPlayerHasEquips>();
        }
        if (!LocalSQLite3Operate.TableExists("IPlayerOtherItem"))
        {
            LocalSQLite3Operate.CreateTable<IPlayerOtherItem>();
        }
        if (!LocalSQLite3Operate.TableExists("IPlayerStamina"))
        {
            LocalSQLite3Operate.CreateTable<IPlayerStamina>();
        }
        if (!LocalSQLite3Operate.TableExists("IPlayerUnlockItem"))
        {
            LocalSQLite3Operate.CreateTable<IPlayerUnlockItem>();
        }
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
