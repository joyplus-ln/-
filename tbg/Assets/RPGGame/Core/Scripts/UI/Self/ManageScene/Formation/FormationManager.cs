using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FormationManager : MonoBehaviour
{

    public FormationItemShow[] formations;

    // Use this for initialization
    void Start()
    {

    }

    void ShowFormation()
    {
        List<PlayerFormation> list = PlayerFormation.DataMap.Values.Where(a => a.playerId == Player.CurrentPlayerId).ToList();
        for (int i = 0; i < list.Count;i++)
        {
            formations[list[i].Position].Show(list[i]);
        }
    }

    public void Refresh()
    {
        ShowFormation(); 
    }
}
