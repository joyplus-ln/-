internal class SqliteParameter
{
    private string v;
    private string playerId;

    //public SqliteParameter(string v, string playerId)
    //{
    //    this.v = v;
    //    this.playerId = playerId;
    //}

    public SqliteParameter(string v, object playerId)
    {
        this.v = v;
        this.playerId = playerId.ToString();
    }
}