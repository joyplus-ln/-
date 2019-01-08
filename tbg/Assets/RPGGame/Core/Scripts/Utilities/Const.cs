using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Const
{

    public const int MaxLevel = 40;
    public const int EnovePrice = 4000;
    public const int NextEXP = 4000;
    public const int RewardExp = 4000;
    public const int SellPrice = 4000;
    public const int LevelUpPrice = 4000;
    //LevelUpPrice

    public const string NormalAttack = "NormalAttack";

    public enum StageType
    {
        Normal,
        Tower
    }

    public enum SkillType
    {
        Attack,
        Custom
    }

    public enum EquipPosition
    {
        weapon,
        cloth,
        shoot
    }
}
