﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class RpguiList : RPGUIBase
{
    public int SelectedAmount { get; protected set; }
    [Header("Selection options/configs")]
    public int limitSelection;
    public bool selectable;
    public bool multipleSelection;
    public UIItemEvent eventSelect;
    public UIItemEvent eventDeselect;
    public UnityEvent eventSelectionChange;
}

public abstract class RpguiList<T> : RpguiList
    where T : MonoBehaviour
{
    public readonly Dictionary<string, T> UIEntries = new Dictionary<string, T>();

    [Header("Generic Elements")]
    public GameObject emptyInfoObject;
    public Transform container;
    public T itemPrefab;

    private void Update()
    {
        if (emptyInfoObject != null)
            emptyInfoObject.SetActive(UIEntries.Count == 0);
    }

    public virtual T SetListItem(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;
        if (UIEntries.ContainsKey(id))
            return UIEntries[id];

        var newItemObject = Instantiate(itemPrefab.gameObject);
        newItemObject.transform.SetParent(container);
        newItemObject.transform.localScale = Vector3.one;
        newItemObject.SetActive(true);

        var newItem = newItemObject.GetComponent<T>();
        UIEntries.Add(id, newItem);
        return newItem;
    }

    public virtual bool RemoveListItem(string id)
    {
        if (UIEntries.ContainsKey(id))
        {
            var item = UIEntries[id];
            if (UIEntries.Remove(id))
            {
                Destroy(item.gameObject);
                return true;
            }
        }
        return false;
    }

    public virtual void ClearListItems()
    {
        var values = new List<T>(UIEntries.Values);
        for (var i = values.Count - 1; i >= 0; --i)
        {
            var item = values[i];
            Destroy(item.gameObject);
        }
        UIEntries.Clear();
    }
}

public abstract class RpguiDataItemList<TUIDataItem, TUIDataItemType> : RpguiList<TUIDataItem>
    where TUIDataItem : RpguiDataItem<TUIDataItemType>
    where TUIDataItemType: class
{
    protected bool isDirtySelection;
    protected readonly List<TUIDataItem> selectedUIList = new List<TUIDataItem>();
    protected readonly List<TUIDataItemType> selectedDataList = new List<TUIDataItemType>();
    protected readonly List<string> selectedIdList = new List<string>();

    public override TUIDataItem SetListItem(string id)
    {
        var newItem = base.SetListItem(id);
        if (newItem != null)
        {
            newItem.clickMode = UIDataItemClickMode.Disable;
            if (selectable)
            {
                newItem.list = this;
                newItem.clickMode = UIDataItemClickMode.Selection;
                newItem.eventSelect.RemoveListener(OnSelect);
                newItem.eventSelect.AddListener(OnSelect);
                newItem.eventDeselect.RemoveListener(OnDeselect);
                newItem.eventDeselect.AddListener(OnDeselect);
            }
        }
        return newItem;
    }

    public override bool RemoveListItem(string id)
    {
        isDirtySelection = true;
        return base.RemoveListItem(id);
    }

    public override void ClearListItems()
    {
        isDirtySelection = true;
        base.ClearListItems();
    }

    public void DeselectedItems(string exceptId)
    {
        var items = UIEntries;
        foreach (var keyValuePair in items)
        {
            var id = keyValuePair.Key;
            var item = keyValuePair.Value;
            if (item.data == null || id == exceptId)
                continue;
            item.Deselect(false);
        }
    }

    protected void OnSelect(RpguiDataItem Rpgui)
    {
        isDirtySelection = true;
        var uiItem = Rpgui as RpguiItem;
        var item = uiItem.data;
        if (!multipleSelection)
            DeselectedItems(item.guid);
        eventSelect.Invoke(uiItem);
    }

    protected void OnDeselect(RpguiDataItem Rpgui)
    {
        isDirtySelection = true;
        var uiItem = Rpgui as RpguiItem;
        eventDeselect.Invoke(uiItem);
    }

    protected void MakeSelectedLists()
    {
        if (isDirtySelection)
        {
            ClearSelectedLists();
            var items = UIEntries;
            foreach (var keyValuePair in items)
            {
                var id = keyValuePair.Key;
                var uiEntry = keyValuePair.Value;
                
                if (uiEntry.Selected)
                    MakeSelectedList(id, uiEntry);
            }
            isDirtySelection = false;
        }
    }

    protected virtual void MakeSelectedList(string id, TUIDataItem uiEntry)
    {
        ++SelectedAmount;
        selectedUIList.Add(uiEntry);
        selectedDataList.Add(uiEntry.data);
        selectedIdList.Add(id);
    }

    protected virtual void ClearSelectedLists()
    {
        SelectedAmount = 0;
        selectedUIList.Clear();
        selectedDataList.Clear();
        selectedIdList.Clear();
    }

    public List<TUIDataItem> GetSelectedUIList()
    {
        MakeSelectedLists();
        return selectedUIList;
    }

    public List<TUIDataItemType> GetSelectedDataList()
    {
        MakeSelectedLists();
        return selectedDataList;
    }

    public List<string> GetSelectedIdList()
    {
        MakeSelectedLists();
        return selectedIdList;
    }
}
