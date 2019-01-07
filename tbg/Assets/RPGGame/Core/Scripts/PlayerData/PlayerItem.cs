using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite3TableDataTmp;
using UnityEngine;

[System.Serializable]
public class PlayerItemTemp : BasePlayerData, ILevel//, IPlayerItem
{
    //public static readonly Dictionary<string, PlayerItem> DataMap = new Dictionary<string, PlayerItem>();
    //在角色拥有表中唯一
    private string itemid;
    public string ItemID { get { return itemid; } set { itemid = value; } }
    private string playerId;
    public string PlayerId { get { return playerId; } set { playerId = value; } }
    //在总角色表中唯一
    private string Guid;
    public string GUID
    {
        get { return Guid; }
        set
        {
            if (Guid != value)
            {
                dirtyExp = -1;
                evolveMaterials = null;
                Guid = value;
            }
        }
    }
    public int amount;
    public int Amount { get { return amount; } set { amount = value; } }
    public int exp;
    public int Exp { get { return exp; } set { exp = value; } }
    private string equipItemguid;
    /// <summary>
    /// 装备了这件装备的item
    /// </summary>
    public string EquipItemGuid { get { return equipItemguid; } set { equipItemguid = value; } }
    public string equipPosition;
    public string EquipPosition { get { return equipPosition; } set { equipPosition = value; } }

    private int level = -1;
    private int collectExp = -1;
    private int dirtyExp = -1;  // Exp for dirty check to calculate `Level` and `CollectExp` fields
    private Dictionary<string, int> evolveMaterials = null;
    public ItemType Itemtype = ItemType.nothing;

    public ItemType itemType
    {
        get { return Itemtype; }

        set { Itemtype = value; }
    }

    public PlayerItemTemp(ItemType itemType)
    {
        itemid = "";
        PlayerId = "";
        GUID = "";
        Amount = 1;
        Exp = 0;
        EquipItemGuid = "";
        EquipPosition = "";
        this.itemType = itemType;
    }

    public PlayerItemTemp Clone()
    {
        var result = new PlayerItemTemp(itemType);
        //CloneTo(this, result);
        return result;
    }

    public static void CloneTo(IPlayerItem from, IPlayerItem to)
    {
        to.ItemID = from.ItemID;
        to.PlayerId = from.PlayerId;
        to.GUID = from.GUID;
        to.Amount = from.Amount;
        to.Exp = from.Exp;
        to.EquipItemGuid = from.EquipItemGuid;
        to.EquipPosition = from.EquipPosition;
        //to.itemType = from.itemType;

    }

    public ICharacter GetICharacter()
    {
        return ICharacter.DataMap[Guid];
    }

    public PlayerItemTemp CreateLevelUpItem(int increaseExp)
    {
        PlayerItemTemp result = Clone();
        result.Exp += increaseExp;
        return result;
    }

    #region Non Serialize Fields


    public ICharacter CharacterData
    {
        get { return ICharacter.DataMap[Guid]; }
    }

    public IEquipment EquipmentData
    {
        get { return IEquipment.DataMap[Guid]; }
    }

    /// <summary>
    /// tower huodong reward data
    /// </summary>
    public CalculationAttributes ExtrAttributesData { get; set; }



    //public Sprite Icon
    //{
    //    get { return ItemData == null ? null : ItemData.icon; }
    //}

    public int Level
    {
        get
        {
            CalculateLevelAndRemainExp();
            return level;
        }
    }

    public int CollectExp
    {
        get
        {
            CalculateLevelAndRemainExp();
            return collectExp;
        }
    }

    public int MaxLevel
    {
        //get { return ActorItemData == null ? 1 : Tier.maxLevel; }
        get { return Const.MaxLevel; }
    }


    public int EvolvePrice
    {
        get { return Const.EnovePrice; }
    }

    public int NextExp
    {
        //get { return Tier == null ? 0 : Tier.expTable.Calculate(Level, Tier.maxLevel); }
        get { return Const.NextEXP; }
    }

    public int SellPrice
    {
        get
        {
            //return Tier == null ? 0 : Tier.sellPriceTable.Calculate(Level, Tier.maxLevel);
            return Const.SellPrice;
        }
    }

    public int LevelUpPrice
    {
        get
        {
            //return Tier == null ? 0 : Tier.levelUpPriceTable.Calculate(Level, Tier.maxLevel);
            return Const.LevelUpPrice;
        }
    }

    public int RewardExp
    {
        get
        {
            //return Tier == null ? 0 : Tier.rewardExpTable.Calculate(Level, Tier.maxLevel);
            return Const.NextEXP;
        }
    }

    public bool IsReachMaxLevel
    {
        get { return Level == MaxLevel; }
    }

    public CalculationAttributes GetItemAttributes()
    {
        if (CharacterData != null)
        {
            var result = new CalculationAttributes();
            //result += CharacterData.CreateCalculationAttributes(Level, MaxLevel);
            if (GameDatabase != null)
                result += GameDatabase.characterBaseAttributes;
            if (ExtrAttributesData != null)
                result += ExtrAttributesData;
            result += EquipmentBonus;
            return result;
        }
        if (EquipmentData != null)
        {
            var result = new CalculationAttributes();
            //result += EquipmentData.CreateCalculationAttributes(Level, MaxLevel);
            //result += EquipmentData.CreateExtraCalculationAttributes();
            return result;
        }
        Debug.LogError("属性 null 不属于 char or equip");
        return null;
    }
    public CalculationAttributes EquipmentBonus
    {
        get
        {
            var equippedItems = EquippedItems.Values;
            var result = new CalculationAttributes();
            foreach (var equippedItem in equippedItems)
                result += equippedItem.Attributes;
            return result;
        }
    }

