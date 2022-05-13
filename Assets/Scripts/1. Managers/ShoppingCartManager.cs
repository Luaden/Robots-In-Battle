using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShoppingCartManager : MonoBehaviour
{
    [SerializeField] private TMP_Text currencyCostText;
    [SerializeField] private TMP_Text timeCostText;

    private List<ShopItemUIController> currentItems = new List<ShopItemUIController>();
    private int currencyCost = 0;
    private float timeCost = 0;

    public void UpdateShoppingCartInventory(List<ShopItemUIController> purchasedItems)
    {
        currentItems = new List<ShopItemUIController>(purchasedItems);
        currencyCost = 0;
        timeCost = 0;

        foreach (ShopItemUIController item in purchasedItems)
        {
            currencyCost += item.BaseSOItemDataObject.CurrencyCost;
            timeCost += item.BaseSOItemDataObject.TimeCost;
        }

        UpdateShoppingCartValues(currencyCost, timeCost);
    }

    public void PurchaseItems()
    {
        if (GameManager.instance.PlayerBankController.GetPlayerCurrency() >= currencyCost &&
            GameManager.instance.PlayerBankController.GetPlayerTime() >= timeCost)
        {
            foreach(ShopItemUIController item in currentItems)
            {
                if (item.BaseSOItemDataObject.ItemType == ItemType.Component)
                    DowntimeManager.instance.InventoryUISlotManager.AddItemToCollection(item, item.ItemSlotController);
                else
                {
                    GameManager.instance.PlayerDeckController.AddCardToPlayerDeck(item.BaseSOItemDataObject);
                    item.ItemSlotController.SlotManager.RemoveItemFromCollection(item);
                    Destroy(item.gameObject);
                }
            }
            GameManager.instance.PlayerBankController.SpendPlayerCurrency(currencyCost);
            GameManager.instance.PlayerBankController.SpendPlayerTime(timeCost);
        }

        currentItems.Clear();
        UpdateShoppingCartInventory(currentItems);
        UpdateShoppingCartValues(currencyCost, timeCost);
    }

    private void UpdateShoppingCartValues(int currency, float time)
    {
        currencyCostText.text = currency.ToString();
        timeCostText.text = time.ToString();
    }
}
