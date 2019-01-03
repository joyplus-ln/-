using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfHeroSelectChangeEquipDialog : Dialog
{

    public Transform content;

    public GameObject selectedImage;

    public UIAttributeShow UiAttributeShow;

    public GameObject item;

    private string characterGuid;


    private SelfHeroSelectChangeEquipData selfHeroSelectChangeEquipData;

    /// <summary>
    /// 当前选择的这件装备的guid
    /// </summary>
    private string selectedEquipGuid;

    // Use this for initialization
    void Start()
    {

    }

    public override void Init(DialogData data)
    {
        base.Init(data);
        selfHeroSelectChangeEquipData = (SelfHeroSelectChangeEquipData)data.obj;
        characterGuid = selfHeroSelectChangeEquipData.heroGuid;
        ShowItems();
    }

    void ShowItems()
    {
        GameObject instItem = null;
        foreach (var key in DBManager.instance.GetHasEquipses().Keys)
        {
            //todo
            if (DBManager.instance.GetHasEquipses()[key].equipPosition == selfHeroSelectChangeEquipData.equipType.ToString())
            {
                instItem = Instantiate(item);
                instItem.GetComponent<SelfHeroSelectChangeItem>().SetInfo(key, characterGuid, selfHeroSelectChangeEquipData.equipType.ToString(), selectedImage, CallBack);
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
        //UiAttributeShow.SetupInfo(PlayerItem.equipDataMap[equipGuid].GetItemAttributes());
    }


    //public void EquipThisItem()
    //{
    //    if (selectedEquipGuid.Length > 0 && PlayerItem.equipDataMap.ContainsKey(selectedEquipGuid))
    //    {
    //        GameInstance.dbBattle.DoEquipItem(characterGuid, selectedEquipGuid, selfHeroSelectChangeEquipData.equipType.ToString(), (result) =>
    //        {
    //            PlayerItem.SetDataRange(result.updateItems);
    //            Close();
    //        });
    //    }
    //}
}

public class SelfHeroSelectChangeEquipData
{
    public string heroGuid;
    public Const.EquipPosition equipType;
    public Action<string> callBack;
}
