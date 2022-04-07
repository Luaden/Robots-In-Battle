using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataObject
{
    [Header("Card UI")]
    [SerializeField] private string cardName;
    [SerializeField] private string cardDescription;
    [SerializeField] private Sprite cardImage;
    [SerializeField] private Sprite cardBackground;

    [Header("Card Attributes")]
    [SerializeField] private CardType cardType;
    [SerializeField] private AttackType attackType;
    [SerializeField] private DefenseType defenseType;
    [SerializeField] private Channels possibleChannels;
    [SerializeField] private AffectedChannels affectedChannels;
    [SerializeField] private Channels selectedChannels;
    [SerializeField] private int energyCost;
    [SerializeField] private int baseDamage;

    [Header("Effect Attributes")]
    [SerializeField] private List<CardEffectObject> cardEffects;

    private GameObject cardUIObject;

    #region Base Card Properties
    public string CardName { get => cardName; }
    public string CardDescription { get => cardDescription; }
    public Sprite CardForeground { get => cardImage; }
    public Sprite CardBackground { get => cardBackground; }
    public CardType CardType { get => cardType; }
    public AttackType AttackType { get => attackType; }
    public DefenseType DefenseType { get => defenseType; }
    public Channels PossibleChannels { get => possibleChannels; }
    public AffectedChannels AffectedChannels { get => affectedChannels; }
    public Channels SelectedChannels { get => selectedChannels; set => selectedChannels = value; }
    public int EnergyCost { get => energyCost; }
    public int BaseDamage { get => baseDamage; }
    public List<CardEffectObject> CardEffects { get => cardEffects; }
    #endregion

    #region Runtime Properties
    public GameObject CardUIObject { get => cardUIObject; set => cardUIObject = value; }
    #endregion

    #region Constructor
    public CardDataObject(SOCardDataObject data)
    {
        cardName = data.CardName;
        cardDescription = data.CardDescription;
        cardImage = data.CardImage;
        cardBackground = data.CardBackground;
        
        cardType = data.CardType;
        attackType = data.AttackType;
        defenseType = data.DefenseType;
        possibleChannels = data.PossibleChannels;
        affectedChannels = data.AffectedChannels;
        selectedChannels = Channels.None;

        energyCost = data.EnergyCost;
        baseDamage = data.BaseDamage;

        cardEffects = data.CardEffects;
    }
    #endregion
}
