using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIFollowWorldObject))]
public class UICharacterStatsGeneric : UIBase
{
    public Text textTitle;
    public Image imageIcon;
    public Text textHpPerMaxHp;
    public Text textHpPercent;

    public Image imageHpGage;
    public UILevel uiLevel;
    public UICharacterBuff[] uiBuffs;
    public BaseCharacterEntity character;
    public bool notFollowCharacter;
    //add 战斗中的血量和角色名
    public Text textHp;
    public Text nameText;

    public Text AAttackText;
    public Text MAttackText;
    public Text ADefText;
    public Text MDefText;
    public Text CirText;
    public Text SpeedText;

    private UIFollowWorldObject tempObjectFollower;
    public UIFollowWorldObject TempObjectFollower
    {
        get
        {
            if (tempObjectFollower == null)
                tempObjectFollower = GetComponent<UIFollowWorldObject>();
            return tempObjectFollower;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (notFollowCharacter)
        {
            TempObjectFollower.enabled = false;
            TempObjectFollower.TempPositionFollower.enabled = false;
        }
    }

    protected virtual void Update()
    {
        if (character == null)
            return;

        if (!notFollowCharacter)
            TempObjectFollower.targetObject = character.uiContainer;

        var itemData = character.Item.ItemData;
        var rate = (float)character.Hp / (float)character.MaxHp;

        if (textHpPerMaxHp != null)
            textHpPerMaxHp.text = character.Hp.ToString("N0") + "/" + character.MaxHp.ToString("N0");

        if (textHpPercent != null)
            textHpPercent.text = (rate * 100).ToString("N2") + "%";

        if (textHp != null)
            textHp.text = character.Hp + "/" + character.MaxHp;

        if (nameText != null)
            nameText.text = character.Item.GUID;

        if (AAttackText != null)
            AAttackText.text = "物攻:" + character.GetTotalAttributes().pAtk;

        if (MAttackText != null)
            MAttackText.text = "魔攻:" + character.GetTotalAttributes().mAtk;

        if (ADefText != null)
            ADefText.text = "物御:" + character.GetTotalAttributes().pDef;

        if (MDefText != null)
            MDefText.text = "魔防:" + character.GetTotalAttributes().mDef;

        if (CirText != null)
            CirText.text = "暴击:" + character.GetTotalAttributes().critChance;

        if (SpeedText != null)
            SpeedText.text = "速度:" + character.GetTotalAttributes().acc;

        if (imageHpGage != null)
            imageHpGage.fillAmount = rate;

        if (textTitle != null)
            textTitle.text = itemData.title;

        //if (imageIcon != null)
        //    imageIcon.sprite = itemData.icon;

        if (uiLevel != null)
        {
            uiLevel.level = character.Item.Level;
            uiLevel.maxLevel = character.Item.MaxLevel;
            uiLevel.collectExp = character.Item.CollectExp;
            uiLevel.nextExp = character.Item.NextExp;
        }

        var i = 0;
        var custom_buffKeys = character.Buffs_custom.Keys;
        foreach (var buffKey in custom_buffKeys)
        {
            if (i >= uiBuffs.Length)
                break;
            var ui = uiBuffs[i];
            ui.custombuff = character.Buffs_custom[buffKey];
            ui.Show();
            ++i;
        }
        for (; i < uiBuffs.Length; ++i)
        {
            var ui = uiBuffs[i];
            ui.Hide();
        }
    }
}
