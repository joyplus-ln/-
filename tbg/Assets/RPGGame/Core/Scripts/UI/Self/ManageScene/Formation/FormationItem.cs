using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationItem : MonoBehaviour
{
    private FormationList formationList;
    private PlayerItem currentItem;
    // Use this for initialization
    void Start()
    {

    }

    public void Init(FormationList formationList, PlayerItem currentItem)
    {
        this.formationList = formationList;
        this.currentItem = currentItem;
    }

    public void Selected()
    {
        formationList.SelectedItem(currentItem);
    }
}
