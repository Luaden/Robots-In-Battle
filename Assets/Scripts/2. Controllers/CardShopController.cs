using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardShopController : MonoBehaviour
{
    private List<ShopItemUIObject> shopItemList;
    [SerializeField] protected CardShopItemUIBuildController shopItemUIBuildController;

    public void SelectItemsToDisplay(List<SOItemDataObject> itemsToDisplay)
    {
        shopItemList = new List<ShopItemUIObject>();
        for(int i = 0; i < itemsToDisplay.Count; i++)
        {
            int minimumChance = Random.Range(1, 101);
            if (minimumChance < itemsToDisplay[i].ChanceToSpawn)
            {
                ShopItemUIObject shopItem = new ShopItemUIObject(itemsToDisplay[i]);
                shopItemList.Add(shopItem);

                GameObject slotGO = new GameObject(name: "Slot " + i, typeof(CardShopVendorSlotController), typeof(Image));

                CardShopVendorSlotController addedSlot = slotGO.GetComponent<CardShopVendorSlotController>();

                CardShopVendorSlotManager slotManager = DowntimeManager.instance.CardShopManager.CardShopVendorSlotManager;

                addedSlot.SetSlotManager(slotManager);
                slotManager.AddSlotToList(addedSlot);

                slotGO.transform.SetParent(slotManager.transform);

                shopItemUIBuildController.BuildAndDisplayItemUI(shopItem, addedSlot);
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

        foreach(CardShopCartUIController shopCartUI in shopCartItemList)
            currencycost += shopCartUI.ShopItemUIObject.CurrencyCost;

        #region Debugging
        Debug.Log("cart total currency cost: " + currencycost);
        Debug.Log("--playerdata--");
        Debug.Log("currency: " + GameManager.instance.PlayerData.CurrencyToSpend);
        #endregion


        if (currencycost <= GameManager.instance.PlayerData.CurrencyToSpend)
        {
            foreach (CardShopCartUIController cartItem in shopCartItemList)
            {
                //add item to deck?
                GameManager.instance.InventoryController.AddItemToInventory(cartItem.ShopItemUIObject.SOItemDataObject);
                RemoveItemFromSlot(cartItem);

            }

            GameManager.instance.PlayerData.CurrencyToSpend -= currencycost;
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
