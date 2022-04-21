using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentShopController : MonoBehaviour
{
    private List<ShopItemUIObject> shopItemList;
    [SerializeField] protected ComponentShopItemUIBuildController shopItemUIBuildController;
    public void InitializeShop(List<SOItemDataObject> itemsToDisplay, Transform startPoint)
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

        List<ComponentShopCartUIController> shopCartItemList = new List<ComponentShopCartUIController>();
        for (int i = 0; i < DowntimeManager.Instance.ComponentShopManager.ComponentShopCartSlotManager.SlotList.Count; i++)
        {
            if (DowntimeManager.Instance.ComponentShopManager.ComponentShopCartSlotManager.SlotList[i].CurrentSlottedItem != null)
            {
                shopCartItemList.Add(DowntimeManager.Instance.ComponentShopManager.ComponentShopCartSlotManager.SlotList[i].CurrentSlottedItem);
            }
        }

        float currencycost = 0;
        float timecost = 0;

        foreach (ComponentShopCartUIController shopCartUI in shopCartItemList)
        {
            currencycost += shopCartUI.ShopItemUIObject.CurrencyCost;
            timecost += shopCartUI.ShopItemUIObject.TimeCost;
        }

        Debug.Log("currency cost: " + currencycost);
        Debug.Log("time cost: " + timecost);

        /*        if (currencycost <= FindObjectOfType<PlayerDataObject>().CurrencyToSpend)
                    foreach (ShopItemUIObject item in shopItemList)
                    {
                        //playerdata.AcquireItem(item)
                    }
                else
                    UndoCart();*/

    }

    public void UndoCart()
    {
        List<ComponentShopCartUIController> shopCartItemList = new List<ComponentShopCartUIController>();
        for (int i = 0; i < DowntimeManager.Instance.ComponentShopManager.ComponentShopCartSlotManager.SlotList.Count; i++)
        {
            if (DowntimeManager.Instance.ComponentShopManager.ComponentShopCartSlotManager.SlotList[i].CurrentSlottedItem != null)
            {
               shopCartItemList.Add(DowntimeManager.Instance.ComponentShopManager.ComponentShopCartSlotManager.SlotList[i].CurrentSlottedItem);
        
            }
        }

        foreach (ComponentShopCartUIController cartItem in shopCartItemList)
        {
            cartItem.ComponentShopSlotUIController.SlotManager.RemoveItemFromCollection(cartItem);
            ComponentShopVendorUIController vendorItem = cartItem.GetComponent<ComponentShopVendorUIController>();

            vendorItem.transform.SetParent(vendorItem.PreviousParentObject);
            cartItem.enabled = false;
            vendorItem.enabled = true;
            vendorItem.isPickedUp = false;

        }
    }

}
