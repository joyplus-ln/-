using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.UI;

public class FormationItemShow : MonoBehaviour
{

    public Text nameText;
    // Use this for initialization
    void Start()
    {

    }

    public void Show(int index,IPlayerFormation item)
    {
        if (item == null)
        {
            nameText.text = "";
            return;
        }
        nameText.text = index + ":" + item.GetHasCharacter().Character.title;
    }
}
