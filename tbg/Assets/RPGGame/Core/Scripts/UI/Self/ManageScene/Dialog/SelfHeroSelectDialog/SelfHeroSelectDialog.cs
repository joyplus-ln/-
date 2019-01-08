using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite3TableDataTmp;
using UnityEngine;

public class SelfHeroSelectDialog : Dialog
{
    public UIAttributeShow AttributeShow;

    public SkillListUI skillList;

    public SelfHeroEquipSHow equip1;
    public SelfHeroEquipSHow equip2;

    public SelfHeroEquipSHow equip3;
    private string heroGuid;

    // Use this for initialization
    void Start()
    {
    }

    private void OnEnable()
    {
        if (!string.IsNullOrEmpty(heroGuid))
        {
            RefreshUI();
        }
    }


    public override void Init()
    {

    }

    void ShowSkill()
    {
        skillList.SetData(new List<CustomSkill>(IPlayerHasCharacters.DataMap[heroGuid].Character.GetCustomSkills().Values));
    }

    public void SetData(string heroGuid)
    {
        this.heroGuid = heroGuid;
        RefreshUI();
    }



    void RefreshUI()
    {
        AttributeShow.SetupInfo(IPlayerHasCharacters.DataMap[heroGuid].GetCalculationAttributesWithProp());
        Dictionary<string, IPlayerHasEquips> hasEquips = IPlayerHasEquips.GetHeroEquipses(heroGuid);
        if (hasEquips.ContainsKey("weapon"))
        {
            equip1.SetEquipInfo(hasEquips["weapon"].guid, heroGuid);
        }
        else
        {
            equip1.SetEquipInfo("", heroGuid);
        }
        if (hasEquips.ContainsKey("cloth"))
        {
            equip2.SetEquipInfo(hasEquips["cloth"].guid, heroGuid);
        }
        else
        {
            equip2.SetEquipInfo("", heroGuid);
        }
        if (hasEquips.ContainsKey("shoot"))
        {
            equip3.SetEquipInfo(hasEquips["shoot"].guid, heroGuid);
        }
        else
        {
            equip3.SetEquipInfo("", heroGuid);
        }
        ShowSkill();
    }

    public void ClickEquip(string pos)
    {
    }

    public void IncreasLevel()
    {
        if (heroGuid.Length > 0)
            GameInstance.dbPlayerData.DoCharacterLevelUpItem(heroGuid, new Dictionary<string, int>(), 200, (result) =>
              {
                  //PlayerItem.SetDataRange(result.updateItems);
                  //PlayerItem.SetDataRange(result.deleteItemIds);
                  //PlayerItem.SetDataRange(result.updateItems);
                  RefreshUI();
              });
    }
}
