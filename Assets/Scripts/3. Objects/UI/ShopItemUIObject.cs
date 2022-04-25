using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemUIObject
{
    private ItemType itemType;
    private string itemName;
    private string itemDescription;
    private Sprite itemImage;
    private float timeCost;
    private int currencyCost;
    private int chanceToSpawn;

    private GameObject shopItemUIController;
    private SOItemDataObject soItemDataObject;

    public ItemType ItemType { get => itemType; }
    public string ItemName { get => itemName; }
    public string ItemDescription { get => itemDescription; }
    public Sprite ItemImage { get => itemImage; }
    public float TimeCost { get => timeCost; }
    public int CurrencyCost { get => currencyCost; }
    public int ChanceToSpawn { get => chanceToSpawn; }
    public GameObject ShopItemUIController { get => shopItemUIController; set => shopItemUIController = value; }
    public SOItemDataObject SOItemDataObject { get => soItemDataObject; }

    public ShopItemUIObject(SOItemDataObject soItemDataObject)
    {
        this.itemType = soItemDataObject.ItemType;
        this.itemName = soItemDataObject.ItemName;
        this.itemDescription = soItemDataObject.ItemDescription;
        this.itemImage = soItemDataObject.ItemImage;
        this.timeCost = soItemDataObject.TimeCost;
        this.currencyCost = soItemDataObject.CurrencyCost;
        this.chanceToSpawn = soItemDataObject.ChanceToSpawn;

        this.soItemDataObject = soItemDataObject;
    }
}
