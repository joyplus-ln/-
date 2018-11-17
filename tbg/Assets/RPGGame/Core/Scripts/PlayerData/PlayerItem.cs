using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerItem : BasePlayerData, ILevel, IPlayerItem
{
    //public static readonly Dictionary<string, PlayerItem> DataMap = new Dictionary<string, PlayerItem>();
    //玩家拥有的
    public static readonly Dictionary<string, PlayerItem> characterDataMap = new Dictionary<string, PlayerItem>();
    public static readonly Dictionary<string, PlayerItem> equipDataMap = new Dictionary<string, PlayerItem>();
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

    public PlayerItem(ItemType itemType)
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

    public PlayerItem Clone()
    {
        var result = new PlayerItem(itemType);
        CloneTo(this, result);
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
        to.itemType = from.itemType;

    }

    public PlayerItem CreateLevelUpItem(int increaseExp)
    {
        PlayerItem result = Clone();
        result.Exp += increaseExp;
        return result;
    }

    #region Non Serialize Fields
    public BaseItem ItemData
    {
        get
        {
            switch (itemType)
            {
                case ItemType.character:
                    if (GameDatabase != null && GameDatabase.characters.ContainsKey(ItemID))
                        return GameDatabase.characters[ItemID];
                    break;
                case ItemType.equip:
                    if (GameDatabase != null && GameDatabase.equipments.ContainsKey(ItemID))
                        return GameDatabase.equipments[ItemID];
                    break;
            }
            Debug.LogError("不存在这个id 肯定是哪里出错了:" + GUID + ":" + itemType + ":" + ItemID);
            return null;
        }
    }

    public BaseActorItem ActorItemData
    {
        get { return ItemData == null ? null : ItemData as BaseActorItem; }
    }

    public CharacterItem CharacterData
    {
        get { return ActorItemData == null ? null : ActorItemData as CharacterItem; }
    }

    public EquipmentItem EquipmentData
    {
        get { return ActorItemData == null ? null : ActorItemData as EquipmentItem; }
    }

    /// <summary>
    /// tower huodong reward data
    /// </summary>
    public CalculationAttributes ExtrAttributesData { get; set; }

    public OtherItem OtherItemData
    {
        get { return ActorItemData == null ? null : ActorItemData as OtherItem; }
    }


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
            result += CharacterData.attributes.CreateCalculationAttributes(Level, MaxLevel);
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
            result += EquipmentData.attributes.CreateCalculationAttributes(Level, MaxLevel);
            result += EquipmentData.extraAttributes;
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
                result += CharacterData.attributes.CreateCalculationAttributes(Level, MaxLevel);
                if (GameDatabase != null)
                    result += GameDatabase.characterBaseAttributes;
                if (ExtrAttributesData != null)
                    result += ExtrAttributesData;
                return result;
            }
            if (EquipmentData != null)
            {
                var result = new CalculationAttributes();
                result += EquipmentData.attributes.CreateCalculationAttributes(Level, MaxLevel);
                result += EquipmentData.extraAttributes;
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

    public PlayerItem EquippedByItem
    {
        get
        {
            PlayerItem equippedByItem;
            if (EquipmentData != null && !string.IsNullOrEmpty(EquipItemGuid) && equipDataMap.TryGetValue(EquipItemGuid, out equippedByItem))
                return equippedByItem;
            return null;
        }
    }

    public Dictionary<string, PlayerItem> EquippedItems
    {
        get
        {
            var result = new Dictionary<string, PlayerItem>();

            if (CharacterData == null)
                return result;

            var valueList = equipDataMap.Values;
            var list = valueList.Where(entry =>
                entry.PlayerId == PlayerId &&
                entry.EquipmentData != null &&
                entry.EquipItemGuid == GUID &&
                !string.IsNullOrEmpty(entry.EquipPosition) &&
                entry.Amount > 0).ToList();

            foreach (var entry in list)
            {
                result.Add(entry.EquipPosition, entry);
            }

            return result;
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

    public static void SetData(PlayerItem data, ItemType type)
    {
        if (data == null || string.IsNullOrEmpty(data.GUID))
            return;
        switch (type)
        {
            case ItemType.character:
                data.itemType = ItemType.character;
                characterDataMap[data.GUID] = data;
                break;
            case ItemType.equip:
                data.itemType = ItemType.equip;
                equipDataMap[data.GUID] = data;
                break;
            case ItemType.other:
                break;
        }
    }

    public static bool RemoveData(string id, ItemType type)
    {
        switch (type)
        {
            case ItemType.character:
                return characterDataMap.Remove(id);
                break;
            case ItemType.equip:
                return equipDataMap.Remove(id);
                break;
            case ItemType.other:
                break;
        }
        return false;
    }

    public static void ClearData()
    {
        characterDataMap.Clear();
        equipDataMap.Clear();
    }

    public static void SetDataRange(IEnumerable<PlayerItem> list)
    {
        foreach (var data in list)
        {
            SetData(data, data.GetItemType());
        }
    }

    public static void RemoveDataRange(Dictionary<string, ItemType> ids)
    {
        foreach (var id in ids.Keys)
        {
            RemoveData(id, ids[id]);
        }
    }

    public static void RemoveDataRange(string playerId)
    {
        var values = characterDataMap.Values;
        foreach (var value in values)
        {
            if (value.PlayerId == playerId)
                RemoveData(value.ItemID, ItemType.character);
        }

        var evalues = equipDataMap.Values;
        foreach (var value in evalues)
        {
            if (value.PlayerId == playerId)
                RemoveData(value.ItemID, ItemType.equip);
        }
    }

    public static void RemoveDataRange()
    {
        RemoveDataRange(Player.CurrentPlayerId);
    }

    public static PlayerItem CreateActorItemWithLevel(BaseActorItem itemData, int level, Const.StageType type, bool isplayer)
    {
        if (level <= 0)
            level = 1;
        //var itemTier = itemData.itemTier;
        var sumExp = 0;
        var result = new PlayerItem(ItemType.character);
        result.itemType = ItemType.character;
        for (var i = 1; i < level; ++i)
        {
            //sumExp += itemTier.expTable.Calculate(i + 1, itemTier.maxLevel);
            sumExp += Const.NextEXP;
        }
        result.ItemID = itemData.itemid;
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