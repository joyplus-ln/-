using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    public Text NameText;
	// Use this for initialization
	void Start () {
		
	}

    public void SetItemData(CharacterItem item)
    {
        NameText.text = item.title;
    }
}
