using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentShopItemUIBuildController : MonoBehaviour
{
    [SerializeField] private GameObject ItemPrefab;
    public void BuildAndDisplayItemUI(ShopItemUIObject shopItem, Transform startPoint)
    {
        GameObject shopItemUIGameObject;
        shopItemUIGameObject = Instantiate(ItemPrefab, transform);
        shopItemUIGameObject.transform.position = startPoint.position;

        shopItem.ShopItemUIController = shopItemUIGameObject;
        ComponentShopVendorUIController componentShopUIController = shopItemUIGameObject.GetComponent<ComponentShopVendorUIController>();

        componentShopUIController.InitUI(shopItem);
        DowntimeManager.Instance.ComponentShopManager.ComponentShopVendorSlotManager.AddItemToCollection(componentShopUIController, null);

        shopItemUIGameObject.SetActive(true);
    }
}
