using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] protected ShopItemSlotManager shopItemSlotManager;
    [SerializeField] protected ShopCartSlotManager shopCartSlotManager;

    [SerializeField] protected GameObject inventory;
    [SerializeField] protected GameObject shopWindow;
    [SerializeField] protected GameObject shopCart;

    private ShopCartController shoppingCartController;
    public ShopCartController ShoppingCartController { get => shoppingCartController; }

    // testing
    private int objectId = 0;
    [SerializeField] private ShopItemUIController shopItemUIController;


    private void Awake()
    {
        shopItemSlotManager = FindObjectOfType<ShopItemSlotManager>(true);
        shopCartSlotManager = FindObjectOfType<ShopCartSlotManager>(true);

        // testing
        // do some random calculations of what items to display before adding items to the shop
        for (int i = 0; i < 12; i++)
        {
            AddItemToShop();
            objectId++;
        }

        shopItemUIController.gameObject.SetActive(false);

    }

    public void AddItemToShop() // takes in SOItemDataObject
    {
        GameObject shopItemgameObject = Instantiate(shopItemUIController.gameObject, transform);
        shopItemgameObject.name = "Item" + objectId; 
        ShopItemUIController shopItemUI = shopItemgameObject.GetComponent<ShopItemUIController>();

        shopItemSlotManager.AddItemToCollection(shopItemUI, null);
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
        for(int i = 0; i < shopCartSlotManager.SlotList.Count; i++)
        {
            if (shopCartSlotManager.SlotList[i].CurrentSlottedItem != null)
                shopCartItemList.Add(shopCartSlotManager.SlotList[i].CurrentSlottedItem);

        }

        shopCart.GetComponent<ShopCartController>().UndoShopping(shopCartItemList.ToArray());
    }
}
