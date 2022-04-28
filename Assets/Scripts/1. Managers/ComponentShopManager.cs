using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentShopManager : MonoBehaviour
{
    [SerializeField] protected GameObject inventory;
    [SerializeField] protected GameObject shopVendorWindow;
    [SerializeField] protected GameObject shopCartWindow;

    protected ComponentShopController componentShopController;
    private ComponentShopCartSlotManager componentShopCartSlotManager;
    private ComponentShopVendorSlotManager componentShopVendorSlotManager;
    public ComponentShopVendorSlotManager ComponentShopVendorSlotManager { get => componentShopVendorSlotManager; }
    public ComponentShopCartSlotManager ComponentShopCartSlotManager { get => componentShopCartSlotManager; }

    private List<SOItemDataObject> itemsToDisplay;
    [SerializeField] protected List<SOShopItemCollectionObject> shopCollectionObjects;

    public void AddToShop(List<SOShopItemCollectionObject> collections)
    {
        foreach (SOShopItemCollectionObject collection in collections)
            foreach (SOItemDataObject item in collection.ItemsInCollection)
                itemsToDisplay.Add(item);
    }
    // should only be called once everytime we change to downtime
    public void InitializeShop()
    {
        componentShopController = GetComponentInChildren<ComponentShopController>(true);
        componentShopVendorSlotManager = GetComponentInChildren<ComponentShopVendorSlotManager>(true);
        componentShopCartSlotManager = GetComponentInChildren<ComponentShopCartSlotManager>(true);

        itemsToDisplay = new List<SOItemDataObject>();
        DowntimeManager.instance.ShopCollectionRandomizeManager.InitComponentList();

        foreach (SOShopItemCollectionObject collection in shopCollectionObjects)
            DowntimeManager.instance.ShopCollectionRandomizeManager.AddToComponentShopCollectionList(collection);

        DowntimeManager.instance.ShopCollectionRandomizeManager.RandomizeComponentShopItemCollection();

        componentShopController.InitializeShop(itemsToDisplay);
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
