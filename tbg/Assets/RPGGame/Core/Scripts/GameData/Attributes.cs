using System;
using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Attribute<T>
{
    public T minValue;
    public T maxValue;
    public float growth;
    public abstract T Calculate(int currentLevel, int maxLevel);
    public virtual string ToJson()
    {
        return "{\"minValue\":" + minValue + "," +
            "\"maxValue\":" + maxValue + "," +
            "\"growth\":" + growth + "}";
    }
}

[Serializable]
public class Int32Attribute : Attribute<int>
{
    public Int32Attribute Clone()
    {
        var result = new Int32Attribute();
        result.minValue = minValue;
        result.maxValue = maxValue;
        result.growth = growth;
        return result;
    }

    public void SetData(int min, int max, float growth)
    {
        this.minValue = min;
        this.maxValue = max;
        this.growth = growth;
    }

    public override int Calculate(int currentLevel, int maxLevel)
    {
        if (currentLevel <= 0)
            currentLevel = 1;
        if (maxLevel <= 0)
            maxLevel = 1;
        if (currentLevel == 1)
            return minValue;
        if (currentLevel == maxLevel)
            return maxValue;
        return minValue + Mathf.RoundToInt((maxValue - minValue) * Mathf.Pow((float)(currentLevel - 1) / (float)(maxLevel - 1), growth));
    }

    public static Int32Attribute operator *(Int32Attribute a, float multiplier)
    {
        var result = a.Clone();
        result.minValue = Mathf.RoundToInt(a.minValue * multiplier);
        result.maxValue = Mathf.RoundToInt(a.maxValue * multiplier);
        return result;
    }
}

[Serializable]
public class SingleAttribute : Attribute<float>
{
    public SingleAttribute Clone()
    {
        var result = new SingleAttribute();
        result.minValue = minValue;
        result.maxValue = maxValue;
        result.growth = growth;
        return result;
    }

    public override float Calculate(int currentLevel, int maxLevel)
    {
        if (currentLevel <= 0)
            currentLevel = 1;
        if (maxLevel <= 0)
            maxLevel = 1;
        if (currentLevel == 1)
            return minValue;
        if (currentLevel == maxLevel)
            return maxValue;
        return minValue + ((maxValue - minValue) * Mathf.Pow((float)(currentLevel - 1) / (float)(maxLevel - 1), growth));
    }

    public static SingleAttribute operator *(SingleAttribute a, float multiplier)
    {
        var result = a.Clone();
        result.minValue = a.minValue * multiplier;
        result.maxValue = a.maxValue * multiplier;
        return result;
    }
}

[Serializable]
public class Attributes
{
    public Int32Attribute hp = new Int32Attribute();
    public Int32Attribute pAtk = new Int32Attribute();
    public Int32Attribute pDef = new Int32Attribute();
    public Int32Attribute mAtk = new Int32Attribute();
    public Int32Attribute mDef = new Int32Attribute();
    public Int32Attribute spd = new Int32Attribute();
    public Int32Attribute eva = new Int32Attribute();
    public Int32Attribute acc = new Int32Attribute();

    public int exp_hp;
    public int exp_patk;
    public int exp_pdef;
    public int exp_matk;
    public int exp_mdef;
    public int exp_spd;
    public int exp_eva;
    public int exp_acc;
    public float exp_hpRate;
    public float exp_pAtkRate;
    public float exp_pDefRate;
    public float exp_mAtkRate;
    public float exp_mDefRate;
    public float exp_spdRate;
    public float exp_evaRate;
    public float exp_accRate;
    public float exp_critChance;
    public float exp_critDamageRate;
    public float exp_blockChance;
    public float exp_blockDamageRate;

    public int level = 1;

    public Attributes()
    {

    }

    public Attributes(int level)
    {
        this.level = level;
    }

    //public Attributes Clone()
    //{
    //    Attributes result = new Attributes();
    //    result.hp = hp.Clone();
    //    result.pAtk = pAtk.Clone();
    //    result.pDef = pDef.Clone();
    //    result.mAtk = mAtk.Clone();
    //    result.mDef = mDef.Clone();
    //    result.spd = spd.Clone();
    //    result.eva = eva.Clone();
    //    result.acc = acc.Clone();
    //    return result;
    //}

