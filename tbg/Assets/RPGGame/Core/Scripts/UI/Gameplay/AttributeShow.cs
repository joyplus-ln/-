using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeShow : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }
    [Header("Fix attributes")]
    public GameObject containerHp;
    public Text textHp;
    public GameObject containerPAtk;
    public Text textPAtk;
    public GameObject containerPDef;
    public Text textPDef;
    public GameObject containerMAtk;
    public Text textMAtk;
    public GameObject containerMDef;
    public Text textMDef;
    public GameObject containerSpd;
    public Text textSpd;
    public GameObject containerEva;
    public Text textEva;
    public GameObject containerAcc;
    public Text textAcc;
    [Header("Rate attributes")]
    public GameObject containerHpRate;
    public Text textHpRate;
    public GameObject containerPAtkRate;
    public Text textPAtkRate;
    public GameObject containerPDefRate;
    public Text textPDefRate;
    public GameObject containerMAtkRate;
    public Text textMAtkRate;
    public GameObject containerMDefRate;
    public Text textMDefRate;
    public GameObject containerSpdRate;
    public Text textSpdRate;
    public GameObject containerEvaRate;
    public Text textEvaRate;
    public GameObject containerAccRate;
    public Text textAccRate;
    [Header("Critical attributes")]
    public GameObject containerCritChance;
    public Text textCritChance;
    public GameObject containerCritDamageRate;
    public Text textCritDamageRate;
    [Header("Block attributes")]
    public GameObject containerBlockChance;
    public Text textBlockChance;
    public GameObject containerBlockDamageRate;
    public Text textBlockDamageRate;
    [Header("Options")]
    public bool useFormatForInfo;
    public bool hideInfoIfEmpty;


    public void Clear()
    {
        SetupInfo(null);
    }

    public void SetupInfo(CalculationAttributes data)
    {
        if (data == null)
            data = new CalculationAttributes();

        if (textHp != null)
        {
            textHp.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_HP, data.hp) : RPGLanguageManager.FormatNumber(data.hp);
            if (hideInfoIfEmpty && containerHp != null)
                containerHp.SetActive(Mathf.Abs(data.hp) > 0);
        }

        if (textPAtk != null)
        {
            textPAtk.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_PATK, data.pAtk) : RPGLanguageManager.FormatNumber(data.pAtk);
            if (hideInfoIfEmpty && containerPAtk != null)
                containerPAtk.SetActive(Mathf.Abs(data.pAtk) > 0);
        }

        if (textPDef != null)
        {
            textPDef.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_PDEF, data.pDef) : RPGLanguageManager.FormatNumber(data.pDef);
            if (hideInfoIfEmpty && containerPDef != null)
                containerPDef.SetActive(Mathf.Abs(data.pDef) > 0);
        }

        if (textMAtk != null)
        {
            textMAtk.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_MATK, data.mAtk) : RPGLanguageManager.FormatNumber(data.mAtk);
            if (hideInfoIfEmpty && containerMAtk != null)
                containerMAtk.SetActive(Mathf.Abs(data.mAtk) > 0);
        }

        if (textMDef != null)
        {
            textMDef.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_MDEF, data.mDef) : RPGLanguageManager.FormatNumber(data.mDef);
            if (hideInfoIfEmpty && containerMDef != null)
                containerMDef.SetActive(Mathf.Abs(data.mDef) > 0);
        }

        if (textSpd != null)
        {
            textSpd.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_SPD, data.spd) : RPGLanguageManager.FormatNumber(data.spd);
            if (hideInfoIfEmpty && containerSpd != null)
                containerSpd.SetActive(Mathf.Abs(data.spd) > 0);
        }

        if (textEva != null)
        {
            textEva.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_EVA, data.eva) : RPGLanguageManager.FormatNumber(data.eva);
            if (hideInfoIfEmpty && containerEva != null)
                containerEva.SetActive(Mathf.Abs(data.eva) > 0);
        }

        if (textAcc != null)
        {
            textAcc.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_ACC, data.acc) : RPGLanguageManager.FormatNumber(data.acc);
            if (hideInfoIfEmpty && containerAcc != null)
                containerAcc.SetActive(Mathf.Abs(data.acc) > 0);
        }

        if (textHpRate != null)
        {
            textHpRate.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_HP_RATE, data.exp_hpRate, true) : RPGLanguageManager.FormatNumber(data.exp_hpRate, true);
            if (hideInfoIfEmpty && containerHpRate != null)
                containerHpRate.SetActive(Mathf.Abs(data.exp_hpRate) > 0);
        }

        if (textPAtkRate != null)
        {
            textPAtkRate.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_PATK_RATE, data.exp_pAtkRate, true) : RPGLanguageManager.FormatNumber(data.exp_pAtkRate, true);
            if (hideInfoIfEmpty && containerPAtkRate != null)
                containerPAtkRate.SetActive(Mathf.Abs(data.exp_pAtkRate) > 0);
        }

        if (textPDefRate != null)
        {
            textPDefRate.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_PDEF_RATE, data.exp_pDefRate, true) : RPGLanguageManager.FormatNumber(data.exp_pDefRate, true);
            if (hideInfoIfEmpty && containerPDefRate != null)
                containerPDefRate.SetActive(Mathf.Abs(data.exp_pDefRate) > 0);
        }

        if (textMAtkRate != null)
        {
            textMAtkRate.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_MATK_RATE, data.exp_mAtkRate, true) : RPGLanguageManager.FormatNumber(data.exp_mAtkRate, true);
            if (hideInfoIfEmpty && containerMAtkRate != null)
                containerMAtkRate.SetActive(Mathf.Abs(data.exp_mAtkRate) > 0);
        }

        if (textMDefRate != null)
        {
            textMDefRate.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_MDEF_RATE, data.exp_mDefRate, true) : RPGLanguageManager.FormatNumber(data.exp_mDefRate, true);
            if (hideInfoIfEmpty && containerMDefRate != null)
                containerMDefRate.SetActive(Mathf.Abs(data.exp_mDefRate) > 0);
        }

        if (textSpdRate != null)
        {
            textSpdRate.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_SPD_RATE, data.exp_spdRate, true) : RPGLanguageManager.FormatNumber(data.exp_spdRate, true);
            if (hideInfoIfEmpty && containerSpdRate != null)
                containerSpdRate.SetActive(Mathf.Abs(data.exp_spdRate) > 0);
        }

        if (textEvaRate != null)
        {
            textEvaRate.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_EVA_RATE, data.exp_evaRate, true) : RPGLanguageManager.FormatNumber(data.exp_evaRate, true);
            if (hideInfoIfEmpty && containerEvaRate != null)
                containerEvaRate.SetActive(Mathf.Abs(data.exp_evaRate) > 0);
        }

        if (textAccRate != null)
        {
            textAccRate.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_ACC_RATE, data.exp_accRate, true) : RPGLanguageManager.FormatNumber(data.exp_accRate, true);
            if (hideInfoIfEmpty && containerAccRate != null)
                containerAccRate.SetActive(Mathf.Abs(data.exp_accRate) > 0);
        }

        if (textCritChance != null)
        {
            textCritChance.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_CRIT_CHANCE, data.exp_critChance, true) : RPGLanguageManager.FormatNumber(data.exp_critChance, true);
            if (hideInfoIfEmpty && containerCritChance != null)
                containerCritChance.SetActive(Mathf.Abs(data.exp_critChance) > 0);
        }

        if (textCritDamageRate != null)
        {
            textCritDamageRate.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_CRIT_DAMAGE_RATE, data.exp_critDamageRate, true) : RPGLanguageManager.FormatNumber(data.exp_critDamageRate, true);
            if (hideInfoIfEmpty && containerCritDamageRate != null)
                containerCritDamageRate.SetActive(Mathf.Abs(data.exp_critDamageRate) > 0);
        }

        if (textBlockChance != null)
        {
            textBlockChance.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_BLOCK_CHANCE, data.exp_blockChance, true) : RPGLanguageManager.FormatNumber(data.exp_blockChance, true);
            if (hideInfoIfEmpty && containerBlockChance != null)
                containerBlockChance.SetActive(Mathf.Abs(data.exp_blockChance) > 0);
        }

        if (textBlockDamageRate != null)
        {
            textBlockDamageRate.text = useFormatForInfo ? RPGLanguageManager.FormatInfo(GameText.TITLE_ATTRIBUTE_BLOCK_DAMAGE_RATE, data.exp_blockDamageRate, true) : RPGLanguageManager.FormatNumber(data.exp_blockDamageRate, true);
            if (hideInfoIfEmpty && containerBlockDamageRate != null)
                containerBlockDamageRate.SetActive(Mathf.Abs(data.exp_blockDamageRate) > 0);
        }
    }
}
