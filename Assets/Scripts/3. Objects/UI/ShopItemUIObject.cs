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

    private MechComponent componentType;
    private Sprite componentImage;
    private int componentHP;
    private int componentEnergy;
    private ElementType componentElement;
    private int bonusDamageFromComponent;
    private bool bonusDamageAsPercent;
    private int reduceDamageToComponent;
    private bool reduceDamageAsPercent;
    private int extraElementStacks;
    private int energyGainModifier;


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

    public string ComponentName { get => itemName; }
    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentImage; }
    public int ComponentHP { get => componentHP; }
    public int ComponentEnergy { get => componentEnergy; }
    public ElementType ComponentElement { get => componentElement; }
    public int BonusDamageFromComponent { get => bonusDamageFromComponent; }
    public bool BonusDamageAsPercent { get => bonusDamageAsPercent; }
    public int ReduceDamageToComponent { get => reduceDamageToComponent; }
    public bool ReduceDamageAsPercent { get => reduceDamageAsPercent; }
    public int ExtraElementStacks { get => extraElementStacks; }
    public int EnergyGainModifier { get => energyGainModifier; }

    public ShopItemUIObject(SOItemDataObject data)
    {
        Debug.Log(data.ItemType);
        if(data.ItemType == ItemType.Card)
        {
            itemName = data.CardName;
            itemDescription = data.CardDescription;
            cardType = data.CardType;
            cardCategory = data.CardCategory;
            possibleChannels = data.PossibleChannels;
            affectedChannels = data.AffectedChannels;
            selectedChannels = Channels.None;

            energyCost = data.EnergyCost;
            baseDamage = data.BaseDamage;

            cardEffects = data.CardEffects;
        }

        if(data.ItemType == ItemType.Component)
        {
            componentType = data.ComponentType;
            componentImage = data.ComponentSprite;
            componentHP = data.ComponentHP;
            componentEnergy = data.ComponentEnergy;
            componentElement = data.ComponentElement;
            energyGainModifier = data.EnergyGainModifier;


            // is it needed to display?
            extraElementStacks = data.ExtraElementStacks;
            bonusDamageFromComponent = data.BonusDamageFromComponent;
            bonusDamageAsPercent = data.BonusDamageAsPercent;
            reduceDamageToComponent = data.ReduceDamageToComponent;
            reduceDamageAsPercent = data.ReduceDamageAsPercent;
            
        }

        this.currencyCost = data.CurrencyCost;
        this.chanceToSpawn = data.ChanceToSpawn;
        this.soItemDataObject = data;


    }




}
