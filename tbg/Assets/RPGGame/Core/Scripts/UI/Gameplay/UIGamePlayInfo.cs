using System.Collections;
using System.Collections.Generic;
using EventUtil;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlayInfo : MonoBehaviour
{

    public CharacterEntity SelfEntity;
    public Text AAttackText;
    public Text MAttackText;
    public Text ADefText;
    public Text MDefText;
    public Text CirText;
    public Text SpeedText;

    public Text hp;
    public Text eva;
    public Text acc;
    public Text cirtchance;
    public Text cirtdamage;
    public Text blockchance;
    public Text blockdamage;




    public Text BuffText;
    public Transform buffContent;

    private BaseCharacterEntity character;

    private List<Text> buffList = new List<Text>();


    // Use this for initialization
    void Start()
    {
        EventDispatcher.AddEventListener<BaseCharacterEntity>("FightInfoButtonClick", FightInfoClick);
        StartCoroutine(ShowInfo());
    }

    void OnDestroy()
    {
        EventDispatcher.RemoveEventListener<BaseCharacterEntity>("FightInfoButtonClick", FightInfoClick);
    }

    public void FightInfoClick(BaseCharacterEntity character)
    {
        this.SelfEntity = character as CharacterEntity;
    }

    IEnumerator ShowInfo()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            ShowPlayInfo();
            SetBuffs();
        }
    }

    // Update is called once per frame
    void ShowPlayInfo()
    {
        if (SelfEntity == null) return;
        if (AAttackText != null)
            AAttackText.text = "物攻:" + SelfEntity.GetTotalAttributes().pAtk;

        if (MAttackText != null)
            MAttackText.text = "魔攻:" + SelfEntity.GetTotalAttributes().mAtk;

        if (ADefText != null)
            ADefText.text = "物御:" + SelfEntity.GetTotalAttributes().pDef;

        if (MDefText != null)
            MDefText.text = "魔防:" + SelfEntity.GetTotalAttributes().mDef;

        if (CirText != null)
            CirText.text = "暴击:" + SelfEntity.GetTotalAttributes().critChance;

        if (SpeedText != null)
            SpeedText.text = "速度:" + SelfEntity.GetTotalAttributes().acc;

        if (hp != null)
            hp.text = "血量:" + SelfEntity.Hp + "/" + SelfEntity.MaxHp;

        if (eva != null)
            eva.text = "回避:" + SelfEntity.GetTotalAttributes().eva;

        if (cirtchance != null)
            cirtchance.text = "暴击:" + SelfEntity.GetTotalAttributes().critChance;

        if (cirtdamage != null)
            cirtdamage.text = "爆伤:" + SelfEntity.GetTotalAttributes().critDamageRate;

        if (blockchance != null)
            blockchance.text = "格挡:" + SelfEntity.GetTotalAttributes().blockChance;

        if (blockdamage != null)
            blockdamage.text = "格挡伤害:" + SelfEntity.GetTotalAttributes().blockDamageRate;

    }



    public void SetBuffs()
    {
        if (SelfEntity == null) return;
        Dictionary<string, BaseCharacterBuff> Buffs = SelfEntity.Buffs;
        Dictionary<string, CustomBuff> Buffs_custom = SelfEntity.Buffs_custom;
        BuffText.text = "Buffs:" + (Buffs.Count + Buffs_custom.Count);
        int i = 0;
        foreach (var key in Buffs.Keys)
        {
            AddBuffText(i, Buffs[key].Buff.des);
            i++;
        }
        foreach (var key in Buffs_custom.Keys)
        {
            AddBuffText(i, Buffs_custom[key].des);
            i++;
        }
        ReMoveBuffText(i);
    }

    void AddBuffText(int index, string dex)
    {
        if (string.IsNullOrEmpty(dex))
            dex = "des null";
        if (buffList.Count > index)
        {
            buffList[index].text = dex;
        }
        else
        {
            GameObject bufftextGameObject = Instantiate(BuffText.gameObject);
            bufftextGameObject.transform.SetParent(buffContent, false);
            Text buText = bufftextGameObject.GetComponent<Text>();
            buText.text = index + "";
            buffList.Add(buText);
            buffList[index].text = dex;

        }
    }

    void ReMoveBuffText(int index)
    {
        if (buffList.Count > index)
        {
            for (int i = index; i < buffList.Count; i++)
            {
                buffList[i].text = "";
            }
        }
    }

}