    /// <summary>
    /// 获取未计算之前的属性,用于展示面板
    /// </summary>
    /// <param name="currentLevel"></param>
    /// <param name="maxLevel"></param>
    /// <returns></returns>
    public CalculationAttributes GetCreateCalculationAttributes( int maxLevel = Const.MaxLevel)
    {
        CalculationAttributes result = new CalculationAttributes();
        result.hp = hp.Calculate(level, maxLevel);
        result.pAtk = pAtk.Calculate(level, maxLevel);
        result.pDef = pDef.Calculate(level, maxLevel);
        result.mAtk = mAtk.Calculate(level, maxLevel);
        result.mDef = mDef.Calculate(level, maxLevel);
        result.spd = spd.Calculate(level, maxLevel);
        result.eva = eva.Calculate(level, maxLevel);
        result.acc = acc.Calculate(level, maxLevel);
        result.exp_hpRate = exp_hpRate;
        result.exp_pAtkRate = exp_pAtkRate;
        result.exp_pDefRate = exp_pDefRate;
        result.exp_mAtkRate = exp_mAtkRate;
        result.exp_mDefRate = exp_mDefRate;
        result.exp_spdRate = exp_spdRate;
        result.exp_evaRate = exp_evaRate;
        result.exp_accRate = exp_accRate;
        result.exp_critChance = exp_critChance;
        result.exp_critDamageRate = exp_critDamageRate;
        result.exp_blockChance = exp_blockChance;
        result.exp_blockDamageRate = exp_blockDamageRate;
        return result;
    }
    /// <summary>
    /// 获取计算后的最终属性,用于战斗计算
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public CalculationAttributes GetSubAttributes()
    {
        var result = GetCreateCalculationAttributes();

        result.hp += exp_hp;
        result.pAtk += exp_patk;
        result.pDef += exp_pdef;
        result.mAtk += exp_matk;
        result.mDef += exp_mdef;
        result.spd += exp_spd;
        result.eva += exp_eva;
        result.acc += exp_acc;

        result.hp += Mathf.CeilToInt(result.exp_hpRate * result.hp);
        result.pAtk += Mathf.CeilToInt(result.exp_pAtkRate * result.pAtk);
        result.pDef += Mathf.CeilToInt(result.exp_pDefRate * result.pDef);
        result.mAtk += Mathf.CeilToInt(result.exp_mAtkRate * result.mAtk);
        result.mDef += Mathf.CeilToInt(result.exp_mDefRate * result.mDef);
        result.spd += Mathf.CeilToInt(result.exp_spdRate * result.spd);
        result.eva += Mathf.CeilToInt(result.exp_evaRate * result.eva);
        result.acc += Mathf.CeilToInt(result.exp_accRate * result.acc);

        return result;
    }

    //public Attributes CreateOverrideMaxLevelAttributes(int defaultMaxLevel, int newMaxLevel)
    //{
    //    Attributes attributes = new Attributes();
    //    var hp = this.hp.Clone();
    //    hp.maxValue = this.hp.Calculate(newMaxLevel, defaultMaxLevel);
    //    attributes.hp = hp;

    //    var pAtk = this.pAtk.Clone();
    //    pAtk.maxValue = this.pAtk.Calculate(newMaxLevel, defaultMaxLevel);
    //    attributes.pAtk = pAtk;

    //    var pDef = this.pDef.Clone();
    //    pDef.maxValue = this.pDef.Calculate(newMaxLevel, defaultMaxLevel);
    //    attributes.pDef = pDef;

    //    var mAtk = this.mAtk.Clone();
    //    mAtk.maxValue = this.mAtk.Calculate(newMaxLevel, defaultMaxLevel);
    //    attributes.mAtk = mAtk;

    //    var mDef = this.mDef.Clone();
    //    mDef.maxValue = this.mDef.Calculate(newMaxLevel, defaultMaxLevel);
    //    attributes.mDef = mDef;

    //    var spd = this.spd.Clone();
    //    spd.maxValue = this.spd.Calculate(newMaxLevel, defaultMaxLevel);
    //    attributes.spd = spd;

