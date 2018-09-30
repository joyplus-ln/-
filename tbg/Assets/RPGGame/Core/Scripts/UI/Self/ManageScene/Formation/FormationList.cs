using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationList : MonoBehaviour
{

    public GameObject item;
    public Transform content;
    public GameObject confirmPanel;

    public PlayerItem currentSelected;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(CreatItem());
    }
    IEnumerator CreatItem()
    {
        yield return null;
        GameObject items = null;
        foreach (var key in PlayerItem.characterDataMap.Keys)
        {
            yield return items = Instantiate(item);
            items.GetComponent<FormationItem>().Init(this, PlayerItem.characterDataMap[key]);
            items.transform.SetParent(content, false);
        }

    }

    public void SelectedItem(PlayerItem selectedItem)
    {
        currentSelected = selectedItem;
        confirmPanel.SetActive(true);
    }

    public void Confirm(int index)
    {
        Debug.LogError(PlayerFormation.DataMap.Count);
        GameInstance.dbBattle.DoSetFormation(currentSelected.CharacterData.itemid, "STAGE_FORMATION_A", index, OnGameServiceFormationListResult);
    }
    public void OnGameServiceFormationListResult(FormationListResult result)
    {
        if (!result.Success)
            return;

        PlayerFormation.SetDataRange(result.list);
    }
}
