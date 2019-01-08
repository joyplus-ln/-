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
        for (int i = 0; i < 5; i++)
        {
            if (IPlayerFormation.DataMap.ContainsKey(i + 1) && !string.IsNullOrEmpty(IPlayerFormation.DataMap[i + 1].itemId))
            {
                formations[i].Show(i + 1, IPlayerFormation.DataMap[i + 1]);
            }
            else
            {
                formations[i].Show(i + 1, null);
            }

        }
    }

    void Clear()
    {
        for (int i = 0; i < formations.Length; i++)
        {
            formations[i].Show(i + 1, null);
        }
    }

    public void Refresh()
    {
        ShowFormation();
    }
}