    public CalculationAttributes Attributes
    {
        get
        {
            // If item is character or equipment
            if (CharacterData != null)
            {
                var result = new CalculationAttributes();
                //result += CharacterData.CreateCalculationAttributes(Level, MaxLevel);
                if (GameDatabase != null)
                    result += GameDatabase.characterBaseAttributes;
                if (ExtrAttributesData != null)
                    result += ExtrAttributesData;
                return result;
            }
            if (EquipmentData != null)
            {
                var result = new CalculationAttributes();
                //result += EquipmentData.CreateCalculationAttributes(Level, MaxLevel);
                //result += EquipmentData.CreateExtraCalculationAttributes();
                return result;
            }
            return null;
        }
    }

    public List<PlayerFormation> InTeamFormations
    {
        get
        {
            if (CharacterData == null)
                return new List<PlayerFormation>();
            var valueList = PlayerFormation.DataMap.Values;
            var list = valueList.Where(entry =>
                entry.PlayerId == PlayerId &&
                entry.ItemId == ItemID).ToList();
            return list;
        }
    }

    public PlayerItemTemp EquippedByItem
    {
        get
        {
            IPlayerHasEquips equippedByItem;
            //if (EquipmentData != null && !string.IsNullOrEmpty(EquipItemGuid) && IPlayerHasEquips.DataMap.TryGetValue(EquipItemGuid, out equippedByItem))
            //    return equippedByItem.GetPlayerItem();
            return null;
        }
    }

    public Dictionary<string, PlayerItemTemp> EquippedItems
    {
        get
        {
            //var result = new Dictionary<string, PlayerItemTemp>();

            //if (CharacterData == null)
            //    return result;

            //var valueList = IPlayerHasEquips.DataMap.Values;
            //var list = valueList.Where(entry =>
            //    entry.playerId == PlayerId &&
            //    entry.Guid == GUID &&
            //    !string.IsNullOrEmpty(entry.equipPosition) &&
            //    entry.amount > 0).ToList();

            //foreach (var entry in list)
            //{
            //    result.Add(entry.equipPosition, entry.GetPlayerItem());
            //}

            return null;
        }
    }

    public bool CanLevelUp
    {
        get { return !IsReachMaxLevel && (CharacterData != null || EquipmentData != null); }
    }


    public bool CanSell
    {
        get { return !PlayerFormation.ContainsDataWithItemId(GUID) && EquippedByItem == null; }
    }

    public bool CanBeMaterial
    {
        get { return !PlayerFormation.ContainsDataWithItemId(GUID) && EquippedByItem == null; }
    }

    public bool CanBeEquipped
    {
        get { return EquipmentData != null; }
    }



    public bool IsInTeamFormation(string formationName)
    {
        var formations = InTeamFormations;
        foreach (var formation in formations)
        {
            if (formation.formationId == formationName)
                return true;
        }
        return false;
    }
    #endregion

    private void CalculateLevelAndRemainExp()
    {
        //if (Tier == null)
        //{
        //    level = 1;
        //    collectExp = 0;
        //    return;
        //}
        if (dirtyExp == -1 || dirtyExp != Exp)
        {
            dirtyExp = Exp;
            var remainExp = Exp;
            for (level = 1; level < MaxLevel; ++level)
            {
                //var nextExp = Tier.expTable.Calculate(level, Tier.maxLevel);
                var nextExp = Const.NextEXP;
                if (remainExp - nextExp < 0)
                    break;
                remainExp -= nextExp;
            }
            collectExp = remainExp;
        }
    }




    public static PlayerItemTemp CreateActorItemWithLevel(ICharacter itemData, int level, Const.StageType type, bool isplayer)
    {
        if (level <= 0)
            level = 1;
        //var itemTier = itemData.itemTier;
        var sumExp = 0;
        var result = new PlayerItemTemp(ItemType.character);
        result.itemType = ItemType.character;
        for (var i = 1; i < level; ++i)
        {
            //sumExp += itemTier.expTable.Calculate(i + 1, itemTier.maxLevel);
            sumExp += Const.NextEXP;
        }
        result.ItemID = itemData.guid;
        //if(!isplayer)
        //result.GUID = itemData.;
        result.Exp = sumExp;
        result.ExtrAttributesData = GetExtraAttributes(type);
        return result;
    }

    static CalculationAttributes GetExtraAttributes(Const.StageType type)
    {
        switch (type)
        {
            case Const.StageType.Normal:
                return null;
                break;
            case Const.StageType.Tower:
                return FormulaUtils.GetTowerExtraAttributes(false);
                break;
        }
        return null;
    }

    public ItemType GetItemType()
    {
        if (itemType == ItemType.nothing)
        {
            throw new Exception("没有给type赋值");
        }
        return itemType;
    }

    public enum ItemType
    {
        character,
        equip,
        other,
        nothing
    }
}