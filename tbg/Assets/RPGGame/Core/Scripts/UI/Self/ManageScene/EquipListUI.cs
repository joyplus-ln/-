using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipListUI : MonoBehaviour
{



    public GameObject item;
    public Transform content;
    public GameObject SelectedGameObject;
    private EquipListItem currentSelectedItem;
    public AttributeShow AttributeShowScript;
    // Use this for initialization
    void Start()
    {
        
    }

    private void OnEnable()
    {
        content.RemoveAllChildren();
        StartCoroutine(CreatItem());
    }


    IEnumerator CreatItem()
    {
        yield return null;
        GameObject items = null;
        foreach (var key in PlayerItem.equipDataMap.Keys)
        {
            yield return items = Instantiate(item);
            items.GetComponent<EquipListItem>().Init(this, PlayerItem.equipDataMap[key]);
            items.transform.SetParent(content, false);
        }

    }

    public void SelectedItem(EquipListItem currentSelectedItem, PlayerItem selectedItem)
    {
        this.currentSelectedItem = currentSelectedItem;
        AttributeShowScript.SetupInfo(selectedItem.Attributes);
    }
}
