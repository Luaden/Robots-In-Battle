using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShopItemUIBuildController : MonoBehaviour
{
    [SerializeField] private GameObject ItemPrefab;
    public void BuildAndDisplayItemUI(ShopItemUIObject shopItem, CardShopVendorSlotController slot)
    {
        GameObject shopItemUIGameObject;
        shopItemUIGameObject = Instantiate(ItemPrefab, transform);
        shopItemUIGameObject.transform.position = slot.transform.position;

        shopItem.ShopItemUIController = shopItemUIGameObject;

        shopItemUIGameObject.AddComponent<CardShopVendorUIController>();
        shopItemUIGameObject.AddComponent<CardShopCartUIController>();

        CardShopVendorUIController cardShopVendorUIController = shopItemUIGameObject.GetComponent<CardShopVendorUIController>();
        CardShopCartUIController cardShopCartUIController = shopItemUIGameObject.GetComponent<CardShopCartUIController>();

        cardShopVendorUIController.InitUI(shopItem);
        cardShopCartUIController.InitUI(shopItem);

        cardShopCartUIController.enabled = false;

        DowntimeManager.instance.CardShopManager.CardShopVendorSlotManager.AddItemToCollection(cardShopVendorUIController, slot);

        shopItemUIGameObject.SetActive(true);
    }
}
