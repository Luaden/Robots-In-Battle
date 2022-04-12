using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopItemUIController shopItemUIController;
    //List<Items> ? IShoppable

    public ShopItemUIController ShopItemUIController { get => shopItemUIController; }

    public void PurchaseItem(SOCardDataObject item, BaseSlotController<ShopItemUIController> slot)
    {
        //shopUIController.PurchaseItem(item, slot);
    }
    public void ReturnItem(SOCardDataObject item, BaseSlotController<ShopItemUIController> slot)
    {
        //shopUIController.ReturnItem(item, slot);
    }
}
