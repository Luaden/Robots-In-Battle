using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentShopItemUIBuildController : MonoBehaviour
{
    [SerializeField] private GameObject ItemPrefab;
    public void BuildAndDisplayItemUI(ShopItemUIObject shopItem, ComponentShopVendorSlotController slot)
    {
        GameObject shopItemUIGameObject;
        shopItemUIGameObject = Instantiate(ItemPrefab, transform);
        shopItemUIGameObject.transform.position = slot.transform.position;

        shopItem.ShopItemUIController = shopItemUIGameObject;

        shopItemUIGameObject.AddComponent<ComponentShopVendorUIController>();
        shopItemUIGameObject.AddComponent<ComponentShopCartUIController>();

        ComponentShopVendorUIController componentShopVendorUIController = shopItemUIGameObject.GetComponent<ComponentShopVendorUIController>();
        ComponentShopCartUIController componentShopCartUIController = shopItemUIGameObject.GetComponent<ComponentShopCartUIController>();

        componentShopVendorUIController.InitUI(shopItem);
        componentShopCartUIController.InitUI(shopItem);

        componentShopCartUIController.enabled = false;

        DowntimeManager.instance.ComponentShopManager.ComponentShopVendorSlotManager.AddItemToCollection(componentShopVendorUIController, slot);

        shopItemUIGameObject.SetActive(true);
    }
}
