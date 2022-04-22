using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShopItemUIBuildController : MonoBehaviour
{
    [SerializeField] private GameObject ItemPrefab;
    public void BuildAndDisplayItemUI(ShopItemUIObject shopItem, Transform startPoint)
    {
        GameObject shopItemUIGameObject;
        shopItemUIGameObject = Instantiate(ItemPrefab, transform);
        shopItemUIGameObject.transform.position = startPoint.position;

        shopItem.ShopItemUIController = shopItemUIGameObject;

        shopItemUIGameObject.AddComponent<CardShopVendorUIController>();
        shopItemUIGameObject.AddComponent<CardShopCartUIController>();

        CardShopVendorUIController cardShopVendorUIController = shopItemUIGameObject.GetComponent<CardShopVendorUIController>();
        CardShopCartUIController cardShopCartUIController = shopItemUIGameObject.GetComponent<CardShopCartUIController>();

        cardShopVendorUIController.InitUI(shopItem);
        cardShopCartUIController.InitUI(shopItem);

        cardShopCartUIController.enabled = false;

        DowntimeManager.instance.CardShopManager.CardShopVendorSlotManager.AddItemToCollection(cardShopVendorUIController, null);

        shopItemUIGameObject.SetActive(true);
    }
}
