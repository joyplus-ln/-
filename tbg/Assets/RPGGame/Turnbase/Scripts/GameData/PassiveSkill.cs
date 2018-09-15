using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : Skill
{
    public int useTimes;
    public int applyTurns;


    public TriggerType type;

    private CharacterEntity self;

    public void ApplyPassiveSkill()
    {
        self.ApplyPassiveBuff(self, 1, this, 0);
    }

    public void Init(CharacterEntity self)
    {
        this.self = self;
    }

    public override List<BaseSkillBuff> GetBuffs()
    {
        return new List<BaseSkillBuff>(buffs);
    }


}



public enum TriggerType
{
    beforeFight,
    fight,
    afterfight,
    receiveDamage,
    beibaoji,
    beigedang,
    beimiss,
    gobaoji,
    gogedang,
    gomiss,

}