using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    [SerializeField] protected GameObject inventory;
    [SerializeField] protected GameObject shopWindow;
    [SerializeField] protected GameObject shopCart;

    [SerializeField] protected ShopController shopController;

    [SerializeField] protected ShopCartController shopCartController;
    public ShopCartController ShopCartController { get => shopCartController; }

    [SerializeField] protected List<SOItemDataObject> itemsToDisplay;



    private void Awake()
    {
        shopController = GetComponent<ShopController>();
        shopCartController = FindObjectOfType<ShopCartController>();
        // testing purposes
        CreateShop();
    }

    public void CreateShop()
    {
        shopController.CreateShopWindow(itemsToDisplay, shopWindow.transform);
    }

    public void OpenShop()
    {
        inventory.SetActive(false);
        shopCart.SetActive(true);
        shopWindow.SetActive(true);
    }
    public void OpenInventory()
    {
        shopCart.SetActive(false);
        shopWindow.SetActive(false);
        inventory.SetActive(true);
    }
    
    public void UndoShopping()
    {
        List<ShopCartItemController> shopCartItemList = new List<ShopCartItemController>();
        ShopCartSlotManager shopCartSlotManager = DowntimeManager.Instance.ShopCartSlotManager;
        for (int i = 0; i < shopCartSlotManager.SlotList.Count; i++)
        {
            if (shopCartSlotManager.SlotList[i].CurrentSlottedItem != null)
                shopCartItemList.Add(shopCartSlotManager.SlotList[i].CurrentSlottedItem);
        }

        shopCart.GetComponent<ShopCartController>().UndoShopping(shopCartItemList.ToArray());
    }
}
