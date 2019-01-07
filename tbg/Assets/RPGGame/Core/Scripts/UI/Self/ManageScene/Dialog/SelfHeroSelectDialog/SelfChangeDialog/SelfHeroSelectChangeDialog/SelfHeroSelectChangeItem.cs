using System;
using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.UI;

public class SelfHeroSelectChangeItem : MonoBehaviour
{

    public Text equipName;

    public Text equipInfo;

    public Text equipedName;

    private string equipGuid;

    private string characterGuid;

    private string equipPosition;

    private Action<string> callBack;

    public GameObject selectImage;
    // Use this for initialization
    void Start()
    {

    }


    public void SetInfo(string equipGuid, string characterGuid, string equipPosition, GameObject selectImage, Action<string> callBack)
    {
        this.equipGuid = equipGuid;
        this.characterGuid = characterGuid;
        this.equipPosition = equipPosition;
        this.selectImage = selectImage;
        this.callBack = callBack;
        ShowItem();
    }

    void ShowItem()
    {
        equipName.text = IPlayerHasEquips.DataMap[equipGuid].IEquipment.title;
        equipInfo.text = IPlayerHasEquips.DataMap[equipGuid].IEquipment.quality;
        equipedName.text = IPlayerHasEquips.DataMap[equipGuid].IEquipment.title;
    }

    /// <summary>
    /// 选择这件装备穿戴
    /// </summary>
    public void ClickItem()
    {

        selectImage.transform.SetParent(transform, true);
        selectImage.transform.localPosition = Vector3.zero;
        if (callBack != null)
        {
            callBack.Invoke(equipGuid);
        }
    }

}
