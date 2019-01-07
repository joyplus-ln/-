using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SQLite3TableDataTmp;

public class FormationManager : MonoBehaviour
{

    public FormationItemShow[] formations;

    // Use this for initialization
    void Start()
    {

    }

    private void OnEnable()
    {
        Refresh();
    }

    void ShowFormation()
    {
        Clear();
        List<PlayerFormation> list = PlayerFormation.DataMap.Values.Where(a => a.playerId == IPlayer.CurrentPlayer.guid).ToList();
        for (int i = 0; i < list.Count; i++)
        {
            formations[list[i].Position].Show(list[i]);
        }
    }

    void Clear()
    {
        for (int i = 0; i < formations.Length; i++)
        {
            formations[i].Show(null);
        }
    }

    public void Refresh()
    {
        ShowFormation();
    }
}
