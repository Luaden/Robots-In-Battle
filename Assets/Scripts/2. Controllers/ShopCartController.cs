using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class ShopCartController : MonoBehaviour
{
    [SerializeField] protected TMP_Text timeCostText;
    [SerializeField] protected TMP_Text currencyCostText;

    private float currencyCost = 0.0f;
    private float timeCost = 0.0f;

    private void Awake()
    {
        DowntimeManager.Instance.ShopCartSlotManager.onItemAdded += UpdateAddedItemText;
        DowntimeManager.Instance.ShopCartSlotManager.onItemRemoved += UpdateRemovedItemText;
    }
    public void PurchaseItem() 
    {
        // check money balance of the player
        // if enough, put item in inventory, remove cost etc.
        // else return, tell that they don't have enough balance
    }
    public void UndoShopping(ShopCartItemController[] shopCartItemsArray) 
    {
        // set transform to be previous parent
        // return items from the cart to their previous place in the shopping window
        foreach (ShopCartItemController shopCartItem in shopCartItemsArray)
        {
            shopCartItem.ShopCartItemSlotController.SlotManager.RemoveItemFromCollection(shopCartItem);
            ShopItemUIController shopItem = shopCartItem.GetComponent<ShopItemUIController>();

            shopItem.transform.SetParent(shopItem.PreviousParentObject);

            shopCartItem.enabled = false;
            shopItem.enabled = true;
            shopItem.isPickedUp = false;

            //shopItem.transform.position = shopItem.ShopItemUISlotController.transform.position;

            Debug.Log(shopItem.ShopItemUISlotController.name);
            Debug.Log(shopItem.PreviousParentObject.name);
        }
    }
    private void UpdateRemovedItemText(BaseSlotController<ShopCartItemController> slot)
    {
        bool[] array = new bool[slot.SlotManager.SlotList.Count];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (slot.SlotManager.SlotList[i].CurrentSlottedItem == null) ? true : false; 
        }
        if(array.All(x => x == true))
        {
            currencyCost = 0.0f;
            timeCost = 0.0f;
        }

        if(slot.CurrentSlottedItem != null)
        {
            ShopItemUIObject shopItemUI = slot.CurrentSlottedItem.GetComponent<ShopItemUIController>().ShopItemUIObject;

            currencyCost -= shopItemUI.CurrencyCost;
            timeCost -= shopItemUI.TimeCost;
        }

        timeCostText.text = timeCost.ToString();
        currencyCostText.text = currencyCost.ToString();
    }
    private void UpdateAddedItemText(BaseSlotController<ShopCartItemController> slot)
    {
        bool[] array = new bool[slot.SlotManager.SlotList.Count];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (slot.SlotManager.SlotList[i].CurrentSlottedItem == null) ? true : false;
        }
        if (array.All(x => x == true))
        {
            currencyCost = 0.0f;
            timeCost = 0.0f;
        }

        if (slot.CurrentSlottedItem != null)
        {
            ShopItemUIObject shopItemUI = slot.CurrentSlottedItem.GetComponent<ShopItemUIController>().ShopItemUIObject;

            currencyCost += shopItemUI.CurrencyCost;
            timeCost += shopItemUI.TimeCost;
        }

        timeCostText.text = timeCost.ToString();
        currencyCostText.text = currencyCost.ToString();
    }

}
