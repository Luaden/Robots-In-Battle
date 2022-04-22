using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollectionRandomizeManager : MonoBehaviour
{
    [SerializeField] protected ShopCollectionRandomizeController shopCollectionController;

    public void AddToComponentShopCollectionList(SOShopItemCollectionObject item) 
    {
        shopCollectionController.AddToComponentShopCollectionList(item); 
    }

    public void AddToCardShopCollectionList(SOShopItemCollectionObject item)
    {
        shopCollectionController.AddToCardShopCollectionList(item);
    }

    public void RandomizeShopItemCollection()
    {
        shopCollectionController.RandomizeShopItemCollection();
    }

    public void InitList()
    {
        shopCollectionController.InitList();
    }
}
