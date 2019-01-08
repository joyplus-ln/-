/*
 * --->SQLite3 dataSyncBase table structure.<---
 * --->This class code is automatically generated。<---
 * --->If you need to modify, please place the custom code between <Self Code Begin> and <Self Code End>.
 *                                                                                    --szn
 */





public class IPlayer
{
    private static IPlayer _CurrentPlayer = null;
    public static IPlayer CurrentPlayer
    {
        get { return _CurrentPlayer; }
    }

    public static void Init()
    {
        _CurrentPlayer = UPlayerPrefs.GetObject<IPlayer>("IPlayer", null);
    }

    public static void InsertNewPlayer(IPlayer player)
    {
        _CurrentPlayer = player;
        Save();
    }

    public static void Save()
    {
        UPlayerPrefs.SetObject("IPlayer", _CurrentPlayer);
    }


    public string guid { get; set; }  //des

    public string profileName { get; set; }  //des

    public int exp { get; set; }  //des

    public string selectedFormation { get; set; }  //des

    public string prefs { get; set; }  //des

    public int Level { get; set; }

    public int TowerAbsLevel { get; set; }
    public int TowerCurrentLevel { get; set; }

    public IPlayer()
    {
    }

    public IPlayer(string Inguid, string InprofileName, int Inexp, string InselectedFormation, string Inprefs)
    {
        guid = Inguid;
        profileName = InprofileName;
        exp = Inexp;
        selectedFormation = InselectedFormation;
        prefs = Inprefs;
    }

    //-------------------------------*Self Code Begin*-------------------------------
    //Custom code.
    //-------------------------------*Self Code End*   -------------------------------


    public string ToString()
    {
        return "IPlayer :  " + ", guid = " + guid + ", profileName = " + profileName + ", exp = " + exp + ", selectedFormation = " + selectedFormation + ", prefs = " + prefs;
    }

}
