using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBody
{

    private CharacterEntity self;
    public CustomBody(CharacterEntity self)
    {
        this.self = self;
    }

    public void Attack(CharacterEntity target, float pAtkRate = 1f, float mAtkRate = 1f, int hitCount = 1, int fixDamage = 0)
    {
        if (target == null)
            return;
        self.ApplySkillAndBuff(TriggerType.fight);
        var attributes = self.GetTotalAttributes();
        AttackInfo info = target.ReceiveDamage(
             Mathf.CeilToInt(attributes.pAtk * pAtkRate),
             Mathf.CeilToInt(attributes.mAtk * mAtkRate),
             (int)attributes.acc,
             attributes.critChance,
             attributes.critDamageRate,
             hitCount,
             fixDamage);

        if (info.gedang) self.ApplySkillAndBuff(TriggerType.beigedang);
        if (info.baoji) self.ApplySkillAndBuff(TriggerType.beibaoji);
        if (info.shanbi) self.ApplySkillAndBuff(TriggerType.beimiss);
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
            self.ApplySkillAndBuff(TriggerType.gobaoji);
        }
        // Block occurs
        if (Random.value <= attributes.blockChance)
        {
            totalDmg = Mathf.CeilToInt(totalDmg / attributes.blockDamageRate);
            isBlock = true;
            attackInfo.gedang = true;
            self.ApplySkillAndBuff(TriggerType.gogedang);
        }

        var hitChance = 0f;
        if (acc > 0)
            hitChance = acc / attributes.eva;

        // Cannot evade, receive damage
        if (hitChance < 0 || Random.value > hitChance)
        {
            self.Manager.SpawnMissText(self);
            attackInfo.shanbi = true;
            self.ApplySkillAndBuff(TriggerType.gomiss);
        }
        else
        {
            if (isBlock)
                self.Manager.SpawnBlockText((int)totalDmg, self);
            else if (isCritical)
                self.Manager.SpawnCriticalText((int)totalDmg, self);
            else
                self.Manager.SpawnDamageText((int)totalDmg, self);

            self.Hp -= (int)totalDmg;
        }
        attackInfo.totalDamage = (int)totalDmg;
        self.ApplySkillAndBuff(TriggerType.receiveDamage);
        // Play hurt animation
        //CacheAnimator.ResetTrigger(ANIM_KEY_HURT);
        //CacheAnimator.SetTrigger(ANIM_KEY_HURT);
        return attackInfo;
    }
}
