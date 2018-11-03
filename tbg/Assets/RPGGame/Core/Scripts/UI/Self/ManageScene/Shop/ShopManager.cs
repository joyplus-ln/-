using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public Transform content;
    [SerializeField]
    private GameObject shopItem;

    private int currentIndex;
    // Use this for initialization
    void Start()
    {

    }

    public void ButtonClick(int id)
    {
        if (currentIndex != id)
        {
            DeleteAll();
        }
        switch (id)
        {
            case 1:
                CreatHero();
                break;
            case 2:
                CreatEquipment();
                break;
            case 3:

                break;
        }
        currentIndex = id;
    }

    private void DeleteAll()
    {
        content.RemoveAllChildren();
    }

    void CreatHero()
    {
        GameObject itemClone = null;
        foreach (var item in GameInstance.GameDatabase.characters.Values)
        {
            itemClone = Instantiate(shopItem);
            itemClone.GetComponent<ShopItem>().SetItemData(item);
            itemClone.transform.SetParent(content, false);
        }
    }

    void CreatEquipment()
    {
        GameObject itemClone = null;
        foreach (var item in GameInstance.GameDatabase.equipments.Values)
        {
            itemClone = Instantiate(shopItem);
            itemClone.transform.SetParent(content, false);
        }
    }
}
