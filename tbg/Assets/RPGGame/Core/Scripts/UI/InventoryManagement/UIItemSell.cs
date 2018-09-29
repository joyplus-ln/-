using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class UIItemSell : UIItemSelection
{
    public UICurrency uiCurrency;
    public PlayerItem.ItemType type = PlayerItem.ItemType.character;

    // Events
    public UnityEvent eventSellSuccess;
    public UnityEvent eventSellFail;

    // Private
    private int totalSellPrice;
    private List<string> selectingItemIds = new List<string>();

    public override void Show()
    {
        base.Show();

        if (uiCurrency != null)
        {
            var currencyData = PlayerCurrency.SoftCurrency.Clone().SetAmount(0, 0);
            uiCurrency.SetData(currencyData);
        }
        SetSelectingItemIds(selectingItemIds);
    }

    public override void Hide()
    {
        base.Hide();

        if (uiCurrency != null)
        {
            var currencyData = PlayerCurrency.SoftCurrency.Clone().SetAmount(0, 0);
            uiCurrency.SetData(currencyData);
        }
        selectingItemIds = null;
    }

    protected override List<PlayerItem> GetAvailableItemList()
    {
        var list = PlayerItem.characterDataMap.Values.Where(a => (a.itemType == type) && a.CanSell).ToList();
        var e_list = PlayerItem.equipDataMap.Values.Where(a => (a.itemType == type) && a.CanSell).ToList();
        list.AddRange(e_list);
        list.SortSellPrice();
        return list;
    }

    protected override void OnSetListItem(UIItem ui)
    {
        base.OnSetListItem(ui);
        ui.displayStats = UIItem.DisplayStats.SellPrice;
    }

    protected override void SelectItem(UIItem ui)
    {
        if (ui.data.CanSell)
            base.SelectItem(ui);
        else
            ui.Selected = false;
        Calculate();
    }

    protected override void DeselectItem(UIItem ui)
    {
        base.DeselectItem(ui);
        Calculate();
    }

    public void Calculate()
    {
        var selectedItem = GetSelectedItems();
        totalSellPrice = 0;
        foreach (var entry in selectedItem)
        {
            totalSellPrice += entry.Amount * entry.SellPrice;
        }

        if (uiCurrency != null)
        {
            var currencyData = PlayerCurrency.SoftCurrency.Clone().SetAmount(totalSellPrice, 0);
            uiCurrency.SetData(currencyData);
        }
    }

    public void SetSelectingItemIds(List<string> itemIds)
    {
        selectingItemIds = itemIds;
        var availableItems = GetAvailableItems();
        if (selectingItemIds != null && selectingItemIds.Count > 0 && availableItems.Count > 0)
        {
            foreach (var selectingItemId in selectingItemIds)
            {
                if (availableItems.ContainsKey(selectingItemId))
                    availableItems[selectingItemId].Select();
            }
        }
    }
    public void OnClickSell()
    {
        var idAmountPair = GetSelectedItemIdAmountPair();
        switch (type)
        {
            case PlayerItem.ItemType.character:
                GameInstance.dbMapItem.DoSellCharacterItems(idAmountPair, OnSellSuccess);
                break;
            case PlayerItem.ItemType.equip:
                GameInstance.dbMapItem.DoSellCharacterItems(idAmountPair, OnSellSuccess);
                break;
        }

    }

    private void OnSellSuccess(ItemResult result)
    {
        GameInstance.Singleton.OnGameServiceItemResult(result);
        eventSellSuccess.Invoke();
        if (uiSelectedItemList != null)
            uiSelectedItemList.ClearListItems();
        var items = GetAvailableItems();
        var updateItem = result.updateItems;
        foreach (var entry in updateItem)
        {
            var id = entry.GUID;
            if (items.ContainsKey(id))
                items[id].SetData(entry);
        }
        var deleteItemIds = result.deleteItemIds;
        foreach (var deleteItemId in deleteItemIds.Keys)
        {
            if (uiAvailableItemList != null)
                uiAvailableItemList.RemoveListItem(deleteItemId);
        }
        var updateCurrencies = result.updateCurrencies;
        foreach (var updateCurrency in updateCurrencies)
        {
            PlayerCurrency.SetData(updateCurrency);
        }
        Calculate();
    }

    private void OnSellFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        eventSellFail.Invoke();
    }
}
