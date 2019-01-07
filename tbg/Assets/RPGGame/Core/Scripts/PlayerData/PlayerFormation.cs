using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;

[System.Serializable]
public class PlayerFormation : BasePlayerData, IIPlayerFormation
{
    public static readonly Dictionary<string, PlayerFormation> DataMap = new Dictionary<string, PlayerFormation>();
    public string characterGuid { get; set; }
    public string playerId;
    public string PlayerId { get { return playerId; } set { playerId = value; } }
    public string FormationId;
    public string formationId { get { return FormationId; } set { FormationId = value; } }
    public int position;
    public int Position { get { return position; } set { position = value; } }
    public string itemId;
    public string ItemId { get { return itemId; } set { itemId = value; } }

    #region Non Serialize Fields
    public Formation FormationData
    {
        get
        {
            Formation formation;
            if (GameDatabase != null && !string.IsNullOrEmpty(formationId) && GameDatabase.Formations.TryGetValue(formationId, out formation))
                return formation;
            return null;
        }
    }
    #endregion


    public static string GetId(string playerId, string dataId, int position)
    {
        return playerId;
        return playerId + "_" + dataId + "_" + position;
    }

    public string GetFormationID()
    {
        return PlayerId + "_" + formationId + "_" + Position;
    }

    public static void SetData(PlayerFormation data)
    {
        if (data == null || string.IsNullOrEmpty(data.characterGuid))
            return;
        DataMap[data.characterGuid] = data;
    }

    public static bool TryGetData(string playerId, string dataId, int position, out PlayerFormation data)
    {
        foreach(var item in DataMap.Values)
        {
            if(item.GetFormationID().Equals(playerId + "_" + dataId + "_" + position))
            {
                data = item;
                return true;
            }
        }
        data = null;
        return false;
    }

    public static bool TryGetData(string dataId, int position, out PlayerFormation data)
    {
        return TryGetData(IPlayer.CurrentPlayer.guid, dataId, position, out data);
    }

    public static bool RemoveData(string id)
    {
        return DataMap.Remove(id);
    }

    public static void ClearData()
    {
        DataMap.Clear();
    }

    public static void SetDataRange(IEnumerable<PlayerFormation> list)
    {
        foreach (var data in list)
        {
            SetData(data);
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
                RemoveData(value.characterGuid);
        }
    }

    public static void RemoveDataRange()
    {
        RemoveDataRange(IPlayer.CurrentPlayer.guid);
    }

    public static bool ContainsDataWithItemId(string itemId)
    {
        var values = DataMap.Values;
        foreach (var value in values)
        {
            if (value.ItemId == itemId)
                return true;
        }
        return false;
    }
}
