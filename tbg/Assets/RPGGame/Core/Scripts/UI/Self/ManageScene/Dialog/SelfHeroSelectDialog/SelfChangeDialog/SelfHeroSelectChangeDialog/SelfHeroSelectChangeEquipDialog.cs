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


    private SelfHeroSelectChangeEquipData selfHeroSelectChangeEquipData;

    /// <summary>
    /// 当前选择的这件装备的guid
    /// </summary>
    private string selectedEquipGuid;

    // Use this for initialization
    void Start()
    {

    }

    public override void Init()
    {
        heroGuid = selfHeroSelectChangeEquipData.heroGuid;
        ShowItems();
    }

    void ShowItems()
    {
        GameObject instItem = null;
        foreach (var key in IPlayerHasEquips.DataMap.Keys)
        {
            //todo
            if (IEquipment.DataMap[IPlayerHasEquips.DataMap[key].dataId].equippablePosition == selfHeroSelectChangeEquipData.equipType.ToString())
            {
                instItem = Instantiate(item);
                instItem.GetComponent<SelfHeroSelectChangeItem>().SetInfo(key, heroGuid, selfHeroSelectChangeEquipData.equipType.ToString(), selectedImage, CallBack);
                instItem.transform.SetParent(content, false);
            }
        }
    }

    public override void Close()
    {
        base.Close();
        if (selfHeroSelectChangeEquipData.callBack != null)
        {
            selfHeroSelectChangeEquipData.callBack.Invoke(selectedEquipGuid);
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
        IPlayerHasEquips.DataMap[selectedEquipGuid].equipPosition = selfHeroSelectChangeEquipData.equipType.ToString();
        IPlayerHasEquips.UpdataDataMap();
        Close();
    }
}

public class SelfHeroSelectChangeEquipData
{
    public string heroGuid;
    public Const.EquipPosition equipType;
    public Action<string> callBack;
}
