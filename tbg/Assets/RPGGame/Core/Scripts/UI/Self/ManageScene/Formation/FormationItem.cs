using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.UI;

public class FormationItem : MonoBehaviour
{
    private FormationList formationList;
    private IPlayerHasCharacters currentItem;

    public Text lvName;
    // Use this for initialization
    void Start()
    {

    }

    public void Init(FormationList formationList, IPlayerHasCharacters currentItem)
    {
        this.formationList = formationList;
        this.currentItem = currentItem;
        lvName.text = currentItem.Character.quality + "-LV" + currentItem.level + "-" + currentItem.Character.title;
    }

    public void Selected()
    {
        formationList.SelectedItem(currentItem);
    }
}
