using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollectionRandomizeController : MonoBehaviour
{
    protected List<ShopItemCollectionObject> componentShopItemCollectionObjects;
    protected List<ShopItemCollectionObject> cardShopItemCollectionObjects;

    public void AddToComponentShopCollectionList(ShopItemCollectionObject item)
    {
        if(item != null && item.CollectionType == ItemType.Component)
            componentShopItemCollectionObjects.Add(item);
    }
    public void AddToCardShopCollectionList(ShopItemCollectionObject item)
    {
        if (item != null && item.CollectionType == ItemType.Card)
            componentShopItemCollectionObjects.Add(item);
    }
    public void RandomizeShopItemCollection()
    {
        List<ShopItemCollectionObject> shopCollectionToSend = new List<ShopItemCollectionObject>();
        foreach (ShopItemCollectionObject collection in componentShopItemCollectionObjects)
        {
            //if (collection.FightsBeforeAppearance >= PlayerDataObject.Fights)
                shopCollectionToSend.Add(collection);

        }

        //DowntimeManager.Instance.ComponentShopManager.AddToShop(componentShopItemCollectionObjects);
        //DowntimeManager.Instance.CardShopManager.AddToShop(cardShopItemCollectionObjects);

    }
}
