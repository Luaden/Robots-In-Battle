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

    [SerializeField] protected List<SOItemDataObject> itemsToDisplay;

    public void CreateShop()
    {
        componentShopController = GetComponentInChildren<ComponentShopController>(true);
        componentShopVendorSlotManager = GetComponentInChildren<ComponentShopVendorSlotManager>(true);
        componentShopCartSlotManager = GetComponentInChildren<ComponentShopCartSlotManager>(true);

        componentShopController.InitializeShop(itemsToDisplay, shopVendorWindow.transform);
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

/*    public void OpenShop()
    {
        inventory.SetActive(false);
        shopVendorWindow.SetActive(true);
        shopCartWindow.SetActive(true);
    }
    public void OpenInventory()
    {
        shopVendorWindow.SetActive(false);
        shopCartWindow.SetActive(false);
        inventory.SetActive(true);
    }*/
}
