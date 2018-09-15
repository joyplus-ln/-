using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIInventoryManager : UIBase
{
    public UIItem uiSelectedInfo;
    public UIItemList uiItemList;
    public PlayerItem.ItemType type = PlayerItem.ItemType.character;

    public override void Show()
    {
        base.Show();

        if (uiItemList != null)
        {
            uiItemList.selectable = true;
            uiItemList.multipleSelection = false;
            uiItemList.eventSelect.RemoveListener(SelectItem);
            uiItemList.eventSelect.AddListener(SelectItem);
            uiItemList.eventDeselect.RemoveListener(DeselectItem);
            uiItemList.eventDeselect.AddListener(DeselectItem);
            uiItemList.ClearListItems();
            var list = PlayerItem.characterDataMap.Values.Where(a => a.itemType == type).ToList();
            var elist = PlayerItem.equipDataMap.Values.Where(a => a.itemType == type).ToList();
            list.AddRange(elist);
            list.SortLevel();
            uiItemList.SetListItems(list);

            if (uiItemList.UIEntries.Count > 0)
            {
                var allUIs = uiItemList.UIEntries.Values.ToList();
                allUIs[0].Selected = true;
                SelectItem(allUIs[0]);
            }
            else
            {
                if (uiSelectedInfo != null)
                {
                    uiSelectedInfo.Clear();
                    uiSelectedInfo.Hide();
                }
            }
        }
    }

    public override void Hide()
    {
        base.Hide();

        if (uiSelectedInfo != null)
            uiSelectedInfo.Clear();

        if (uiItemList != null)
            uiItemList.ClearListItems();
    }

    protected virtual void SelectItem(UIItem ui)
    {
        if (uiSelectedInfo != null)
            uiSelectedInfo.SetData(ui.data);
    }

    protected virtual void DeselectItem(UIItem ui)
    {
        // Don't deselect
        ui.Selected = true;
    }
}
