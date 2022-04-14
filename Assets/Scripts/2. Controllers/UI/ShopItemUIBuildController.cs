using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemUIBuildController : MonoBehaviour
{
    [SerializeField] private GameObject ItemPrefab;
    public void BuildAndDisplayItemUI(ShopItemUIObject shopItem, Transform startPoint)
    {
        GameObject shopItemUIGameObject;
        shopItemUIGameObject = Instantiate(ItemPrefab, transform);
        shopItemUIGameObject.transform.position = startPoint.position;

        shopItem.ShopItemUIController = shopItemUIGameObject;
        ShopItemUIController shopItemUIObject = shopItemUIGameObject.GetComponent<ShopItemUIController>();

        shopItemUIObject.InitShopItemUI(shopItem);

        DowntimeManager.Instance.ShopItemSlotManager.AddItemToCollection(shopItemUIObject, null);

        shopItemUIGameObject.SetActive(true);
    }
}
