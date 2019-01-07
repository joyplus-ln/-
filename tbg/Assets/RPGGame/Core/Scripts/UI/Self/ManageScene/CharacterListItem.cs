using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListItem : MonoBehaviour
{
    private CharacterListUI listui;
    public IPlayerHasCharacters characterItem;

    public Text lvName;

    // Use this for initialization
    void Start()
    {
    }

    public void Init(CharacterListUI listui, IPlayerHasCharacters characterItem)
    {
        this.listui = listui;
        this.characterItem = characterItem;
        lvName.text = characterItem.Character.quality + "-LV" + characterItem.level + "-" +
                      characterItem.Character.title;
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
        ShowSelectHeroDialog();
    }

    public void ShowSelectHeroDialog()
    {
        Dialog data = DialogController.instance.ShowDialog(DialogController.instance.SelfHeroSelectDialog, DialogController.DialogType.wait);
        ((SelfHeroSelectDialog)data).SetData(characterItem.guid);
    }
}