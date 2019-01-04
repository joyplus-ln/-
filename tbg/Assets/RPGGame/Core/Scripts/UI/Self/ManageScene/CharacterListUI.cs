using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;

public class CharacterListUI : MonoBehaviour
{

    public GameObject item;
    public Transform content;
    public GameObject SelectedGameObject;
    public Transform SelectedGameObjectParent;
    private CharacterListItem currentSelectedItem;
    // Use this for initialization
    void Start()
    {
        
    }

    private void OnEnable()
    {
        SelectedGameObject.transform.SetParent(SelectedGameObjectParent, false);
        content.RemoveAllChildren();
        StartCoroutine(CreatItem());
    }


    IEnumerator CreatItem()
    {
        yield return null;
        //todo
        GameObject items = null;
        foreach (var key in IPlayerHasCharacters.DataMap.Keys)
        {
            yield return items = Instantiate(item);
            items.GetComponent<CharacterListItem>().Init(this, IPlayerHasCharacters.DataMap[key].GetPlayerItem());
            items.transform.SetParent(content, false);
        }

    }

    public void SelectedItem(CharacterListItem currentSelectedItem, PlayerItem selectedItem)
    {
        this.currentSelectedItem = currentSelectedItem;
    }

    public PlayerItem GetSelectedPlayerItem()
    {
        return currentSelectedItem.characterItem;
    }

}
