using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CustomBody
{

    private CharacterEntity self;
    public CustomBody(CharacterEntity self)
    {
        this.self = self;
    }

    public AttackInfo Attack(CharacterEntity target, float pAtkRate = 1f, float mAtkRate = 1f, int hitCount = 1, int fixDamage = 0)
    {
        if (target == null)
            return null;
        self.ApplySkillAndBuff(CustomSkill.TriggerType.fight);
        var attributes = self.GetTotalAttributes();
        AttackInfo info = target.ReceiveDamage(
             Mathf.CeilToInt(attributes.pAtk * pAtkRate),
             Mathf.CeilToInt(attributes.mAtk * mAtkRate),
             (int)attributes.acc,
             attributes.critChance,
             attributes.critDamageRate,
             hitCount,
             fixDamage);

        if (info.gedang) self.ApplySkillAndBuff(CustomSkill.TriggerType.beigedang);
        if (info.baoji) self.ApplySkillAndBuff(CustomSkill.TriggerType.beibaoji);
        if (info.shanbi) self.ApplySkillAndBuff(CustomSkill.TriggerType.beimiss);
        return info;
    }

    public AttackInfo ReceiveDamage(int pAtk, int mAtk, int acc, float critChance, float critDamageRate, int hitCount = 1, int fixDamage = 0)
    {
        AttackInfo attackInfo = new AttackInfo();
        if (hitCount <= 0)
            hitCount = 1;
        var attributes = self.GetTotalAttributes();
        var isCritical = false;
        var isBlock = false;
        var totalDmg = FormulaUtils.FightFormula(pAtk, mAtk, acc, critChance, critDamageRate, attributes, hitCount, fixDamage);
        // Critical occurs
        if (Random.value <= critChance)
        {
            totalDmg = Mathf.CeilToInt(totalDmg * critDamageRate);
            isCritical = true;
            attackInfo.baoji = true;
            self.ApplySkillAndBuff(CustomSkill.TriggerType.gobaoji);
        }
        // Block occurs
        if (Random.value <= attributes.blockChance)
        {
            totalDmg = Mathf.CeilToInt(totalDmg / attributes.blockDamageRate);
            isBlock = true;
            attackInfo.gedang = true;
            self.ApplySkillAndBuff(CustomSkill.TriggerType.gogedang);
        }

        var hitChance = 0f;
        if (acc > 0)
            hitChance = acc / attributes.eva;

        // Cannot evade, receive damage
        if (hitChance < 0 || Random.value > hitChance)
        {

            attackInfo.shanbi = true;
            DeductBlood((int)totalDmg, DmgType.Miss);
            self.ApplySkillAndBuff(CustomSkill.TriggerType.gomiss);
        }
        else
        {
            if (isBlock)
                DeductBlood((int)totalDmg, DmgType.Block);
            else if (isCritical)
                DeductBlood((int)totalDmg, DmgType.Critical);
            else
                DeductBlood((int)totalDmg, DmgType.Normal);
        }
        attackInfo.totalDamage = (int)totalDmg;
        self.ApplySkillAndBuff(CustomSkill.TriggerType.receiveDamage);
        // Play hurt animation
        //CacheAnimator.ResetTrigger(ANIM_KEY_HURT);
        //CacheAnimator.SetTrigger(ANIM_KEY_HURT);
        AnimReceiveDamage();
        return attackInfo;
    }

    //播放动画 receiveDamage
    void AnimReceiveDamage()
    {
        self.transform.DOShakePosition(1, new Vector3(15, 15, 15));
    }

    public void DeductBlood(int totalDmg, DmgType type)
    {
        switch (type)
        {
            case DmgType.Normal:
                self.Manager.SpawnDamageText(totalDmg, self);
                break;
            case DmgType.Block:
                self.Manager.SpawnBlockText(totalDmg, self);
                break;
            case DmgType.Critical:
                self.Manager.SpawnCriticalText(totalDmg, self);
                break;
            case DmgType.Miss:
                self.Manager.SpawnMissText(self);
                return;
                break;
            case DmgType.Heal:
                self.Manager.SpawnAddHealText("+", totalDmg, self);
                break;
        }
        self.Hp -= totalDmg;
    }

    #region 战斗相关，自身状态变化，不是来自buff内叠加的那种

    /// <summary>
    /// 增加气血
    /// </summary>
    /// <param name="flood"></param>
    public void AddFlood(int flood)
    {
        DeductBlood(-flood, DmgType.Heal);
    }

    public int GetMaxHP()
    {
        return self.MaxHp;
    }
    #endregion

    /// <summary>
    /// 自定义字体
    /// </summary>
    public void CustomText(string customText)
    {
        self.Manager.SpawnCustomText(customText, 0, self);
    }

    /// <summary>
    /// 是否可以使用技能
    /// </summary>
    /// <returns></returns>
    public bool CanUseKill()
    {
        bool canuse = true;
        foreach (var buff in self.Buffs_custom.Values)
        {
            if (!buff.CanDoAction())
                canuse = false;
        }
        return canuse;

    }
}

public enum DmgType
{
    Normal,
    Block,
    Critical,
    Miss,
    Heal


}
