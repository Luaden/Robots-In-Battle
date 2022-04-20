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
        CardShopVendorUIController cardShopUIController = shopItemUIGameObject.GetComponent<CardShopVendorUIController>();

        cardShopUIController.InitUI(shopItem);
        DowntimeManager.Instance.CardShopManager.CardShopVendorSlotManager.AddItemToCollection(cardShopUIController, null);

        shopItemUIGameObject.SetActive(true);
    }
}
