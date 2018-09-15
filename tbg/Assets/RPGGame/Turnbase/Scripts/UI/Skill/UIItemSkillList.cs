using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemSkillList : MonoBehaviour
{
    public UIItem uiItem;
    public UISkillList uiSkillList;

    private void Awake()
    {
        uiItem.eventUpdate.RemoveListener(OnItemDataUpdate);
        uiItem.eventUpdate.AddListener(OnItemDataUpdate);
    }

    private void Start()
    {
        if (uiSkillList == null)
            return;

        if (uiItem == null)
        {
            uiSkillList.Hide();
            return;
        }
    }

    private void OnDestroy()
    {
        uiItem.eventUpdate.RemoveListener(OnItemDataUpdate);
    }

    private void OnItemDataUpdate(UIDataItem ui)
    {
        var uiItem = ui as UIItem;
        var data = uiItem.data;
        if (data.CharacterData != null)
        {
            List<BaseSkill> skills = new List<BaseSkill>();
            if (data.CharacterData.skills != null)
                skills.AddRange(data.CharacterData.skills);
            if (data.CharacterData.passiveskills != null)
                for (int i = 0; i < data.CharacterData.passiveskills.Count; i++)
                {
                    skills.Add(data.CharacterData.passiveskills[i]);
                }

            uiSkillList.SetListItems(skills);
            uiSkillList.Show();
        }
        else
            uiSkillList.Hide();
    }
}
