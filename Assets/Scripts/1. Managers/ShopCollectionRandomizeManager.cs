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

    public void RandomizeCardShopItemCollection()
    {
        shopCollectionController.RandomizeCardShopItemCollection();
    }
    public void RandomizeComponentShopItemCollection()
    {
        shopCollectionController.RandomizeComponentShopItemCollection();
    }

    public void InitCardList()
    {
        Debug.Log("init card list");
        shopCollectionController.InitCardList();
    }
    public void InitComponentList()
    {
        Debug.Log("init component list");
        shopCollectionController.InitComponentList();
    }
}