    //    var eva = this.eva.Clone();
    //    eva.maxValue = this.eva.Calculate(newMaxLevel, defaultMaxLevel);
    //    attributes.eva = eva;

    //    var acc = this.acc.Clone();
    //    acc.maxValue = this.acc.Calculate(newMaxLevel, defaultMaxLevel);
    //    attributes.acc = acc;

    //    return attributes;
    //}

    public static Attributes operator *(Attributes a, float b)
    {
        Attributes result = new Attributes();
        result.hp = a.hp * b;
        result.pAtk = a.pAtk * b;
        result.pDef = a.pDef * b;
        result.mAtk = a.mAtk * b;
        result.mDef = a.mDef * b;
        result.spd = a.spd * b;
        result.eva = a.eva * b;
        result.acc = a.acc * b;
        return result;
    }

}

[Serializable]
public class CalculationAttributes
{
    public float hp;
    public float pAtk;
    public float pDef;
    public float mAtk;
    public float mDef;
    public float spd;
    /// <summary>
    /// 回避
    /// </summary>
    public float eva;
    /// <summary>
    /// 命中
    /// </summary>
    public float acc;

    public float exp_hpRate;
    public float exp_pAtkRate;
    public float exp_pDefRate;
    public float exp_mAtkRate;
    public float exp_mDefRate;
    public float exp_spdRate;
    public float exp_evaRate;
    public float exp_accRate;

    /// <summary>
    /// 暴击 0-1
    /// </summary>
    public float exp_critChance;
    [Range(1f, 100f)]
    public float exp_critDamageRate;
    //格挡    
    public float exp_blockChance;
    [Range(1f, 100f)]
    public float exp_blockDamageRate;


    public CalculationAttributes Clone()
    {
        CalculationAttributes result = new CalculationAttributes();
        result.hp = hp;
        result.pAtk = pAtk;
        result.pDef = pDef;
        result.mAtk = mAtk;
        result.mDef = mDef;
        result.spd = spd;
        result.eva = eva;
        result.acc = acc;

        result.exp_hpRate = exp_hpRate;
        result.exp_pAtkRate = exp_pAtkRate;
        result.exp_pDefRate = exp_pDefRate;
        result.exp_mAtkRate = exp_mAtkRate;
        result.exp_mDefRate = exp_mDefRate;
        result.exp_spdRate = exp_spdRate;
        result.exp_evaRate = exp_evaRate;
        result.exp_accRate = exp_accRate;

        result.exp_critChance = exp_critChance;
        result.exp_critDamageRate = exp_critDamageRate;
        result.exp_blockChance = exp_blockChance;
        result.exp_blockDamageRate = exp_blockDamageRate;
        return result;
    }


    #region Calculating between CalculationAttributes and CalculationAttributes
    public static CalculationAttributes operator +(CalculationAttributes a, CalculationAttributes b)
    {
        CalculationAttributes result = a.Clone();
        result.hp += b.hp;
        result.pAtk += b.pAtk;
        result.pDef += b.pDef;
        result.mAtk += b.mAtk;
        result.mDef += b.mDef;
        result.spd += b.spd;
        result.eva += b.eva;
        result.acc += b.acc;

        result.exp_hpRate += b.exp_hpRate;
        result.exp_pAtkRate += b.exp_pAtkRate;
        result.exp_pDefRate += b.exp_pDefRate;
        result.exp_mAtkRate += b.exp_mAtkRate;
        result.exp_mDefRate += b.exp_mDefRate;
        result.exp_spdRate += b.exp_spdRate;
        result.exp_evaRate += b.exp_evaRate;
        result.exp_accRate += b.exp_accRate;

        result.exp_critChance += b.exp_critChance;
        result.exp_critDamageRate += b.exp_critDamageRate;
        result.exp_blockChance += b.exp_blockChance;
        result.exp_blockDamageRate += b.exp_blockDamageRate;
        return result;
    }

