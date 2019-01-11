using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    public Text NameText;

    private string heruid;
    // Use this for initialization
    void Start()
    {

    }

    public void SetItemData(ICharacter item)
    {
        NameText.text = item.title;
        heruid = item.guid;
    }

    public void Click()
    {
        Dialog dialog = DialogController.instance.ShowDialog(DialogController.instance.ShopHeroDialog, DialogController.DialogType.stack);
        ((ShopHeroDialog)dialog).SetData(heruid);
    }
}
