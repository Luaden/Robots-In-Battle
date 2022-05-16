using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<SOShopItemCollectionObject> shopCollectionObjects;
    
    private List<SOItemDataObject> itemsToDisplay;
    private ShopItemUIController currentItemSelected;

    private ShopCollectionRandomizationController shopCollectionRandomizationController;
    public ShopItemUIController CurrentItemSelected { get => currentItemSelected; set => currentItemSelected = value; }
    
    public void InitializeShop()
    {
        itemsToDisplay = shopCollectionRandomizationController.RandomizeShopItemCollection(shopCollectionObjects);

        foreach (SOItemDataObject item in itemsToDisplay)
            DowntimeManager.instance.ShopItemUIBuildController.BuildAndDisplayItemUI(item, DowntimeManager.instance.ShopUISlotManager);
    }

    private void Awake()
    {
        shopCollectionRandomizationController =  new ShopCollectionRandomizationController();
    }

    public void PurchaseItem()
    {
        if(currentItemSelected == null)
        {
            Debug.Log("You haven't selected an item to purchase.");
            return;
        }

        if(currentItemSelected.BaseSOItemDataObject.CurrencyCost <= GameManager.instance.PlayerBankController.GetPlayerCurrency() ||
            currentItemSelected.BaseSOItemDataObject.TimeCost <= GameManager.instance.PlayerBankController.GetPlayerTime())
        {
            if (currentItemSelected.BaseSOItemDataObject.ItemType == ItemType.Component)
                DowntimeManager.instance.InventoryUISlotManager.AddItemToCollection(currentItemSelected, currentItemSelected.ItemSlotController);
            else
            {
                GameManager.instance.PlayerDeckController.AddCardToPlayerDeck(currentItemSelected.BaseSOItemDataObject);
                currentItemSelected.ItemSlotController.SlotManager.RemoveItemFromCollection(currentItemSelected);
                Destroy(currentItemSelected.gameObject);
            }

            GameManager.instance.PlayerBankController.SpendPlayerCurrency(currentItemSelected.BaseSOItemDataObject.CurrencyCost);
            GameManager.instance.PlayerBankController.SpendPlayerTime(currentItemSelected.BaseSOItemDataObject.TimeCost);
        }
    }

}
