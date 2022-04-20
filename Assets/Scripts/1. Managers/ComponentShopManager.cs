using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentShopManager : MonoBehaviour
{
    [SerializeField] protected GameObject inventory;
    [SerializeField] protected GameObject shopVendorWindow;
    [SerializeField] protected GameObject shopCartWindow;

    private ComponentShopCartSlotManager componentShopCartSlotManager;
    private ComponentShopVendorSlotManager componentShopVendorSlotManager;

    public ComponentShopVendorSlotManager ComponentShopVendorSlotManager { get => componentShopVendorSlotManager; }
    public ComponentShopCartSlotManager ComponentShopCartSlotManager { get => componentShopCartSlotManager; }


    [SerializeField] protected ComponentShopController componentShopController;

    [SerializeField] protected List<SOItemDataObject> itemsToDisplay;



    private void Awake()
    {
        componentShopController = GetComponentInChildren<ComponentShopController>();
        componentShopVendorSlotManager = GetComponentInChildren<ComponentShopVendorSlotManager>(true);
        componentShopCartSlotManager = GetComponentInChildren<ComponentShopCartSlotManager>(true);

        // testing purposes
        CreateShop();
    }

    public void CreateShop()
    {
        shopVendorWindow.SetActive(true);
        shopCartWindow.SetActive(true);
        componentShopController.CreateShopWindow(itemsToDisplay, shopVendorWindow.transform);
    }

    public void OpenShop()
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
    }

    public void UndoShopping()
    {
        /*List<ShopCartItemController> shopCartItemList = new List<ShopCartItemController>();
        for (int i = 0; i < CardShopCartSlotManager.SlotList.Count; i++)
        {
            if (CardShopCartSlotManager.SlotList[i].CurrentSlottedItem != null)
            {

            }
            //shopCartItemList.Add(CardShopCartSlotManager.SlotList[i].CurrentSlottedItem);
        }

        shopCartWindow.GetComponent<ShopCartController>().UndoShopping(shopCartItemList.ToArray());*/
    }
}
