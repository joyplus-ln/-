using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ToggleGroup))]
public class RpguiCharacterActionManager : RPGUIBase
{
    public RpguiCharacterAction[] RpguiActions;
    public GamePlayManager Manager { get { return GamePlayManager.Singleton; } }
    private readonly List<RpguiCharacterActionSkill> UICharacterSkills = new List<RpguiCharacterActionSkill>();

    private ToggleGroup tempToggleGroup;
    public ToggleGroup TempToggleGroup
    {
        get
        {
            if (tempToggleGroup == null)
                tempToggleGroup = GetComponent<ToggleGroup>();
            return tempToggleGroup;
        }
    }

    public CharacterEntity ActiveCharacter
    {
        get { return Manager.ActiveCharacter; }
    }

    public bool IsPlayerCharacterActive
    {
        get { return ActiveCharacter != null && ActiveCharacter.IsPlayerCharacter; }
    }

    protected override void Awake()
    {
        base.Awake();
        TempToggleGroup.allowSwitchOff = false;
        var skillIndex = 0;
        foreach (var uiAction in RpguiActions)
        {
            uiAction.ActionManager = this;
            uiAction.IsOn = false;
            var uiSkill = uiAction as RpguiCharacterActionSkill;
            if (uiSkill != null)
            {
                uiSkill.skillIndex = skillIndex;
                UICharacterSkills.Add(uiSkill);
                ++skillIndex;
            }
        }
    }

    private void Update()
    {
        if (!IsPlayerCharacterActive || ActiveCharacter.IsDoingAction)
        {
            Hide();
            return;
        }

        var i = 0;
        foreach (var skill in Manager.ActiveCharacter.CustomSkills)
        {
            if (i >= UICharacterSkills.Count)
                break;
            var ui = UICharacterSkills[i];
            ui.cskill = skill;
            ui.Show();
            ++i;
        }

        for (; i < UICharacterSkills.Count; ++i)
        {
            var ui = UICharacterSkills[i];
            ui.Hide();
        }
    }

    public override void Show()
    {
        var i = 0;
        for (i = 0; i < RpguiActions.Length; ++i)
        {
            RpguiActions[i].IsOn = false;
            if (i == 0)
                RpguiActions[i].IsOn = true;
        }
        i = 0;
        for (; i < UICharacterSkills.Count; ++i)
        {
            var ui = UICharacterSkills[i];
            ui.Hide();
        }
        base.Show();
    }
}
