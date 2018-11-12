using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListItem : MonoBehaviour
{
    private CharacterListUI listui;
    public PlayerItem characterItem;

    public Text lvName;

    // Use this for initialization
    void Start()
    {
    }

    public void Init(CharacterListUI listui, PlayerItem characterItem)
    {
        this.listui = listui;
        this.characterItem = characterItem;
        lvName.text = characterItem.CharacterData.quality + "-LV" + characterItem.Level + "-" +
                      characterItem.CharacterData.title;
    }

    public void PointDown()
    {
        Click();
    }

    public void Click()
    {
        listui.SelectedGameObject.transform.SetParent(transform, false);
        listui.SelectedGameObject.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        listui.SelectedGameObject.GetComponent<RectTransform>().anchorMax = Vector2.one;
        listui.SelectedItem(this, characterItem);
        Debug.LogError("equipment:" + characterItem.EquippedItems.Count);
        ShowSelectHeroDialog();
    }

    public void ShowSelectHeroDialog()
    {
        DialogData data = new DialogData();
        data.dialog = DialogController.instance.SelfHeroSelectDialog;
        SelfHeroSelectData selfHeroSelectData = new SelfHeroSelectData();
        selfHeroSelectData.heroGuid = characterItem.GUID;
        data.obj = selfHeroSelectData;
        DialogController.instance.ShowDialog(data, DialogController.DialogType.wait);
    }
}