using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    [SerializeField] protected GameObject inventory;
    [SerializeField] protected GameObject shopWindow;
    [SerializeField] protected GameObject shopCart;

    private ShopCartSlotManager shopCartSlotManager;
    private ShopItemSlotManager shopItemSlotManager;
    public ShopItemSlotManager ShopItemSlotManager { get => shopItemSlotManager; }
    public ShopCartSlotManager ShopCartSlotManager { get => shopCartSlotManager; }
    

    [SerializeField] protected ShopController shopController;

    private ShopCartController shopCartController;
    public ShopCartController ShopCartController { get => shopCartController; }
    

    [SerializeField] protected List<SOItemDataObject> itemsToDisplay;



    private void Awake()
    {
        shopController = GetComponent<ShopController>();
        shopCartController = GetComponentInChildren<ShopCartController>(true);
        shopItemSlotManager = GetComponentInChildren<ShopItemSlotManager>(true);
        shopCartSlotManager = GetComponentInChildren<ShopCartSlotManager>(true);

        // testing purposes
        CreateShop();
    }

    public void CreateShop()
    {
        shopWindow.SetActive(true);
        shopCart.SetActive(true);
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
        for (int i = 0; i < ShopCartSlotManager.SlotList.Count; i++)
        {
            if (ShopCartSlotManager.SlotList[i].CurrentSlottedItem != null)
                shopCartItemList.Add(ShopCartSlotManager.SlotList[i].CurrentSlottedItem);
        }

        shopCart.GetComponent<ShopCartController>().UndoShopping(shopCartItemList.ToArray());
    }
}
