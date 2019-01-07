using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite3TableDataTmp;
using UnityEngine;

public class TowerGamePlayFormation : BaseGamePlayFormation
{
    public TowerGamePlayFormation foeFormation;
    public bool isPlayerFormation;
    public TowerGamePlayManager Manager { get { return TowerGamePlayManager.Singleton; } }
    public readonly Dictionary<int, RpguiCharacterStats> UIStats = new Dictionary<int, RpguiCharacterStats>();

    private void Start()
    {
        if (isPlayerFormation)
            SetFormationCharacters();
    }

    public override BaseCharacterEntity SetCharacter(int position, IPlayerHasCharacters item)
    {
        var character = base.SetCharacter(position, item) as CharacterEntity;

        if (character == null)
            return null;

        RpguiCharacterStats rpguiStats;
        if (UIStats.TryGetValue(position, out rpguiStats))
        {
            Destroy(rpguiStats.gameObject);
            UIStats.Remove(position);
        }

        if (Manager != null)
        {
            rpguiStats = Instantiate(Manager.RpguiCharacterStatsPrefab, Manager.uiCharacterStatsContainer);
            rpguiStats.transform.localScale = Vector3.one;
            rpguiStats.transform.localPosition = new Vector3(200, 0, 0);
            rpguiStats.character = character;
            character.RpguiCharacterStats = rpguiStats;
            character.transform.rotation = Quaternion.AngleAxis(0,Vector3.zero);
        }

        return character;
    }

    public void Revive()
    {
        var characters = Characters.Values;
        foreach (var character in characters)
        {
            character.Revive();
        }
    }

    public bool IsAnyCharacterAlive()
    {
        var characters = Characters.Values;
        foreach (var character in characters)
        {
            if (character.Hp > 0)
                return true;
        }
        return false;
    }

    public bool TryGetHeadingToFoeRotation(out Quaternion rotation)
    {
        if (foeFormation != null)
        {
            var rotateHeading = foeFormation.transform.position - transform.position;
            rotation = Quaternion.LookRotation(rotateHeading);
            return true;
        }
        rotation = Quaternion.identity;
        return false;
    }

    public Coroutine MoveCharactersToFormation(bool stillForceMoving)
    {
        return StartCoroutine(MoveCharactersToFormationRoutine(stillForceMoving));
    }

    private IEnumerator MoveCharactersToFormationRoutine(bool stillForceMoving)
    {
        var characters = Characters.Values;
        foreach (var character in characters)
        {
            var castedCharacter = character as CharacterEntity;
            castedCharacter.forcePlayMoving = stillForceMoving;
            castedCharacter.MoveTo(character.Container.position, Manager.formationMoveSpeed);
        }
        while (true)
        {
            yield return 0;
            var ifEveryoneReachedTarget = true;
            foreach (var character in characters)
            {
                var castedCharacter = character as CharacterEntity;
                if (castedCharacter.IsMovingToTarget)
                {
                    ifEveryoneReachedTarget = false;
                    break;
                }
            }
            if (ifEveryoneReachedTarget)
                break;
        }
    }

    public void SetActiveDeadCharacters(bool isActive)
    {
        var characters = Characters.Values;
        foreach (var character in characters)
        {
            if (character.Hp <= 0)
                character.gameObject.SetActive(isActive);
        }
    }

    public Coroutine ForceCharactersPlayMoving(float duration)
    {
        return StartCoroutine(ForceCharactersPlayMovingRoutine(duration));
    }

    private IEnumerator ForceCharactersPlayMovingRoutine(float duration)
    {
        var characters = Characters.Values;
        foreach (var character in characters)
        {
            var castedCharacter = character as CharacterEntity;
            castedCharacter.forcePlayMoving = true;
        }
        yield return new WaitForSeconds(duration);
        foreach (var character in characters)
        {
            var castedCharacter = character as CharacterEntity;
            castedCharacter.forcePlayMoving = false;
        }
    }

    public void SetCharactersSelectable(bool selectable)
    {
        var characters = Characters.Values;
        foreach (var character in characters)
        {
            var castedCharacter = character as CharacterEntity;
            castedCharacter.selectable = selectable;
        }
    }

    public int CountDeadCharacters()
    {
        return Characters.Values.Where(a => a.Hp <= 0).ToList().Count;
    }
}
