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
    [SerializeField] private AttackType attackType;
    [Tooltip("Defenses handle incoming damage in the selected channel(s). Guard reduces incoming damage, while Counter will nullify the incoming damage " +
    "and reflect the amount detailed below.")]
    [SerializeField] private DefenseType defenseType;
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

    [Header("Effect Attributes")]
    [SerializeField] private List<CardEffectObject> cardEffects;


    #region Card Attribute Properties
    public string CardName { get => cardName; }
    public string CardDescription { get => cardDescription; }
    public Sprite CardImage { get => cardImage; }
    public Sprite CardBackground { get => cardBackground; }
    public CardType CardType { get => cardType; }
    public AttackType AttackType { get => attackType; }
    public DefenseType DefenseType { get => defenseType; }
    public Channels PossibleChannels { get => possibleChannels; }
    public AffectedChannels AffectedChannels { get => affectedChannels; }
    public int EnergyCost { get => energyCost; }
    public int BaseDamage { get => baseDamage; }
    #endregion

    #region Effects
    public List<CardEffectObject> CardEffects { get => cardEffects; }
    #endregion
}