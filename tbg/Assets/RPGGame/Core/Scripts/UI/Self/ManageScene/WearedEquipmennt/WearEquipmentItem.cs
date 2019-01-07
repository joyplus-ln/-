using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.UI;

public class WearEquipmentItem : MonoBehaviour {

    public Text nameText;
    public string Name;
    public WearEquipment wearEquipment;
	// Use this for initialization
	void Start () {
		
	}

    public void SetData(IPlayerHasCharacters item)
    {
        nameText.text = "unknown";
    }

    public void Onclick()
    {
        wearEquipment.Selected(Name);
    }

    public void Clear()
    {
        nameText.text = "";
    }
}