    public static CalculationAttributes operator -(CalculationAttributes a, CalculationAttributes b)
    {
        CalculationAttributes result = a.Clone();
        result.hp -= b.hp;
        result.pAtk -= b.pAtk;
        result.pDef -= b.pDef;
        result.mAtk -= b.mAtk;
        result.mDef -= b.mDef;
        result.spd -= b.spd;
        result.eva -= b.eva;
        result.acc -= b.acc;

        result.exp_hpRate -= b.exp_hpRate;
        result.exp_pAtkRate -= b.exp_pAtkRate;
        result.exp_pDefRate -= b.exp_pDefRate;
        result.exp_mAtkRate -= b.exp_mAtkRate;
        result.exp_mDefRate -= b.exp_mDefRate;
        result.exp_spdRate -= b.exp_spdRate;
        result.exp_evaRate -= b.exp_evaRate;
        result.exp_accRate -= b.exp_accRate;

        result.exp_critChance -= b.exp_critChance;
        result.exp_critDamageRate -= b.exp_critDamageRate;

        result.exp_blockChance -= b.exp_blockChance;
        result.exp_blockDamageRate -= b.exp_blockDamageRate;
        return result;
    }

    public static CalculationAttributes operator *(CalculationAttributes a, float b)
    {
        CalculationAttributes result = new CalculationAttributes();
        result.hp = Mathf.CeilToInt(a.hp * b);
        result.pAtk = Mathf.CeilToInt(a.pAtk * b);
        result.pDef = Mathf.CeilToInt(a.pDef * b);
        result.mAtk = Mathf.CeilToInt(a.mAtk * b);
        result.mDef = Mathf.CeilToInt(a.mDef * b);
        result.spd = Mathf.CeilToInt(a.spd * b);
        result.eva = Mathf.CeilToInt(a.eva * b);
        result.acc = Mathf.CeilToInt(a.acc * b);

        result.exp_hpRate = a.exp_hpRate * b;
        result.exp_pAtkRate = a.exp_pAtkRate * b;
        result.exp_pDefRate = a.exp_pDefRate * b;
        result.exp_mAtkRate = a.exp_mAtkRate * b;
        result.exp_mDefRate = a.exp_mDefRate * b;
        result.exp_spdRate = a.exp_spdRate * b;
        result.exp_evaRate = a.exp_evaRate * b;
        result.exp_accRate = a.exp_accRate * b;
        result.exp_critChance = a.exp_critChance * b;
        result.exp_critDamageRate = a.exp_critDamageRate * b;
        result.exp_blockChance = a.exp_blockChance * b;
        result.exp_blockDamageRate = a.exp_blockDamageRate * b;
        return result;
    }
    #endregion



    public void SetExtraAtt(int totalWeight)
    {
        float hpf = Random.Range(0.2f, 0.5f);
        hp = totalWeight * hpf;
        float pAtkf = Random.Range(0, 1 - hpf);
        pAtk = pAtkf * totalWeight * 0.3f;
        float pDeff = Random.Range(0, 1 - hpf - pAtkf);
        pDef = pDeff * totalWeight * 0.3f;
        float mAtkf = Random.Range(0, 1 - hpf - pAtkf - pDeff);
        mAtk = mAtkf * totalWeight * 0.3f;
        float mDeff = Random.Range(0, 1 - hpf - pAtkf - pDeff - mAtkf);
        mDef = mDeff * totalWeight * 0.3f;
        float spdf = Random.Range(0, 1 - hpf - pAtkf - pDeff - mAtkf - mDeff);
        spd = spdf * totalWeight * 0.3f;
        float evaf = Random.Range(0, 1 - hpf - pAtkf - pDeff - mAtkf - mDeff - spdf);
        eva = evaf * totalWeight * 0.3f;
        float accf = Random.Range(0, 1 - hpf - pAtkf - pDeff - mAtkf - mDeff - spdf - evaf);
        acc = accf * totalWeight * 0.3f;

        //hpRate = hpRate;
        //pAtkRate = pAtkRate;
        //pDefRate = pDefRate;
        //mAtkRate = mAtkRate;
        //mDefRate = mDefRate;
        //spdRate = spdRate;
        //evaRate = evaRate;
        //accRate = accRate;

        //critChance = critChance;
        //critDamageRate = critDamageRate;

        //blockChance = blockChance;
        //blockDamageRate = blockDamageRate;
    }
}
