using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShopManager : MonoBehaviour
{
    [SerializeField] protected GameObject inventory;
    [SerializeField] protected GameObject shopVendorWindow;
    [SerializeField] protected GameObject shopCartWindow;

    private CardShopCartSlotManager cardShopCartSlotManager;
    private CardShopVendorSlotManager cardShopVendorSlotManager;

    public CardShopVendorSlotManager CardShopVendorSlotManager { get => cardShopVendorSlotManager; }
    public CardShopCartSlotManager CardShopCartSlotManager { get => cardShopCartSlotManager; }


    [SerializeField] protected CardShopController cardShopController;

    //private ShopCartController shopCartController;
    //public ShopCartController ShopCartController { get => shopCartController; }


    [SerializeField] protected List<SOItemDataObject> itemsToDisplay;



    private void Awake()
    {
        cardShopController = GetComponentInChildren<CardShopController>();
        //shopCartController = GetComponentInChildren<ShopCartController>(true);*/
        cardShopVendorSlotManager = GetComponentInChildren<CardShopVendorSlotManager>(true);
        cardShopCartSlotManager = GetComponentInChildren<CardShopCartSlotManager>(true);

        // testing purposes
        CreateShop();
    }

    public void CreateShop()
    {
        shopVendorWindow.SetActive(true);
        shopCartWindow.SetActive(true);
        cardShopController.CreateShopWindow(itemsToDisplay, shopVendorWindow.transform);
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
/*        List<ShopCartItemController> shopCartItemList = new List<ShopCartItemController>();
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
