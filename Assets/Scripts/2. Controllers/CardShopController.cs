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
        // check resources of player- time/money
        // if enough, send to inventory, remove resources from playerdata/inventory
        // if not, warn -> not enough time/money available

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

        Debug.Log("currency cost: " + currencycost);
        Debug.Log("time cost: " + timecost);

/*        if (currencycost <= FindObjectOfType<PlayerDataObject>().CurrencyToSpend)
            foreach (ShopItemUIObject item in shopItemList)
            {
                // somewhere where we will add the items to the player
                //playerdata.AcquireItem(item)
            }
        else
            UndoCart();*/


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

}
