using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class UIItemEvent : UnityEvent<RpguiItem> { }

public class RpguiItem : RpguiDataItem<ICharacter>
{
    public enum DisplayStats
    {
        Level,
        SellPrice,
        RewardExp,
        SelectedAmount,
        AttributeHp,
        AttributePAtk,
        AttributePDef,
        AttributeMAtk,
        AttributeMDef,
        AttributeSpd,
        AttributeEva,
        AttributeAcc,
        AttributeHpRate,
        AttributePAtkRate,
        AttributePDefRate,
        AttributeMAtkRate,
        AttributeMDefRate,
        AttributeSpdRate,
        AttributeEvaRate,
        AttributeAccRate,
        AttributeCritChance,
        AttributeCritDamageRate,
        AttributeBlockChance,
        AttributeBlockDamageRate,
        Hidden,
    }
    [Header("General")]
    public Text textTitle;
    public Text textDescription;
    public Image imageIcon;
    public Text nameText;//角色名字
    [Header("Relates Level Up UIs")]
    public Button buttonLevelUp;
    public UnityEvent eventSelectLevelUpItem;
    [Header("Relates Evolve UIs")]
    public Button buttonEvolve;
    public UnityEvent eventSelectEvolveItem;
    [Header("Relates Sell UIs")]
    public Button buttonSell;
    public UnityEvent eventSelectSellItem;
    [Header("Relates Equipment Manager UIs")]
    public Button buttonEquipmentManage;
    public UnityEvent eventSelectEquipmentManageCharacter;
    [Header("Usage status")]
    public GameObject inTeamObject;
    public GameObject inSelectedTeamObject;
    public GameObject equippedObject;
    public bool notShowInTeamStatus;
    public bool notShowEquippedStatus;
    [Header("Stats")]
    public DisplayStats displayStats;
    public Text textDisplayStats;
    public Text textAttributes;
    public UILevel uiLevel;
    public RpguiAttributes RpguiAttributes;
    public RpguiCurrency RpguiEvolvePrice;
    public RpguiCurrency RpguiSellPrice;
    public Text textRewardExp;
    public Text textSelectedAmount;
    [Header("Character")]
    public Transform characterModelContainer;
    public int characterModelLayer;
    [Header("Options")]
    public bool useFormatForInfo;
    public bool excludeEquipmentAttributes;

    // Selection
    private int selectedAmount;
    public int SelectedAmount
    {
        get { return selectedAmount; }
        set
        {
            //todo
            //var maxAmount = data == null ? 0 : data.amount;
            //selectedAmount = value > maxAmount ? maxAmount : value;
            SetupSelectedAmount();
        }
    }

    private int requiredAmount;
    public int RequiredAmount
    {
        get { return requiredAmount; }
        set
        {
            requiredAmount = value;
            SetupSelectedAmount();
        }
    }

