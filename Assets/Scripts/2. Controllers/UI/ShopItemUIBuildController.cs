using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemUIBuildController : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;

    public void BuildAndDisplayItemUI(SOItemDataObject shopItem, BaseSlotManager<ShopItemUIController> slotManager)
    {
        GameObject shopItemUIGameObject;
        shopItemUIGameObject = Instantiate(itemPrefab);

        ShopItemUIController shopItemUIController = shopItemUIGameObject.GetComponent<ShopItemUIController>();
        shopItemUIController.InitUI(shopItem);

        slotManager.AddItemToCollection(shopItemUIController, null);
        shopItemUIGameObject.SetActive(true);
    }
}
