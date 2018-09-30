using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageFoe
{
    public string characterId;
    public int level;
}

[System.Serializable]
public class StageRandomFoe
{
    public StageFoe[] foes;
    public int randomWeight;
}

[System.Serializable]
public class StageWave
{
    public bool useRandomFoes;
    public StageFoe[] foes;
}

public class NormalStage : BaseStage
{
    [Header("Battle")]
    public StageWave[] waves;
    public StageRandomFoe[] randomFoes;

    public StageRandomFoe RandomFoes()
    {
        var weight = new Dictionary<StageRandomFoe, int>();
        foreach (var randomFoe in randomFoes)
        {
            weight.Add(randomFoe, randomFoe.randomWeight);
        }
        return WeightedRandomizer.From(weight).TakeOne();
    }

    public override List<PlayerItem> GetCharacters()
    {
        var dict = new Dictionary<string, PlayerItem>();
        foreach (var randomFoe in randomFoes)
        {
            foreach (var foe in randomFoe.foes)
            {

                if (!string.IsNullOrEmpty(foe.characterId))
                {

                    var newEntry = PlayerItem.CreateActorItemWithLevel(GameInstance.Singleton.gameDatabase.characters[foe.characterId], foe.level, Const.StageType.Normal, false);
                    newEntry.GUID = GameInstance.Singleton.gameDatabase.characters[foe.characterId].itemid + "_" + foe.level;
                    dict[GameInstance.Singleton.gameDatabase.characters[foe.characterId].itemid + "_" + foe.level] = newEntry;
                }
            }
        }
        foreach (var wave in waves)
        {
            if (wave.useRandomFoes)
                continue;

            var foes = wave.foes;
            foreach (var foe in foes)
            {
                var item = GameInstance.Singleton.gameDatabase.characters[foe.characterId];
                if (item != null)
                {
                    var newEntry = PlayerItem.CreateActorItemWithLevel(item, foe.level, Const.StageType.Normal, false);
                    newEntry.GUID = GameInstance.Singleton.gameDatabase.characters[foe.characterId].itemid + "_" + foe.level;
                    dict[GameInstance.Singleton.gameDatabase.characters[foe.characterId].itemid + "_" + foe.level] = newEntry;
                }
            }
        }
        return new List<PlayerItem>(dict.Values);
    }
}

