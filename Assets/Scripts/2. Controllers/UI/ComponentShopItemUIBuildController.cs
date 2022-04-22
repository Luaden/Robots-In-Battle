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

        shopItemUIGameObject.AddComponent<ComponentShopVendorUIController>();
        shopItemUIGameObject.AddComponent<ComponentShopCartUIController>();

        ComponentShopVendorUIController componentShopVendorUIController = shopItemUIGameObject.GetComponent<ComponentShopVendorUIController>();
        ComponentShopCartUIController componentShopCartUIController = shopItemUIGameObject.GetComponent<ComponentShopCartUIController>();

        componentShopVendorUIController.InitUI(shopItem);
        componentShopCartUIController.InitUI(shopItem);

        componentShopCartUIController.enabled = false;

        DowntimeManager.instance.ComponentShopManager.ComponentShopVendorSlotManager.AddItemToCollection(componentShopVendorUIController, null);

        shopItemUIGameObject.SetActive(true);
    }
}
