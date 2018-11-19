using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearEquipmentList : MonoBehaviour
{

    public GameObject item;
    public Transform content;
    public CharacterListUI characterListUI;
    private string currentName;

    void Start()
    {

    }

    IEnumerator CreatItem(string name)
    {
        yield return null;
        GameObject items = null;
        foreach (var key in PlayerItem.equipDataMap.Keys)
        {
            //Debug.LogError(PlayerItem.equipDataMap[key].EquipmentData.equippablePositions.ToArray());
            if (PlayerItem.equipDataMap[key].EquipmentData.equippablePosition != name)
                continue;

            yield return items = Instantiate(item);
            items.GetComponent<WearEquipmentListItem>().Init(this, PlayerItem.equipDataMap[key]);
            items.transform.SetParent(content, false);
        }

    }

    public void ShowAll(string name)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        this.currentName = name;
        StartCoroutine(CreatItem(name));
    }

    public void Click(PlayerItem clickitem)
    {
        Debug.LogError(characterListUI == null);
        Debug.LogError(characterListUI.GetSelectedPlayerItem() == null);
        Debug.LogError(clickitem == null);
        //xuanze 
        GameInstance.dbBattle.DoEquipItem(characterListUI.GetSelectedPlayerItem().GUID, clickitem.GUID, currentName, GameInstance.Singleton.OnGameServiceItemResult);

    }
    //private void OnSetEquipmentSuccess(ItemResult result)
    //{
    //    GameInstance.Singleton.OnGameServiceItemResult(result);
    //}
}
