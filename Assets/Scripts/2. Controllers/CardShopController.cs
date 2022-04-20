using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShopController : MonoBehaviour
{
    private List<ShopItemUIObject> shopItemList;
    [SerializeField] protected CardShopItemUIBuildController shopItemUIBuildController;
    public void CreateShopWindow(List<SOItemDataObject> itemsToDisplay, Transform startPoint)
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
        // if enough, send to inventory
    }

    public void UndoCart()
    {
        List<CardShopCartUIController> shopCartItemList = new List<CardShopCartUIController>();
        for (int i = 0; i < DowntimeManager.Instance.CardShopManager.CardShopCartSlotManager.SlotList.Count; i++)
        {
            if (DowntimeManager.Instance.CardShopManager.CardShopCartSlotManager.SlotList[i].CurrentSlottedItem != null)
            {
                shopCartItemList.Add(DowntimeManager.Instance.CardShopManager.CardShopCartSlotManager.SlotList[i].CurrentSlottedItem);

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
