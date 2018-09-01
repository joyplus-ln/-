using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseUISkillList<TUI, CSkill> : UIDataItemList<TUI, CSkill>
    where TUI : UIDataItem<CSkill>
    where CSkill : class
{
    public void SetListItems(List<CSkill> list, UnityAction<TUI> onSetListItem = null)
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

    public TUI SetListItem(CSkill data)
    {
        if (data == null)
            return null;
        var item = base.SetListItem((data as CustomSkill).skillName);
        item.SetData(data);
        return item;
    }
}
