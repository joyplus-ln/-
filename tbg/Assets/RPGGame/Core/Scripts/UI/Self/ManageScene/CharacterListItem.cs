using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListItem : MonoBehaviour
{
    private CharacterListUI listui;
    public PlayerItem characterItem;
    // Use this for initialization
    void Start()
    {

    }

    public void Init(CharacterListUI listui, PlayerItem characterItem)
    {
        this.listui = listui;
        this.characterItem = characterItem;
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
        listui.SelectedItem(this, characterItem);
        Debug.LogError("equipment:" + characterItem.EquippedItems.Count);
    }
}
