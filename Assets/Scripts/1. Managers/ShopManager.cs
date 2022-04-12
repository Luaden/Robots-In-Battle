using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    // testing
    private ShopItemSlotManager shopItemSlotManager;
    private ShopCartSlotManager shopCartSlotManager;
    [SerializeField] private ShopItemUIController shopItemUIController;

    private ShopCartController shoppingCartController;
    public ShopCartController ShoppingCartController { get => shoppingCartController; }


    private void Awake()
    {
        shopItemSlotManager = FindObjectOfType<ShopItemSlotManager>(true);
        shopCartSlotManager = FindObjectOfType<ShopCartSlotManager>(true);

        // testing
        // do some random calculations of what items to display before adding items to the shop
        for (int i = 0; i < 12; i++)
            AddItemsToShop();

        shopItemUIController.gameObject.SetActive(false);

    }

    public void AddItemsToShop() // takes in SOItemDataObject
    {
        GameObject shopItemgameObject = Instantiate(shopItemUIController.gameObject, transform);
        ShopItemUIController shopItemUI = shopItemgameObject.GetComponent<ShopItemUIController>();

        shopItemSlotManager.AddItemToCollection(shopItemUI, null);
    }
}
