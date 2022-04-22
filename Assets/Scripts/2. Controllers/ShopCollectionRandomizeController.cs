using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollectionRandomizeController : MonoBehaviour
{
    protected List<SOShopItemCollectionObject> componentShopItemCollectionObjects;
    protected List<SOShopItemCollectionObject> cardShopItemCollectionObjects;

    // gets called first to add the CollectionObjects to a list, which gets sent to their respective shopmanagers Add to Shop
    public void AddToComponentShopCollectionList(SOShopItemCollectionObject collection)
    {
        if(collection != null && collection.CollectionType == ItemType.Component)
            componentShopItemCollectionObjects.Add(collection);
    }
    public void AddToCardShopCollectionList(SOShopItemCollectionObject collection)
    {
        if (collection != null && collection.CollectionType == ItemType.Card)
            componentShopItemCollectionObjects.Add(collection);
    }
    public void RandomizeShopItemCollection()
    {
        // needs to have AddToComponentShopCollectionList to have been performed before this gets called
        // and AddToCardCollectionList
        // test
        int playerFights = 3;
        List<SOShopItemCollectionObject> shopCollectionToSend = new List<SOShopItemCollectionObject>();
        foreach (SOShopItemCollectionObject collection in componentShopItemCollectionObjects)
        {
            if (collection.FightsBeforeAppearance <= playerFights)
                shopCollectionToSend.Add(collection);

        }

        DowntimeManager.Instance.ComponentShopManager.AddToShop(componentShopItemCollectionObjects);
        DowntimeManager.Instance.CardShopManager.AddToShop(cardShopItemCollectionObjects);

    }
}
