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
    public string passiveskill;

    [Header("Character Data")]
    public List<BaseSkill> skills;
    public List<PassiveSkill> passiveskills;
    public List<string> customSkills;

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
