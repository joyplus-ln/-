using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CharacterItem : BaseActorItem
{
    public string region;
    public string quality;
    public string customSkill;
    /// <summary>
    /// 阵营
    /// </summary>
    public int alliance;

    private List<CustomSkill> customSkills { get; set; }

    public List<CustomSkill> GetCustomSkills()
    {
        if (customSkills != null)
            return customSkills;
        string[] cskills = customSkill.Split(';');
        List<CustomSkill> customs = new List<CustomSkill>();
        for (int i = 0; i < cskills.Length; i++)
        {
            if (!string.IsNullOrEmpty(cskills[i]))
            {
                customs.Add(SkillUtils.MakeCustomSkill(cskills[i]));
            }
        }
        customSkills = customs;
        return customs;
    }

    //战斗中使用的，不至于共用
    public List<CustomSkill> GetBattleCustomSkills()
    {
        string[] cskills = customSkill.Split(';');
        List<CustomSkill> customs = new List<CustomSkill>();
        for (int i = 0; i < cskills.Length; i++)
        {
            if (!string.IsNullOrEmpty(cskills[i]))
            {
                customs.Add(SkillUtils.MakeCustomSkill(cskills[i]));
            }
        }
        customSkills = customs;
        return customs;
    }

}

[System.Serializable]
public class CharacterItemAmount : BaseItemAmount<CharacterItem> { }

[System.Serializable]
public class CharacterItemDrop : BaseItemDrop<CharacterItem> { }

[System.Serializable]
public class CharacterItemEvolve : SpecificItemEvolve<CharacterItem>
{
    public override SpecificItemEvolve<CharacterItem> Create()
    {
        return new CharacterItemEvolve();
    }
}
