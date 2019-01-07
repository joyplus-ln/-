using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;


public class TowerStage : BaseStage
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

    public override List<ICharacter> GetCharacters()
    {
        var dict = new Dictionary<string, ICharacter>();
        //foreach (var randomFoe in randomFoes)
        //{
        //    foreach (var foe in randomFoe.foes)
        //    {
                
        //        if (!string.IsNullOrEmpty(foe.characterId))
        //        {
        //            var newEntry = PlayerItem.CreateActorItemWithLevel(ICharacter.DataMap[foe.characterId], foe.level,Const.StageType.Tower,false);
        //            newEntry.GUID = ICharacter.DataMap[foe.characterId].guid + "_" + foe.level;
        //            dict[ICharacter.DataMap[foe.characterId].guid + "_" + foe.level] = newEntry;
        //        }
        //    }
        //}
        //foreach (var wave in waves)
        //{
        //    if (wave.useRandomFoes)
        //        continue;

        //    var foes = wave.foes;
        //    foreach (var foe in foes)
        //    {
        //        var item = ICharacter.DataMap[foe.characterId];
        //        if (item != null)
        //        {
        //            var newEntry = PlayerItem.CreateActorItemWithLevel(item, foe.level,Const.StageType.Tower,false);
        //            newEntry.GUID = ICharacter.DataMap[foe.characterId].guid + "_" + foe.level;
        //            dict[ICharacter.DataMap[foe.characterId].guid + "_" + foe.level] = newEntry;
        //        }
        //    }
        //}
        return new List<ICharacter>(dict.Values);
    }
}