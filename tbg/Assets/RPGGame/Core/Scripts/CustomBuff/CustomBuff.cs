using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBuff
{
    public string des;
    public string guid;
    public string buffText;

    public CalculationAttributes SelfAttributes = new CalculationAttributes();

    public virtual void Init()
    {
        
    }

    public virtual void BeforeFight()
    {
        
    }

    public virtual void Fight()
    {
    }

    public virtual void Afterfight()
    {
    }

    public virtual void ReceiveDamage()
    {
    }

    public virtual void Beibaoji()
    {
    }

    public virtual void Beigedang()
    {
    }

    public virtual void Beimiss()
    {
    }

    public virtual void Gobaoji()
    {
    }

    public virtual void Gogedang()
    {
    }

    public virtual void Gomiss()
    {
    }

    public virtual void BuffRemove()
    {

    }

    public void Trigger(TriggerType type)
    {
        switch (type)
        {
            case TriggerType.beforeFight:
                BeforeFight();
                break;
            case TriggerType.fight:
                Fight();
                break;
            case TriggerType.afterfight:
                Afterfight();
                break;
            case TriggerType.receiveDamage:
                ReceiveDamage();
                break;
            case TriggerType.beibaoji:
                Beibaoji();
                break;
            case TriggerType.beigedang:
                Beigedang();
                break;
            case TriggerType.beimiss:
                Beimiss();
                break;
            case TriggerType.gobaoji:
                Gobaoji();
                break;
            case TriggerType.gogedang:
                Gogedang();
                break;
            case TriggerType.gomiss:
                Gomiss();
                break;
            default:
                break;
        }
    }
}
