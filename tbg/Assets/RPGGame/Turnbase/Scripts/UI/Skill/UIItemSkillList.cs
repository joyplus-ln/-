using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemSkillList : MonoBehaviour
{
    public RpguiItem RpguiItem;
    public RpguiSkillList RpguiSkillList;

    private void Awake()
    {
        RpguiItem.eventUpdate.RemoveListener(OnItemDataUpdate);
        RpguiItem.eventUpdate.AddListener(OnItemDataUpdate);
    }

    private void Start()
    {
        if (RpguiSkillList == null)
            return;

        if (RpguiItem == null)
        {
            RpguiSkillList.Hide();
            return;
        }
    }

    private void OnDestroy()
    {
        RpguiItem.eventUpdate.RemoveListener(OnItemDataUpdate);
    }

    private void OnItemDataUpdate(RpguiDataItem Rpgui)
    {
        var uiItem = Rpgui as RpguiItem;
        var data = uiItem.data;
        if (data.CharacterData != null)
        {
            List<CustomSkill> skills = new List<CustomSkill>();
            if (data.CharacterData.GetCustomSkills() != null)
                skills.AddRange(data.CharacterData.GetCustomSkills());
            RpguiSkillList.SetListItems(skills);
            RpguiSkillList.Show();
        }
        else
            RpguiSkillList.Hide();
    }
}
