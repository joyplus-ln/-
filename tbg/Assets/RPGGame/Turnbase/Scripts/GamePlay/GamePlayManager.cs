using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GamePlayManager : BaseGamePlayManager
{
    public static GamePlayManager Singleton { get; private set; }
    public Camera inputCamera;
    [Header("Formation/Spawning")]
    public GamePlayFormation playerFormation;
    public GamePlayFormation foeFormation;
    //public EnvironmentManager environmentManager;
    public Transform mapCenter;
    public float spawnOffset = 5f;
    [Header("Speed/Delay")]
    public float formationMoveSpeed = 5f;
    public float doActionMoveSpeed = 8f;
    public float actionDoneMoveSpeed = 10f;
    public float beforeMoveToNextWaveDelay = 2f;
    public float moveToNextWaveDelay = 1f;
    [Header("UI")]
    public Transform uiCharacterStatsContainer;
    public RpguiCharacterStats RpguiCharacterStatsPrefab;
    public RpguiCharacterActionManager RpguiCharacterActionManager;
    public UseSkillManager uiUseSkillManager;
    public CharacterEntity ActiveCharacter { get; private set; }
    public int CurrentWave { get; private set; }
    //public NormalStage CastedNormalStage { get { return PlayingStage as NormalStage; } }
    public int MaxWave { get { return GetWavesLength(); } }
    //当前选定的
    public GameObject CurrentSelectedEntity { get; set; }

    public Vector3 MapCenterPosition
    {
        get
        {
            if (mapCenter == null)
                return Vector3.zero;
            return mapCenter.position;
        }
    }

    private void Awake()
    {
        Singleton = this;
        if (inputCamera == null)
            inputCamera = Camera.main;
        // Setup uis
        RpguiCharacterActionManager.Hide();
        uiUseSkillManager.Hide();
        // Setup player formation
        playerFormation.foeFormation = foeFormation;
        // Setup foe formation
        foeFormation.ClearCharacters();
        foeFormation.foeFormation = playerFormation;

        //SetupEnvironment();
    }

    private void Start()
    {
        StartBattle();
    }

    /// <summary>
    /// 战斗开始，所有角色初始化好后调用
    /// </summary>
    public void StartBattle()
    {
        CurrentWave = 0;
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        if (RpguiPauseGame.IsVisible())
        {
            Time.timeScale = 0;
            return;
        }

        if (IsAutoPlay != isAutoPlayDirty)
        {
            if (IsAutoPlay)
            {
                RpguiCharacterActionManager.Hide();
                uiUseSkillManager.Hide();
                if (ActiveCharacter != null)
                    ActiveCharacter.RandomAction();
            }
            isAutoPlayDirty = IsAutoPlay;
        }

        Time.timeScale = !isEnding && IsSpeedMultiply ? 2 : 1;

        if (Input.GetMouseButtonDown(0) && ActiveCharacter != null && ActiveCharacter.IsPlayerCharacter)
        {
            //Ray ray = inputCamera.ScreenPointToRay(RPGInputManager.MousePosition());
            //Debug.DrawRay(ray.origin, ray.direction, Color.red);
            //RaycastHit hitInfo;
            //if (!Physics.Raycast(ray, out hitInfo))
            //    return;
            if (CurrentSelectedEntity == null) return;
            //var targetCharacter = hitInfo.collider.GetComponent<CharacterEntity>();
            var targetCharacter = CurrentSelectedEntity.GetComponent<CharacterEntity>();
            if (targetCharacter != null)
            {
                if (ActiveCharacter.DoAction(targetCharacter))
                {
                    playerFormation.SetCharactersSelectable(false);
                    foeFormation.SetCharactersSelectable(false);
                }
            }
        }
    }

    IEnumerator StartGame()
    {
        yield return playerFormation.MoveCharactersToFormation(true);
        //environmentManager.isPause = false;
        yield return playerFormation.ForceCharactersPlayMoving(moveToNextWaveDelay);
        //environmentManager.isPause = true;
        NextWave();
        yield return foeFormation.MoveCharactersToFormation(false);
        InitCustoms();
        FirstTurn();
    }

    /// <summary>
    /// 初始化一些需要开局初始化的东西
    /// </summary>
    public void InitCustoms()
    {

    }

    public void NextWave()
    {
        PlayerItem[] characters;
        StageFoe[] foes;
        var wave = GetWave(CurrentWave);
        if (!wave.useRandomFoes && wave.foes.Length > 0)
            foes = wave.foes;
        else
            foes = GetRandomFoes();

        characters = new PlayerItem[foes.Length];
        for (var i = 0; i < characters.Length; ++i)
        {
            var foe = foes[i];
            if (foe != null && !string.IsNullOrEmpty(foe.characterId))
            {
                var character = PlayerItem.CreateActorItemWithLevel(GameInstance.Singleton.gameDatabase.characters[foe.characterId], foe.level, GetRandomFoesType(), false);
                characters[i] = character;
            }
        }

        if (characters.Length == 0)
            Debug.LogError("Missing Foes Data");

        foeFormation.SetCharacters(characters);
        foeFormation.Revive();
        ++CurrentWave;
    }

    IEnumerator MoveToNextWave()
    {
        yield return new WaitForSeconds(beforeMoveToNextWaveDelay);
        foeFormation.ClearCharacters();
        playerFormation.SetActiveDeadCharacters(false);
        //environmentManager.isPause = false;
        yield return playerFormation.ForceCharactersPlayMoving(moveToNextWaveDelay);
        //environmentManager.isPause = true;
        playerFormation.SetActiveDeadCharacters(true);
        NextWave();
        yield return foeFormation.MoveCharactersToFormation(false);
        NewTurn();
    }

    public void FirstTurn()
    {
        if (ActiveCharacter != null)
            ActiveCharacter.currentTimeCount = 0;

        CharacterEntity activatingCharacter = null;
        var maxTime = int.MinValue;
        List<BaseCharacterEntity> characters = new List<BaseCharacterEntity>();
        characters.AddRange(playerFormation.Characters.Values);
        characters.AddRange(foeFormation.Characters.Values);
        for (int i = 0; i < characters.Count; ++i)
        {
            CharacterEntity character = characters[i] as CharacterEntity;
            if (character != null)
            {
                if (character.Hp > 0)
                {
                    int spd = (int)character.GetTotalAttributes().spd;
                    if (spd <= 0)
                        spd = 1;
                    character.currentTimeCount += spd;
                    if (character.currentTimeCount > maxTime)
                    {
                        maxTime = character.currentTimeCount;
                        activatingCharacter = character;
                    }
                }
                else
                    character.currentTimeCount = 0;
            }
        }
        ActiveCharacter = activatingCharacter;
        bool isStun = activatingCharacter.IsStun();
        ActiveCharacter.ApplySkillAndBuff(CustomSkill.TriggerType.beforeFight);
        ActiveCharacter.DecreaseBuffsTurn();
        //ActiveCharacter.DecreaseSkillsTurn();
        ActiveCharacter.ResetStates();
        if (ActiveCharacter.Hp > 0 && !isStun)
        {
            if (ActiveCharacter.IsPlayerCharacter)
            {
                if (IsAutoPlay)
                    ActiveCharacter.RandomAction();
                else
                {
                    RpguiCharacterActionManager.Show();
                    uiUseSkillManager.SetData(ActiveCharacter);
                }
            }
            else
            {
                ActiveCharacter.RandomAction();
            }


        }
        else
            ActiveCharacter.NotifyEndAction();
    }

    public void NewTurn()
    {
        if (ActiveCharacter != null)
            ActiveCharacter.currentTimeCount = 0;

        CharacterEntity activatingCharacter = null;
        var maxTime = int.MinValue;
        List<BaseCharacterEntity> characters = new List<BaseCharacterEntity>();
        characters.AddRange(playerFormation.Characters.Values);
        characters.AddRange(foeFormation.Characters.Values);
        for (int i = 0; i < characters.Count; ++i)
        {
            CharacterEntity character = characters[i] as CharacterEntity;
            if (character != null)
            {
                if (character.Hp > 0)
                {
                    int spd = (int)character.GetTotalAttributes().spd;
                    if (spd <= 0)
                        spd = 1;
                    character.currentTimeCount += spd;
                    if (character.currentTimeCount > maxTime)
                    {
                        maxTime = character.currentTimeCount;
                        activatingCharacter = character;
                    }
                }
                else
                    character.currentTimeCount = 0;
            }
        }
        ActiveCharacter = activatingCharacter;
        bool isStun = activatingCharacter.IsStun();
        ActiveCharacter.ApplySkillAndBuff(CustomSkill.TriggerType.beforeFight);
        ActiveCharacter.DecreaseBuffsTurn();
        ActiveCharacter.DecreaseSkillsTurn();
        ActiveCharacter.ResetStates();
        if (ActiveCharacter.Hp > 0 && !isStun)
        {
            if (ActiveCharacter.IsPlayerCharacter)
            {
                if (IsAutoPlay)
                    ActiveCharacter.RandomAction();
                else
                {
                    RpguiCharacterActionManager.Show();
                    uiUseSkillManager.SetData(ActiveCharacter);
                }
            }
            else
            {
                ActiveCharacter.RandomAction();
            }


        }
        else
            ActiveCharacter.NotifyEndAction();
    }

    /// <summary>
    /// This will be called by Character class to show target scopes or do action
    /// </summary>
    /// <param name="character"></param>
    public void ShowTargetScopesOrDoAction(CharacterEntity character)
    {
        var allyTeamFormation = character.IsPlayerCharacter ? playerFormation : foeFormation;
        var foeTeamFormation = !character.IsPlayerCharacter ? playerFormation : foeFormation;
        allyTeamFormation.SetCharactersSelectable(false);
        foeTeamFormation.SetCharactersSelectable(false);
        if (character.Action == CharacterEntity.ACTION_ATTACK)
            foeTeamFormation.SetCharactersSelectable(true);
        else
        {
            ShowCustomSkillScopesOrDoAction(character, allyTeamFormation, foeTeamFormation);
        }
    }

    void ShowCustomSkillScopesOrDoAction(CharacterEntity character, GamePlayFormation allyTeamFormation, GamePlayFormation foeTeamFormation)
    {
        switch (character.SelectedCustomSkill.usageScope)
        {
            case CustomSkill.SkillUsageScope.Self:
                character.selectable = true;
                break;
            case CustomSkill.SkillUsageScope.Ally:
                allyTeamFormation.SetCharactersSelectable(true);
                break;
            case CustomSkill.SkillUsageScope.Enemy:
                foeTeamFormation.SetCharactersSelectable(true);
                break;
            case CustomSkill.SkillUsageScope.All:
                allyTeamFormation.SetCharactersSelectable(true);
                foeTeamFormation.SetCharactersSelectable(true);
                break;
        }
    }


    /// <summary>
    /// 获取所有己方英雄
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public List<BaseCharacterEntity> GetAllies(CharacterEntity character)
    {
        if (character.IsPlayerCharacter)
            return playerFormation.Characters.Values.Where(a => a.Hp > 0).ToList();
        else
            return foeFormation.Characters.Values.Where(a => a.Hp > 0).ToList();
    }

    /// <summary>
    /// 获取所有敌方英雄
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public List<BaseCharacterEntity> GetFoes(CharacterEntity character)
    {
        if (character.IsPlayerCharacter)
            return foeFormation.Characters.Values.Where(a => a.Hp > 0).ToList();
        else
            return playerFormation.Characters.Values.Where(a => a.Hp > 0).ToList();
    }

    public void NotifyEndAction(CharacterEntity character)
    {
        if (character != ActiveCharacter)
            return;

        if (!playerFormation.IsAnyCharacterAlive())
        {
            ActiveCharacter = null;
            StartCoroutine(LoseGameRoutine());
        }
        else if (!foeFormation.IsAnyCharacterAlive())
        {
            ActiveCharacter = null;
            if (CurrentWave >= GetWavesLength())
            {
                StartCoroutine(WinGameRoutine());
                return;
            }
            StartCoroutine(MoveToNextWave());
        }
        else
            NewTurn();
    }

    public override void OnRevive()
    {
        base.OnRevive();
        playerFormation.Revive();
        NewTurn();
    }

    public override int CountDeadCharacters()
    {
        return playerFormation.CountDeadCharacters();
    }

    public StageFoe[] GetRandomFoes()
    {
        if (PlayingStage as NormalStage != null)
            return (PlayingStage as NormalStage).RandomFoes().foes;
        if (PlayingStage as TowerStage != null)
            return (PlayingStage as TowerStage).RandomFoes().foes;
        return null;
    }

    public Const.StageType GetRandomFoesType()
    {
        if (PlayingStage as NormalStage != null)
            return Const.StageType.Normal;
        if (PlayingStage as TowerStage != null)
            return Const.StageType.Tower;
        return Const.StageType.Tower;
    }

    public int GetWavesLength()
    {
        if (PlayingStage as NormalStage != null)
            return (PlayingStage as NormalStage).waves.Length;
        if (PlayingStage as TowerStage != null)
            return (PlayingStage as TowerStage).waves.Length;
        return 0;
    }


    public StageWave GetWave(int index)
    {
        if (PlayingStage as NormalStage != null)
            return (PlayingStage as NormalStage).waves[index];
        if (PlayingStage as TowerStage != null)
            return (PlayingStage as TowerStage).waves[index];
        return null;
    }

}
