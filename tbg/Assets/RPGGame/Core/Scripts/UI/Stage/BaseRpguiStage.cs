using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class BaseRpguiStage<TPreparation, TStage> : RpguiDataItem<TStage>
    where TPreparation : RpguiDataItem<TStage>
    where TStage : BaseStage
{
    [Header("General")]
    public Text textTitle;
    public Text textDescription;
    public Text textStageNumber;
    public Image imageIcon;
    public RpguiStamina RpguiRequireStamina;
    public Text textRewardPlayerExp;
    public Text textRewardCharacterExp;
    public RpguiItemList RpguiRewardsItemList;
    public RpguiItemList RpguiEnemyItemList;
    [Header("Relates Elements")]
    public Button[] interactableButtonsWhenUnlocked;
    public GameObject[] activeObjectsWhenUnlocked;
    public GameObject[] inactiveObjectsWhenUnlocked;
    public UnityEvent eventSetStagePreparation;

    public override void Clear()
    {
        // Don't clear
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.Id);
    }

    public override void UpdateData()
    {
        SetupInfo(data);
    }

    private void SetupInfo(TStage data)
    {
        if (textTitle != null)
            textTitle.text = data.title;

        if (textDescription != null)
            textDescription.text = data.description;

        if (textStageNumber != null)
            textStageNumber.text = data.stageNumber;

        if (imageIcon != null)
            imageIcon.sprite = data.icon;

        if (RpguiRequireStamina != null)
        {
            var staminaData = PlayerStamina.StageStamina.Clone().SetAmount(data.requireStamina, 0);
            RpguiRequireStamina.SetData(staminaData);
        }

        if (textRewardPlayerExp != null)
            textRewardPlayerExp.text = data.rewardPlayerExp.ToString("N0");

        if (textRewardCharacterExp != null)
            textRewardCharacterExp.text = data.rewardCharacterExp.ToString("N0");

        if (RpguiRewardsItemList != null)
        {
            var list = data.GetRewardItems();
            //RpguiRewardsItemList.SetListItems(list, (ui) => ui.displayStats = RpguiItem.DisplayStats.Hidden);
        }

        if (RpguiEnemyItemList != null)
        {
            var list = data.GetCharacters();
            list.SortLevel();
            //RpguiEnemyItemList.SetListItems(list, (ui) => ui.displayStats = RpguiItem.DisplayStats.Level);
        }

        UpdateElementsWhenUnlocked();
    }

    public void UpdateElementsWhenUnlocked()
    {
        var isUnlocked = PlayerClearStage.IsUnlock(data);
        foreach (var button in interactableButtonsWhenUnlocked)
        {
            button.interactable = isUnlocked;
        }
        foreach (var obj in activeObjectsWhenUnlocked)
        {
            obj.SetActive(isUnlocked);
        }
        foreach (var obj in inactiveObjectsWhenUnlocked)
        {
            obj.SetActive(!isUnlocked);
        }
    }

    public void SetStagePreparationData()
    {
        if (StagePreparation != null)
        {
            StagePreparation.data = data;
            eventSetStagePreparation.Invoke();
        }
    }

    public void OnClickStartStage()
    {
        BaseGamePlayManager.StartStage(data);
    }

    public abstract TPreparation StagePreparation { get; }
}
