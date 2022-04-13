using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopCartController : MonoBehaviour
{
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

}
