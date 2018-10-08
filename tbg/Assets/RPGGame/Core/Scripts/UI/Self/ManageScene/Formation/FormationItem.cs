using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationItem : MonoBehaviour
{
    private FormationList formationList;
    private PlayerItem currentItem;

    public Text lvName;
    // Use this for initialization
    void Start()
    {

    }

    public void Init(FormationList formationList, PlayerItem currentItem)
    {
        this.formationList = formationList;
        this.currentItem = currentItem;
        lvName.text = currentItem.CharacterData.quality + "-LV" + currentItem.Level + "-" + currentItem.CharacterData.title;
    }

    public void Selected()
    {
        formationList.SelectedItem(currentItem);
    }
}
