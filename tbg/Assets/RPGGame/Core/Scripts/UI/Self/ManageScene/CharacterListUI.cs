using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListUI : MonoBehaviour
{

    public GameObject item;
    public Transform content;
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
            items.transform.SetParent(content, false);
        }

    }

}
