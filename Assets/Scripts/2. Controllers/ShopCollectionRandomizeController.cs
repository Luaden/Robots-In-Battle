using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollectionRandomizeController : MonoBehaviour
{
    protected List<SOShopItemCollectionObject> componentShopItemCollectionObjects;
    protected List<SOShopItemCollectionObject> cardShopItemCollectionObjects;

    public void InitList()
    { 
        componentShopItemCollectionObjects = new List<SOShopItemCollectionObject>();
        cardShopItemCollectionObjects = new List<SOShopItemCollectionObject>();
    }

    public void AddToComponentShopCollectionList(SOShopItemCollectionObject collection)
    {
        if(collection != null && collection.CollectionType == ItemType.Component)
            componentShopItemCollectionObjects.Add(collection);
    }
    public void AddToCardShopCollectionList(SOShopItemCollectionObject collection)
    {
        if (collection != null && collection.CollectionType == ItemType.Card)
            cardShopItemCollectionObjects.Add(collection);
    }
    public void RandomizeShopItemCollection()
    {
        //test
        int playerFights = 3;

        if (cardShopItemCollectionObjects == null || cardShopItemCollectionObjects.Count == 0)
        {
            Debug.Log("missing card collection object");
            return;
        }
        if(componentShopItemCollectionObjects == null || componentShopItemCollectionObjects.Count == 0)
        {
            Debug.Log("missing component collection object");
            return;
        }

        List<SOShopItemCollectionObject> shopCollectionToSend = new List<SOShopItemCollectionObject>();
        foreach (SOShopItemCollectionObject collection in cardShopItemCollectionObjects)
        {
            if (collection.FightsBeforeAppearance <= playerFights)
            {
                shopCollectionToSend.Add(collection);
            }

        }
        foreach (SOShopItemCollectionObject collection in componentShopItemCollectionObjects)
        {
            if (collection.FightsBeforeAppearance <= playerFights)
                shopCollectionToSend.Add(collection);

        }

        DowntimeManager.instance.ComponentShopManager.AddToShop(componentShopItemCollectionObjects);
        DowntimeManager.instance.CardShopManager.AddToShop(cardShopItemCollectionObjects);

    }
}