    public override bool Selected
    {
        get { return SelectedAmount > 0; }
        set
        {//todo
            //if (value)
            //    SelectedAmount = data.amount;
            //else
            //    SelectedAmount = 0;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (inTeamObject != null)
            inTeamObject.SetActive(false);
        if (inSelectedTeamObject != null)
            inSelectedTeamObject.SetActive(false);
        if (equippedObject != null)
            equippedObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        UpdateDisplayStats();
        if (!IsEmpty())
        {
            if (!notShowInTeamStatus)
            {
                //todo
                //var isInAnyTeam = data.InTeamFormations.Count > 0;
                //if (inTeamObject != null)
                //    inTeamObject.SetActive(isInAnyTeam);

            }
            else
            {
                if (inTeamObject != null)
                    inTeamObject.SetActive(false);
                if (inSelectedTeamObject != null)
                    inSelectedTeamObject.SetActive(false);
            }

            if (!notShowEquippedStatus)
            {
                //todo
                //var isEquipped = data.equipItemId != null;
                //if (equippedObject != null)
                    //equippedObject.SetActive(isEquipped);
            }
            else
            {
                if (equippedObject != null)
                    equippedObject.SetActive(false);
            }
        }
        else
        {
            if (inTeamObject != null)
                inTeamObject.SetActive(false);
            if (inSelectedTeamObject != null)
                inSelectedTeamObject.SetActive(false);
            if (equippedObject != null)
                equippedObject.SetActive(false);
        }
    }

    private void UpdateDisplayStats()
    {
        if (textDisplayStats == null)
            return;

        if (IsEmpty())
        {
            textDisplayStats.text = "";
            return;
        }

        var attributes = data.GetAttributes().GetSubAttributes();

        switch (displayStats)
        {
            case DisplayStats.Level:
                textDisplayStats.text = RPGLanguageManager.FormatInfo(GameText.TITLE_LEVEL, 99999);//data.Level);
                return;
            case DisplayStats.SellPrice:
                textDisplayStats.text = "unknown";//data.SellPrice.ToString("N0");
                return;
            case DisplayStats.RewardExp:
                textDisplayStats.text = "unknown"; //data.RewardExp.ToString("N0");
                return;
            case DisplayStats.SelectedAmount:
                textDisplayStats.text = SelectedAmount.ToString("N0");
                return;
            case DisplayStats.AttributeHp:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_HP, attributes.hp);
                return;
            case DisplayStats.AttributePAtk:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_PATK, attributes.pAtk);
                return;
            case DisplayStats.AttributePDef:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_PDEF, attributes.pDef);
                return;
            case DisplayStats.AttributeMAtk:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_MATK, attributes.mAtk);
                return;
            case DisplayStats.AttributeMDef:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_MDEF, attributes.mDef);
                return;
            case DisplayStats.AttributeSpd:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_SPD, attributes.spd);
                return;
            case DisplayStats.AttributeEva:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_EVA, attributes.eva);
                return;
            case DisplayStats.AttributeAcc:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_ACC, attributes.acc);
                return;
            case DisplayStats.AttributeHpRate:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_HP_RATE, attributes.exp_hpRate);
                return;
            case DisplayStats.AttributePAtkRate:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_PATK_RATE, attributes.exp_pAtkRate);
                return;
            case DisplayStats.AttributePDefRate:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_PDEF_RATE, attributes.exp_pDefRate);
                return;
            case DisplayStats.AttributeMAtkRate:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_MATK_RATE, attributes.exp_mAtkRate);
                return;
            case DisplayStats.AttributeMDefRate:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_MDEF_RATE, attributes.exp_mDefRate);
                return;
            case DisplayStats.AttributeSpdRate:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_SPD_RATE, attributes.exp_spdRate);
                return;
            case DisplayStats.AttributeEvaRate:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_EVA_RATE, attributes.exp_evaRate);
                return;
            case DisplayStats.AttributeAccRate:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_ACC_RATE, attributes.exp_accRate);
                return;
            case DisplayStats.AttributeCritChance:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_CRIT_CHANCE, attributes.exp_critChance);
                return;
            case DisplayStats.AttributeCritDamageRate:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_CRIT_DAMAGE_RATE, attributes.exp_critDamageRate);
                return;
            case DisplayStats.AttributeBlockChance:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_BLOCK_CHANCE, attributes.exp_blockChance);
                return;
            case DisplayStats.AttributeBlockDamageRate:
                textDisplayStats.text = attributes == null ? "" : RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_BLOCK_DAMAGE_RATE, attributes.exp_blockDamageRate);
                return;
            default:
                textDisplayStats.text = "";
                return;
        }
    }

    public override void UpdateData()
    {
        //SetupInfo(data.GetPlayerItem());
        SetupSelectedAmount();
    }

    public override void Clear()
    {
        SetupInfo(null);
        SelectedAmount = 0;
        RequiredAmount = 0;
        SetupSelectedAmount();
    }

    private void SetupInfo(IPlayerHasCharacters data)
    {
        if (data == null)
            return;

        //var attributes = data.GetItemAttributes();
        var itemData = data.Character;

        if (textTitle != null)
            textTitle.text = itemData == null ? "" : itemData.title;

        if (textDescription != null)
            textDescription.text = itemData == null ? "" : itemData.description;

        //if (imageIcon != null)
        //    imageIcon.sprite = data.Icon;//暂时隐藏图片

        if (nameText != null)
        {
            nameText.text = data.guid;
            nameText.color = GameInstance.GameDatabase.SsrColor;
        }

        //if (textAttributes != null)
        //    textAttributes.text = attributes == null ? "" : attributes.GetDescription(data.EquipmentBonus);

        //// Attributes
        //if (RpguiAttributes != null)
        //{
        //    //todo
        //    //if (data.ActorItemData != null)
        //    //    RpguiAttributes.Show();
        //    //else
        //    //    RpguiAttributes.Hide();

        //    if (excludeEquipmentAttributes)
        //        RpguiAttributes.SetData(attributes);
        //    else
        //        RpguiAttributes.SetData(attributes == null ? data.EquipmentBonus : attributes + data.EquipmentBonus);
        //}

        // Stats
        //if (uiLevel != null)
        //{
        //    uiLevel.gameObject.SetActive(true);//(data.ActorItemData != null);
        //    uiLevel.level = data.Level;
        //    uiLevel.maxLevel = data.MaxLevel;
        //    uiLevel.collectExp = data.CollectExp;
        //    uiLevel.nextExp = data.NextExp;
        //}

        if (RpguiEvolvePrice != null)
        {
            var amount = Const.EnovePrice;
            var currencyData = IPlayerCurrency.SoftCurrency.Clone().SetAmount(amount, 0);
            RpguiEvolvePrice.SetData(currencyData);
        }

        if (RpguiSellPrice != null)
        {
            var amount = Const.SellPrice;
            var currencyData = IPlayerCurrency.SoftCurrency.Clone().SetAmount(amount, 0);
            RpguiSellPrice.SetData(currencyData);
        }

        //if (textRewardExp != null)
        //    textRewardExp.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_REWARD_EXP, data.RewardExp) : data.RewardExp.ToString("N0");

        //if (characterModelContainer != null)
        //{
        //    characterModelContainer.RemoveAllChildren();
        //    if (data.CharacterData != null)
        //    {
        //        var character = Instantiate(GameInstance.Singleton.model);
        //        character.Container = characterModelContainer;
        //        var characterRenderers = character.GetComponentsInChildren<Renderer>();
        //        foreach (var characterRenderer in characterRenderers)
        //        {
        //            characterRenderer.gameObject.layer = characterModelLayer;
        //        }
        //    }
        //}
    }

    public void SetupSelectedAmount()
    {
        SetupSelectedAmount(SelectedAmount, RequiredAmount);
    }

    public void SetupSelectedAmount(int selectedAmount, int requiredAmount)
    {
        if (textSelectedAmount != null)
        {
            if (selectedAmount == 0 && requiredAmount == 0)
                textSelectedAmount.text = "";
            else if (requiredAmount == 0)
                textSelectedAmount.text = selectedAmount.ToString("N0");
            else if (selectedAmount == 0)
                textSelectedAmount.text = "0/" + requiredAmount.ToString("N0");
            else
                textSelectedAmount.text = selectedAmount.ToString("N0") + "/" + requiredAmount.ToString("N0");
        }
    }

    public void Select(int amount, bool invokeEvent = true)
    {
        SelectedAmount = amount;
        if (invokeEvent)
            eventSelect.Invoke(this);
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.guid);
    }
}
