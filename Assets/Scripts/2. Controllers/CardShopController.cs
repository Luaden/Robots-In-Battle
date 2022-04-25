using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShopController : MonoBehaviour
{
    private List<ShopItemUIObject> shopItemList;
    [SerializeField] protected CardShopItemUIBuildController shopItemUIBuildController;

    public void SelectItemsToDisplay(List<SOItemDataObject> itemsToDisplay, Transform startPoint)
    {
        shopItemList = new List<ShopItemUIObject>();
        foreach (SOItemDataObject item in itemsToDisplay)
        {
            int minimumChance = Random.Range(1, 101);
            if (minimumChance < item.ChanceToSpawn)
            {
                ShopItemUIObject shopItem = new ShopItemUIObject(item);
                shopItemList.Add(shopItem);

                shopItemUIBuildController.BuildAndDisplayItemUI(shopItem, startPoint);
            }
        }
    }
    public void PurchaseItem()
    {

        List<CardShopCartUIController> shopCartItemList = new List<CardShopCartUIController>();
        for (int i = 0; i < DowntimeManager.instance.CardShopManager.CardShopCartSlotManager.SlotList.Count; i++)
        {
            if (DowntimeManager.instance.CardShopManager.CardShopCartSlotManager.SlotList[i].CurrentSlottedItem != null)
            {
                shopCartItemList.Add(DowntimeManager.instance.CardShopManager.CardShopCartSlotManager.SlotList[i].CurrentSlottedItem);
            }
        }

        float currencycost = 0;
        float timecost = 0;

        foreach(CardShopCartUIController shopCartUI in shopCartItemList)
        {
            currencycost += shopCartUI.ShopItemUIObject.CurrencyCost;
            timecost += shopCartUI.ShopItemUIObject.TimeCost;
        }

        #region Debugging
        Debug.Log("cart total currency cost: " + currencycost);
        Debug.Log("cart total time cost: " + timecost);
        Debug.Log("--playerdata--");
        Debug.Log("currency: " + GameManager.instance.PlayerData.CurrencyToSpend);
        Debug.Log("time left to spend: " + GameManager.instance.PlayerData.TimeLeftToSpend);
        #endregion


        if (currencycost <= GameManager.instance.PlayerData.CurrencyToSpend &&
            timecost <= GameManager.instance.PlayerData.TimeLeftToSpend)
        {
            foreach (CardShopCartUIController cartItem in shopCartItemList)
            {
                GameManager.instance.InventoryController.AddItemToInventory(cartItem.ShopItemUIObject.SOItemDataObject);
                RemoveItemFromSlot(cartItem);

            }

            GameManager.instance.PlayerData.CurrencyToSpend -= currencycost;
            GameManager.instance.PlayerData.TimeLeftToSpend -= timecost;
        }
        else
        {
            Debug.Log("not enough currency or time for the items in the cart");
            UndoCart();
        }


    }
    public void UndoCart()
    {
        List<CardShopCartUIController> shopCartItemList = new List<CardShopCartUIController>();
        for (int i = 0; i < DowntimeManager.instance.CardShopManager.CardShopCartSlotManager.SlotList.Count; i++)
        {
            if (DowntimeManager.instance.CardShopManager.CardShopCartSlotManager.SlotList[i].CurrentSlottedItem != null)
            {
                shopCartItemList.Add(DowntimeManager.instance.CardShopManager.CardShopCartSlotManager.SlotList[i].CurrentSlottedItem);

            }
        }

        foreach (CardShopCartUIController cartItem in shopCartItemList)
        {
            cartItem.CardShopCartSlotController.SlotManager.RemoveItemFromCollection(cartItem);
            CardShopVendorUIController vendorItem = cartItem.GetComponent<CardShopVendorUIController>();

            vendorItem.transform.SetParent(vendorItem.PreviousParentObject);
            cartItem.enabled = false;
            vendorItem.enabled = true;
            vendorItem.isPickedUp = false;

        }

    }
    public void RemoveItemFromSlot(CardShopCartUIController cartItem)
    {
        cartItem.CardShopCartSlotController.SlotManager.RemoveItemFromCollection(cartItem);
        Destroy(cartItem.gameObject);
    }

}
