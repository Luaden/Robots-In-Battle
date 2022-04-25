using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentShopController : MonoBehaviour
{
    private List<ShopItemUIObject> shopItemList;
    [SerializeField] protected ComponentShopItemUIBuildController shopItemUIBuildController;
    public void InitializeShop(List<SOItemDataObject> itemsToDisplay)
    {
        shopItemList = new List<ShopItemUIObject>();
        for (int i = 0; i < itemsToDisplay.Count; i++)
        {
            int minimumChance = Random.Range(1, 101);
            if (minimumChance < itemsToDisplay[i].ChanceToSpawn)
            {
                ShopItemUIObject shopItem = new ShopItemUIObject(itemsToDisplay[i]);
                shopItemList.Add(shopItem);

                GameObject slotGO = new GameObject(name: "Slot " + i, typeof(ComponentShopVendorSlotController), typeof(Image));

                ComponentShopVendorSlotController addedSlot = slotGO.GetComponent<ComponentShopVendorSlotController>();

                ComponentShopVendorSlotManager slotManager = DowntimeManager.instance.ComponentShopManager.ComponentShopVendorSlotManager;

                addedSlot.SetSlotManager(slotManager);
                slotManager.AddSlotToList(addedSlot);

                Vector3 scale = slotGO.transform.localScale;
                slotGO.transform.SetParent(slotManager.transform);
                slotGO.transform.localScale = scale;

                shopItemUIBuildController.BuildAndDisplayItemUI(shopItem, addedSlot);
            }
        }
    }

    public void PurchaseItem()
    {
        List<ComponentShopCartUIController> shopCartItemList = new List<ComponentShopCartUIController>();
        for (int i = 0; i < DowntimeManager.instance.ComponentShopManager.ComponentShopCartSlotManager.SlotList.Count; i++)
        {
            if (DowntimeManager.instance.ComponentShopManager.ComponentShopCartSlotManager.SlotList[i].CurrentSlottedItem != null)
            {
                shopCartItemList.Add(DowntimeManager.instance.ComponentShopManager.ComponentShopCartSlotManager.SlotList[i].CurrentSlottedItem);
            }
        }

        int currencyCost = 0;
        float timeCost = 0;

        foreach (ComponentShopCartUIController shopCartUI in shopCartItemList)
        {
            currencyCost += shopCartUI.ShopItemUIObject.CurrencyCost;
            timeCost += shopCartUI.ShopItemUIObject.TimeCost;
        }

        #region Debugging
        Debug.Log("--costs of cart items--");
        Debug.Log("cart total currency cost: " + currencyCost);
        Debug.Log("cart total time cost: " + timeCost);
        Debug.Log("--playerdata--");
        Debug.Log("currency: " + GameManager.instance.PlayerBankController.GetPlayerCurrency());
        Debug.Log("time left to spend: " + GameManager.instance.PlayerBankController.GetPlayerTime());
        #endregion


        if (currencyCost <= GameManager.instance.PlayerBankController.GetPlayerCurrency() &&
            timeCost <= GameManager.instance.PlayerBankController.GetPlayerTime())
        {
            foreach(ComponentShopCartUIController cartItem in shopCartItemList)
            {
                GameManager.instance.PlayerInventoryController.AddItemToInventory(cartItem.ShopItemUIObject.SOItemDataObject);
                RemoveItemFromSlot(cartItem);

            }
            GameManager.instance.PlayerBankController.SpendPlayerCurrency(currencyCost);
            GameManager.instance.PlayerBankController.SpendPlayerTime(timeCost);
        }
        else
        {
            Debug.Log("not enough currency or time for the items in the cart");
            //UndoCart();
        }

    }

    public void UndoCart()
    {
        List<ComponentShopCartUIController> shopCartItemList = new List<ComponentShopCartUIController>();
        for (int i = 0; i < DowntimeManager.instance.ComponentShopManager.ComponentShopCartSlotManager.SlotList.Count; i++)
        {
            if (DowntimeManager.instance.ComponentShopManager.ComponentShopCartSlotManager.SlotList[i].CurrentSlottedItem != null)
            {
               shopCartItemList.Add(DowntimeManager.instance.ComponentShopManager.ComponentShopCartSlotManager.SlotList[i].CurrentSlottedItem);
        
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
    public void RemoveItemFromSlot(ComponentShopCartUIController cartItem)
    {
        cartItem.ComponentShopSlotUIController.SlotManager.RemoveItemFromCollection(cartItem);
        Destroy(cartItem.gameObject);
    }

}
