using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    // testing
    [SerializeField] protected ShopItemSlotManager shopItemSlotManager;
    [SerializeField] protected ShopCartSlotManager shopCartSlotManager;
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
            AddItemToShop();
        AddItemsToCart();

        shopItemUIController.gameObject.SetActive(false);

    }

    public void AddItemToShop() // takes in SOItemDataObject
    {
        GameObject shopItemgameObject = Instantiate(shopItemUIController.gameObject, transform);
        ShopItemUIController shopItemUI = shopItemgameObject.GetComponent<ShopItemUIController>();

        shopItemSlotManager.AddItemToCollection(shopItemUI, null);
    }

    public void AddItemsToCart()
    {

    }
}
