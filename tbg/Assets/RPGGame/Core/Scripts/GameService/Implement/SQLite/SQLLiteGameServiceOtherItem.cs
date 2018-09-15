using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mono.Data.Sqlite;

public partial class SQLiteGameService
{
    //            Guid TEXT NOT NULL,
    //            amount INTEGER NOT NULL
    public override void AddOtherItem(string id, int amount)//Guid
    {
        ExecuteNonQuery(@"INSERT INTO playerOtherItem (id,playerId,Guid,amount) VALUES (@id,@playerId,@Guid,@amount)",
            new SqliteParameter("@id", id),
            new SqliteParameter("@playerId", Player.CurrentPlayerId),
            new SqliteParameter("@Guid", "otherItem" + System.Guid.NewGuid()),
            new SqliteParameter("@amount", amount));

    }

    public override void DpdateOtherItem(string id, int amount)
    {
        ExecuteNonQuery(@"UPDATE playerOtherItem SET amount=@amount WHERE id=@id AND playerId=@playerId",
                        new SqliteParameter("@amount", amount),
                        new SqliteParameter("@playerId", Player.CurrentPlayerId),
                        new SqliteParameter("@id", id));
    }

    public override void DeleteOtherItem(string id)
    {
        ExecuteNonQuery(@"DELETE FROM playerOtherItem WHERE id=@id",
                        new SqliteParameter("@id", id),
                        new SqliteParameter("@playerId", Player.CurrentPlayerId));
    }
}
