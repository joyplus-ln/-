using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;

public class FormationList : MonoBehaviour
{

    public GameObject item;
    public Transform content;

    public IPlayerHasCharacters currentSelected;

    public FormationManager formationManager;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(CreatItem());
    }
    IEnumerator CreatItem()
    {
        yield return null;
        GameObject items = null;
        foreach (var key in IPlayerHasCharacters.DataMap.Keys)
        {
            yield return items = Instantiate(item);
            items.GetComponent<FormationItem>().Init(this, IPlayerHasCharacters.DataMap[key]);
            items.transform.SetParent(content, false);
        }

    }

    public void SelectedItem(IPlayerHasCharacters selectedItem)
    {
        currentSelected = selectedItem;
        //DialogData dialogdata = new DialogData();
        //HeroFormationData data = new HeroFormationData();
        //dialogdata.obj = data;
        //dialogdata.dialog = DialogController.instance.heroFormationDialog;
        //data.HeroGuid = selectedItem.guid;
        //data.callback += () =>
        //{
        //    formationManager.Refresh();
        //};
        Dialog dialog = DialogController.instance.ShowDialog(DialogController.instance.heroFormationDialog, DialogController.DialogType.stack);
        ((HeroFormationDialog)dialog).SetData(selectedItem.guid, CallBack);
    }

    public void CallBack(int index)
    {
        formationManager.Refresh();
    }

    //public void Confirm(int index)
    //{
    //    Debug.LogError(PlayerFormation.DataMap.Count);
    //    Debug.LogError(currentSelected.CharacterData.itemid);
    //    GameInstance.dbBattle.DoSetFormation(currentSelected.GUID,currentSelected.CharacterData.itemid, "STAGE_FORMATION_A", index, OnGameServiceFormationListResult);
    //}
    //public void OnGameServiceFormationListResult(FormationListResult result)
    //{
    //    if (!result.Success)
    //        return;
    //    PlayerFormation.DataMap.Clear();
    //    PlayerFormation.SetDataRange(result.list);
    //    formationManager.Refresh();
    //}
}
