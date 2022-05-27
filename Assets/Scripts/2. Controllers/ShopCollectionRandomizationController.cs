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
            int fightCheckCount = GameManager.instance.PlayerWins;

            if (GameManager.instance.PlayerWins == 0)
                fightCheckCount = 5;

            if (collection.FightsBeforeAppearance > fightCheckCount)
                continue;

            foreach(SOItemDataObject item in collection.ItemsInCollection)
                possibleItems.Add(item);
        }

        foreach (SOItemDataObject item in possibleItems)
        {
            int roll = Random.Range(0, 101);

            if (item.ChanceToSpawn <= roll)
                shopItemsToSend.Add(item);

            if(possibleItems.Count == 0)
            {
                for(int i = 0; i < DowntimeManager.instance.MinimumShopItemCount; i++)
                {
                    shopItemsToSend.Add(possibleItems[i]);
                }
            }
        }

        foreach (SOItemDataObject item in shopItemsToSend)
            if (possibleItems.Contains(item))
                possibleItems.Remove(item);

        if (shopItemsToSend.Count < DowntimeManager.instance.MinimumShopItemCount)
        {
            int i = DowntimeManager.instance.MinimumShopItemCount - shopItemsToSend.Count;

            for (int j = 0; j < i; j++)
            {
                int itemRoll = Random.Range(0, possibleItems.Count);

                shopItemsToSend.Add(possibleItems[itemRoll]);
                possibleItems.Remove(possibleItems[itemRoll]);
            }
        }

        if(shopItemsToSend.Count > DowntimeManager.instance.MaximumShopItemCount)
        {
            int i = shopItemsToSend.Count - DowntimeManager.instance.MinimumShopItemCount;

            for(int j = 0; j < i; j++)
            {
                int itemRoll = Random.Range(0, shopItemsToSend.Count);

                shopItemsToSend.Remove(shopItemsToSend[itemRoll]);
            }
        }

        return shopItemsToSend;
    }
}
