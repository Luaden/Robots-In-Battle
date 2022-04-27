using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollectionRandomizeController : MonoBehaviour
{
    protected List<SOShopItemCollectionObject> componentShopItemCollectionObjects;
    protected List<SOShopItemCollectionObject> cardShopItemCollectionObjects;

    public void InitComponentList()
    { 
        componentShopItemCollectionObjects = new List<SOShopItemCollectionObject>();
    }

    public void InitCardList()
    {
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
    public void RandomizeCardShopItemCollection()
    {
        Debug.Log("randomizing shop collection");
        //test
        int playerFights = 0;

        List<SOShopItemCollectionObject> shopCollectionToSend = new List<SOShopItemCollectionObject>();
        foreach (SOShopItemCollectionObject collection in cardShopItemCollectionObjects)
        {
            if (collection.FightsBeforeAppearance <= playerFights)
                shopCollectionToSend.Add(collection);

        }

        DowntimeManager.instance.CardShopManager.AddToShop(cardShopItemCollectionObjects);

    }
    public void RandomizeComponentShopItemCollection()
    {
        Debug.Log("randomizing shop collection");
        //test
        int playerFights = 0;

        List<SOShopItemCollectionObject> shopCollectionToSend = new List<SOShopItemCollectionObject>();
        foreach (SOShopItemCollectionObject collection in componentShopItemCollectionObjects)
        {
            if (collection.FightsBeforeAppearance <= playerFights)
                shopCollectionToSend.Add(collection);

        }

        DowntimeManager.instance.ComponentShopManager.AddToShop(componentShopItemCollectionObjects);

    }
}
