using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentShopController : MonoBehaviour
{
    private List<ShopItemUIObject> shopItemList;
    [SerializeField] protected ShopItemUIBuildController shopItemUIBuildController;
    public void CreateShopWindow(List<SOItemDataObject> itemsToDisplay, Transform startPoint)
    {
        shopItemList = new List<ShopItemUIObject>();
        foreach (SOItemDataObject item in itemsToDisplay)
        {
            int minimumChance = Random.Range(1, 101);
            if (minimumChance < item.ChanceToSpawn)
            {
                ShopItemUIObject shopItem = new ShopItemUIObject(item);
                shopItemList.Add(shopItem);

                shopItemUIBuildController.BuildAndDisplayItemUI(shopItem, startPoint);
            }
        }
    }
}
