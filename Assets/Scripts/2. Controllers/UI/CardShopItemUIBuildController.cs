using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShopItemUIBuildController : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    public void BuildAndDisplayItemUI(ShopItemUIObject shopItem, CardShopVendorSlotController slot)
    {
        GameObject shopItemUIGameObject;
        shopItemUIGameObject = Instantiate(itemPrefab, transform);
        shopItemUIGameObject.transform.position = slot.transform.position;

        shopItem.ShopItemUIController = shopItemUIGameObject;

        CardShopVendorUIController cardShopVendorUIController = shopItemUIGameObject.GetComponent<CardShopVendorUIController>();
        CardShopCartUIController cardShopCartUIController = shopItemUIGameObject.GetComponent<CardShopCartUIController>();

        cardShopVendorUIController.InitUI(shopItem);
        cardShopCartUIController.InitUI(shopItem);

        cardShopVendorUIController.enabled = true;
        cardShopCartUIController.enabled = false;

        DowntimeManager.instance.CardShopManager.CardShopVendorSlotManager.AddItemToCollection(cardShopVendorUIController, slot);

        shopItemUIGameObject.SetActive(true);
    }
}
