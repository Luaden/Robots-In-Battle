using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Cards or Components/New Item", order = 0)]
[System.Serializable]
public class SOItemDataObject : ScriptableObject
{
    [Header("Item Attributes")]
    [SerializeField] private ItemType itemType;
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private Sprite itemShopImage;
    [Tooltip("Time cost to implement the item into the player mech or deck.")]
    [SerializeField] private float timeCost;
    [Tooltip("Cost of the item in the shop.")]
    [SerializeField] private int currencyCost;
    [Tooltip("This is the chance for this card to spawn at the shop.")]
    [Range(1, 100)] [SerializeField] private int chanceToSpawn;
    
    [Header("Base Component Attributes")]
    [SerializeField] private MechComponent componentType;
    [Tooltip("Exact name of the part as it is on the mech. This is the primary component or the closest component to the mech. " +
        "It is the Mech's right, if applicable.")]
    [SerializeField] private string primaryComponentSpriteID;
    [Tooltip("Exact name of the part as it is on the mech. This is the component links to the Component Sprite ID. " +
        "It is the Mech's right, if applicable.")]
    [SerializeField] private string secondaryComponentSpriteID;
    [Tooltip("Exact name of the part as it is on the mech. This is the component links to the Connection Component Sprite ID. " +
        "It is the Mech's right, if applicable.")]
    [SerializeField] private string tertiaryComponentSpriteID;
    [Tooltip("Exact name of the part as it is on the mech. This is the primary component or the closest component to the mech. " +
        "It is the Mech's left, if applicable.")]
    [SerializeField] private string altPrimaryComponentSpriteID;
    [Tooltip("Exact name of the part as it is on the mech. This is the component links to the Component Sprite ID. " +
        "It is the Mech's left, if applicable.")]
    [SerializeField] private string altSecondaryComponentSpriteID;
    [Tooltip("Exact name of the part as it is on the mech. This is the component links to the Connection Component Sprite ID. " +
        "It is the Mech's left, if applicable.")]
    [SerializeField] private string altTertiaryComponentSpriteID;
    [SerializeField] private int componentHP;
    [SerializeField] private int componentEnergy;
    [Tooltip("Component applies one stack of this element when an attack is used that utilizes this component.")]
    [SerializeField] private ElementType componentElement;
    [Tooltip("Component Damage Multiplier adds bonus component damage to an attack that utilizes this component.")]
    [SerializeField] private float cDMFromComponent;
    [Tooltip("Component Damage Multiplier reduces damage from an attack that targets this component.")]
    [SerializeField] private float cDMToComponent;
    [Tooltip("Gives bonus element stacks to an attack that utilizes this component. This includes element stacks from cards as well as this component.")]
    [SerializeField] private int extraElementStacks;
    [Tooltip("Increases overall energy gained at the start of the turn.")]
    [SerializeField] private int energyGainModifier;

    [Header("Card Attributes")]
    [SerializeField] private CardType cardType;
    [Tooltip("Offenses deal damage in the selected channel(s). Punch, Kick, and Special will be affected by their respective Mech Component counterparts." +
    " Defenses handle incoming damage in the selected channel(s). Guard reduces incoming damage, while Counter will nullify the incoming damage " +
    "and reflect the amount detailed below.")]
    [SerializeField] private CardCategory cardCategory;
    [Tooltip("Channels this card may be played in.")]
    [SerializeField] private Channels possibleChannels;
    [Tooltip("Determines whether the card is played in one of the possible channels or all of the possible channels.")]
    [SerializeField] private AffectedChannels affectedChannels;
    [SerializeField] private int energyCost;
    [Tooltip("Attacks treat this as damage to deal. Defenses treat this as damage to nullify.")]
    [SerializeField] private int baseDamage;
    [Tooltip("Applies the effect before damage or interaction with the opponent. E.g. Shields will be applied before combat vs after.")]
    [SerializeField] private bool applyEffectsFirst = false;

    [Header("Effect Attributes")]
    [SerializeField] private List<SOCardEffectObject> cardEffects;

    #region Card Attribute Properties
    public string CardName { get => itemName; }
    public string CardDescription { get => itemDescription; }
    public CardType CardType { get => cardType; }
    public CardCategory CardCategory { get => cardCategory; }
    public Channels PossibleChannels { get => possibleChannels; }
    public AffectedChannels AffectedChannels { get => affectedChannels; }
    public int EnergyCost { get => energyCost; }
    public int BaseDamage { get => baseDamage; }
    public bool ApplyEffectsFirst { get => applyEffectsFirst; }
    #endregion

    #region Component Attribute Properties
    public string ComponentName { get => itemName; }
    public MechComponent ComponentType { get => componentType; }
    public Sprite ItemShopSprite { get => itemShopImage; }
    public string PrimaryComponentSpriteID { get => primaryComponentSpriteID; }
    public string SecondaryComponentSpriteID { get => secondaryComponentSpriteID; }
    public string TertiaryComponentID { get => tertiaryComponentSpriteID; }
    public string AltPrimaryComponentSpriteID { get => altPrimaryComponentSpriteID; }
    public string AltSecondaryComponentSpriteID { get => altSecondaryComponentSpriteID; }
    public string AltTertiaryComponentSpriteID { get => altTertiaryComponentSpriteID; }
    public int ComponentHP { get => componentHP; }
    public int ComponentEnergy { get => componentEnergy; }
    public ElementType ComponentElement { get => componentElement; }
    public float CDMFromComponent { get => cDMFromComponent; }
    public float CDMToComponent { get => cDMToComponent; }
    public int ExtraElementStacks { get => extraElementStacks; }
    public int EnergyGainModifier { get => energyGainModifier; }
    #endregion

    #region Item Attribute Properties
    public ItemType ItemType { get => itemType; }
    public string ItemName { get => itemName; }
    public string ItemDescription { get => itemDescription; }
    public Sprite ItemShopImage { get => itemShopImage; }
    public float TimeCost { get => timeCost; }
    public int CurrencyCost { get => currencyCost; }
    public int ChanceToSpawn { get => chanceToSpawn; }

    #endregion

    #region Effects
    public List<SOCardEffectObject> CardEffects { get => cardEffects; }
    #endregion
}