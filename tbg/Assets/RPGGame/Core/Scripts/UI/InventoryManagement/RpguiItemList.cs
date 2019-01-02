﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RpguiItemList : RpguiDataItemList<RpguiItem, PlayerItem>
{
    // Private
    private readonly Dictionary<string, int> selectedItemIdAmountPair = new Dictionary<string, int>();

    public void SetListItems(List<PlayerItem> list, UnityAction<RpguiItem> onSetListItem = null)
    {
        ClearListItems();
        Debug.Log("item list " + list.Count);
        foreach (var entry in list)
        {
            var ui = SetListItem(entry);
            if (ui != null && onSetListItem != null)
                onSetListItem(ui);
        }
    }

    public RpguiItem SetListItem(PlayerItem data)
    {
        if (data == null || string.IsNullOrEmpty(data.GUID))
            return null;
        var item = SetListItem(data.GUID);
        item.SetData(data);
        return item;
    }

    protected override void MakeSelectedList(string id, RpguiItem rpguiEntry)
    {
        base.MakeSelectedList(id, rpguiEntry);
        selectedItemIdAmountPair.Add(id, rpguiEntry.SelectedAmount);
    }

    protected override void ClearSelectedLists()
    {
        base.ClearSelectedLists();
        selectedItemIdAmountPair.Clear();
    }

    public List<RpguiItem> GetSelectedUIList(string dataId)
    {
        var valueList = GetSelectedUIList();
        var list = valueList.Where(entry =>
            entry != null &&
            entry.data != null &&
            entry.data.ItemData != null &&
            entry.data.GUID.Equals(dataId)).ToList();
        return list;
    }

    public List<PlayerItem> GetSelectedDataList(string dataId)
    {
        var valueList = GetSelectedDataList();
        var list = valueList.Where(entry =>
            entry != null &&
            entry.ItemData != null &&
            entry.GUID.Equals(dataId)).ToList();
        return list;
    }

    public Dictionary<string, int> GetSelectedItemIdAmountPair()
    {
        MakeSelectedLists();
        return selectedItemIdAmountPair;
    }

    public bool ContainsItemWithDataId(string dataId)
    {
        MakeSelectedLists();
        var list = GetSelectedDataList();
        foreach (var entry in list)
        {
            if (entry.GUID == dataId)
                return true;
        }
        return false;
    }
}