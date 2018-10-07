using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearEquipmentListItem : MonoBehaviour {

    WearEquipmentList wearEquipmentList;
    PlayerItem playerItem;
	// Use this for initialization
	void Start () {
		
	}
	


    internal void Init(WearEquipmentList wearEquipmentList, PlayerItem playerItem)
    {
        this.wearEquipmentList = wearEquipmentList;
        this.playerItem = playerItem;
    }

    public void Click()
    {
        Debug.LogError("click");
        wearEquipmentList.Click(playerItem);
    }
}
