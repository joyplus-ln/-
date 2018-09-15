using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseGamePlayManager : MonoBehaviour
{
    public static string BattleSession { get; private set; }
    public static BaseStage PlayingStage { get; protected set; }
    [Header("Combat Texts")]
    public Transform combatTextContainer;
    public UICombatText combatDamagePrefab;
    public UICombatText combatCriticalPrefab;
    public UICombatText combatBlockPrefab;
    public UICombatText combatHealPrefab;
    public UICombatText combatPoisonPrefab;
    public UICombatText combatMissPrefab;
    [Header("Gameplay UI")]
    public UIWin uiWin;
    public UILose uiLose;
    public UIPauseGame uiPauseGame;
    public float winGameDelay = 2f;
    public float loseGameDelay = 2f;

    public bool IsAutoPlay { get; set; }
    public bool IsSpeedMultiply { get; set; }
    protected bool isAutoPlayDirty;
    protected bool isEnding;

    public void SpawnDamageText(int amount, BaseCharacterEntity character)
    {
        SpawnCombatText(combatDamagePrefab, amount, character);
    }

    public void SpawnCriticalText(int amount, BaseCharacterEntity character)
    {
        SpawnCombatText(combatCriticalPrefab, amount, character);
    }

    public void SpawnBlockText(int amount, BaseCharacterEntity character)
    {
        SpawnCombatText(combatBlockPrefab, amount, character);
    }

    public void SpawnHealText(int amount, BaseCharacterEntity character)
    {
        SpawnCombatText(combatHealPrefab, amount, character);
    }

    public void SpawnPoisonText(int amount, BaseCharacterEntity character)
    {
        SpawnCombatText(combatPoisonPrefab, amount, character);
    }

    public void SpawnMissText(BaseCharacterEntity character)
    {
        var combatText = Instantiate(combatMissPrefab, combatTextContainer);
        combatText.transform.localScale = Vector3.one;
        combatText.TempObjectFollower.targetObject = character.bodyEffectContainer;
        combatText.Amount = 0;
        combatText.TempText.text = LanguageManager.Texts[GameText.COMBAT_MISS];
    }

    public void SpawnCombatText(UICombatText prefab, int amount, BaseCharacterEntity character)
    {
        var combatText = Instantiate(prefab, combatTextContainer);
        combatText.transform.localScale = Vector3.one;
        combatText.TempObjectFollower.targetObject = character.bodyEffectContainer;
        combatText.Amount = amount;
    }


    protected IEnumerator WinGameRoutine()
    {
        isEnding = true;
        yield return new WaitForSeconds(winGameDelay);
        WinGame();
    }

    protected virtual void WinGame()
    {
        var deadCharacters = CountDeadCharacters();
        GameInstance.GameService.FinishStage(GetStageType(), BattleSession, BaseGameService.BATTLE_RESULT_WIN, deadCharacters, (result) =>
         {
             isEnding = true;
             Time.timeScale = 1;
             GameInstance.Singleton.OnGameServiceFinishStageResult(result);
             uiWin.SetData(result);
             uiWin.Show();
         }, (error) =>
         {
             GameInstance.Singleton.OnGameServiceError(error, WinGame);
         });
    }

    protected IEnumerator LoseGameRoutine()
    {
        isEnding = true;
        yield return new WaitForSeconds(loseGameDelay);
        uiLose.Show();
    }

    public virtual void Revive(UnityAction onError)
    {
        GameInstance.GameService.ReviveCharacters((result) =>
        {
            OnRevive();
        }, (error) =>
        {
            GameInstance.Singleton.OnGameServiceError(error, onError);
        });
    }

    public void Giveup(UnityAction onError)
    {
        var deadCharacters = CountDeadCharacters();
        GameInstance.GameService.FinishStage(GetStageType(),BattleSession, BaseGameService.BATTLE_RESULT_LOSE, deadCharacters, (result) =>
        {
            isEnding = true;
            Time.timeScale = 1;
            GameInstance.Singleton.GetAllPlayerData(GameInstance.LoadAllPlayerDataState.GoToManageScene);
        }, (error) =>
        {
            GameInstance.Singleton.OnGameServiceError(error, onError);
        });
    }

    public void Restart()
    {
        StartStage(PlayingStage);
    }

    public static void StartStage(BaseStage data)
    {
        PlayingStage = data;
        GameInstance.GameService.StartStage(data.Id, (result) =>
        {
            GameInstance.Singleton.OnGameServiceStartStageResult(result);
            BattleSession = result.session;
            GameInstance.Singleton.LoadBattleScene();
        }, (error) =>
        {
            GameInstance.Singleton.OnGameServiceError(error);
        });
    }

    public static void StartTowerStage(BaseStage data, int level)
    {
        PlayingStage = data;
        GameInstance.GameService.StartTowerStage(data.Id + "_" + level, (result) =>
        {
            GameInstance.Singleton.OnGameServiceStartStageResult(result);
            BattleSession = result.session;
            GameInstance.Singleton.LoadBattleScene();
        }, (error) =>
        {
            GameInstance.Singleton.OnGameServiceError(error);
        });
    }

    public virtual void OnRevive()
    {
        isEnding = false;
    }

    public virtual Const.StageType GetStageType()
    {
        return Const.StageType.Tower;
    }

    public abstract int CountDeadCharacters();
}
