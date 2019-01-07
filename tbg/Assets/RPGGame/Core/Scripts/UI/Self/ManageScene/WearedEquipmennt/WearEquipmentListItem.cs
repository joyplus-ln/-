using System;
using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.UI;

public class WearEquipmentListItem : MonoBehaviour {

    WearEquipmentList wearEquipmentList;
    IPlayerHasEquips playerItem;

    public Text lvName;
	// Use this for initialization
	void Start () {
		
	}
	


    internal void Init(WearEquipmentList wearEquipmentList, IPlayerHasEquips playerItem)
    {
        this.wearEquipmentList = wearEquipmentList;
        this.playerItem = playerItem;
        //lvName.text = playerItem.EquipmentData.quality + "-LV" + playerItem.Level + "-" + playerItem.EquipmentData.title;
    }

    public void Click()
    {
        Debug.LogError("click");
        wearEquipmentList.Click(playerItem);
    }
}
