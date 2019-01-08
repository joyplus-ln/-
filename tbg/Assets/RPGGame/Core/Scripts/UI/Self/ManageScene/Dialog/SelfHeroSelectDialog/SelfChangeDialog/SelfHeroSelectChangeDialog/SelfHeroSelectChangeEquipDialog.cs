using System;
using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;

public class SelfHeroSelectChangeEquipDialog : Dialog
{

    public Transform content;

    public GameObject selectedImage;

    public UIAttributeShow UiAttributeShow;

    public GameObject item;

    private string heroGuid;

    /// <summary>
    /// 当前选择的这件装备的guid
    /// </summary>
    private string selectedEquipGuid;

    private Const.EquipPosition equipType;
    private Action<string> callBack;

    // Use this for initialization
    void Start()
    {

    }

    public override void Init()
    {
    }

    public void SetData(string heroGuid, Const.EquipPosition equipType, Action<string> callBack)
    {
        this.heroGuid = heroGuid;
        this.equipType = equipType;
        this.callBack = callBack;
        ShowItems();
    }


    void ShowItems()
    {
        GameObject instItem = null;
        foreach (var key in IPlayerHasEquips.DataMap.Keys)
        {
            //todo
            if (IEquipment.DataMap[IPlayerHasEquips.DataMap[key].dataId].equippablePosition == equipType.ToString())
            {
                instItem = Instantiate(item);
                instItem.GetComponent<SelfHeroSelectChangeItem>().SetInfo(key, heroGuid, equipType.ToString(), selectedImage, CallBack);
                instItem.transform.SetParent(content, false);
            }
        }
    }

    public override void Close()
    {
        base.Close();
        if (callBack != null)
        {
            callBack.Invoke(selectedEquipGuid);
        }
    }

    /// <summary>
    /// 点击了item 显示其属性
    /// </summary>
    /// <param name="equipGuid"></param>
    void CallBack(string equipGuid)
    {
        this.selectedEquipGuid = equipGuid;
        UiAttributeShow.SetupInfo(IPlayerHasEquips.DataMap[selectedEquipGuid].IEquipment.GetAttributes().GetCreateCalculationAttributes());
    }


    public void EquipThisItem()
    {
        IPlayerHasEquips.DataMap[selectedEquipGuid].equipItemId = heroGuid;
        IPlayerHasEquips.DataMap[selectedEquipGuid].equipPosition = equipType.ToString();
        IPlayerHasEquips.UpdataDataMap();
        Close();
    }
}
