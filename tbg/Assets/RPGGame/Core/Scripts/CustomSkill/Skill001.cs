using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill001 : CustomSkill
{

    public override void DoSkill()
    {

    }
    public override void BeforeFight()
    {

    }

    public override void Fight()
    {
        Debug.Log("fight" + id);

    }

    public override void Afterfight()
    {

    }

    public override void ReceiveDamage()
    {
    }

    public override void Beibaoji()
    {
    }

    public override void Beigedang()
    {
    }

    public override void Beimiss()
    {
    }

    public override void Gobaoji()
    {
    }

    public override void Gogedang()
    {
    }

    public override void Gomiss()
    {
    }

    public override IEnumerator ApplyBuffLogic()
    {
        CustomBuff buff001 = SkillUtils.MakeCustomBuff("001");
        GetSelf().ApplyCustomBuff(buff001);
        Debug.Log("执行了 添加自定义buff 001");
        yield return null;
    }
}
