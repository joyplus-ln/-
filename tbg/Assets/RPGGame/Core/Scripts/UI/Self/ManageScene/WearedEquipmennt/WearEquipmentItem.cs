using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WearEquipmentItem : MonoBehaviour {

    public Text nameText;
    public string Name;
    public WearEquipment wearEquipment;
	// Use this for initialization
	void Start () {
		
	}

    public void SetData(PlayerItem item)
    {
        nameText.text = item.EquipmentData.title;
    }

    public void Onclick()
    {
        wearEquipment.Selected(Name);
    }
}
