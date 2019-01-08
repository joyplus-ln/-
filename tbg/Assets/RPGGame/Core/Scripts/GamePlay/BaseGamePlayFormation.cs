using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;

public class BaseGamePlayFormation : MonoBehaviour
{
    public Transform[] containers;
    public readonly Dictionary<int, BaseCharacterEntity> Characters = new Dictionary<int, BaseCharacterEntity>();


    public virtual void SetFormationCharacters()
    {
        ClearCharacters();
        for (var i = 0; i < containers.Length; ++i)
        {
            foreach (int key in IPlayerFormation.DataMap.Keys)
            {
                if (key >= 1 && key <= 5)
                {
                    if (!string.IsNullOrEmpty(IPlayerFormation.DataMap[key].itemId))
                    {
                        SetCharacter(i, new BattleItem(IPlayerHasCharacters.DataMap[IPlayerFormation.DataMap[key].itemId], Const.StageType.Normal));
                    }
                }
            }
            //if (PlayerFormation.TryGetData(formationName, i, out playerFormation))
            //{
            //    SetCharacter(i, new BattleItem(IPlayerHasCharacters.DataMap[characterGuid], Const.StageType.Normal));
            //}
        }
    }

    public virtual void SetCharacters(BattleItem[] items)
    {
        ClearCharacters();
        for (var i = 0; i < containers.Length; ++i)
        {
            if (items.Length <= i)
                break;
            var item = items[i];
            SetCharacter(i, item);
        }
    }

    public virtual BaseCharacterEntity SetCharacter(int position, BattleItem item)
    {
        //if (position < 0 || position >= containers.Length || item == null || item.CharacterData == null)
        //return null;


        var container = containers[position];
        container.RemoveAllChildren();

        var character = Instantiate(GameInstance.Singleton.model);
        character.SetFormation(this, position);
        character.Item = item;
        Characters[position] = character;
        character.transform.rotation = Quaternion.Euler(0, 0, 0);
        character.transform.localRotation = Quaternion.Euler(0, 0, 0);
        (character as CharacterEntity).customSkillActionLogic.InitBattleCustomSkill();
        return character;
    }

    public virtual void ClearCharacters()
    {
        foreach (var container in containers)
        {
            container.RemoveAllChildren();
        }
        Characters.Clear();
    }
}
