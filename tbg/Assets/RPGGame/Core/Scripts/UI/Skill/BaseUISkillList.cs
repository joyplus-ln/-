using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseUISkillList<TUI, CustomSkill> : UIDataItemList<TUI, CustomSkill>
    where TUI : UIDataItem<CustomSkill>
    where CustomSkill : class 
{
    public void SetListItems(List<CustomSkill> list, UnityAction<TUI> onSetListItem = null)
    {
        ClearListItems();
        Debug.Log("技能:" + list.Count);
        foreach (var entry in list)
        {
            var ui = SetListItem(entry);
            if (ui != null && onSetListItem != null)
                onSetListItem(ui);
        }
    }

    public TUI SetListItem(CustomSkill data)
    {
        if (data == null)
            return null;
        var item = SetListItem(data);
        item.SetData(data);
        return item;
    }
}
