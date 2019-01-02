using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOtherItem : BasePlayerData, IPlayerOtherItem
{
    public static readonly Dictionary<string, PlayerOtherItem> DataMap = new Dictionary<string, PlayerOtherItem>();
    public string itemid;
    public string ItemID { get { return itemid; } set { itemid = value; } }
    public string playerId;
    public string PlayerId { get { return playerId; } set { playerId = value; } }
    public string dataId;
    public string DataId
    {
        get { return dataId; }
        set
        {
            if (dataId != value)
            {
                dataId = value;
            }
        }
    }

    public string des;
    public string Des
    {
        get { return des; }
        set
        {
            if (des != value)
            {
                des = value;
            }
        }
    }

    public int amount;
    public int Amount { get { return amount; } set { amount = value; } }
    public int exp;
    public int Exp { get { return exp; } set { exp = value; } }


    private int level = -1;
    public PlayerOtherItem()
    {
        itemid = "";
        PlayerId = "";
        DataId = "";
        Amount = 1;
        Exp = 0;
    }



    public static bool RemoveData(string id)
    {
        return DataMap.Remove(id);
    }

    public static void ClearData()
    {
        DataMap.Clear();
    }

    public static void SetData(List<PlayerOtherItem> list)
    {
        foreach (PlayerOtherItem item in list)
        {
            if (item == null || string.IsNullOrEmpty(item.ItemID))
                return;
            DataMap[item.ItemID] = item;
        }
    }

    public static void RemoveDataRange(IEnumerable<string> ids)
    {
        foreach (var id in ids)
        {
            RemoveData(id);
        }
    }

    public static void RemoveDataRange(string playerId)
    {
        var values = DataMap.Values;
        foreach (var value in values)
        {
            if (value.PlayerId == playerId)
                RemoveData(value.ItemID);
        }
    }

    public static void RemoveDataRange()
    {
        RemoveDataRange(Player.CurrentPlayerId);
    }

    public static void AddOneItem(string id, int num)
    {
        if (GameDatabase.otherItem.Find(x => x.Id == id) == null)
        {
            Debug.LogWarning("gameDataBase dont contain this id:" + id);
            return;
        }
        if (DataMap.ContainsKey(id))
        {
            DataMap[id].Amount += num;
            UpdateItem(id);
        }
        else
        {
            PlayerOtherItem item = GameDatabase.otherItem.Find(x => x.Id == id).CloneTo();
            item.Amount = num;
            DataMap.Add(id, item);
            AddItem(id);
        }
    }

    /// <summary>
    /// 减少item
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public static bool ReduceItem(string id, int num)
    {
        if (DataMap.ContainsKey(id) && DataMap[id].Amount >= num)
        {
            if (DataMap[id].Amount == num)
            {
                DataMap.Remove(id);
                DeleteItem(id);
            }
            else
            {
                DataMap[id].Amount -= num;
                UpdateItem(id);
            }
            return true;
        }
        return false;
    }

    protected static void UpdateItem(string id)
    {
        GameInstance.dbMapItem.DpdateOtherItem(id, DataMap[id].Amount);
    }

    protected static void AddItem(string id)
    {
        GameInstance.dbMapItem.AddOtherItem(id, DataMap[id].Amount);
    }

    protected static void DeleteItem(string id)
    {
        GameInstance.dbMapItem.DeleteOtherItem(id);
    }
}
[System.Serializable]
public class GameOtherItem
{
    public string Id;
    public string Des;
    public int Exp;

    public PlayerOtherItem CloneTo()
    {
        PlayerOtherItem cloneitem = new PlayerOtherItem();
        cloneitem.ItemID = Id;
        cloneitem.Amount = 1;
        return cloneitem;
    }
}