using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.Events;

public class SqliteUtils
{




    #region MyRegion

    //public void ExecuteNonQuery(string sql, params SqliteParameter[] args)
    //{
    //    SQLiteGameService.connection.Open();
    //    using (var cmd = new SqliteCommand(sql, SQLiteGameService.connection))
    //    {
    //        foreach (var arg in args)
    //        {
    //            cmd.Parameters.Add(arg);
    //        }
    //        cmd.ExecuteNonQuery();
    //    }
    //    SQLiteGameService.connection.Close();
    //}

    //public object ExecuteScalar(string sql, params SqliteParameter[] args)
    //{
    //    object result;
    //    SQLiteGameService.connection.Open();
    //    using (var cmd = new SqliteCommand(sql, SQLiteGameService.connection))
    //    {
    //        foreach (var arg in args)
    //        {
    //            cmd.Parameters.Add(arg);
    //        }
    //        result = cmd.ExecuteScalar();
    //    }
    //    SQLiteGameService.connection.Close();
    //    return result;
    //}

    //public DbRowsReader ExecuteReader(string sql, params SqliteParameter[] args)
    //{
    //    DbRowsReader result = new DbRowsReader();
    //    SQLiteGameService.connection.Open();
    //    using (var cmd = new SqliteCommand(sql, SQLiteGameService.connection))
    //    {
    //        foreach (var arg in args)
    //        {
    //            cmd.Parameters.Add(arg);
    //        }
    //        result.Init(cmd.ExecuteReader());
    //    }
    //    SQLiteGameService.connection.Close();
    //    return result;
    //}

    #endregion


}
