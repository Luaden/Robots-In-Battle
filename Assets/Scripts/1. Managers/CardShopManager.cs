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

    private List<SOItemDataObject> itemsToDisplay;
    [SerializeField] protected List<SOShopItemCollectionObject> shopCollectionObjects;

    public void AddToShop(List<SOShopItemCollectionObject> collections)
    {

        foreach (SOShopItemCollectionObject collection in collections)
        {
            foreach (SOItemDataObject item in collection.ItemsInCollection)
            {
                itemsToDisplay.Add(item);
            }
        }
    }

    // should only be called once everytime we change to downtime
    public void InitializeShop()
    {
        cardShopController = GetComponentInChildren<CardShopController>(true);
        cardShopVendorSlotManager = GetComponentInChildren<CardShopVendorSlotManager>(true);
        cardShopCartSlotManager = GetComponentInChildren<CardShopCartSlotManager>(true);

        itemsToDisplay = new List<SOItemDataObject>();

        foreach(SOShopItemCollectionObject collection in shopCollectionObjects)
        {
            DowntimeManager.instance.ShopCollectionRandomizeManager.InitCardList();
            DowntimeManager.instance.ShopCollectionRandomizeManager.AddToCardShopCollectionList(collection);
        }
        
        DowntimeManager.instance.ShopCollectionRandomizeManager.RandomizeShopItemCollection();

        cardShopController.SelectItemsToDisplay(itemsToDisplay);
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
