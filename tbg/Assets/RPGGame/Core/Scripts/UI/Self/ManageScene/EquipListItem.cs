using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.UI;

public class EquipListItem : MonoBehaviour
{


    private EquipListUI listui;
    private IPlayerHasEquips equipItem;

    public Text lvName;
    // Use this for initialization
    void Start()
    {

    }

    public void Init(EquipListUI listui, IPlayerHasEquips equipItem)
    {
        this.listui = listui;
        this.equipItem = equipItem;
        //lvName.text = equipItem.EquipmentData.quality + "-LV" + equipItem.Level + "-" + equipItem.EquipmentData.title;
    }

    public void PointDown()
    {
        Click();
    }

    public void Click()
    {
        listui.SelectedGameObject.transform.SetParent(transform, false);
        listui.SelectedGameObject.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        listui.SelectedGameObject.GetComponent<RectTransform>().anchorMax = Vector2.one;
        listui.SelectedItem(this, equipItem);
    }
}
