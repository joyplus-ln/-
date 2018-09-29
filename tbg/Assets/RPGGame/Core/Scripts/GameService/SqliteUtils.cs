using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;
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
public class SqliteUtils
{
    public string dbPath = "./table.sqlite3";

    public static SqliteConnection connection;

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
        // open connection
        connection = new SqliteConnection("URI=file:" + dbPath);
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
