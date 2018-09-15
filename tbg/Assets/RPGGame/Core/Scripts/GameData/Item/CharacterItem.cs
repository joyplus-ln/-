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

    [Header("Character Data")]
    public List<string> customSkills;

    public List<CustomSkill> GetCustomSkills()
    {
        List<CustomSkill> customs = new List<CustomSkill>();
        Debug.LogError("获取了技能列表，但是这里是空的，代码还没填");
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
