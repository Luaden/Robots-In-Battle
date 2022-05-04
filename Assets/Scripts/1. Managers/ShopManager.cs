using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<SOShopItemCollectionObject> shopCollectionObjects;
    
    private List<SOItemDataObject> itemsToDisplay;

    private ShopCollectionRandomizationController shopCollectionRandomizationController;

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
}
