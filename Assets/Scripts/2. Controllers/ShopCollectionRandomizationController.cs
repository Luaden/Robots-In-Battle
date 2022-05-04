using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollectionRandomizationController
{
    public List<SOItemDataObject> RandomizeShopItemCollection(List<SOShopItemCollectionObject> shopItemCollectionObject)
    {
        List<SOItemDataObject> shopItemsToSend = new List<SOItemDataObject>();
        List<SOItemDataObject> possibleItems = new List<SOItemDataObject>();

        foreach (SOShopItemCollectionObject collection in shopItemCollectionObject)
        {
            if (collection.FightsBeforeAppearance > GameManager.instance.PlayerWins)
                continue;

            foreach(SOItemDataObject item in collection.ItemsInCollection)
                possibleItems.Add(item);
        }

        foreach (SOItemDataObject item in possibleItems)
        {
            int roll = Random.Range(0, 101);

            if (item.ChanceToSpawn <= roll)
                shopItemsToSend.Add(item);
        }

        foreach (SOItemDataObject item in shopItemsToSend)
            if (possibleItems.Contains(item))
                possibleItems.Remove(item);

        if (shopItemsToSend.Count < DowntimeManager.instance.MinimumShopItemCount)
        {
            for (int i = shopItemsToSend.Count; i < DowntimeManager.instance.MinimumShopItemCount; i++)
            {
                int itemRoll = Random.Range(0, possibleItems.Count);

                shopItemsToSend.Add(possibleItems[itemRoll]);
                possibleItems.Remove(possibleItems[itemRoll]);
            }
        }

        return shopItemsToSend;
    }
}
