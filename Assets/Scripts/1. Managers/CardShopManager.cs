using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShopManager : MonoBehaviour
{
    [SerializeField] protected GameObject inventory;
    [SerializeField] protected GameObject shopVendorWindow;
    [SerializeField] protected GameObject shopCartWindow;

    protected CardShopController cardShopController;
    private CardShopCartSlotManager cardShopCartSlotManager;
    private CardShopVendorSlotManager cardShopVendorSlotManager;

    public CardShopVendorSlotManager CardShopVendorSlotManager { get => cardShopVendorSlotManager; }
    public CardShopCartSlotManager CardShopCartSlotManager { get => cardShopCartSlotManager; }

    [SerializeField] protected List<SOItemDataObject> itemsToDisplay;

    public void AddToShop(ShopItemCollectionObject collectionObject)
    {
        foreach (SOItemDataObject dataObject in collectionObject.ItemsInCollection)
            itemsToDisplay.Add(dataObject);
    }

    public void CreateShop()
    {
        cardShopController = GetComponentInChildren<CardShopController>();
        cardShopVendorSlotManager = GetComponentInChildren<CardShopVendorSlotManager>(true);
        cardShopCartSlotManager = GetComponentInChildren<CardShopCartSlotManager>(true);

        cardShopController.InitializeShop(itemsToDisplay, shopVendorWindow.transform);
    }

    public void OpenAndClose()
    {
        bool active = gameObject.activeInHierarchy;
        gameObject.SetActive(!active);
    }

    public void UpdateItemsToDisplay(List<SOItemDataObject> sOItemDataObjects)
    {
        itemsToDisplay = sOItemDataObjects;
    }

}
