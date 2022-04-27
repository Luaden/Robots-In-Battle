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

    private CardType cardType;
    private CardCategory cardCategory;
    private Channels possibleChannels;
    private AffectedChannels affectedChannels;
    private Channels selectedChannels;

    private int energyCost;
    private int baseDamage;
    private List<SOCardEffectObject> cardEffects;


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
    public CardType CardType { get => cardType; }
    public CardCategory CardCategory { get => cardCategory; }
    public Channels PossibleChannels { get => possibleChannels; }
    public AffectedChannels AffectedChannels { get => affectedChannels; }
    public Channels SelectedChannels { get => selectedChannels; }
    public int EnergyCost { get => energyCost; }
    public int BaseDamage { get => baseDamage; }
    public List <SOCardEffectObject> CardEffects { get => cardEffects; }

/*    public ShopItemUIObject(SOItemDataObject soItemDataObject)
    {
        this.itemType = soItemDataObject.ItemType;
        this.itemName = soItemDataObject.ItemName;
        this.itemDescription = soItemDataObject.ItemDescription;
        this.itemImage = soItemDataObject.ItemImage;
        this.timeCost = soItemDataObject.TimeCost;
        this.currencyCost = soItemDataObject.CurrencyCost;
        this.chanceToSpawn = soItemDataObject.ChanceToSpawn;

        this.soItemDataObject = soItemDataObject;
    }*/

    public ShopItemUIObject(SOItemDataObject data)
    {
        Debug.Log(data.ItemType);
        if(data.ItemType == ItemType.Card)
        {
            Debug.Log("assigning card values");
            itemName = data.CardName;
            Debug.Log("cardName: " + data.CardName);
            itemDescription = data.CardDescription;

            cardType = data.CardType;
            cardCategory = data.CardCategory;
            possibleChannels = data.PossibleChannels;
            affectedChannels = data.AffectedChannels;
            selectedChannels = Channels.None;

            energyCost = data.EnergyCost;
            baseDamage = data.BaseDamage;

            cardEffects = data.CardEffects;
            this.soItemDataObject = data;

        }

        if(data.ItemType == ItemType.Component)
        {
            this.itemName = data.ItemName;
            this.itemType = data.ItemType;
            this.itemDescription = data.ItemDescription;
            this.itemImage = data.ItemImage;
            this.timeCost = data.TimeCost;
            this.currencyCost = data.CurrencyCost;
            this.chanceToSpawn = data.ChanceToSpawn;

            this.soItemDataObject = data;
        }

        this.currencyCost = data.CurrencyCost;
        this.chanceToSpawn = data.ChanceToSpawn;


    }


}
