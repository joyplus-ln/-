using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIFollowWorldObject))]
public class RpguiCharacterStatsGeneric : RPGUIBase
{
    public Text textTitle;
    public Image imageIcon;
    public Text textHpPerMaxHp;
    public Text textHpPercent;

    public Image imageHpGage;
    public UILevel uiLevel;
    public RpguiCharacterBuff[] RpguiBuffs;
    public BaseCharacterEntity character;
    public bool notFollowCharacter;
    //add 战斗中的血量和角色名
    public Text textHp;
    public Text nameText;


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

        //var itemData = character.Item.ItemData;
        var rate = (float)character.Hp / (float)character.MaxHp;

        if (textHpPerMaxHp != null)
            textHpPerMaxHp.text = character.Hp.ToString("N0") + "/" + character.MaxHp.ToString("N0");

        if (textHpPercent != null)
            textHpPercent.text = (rate * 100).ToString("N2") + "%";

        if (textHp != null)
            textHp.text = character.Hp + "/" + character.MaxHp;

        if (nameText != null)
            nameText.text = character.Item.CharacterData.title;


        if (imageHpGage != null)
            imageHpGage.fillAmount = rate;

        //if (textTitle != null)
        //    textTitle.text = itemData.title;

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
            if (i >= RpguiBuffs.Length)
                break;
            var ui = RpguiBuffs[i];
            ui.custombuff = character.Buffs_custom[buffKey];
            ui.Show();
            ++i;
        }
        for (; i < RpguiBuffs.Length; ++i)
        {
            var ui = RpguiBuffs[i];
            ui.Hide();
        }
    }
}
