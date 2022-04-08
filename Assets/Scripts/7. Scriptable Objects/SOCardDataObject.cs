using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/NewCard", order = 0)]
[System.Serializable]
public class SOCardDataObject : ScriptableObject
{
    [Header("Card UI")]
    [SerializeField] private string cardName;
    [SerializeField] private string cardDescription;
    [SerializeField] private Sprite cardImage;
    [SerializeField] private Sprite cardBackground;

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
    [Tooltip("Converts and treats Base Damage as a percent of the considered target. Attacks will deal percentage of health as damage, " +
    "defenses will nullify or reflect a percentage of the incoming damage.")]
    [SerializeField] private bool treatDamageAsPercent;
    [Tooltip("Damage dealt to components is based on base damage in combination with the Component Damage Multiplier. .5 deals 50% of Base Damage to components. " +
    "1.5 will deal base damage plus an additional 50%.")]
    [SerializeField] private int componentDamageMultiplier;

    [Header("Effect Attributes")]
    [SerializeField] private List<CardEffectObject> cardEffects;


    #region Card Attribute Properties
    public string CardName { get => cardName; }
    public string CardDescription { get => cardDescription; }
    public Sprite CardImage { get => cardImage; }
    public Sprite CardBackground { get => cardBackground; }
    public CardType CardType { get => cardType; }
    public CardCategory CardCategory { get => cardCategory; }
    public Channels PossibleChannels { get => possibleChannels; }
    public AffectedChannels AffectedChannels { get => affectedChannels; }
    public int EnergyCost { get => energyCost; }
    public int BaseDamage { get => baseDamage; }
    #endregion

    #region Effects
    public List<CardEffectObject> CardEffects { get => cardEffects; }
    #endregion
}