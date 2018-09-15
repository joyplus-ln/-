public class Buff001 : CustomBuff
{
    public override void Init()
    {
        base.Init();
        des = "自定义技能 ID 001";
        buffText = "攻";
    }

    public override void BeforeFight()
    {

    }

    public override void Fight()
    {
        SelfAttributes.hp += 1000;
        SelfAttributes.mAtk += 200;
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

}
