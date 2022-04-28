using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataObject
{
    [Header("Card UI")]
    [SerializeField] private string cardName;
    [SerializeField] private string cardDescription;

    [Header("Card Attributes")]
    [SerializeField] private CardType cardType;
    [SerializeField] private CardCategory cardCategory;
    [SerializeField] private Channels possibleChannels;
    [SerializeField] private AffectedChannels affectedChannels;
    [SerializeField] private Channels selectedChannels;
    [SerializeField] private int energyCost;
    [SerializeField] private int baseDamage;

    [Header("Effect Attributes")]
    [SerializeField] private List<SOCardEffectObject> cardEffects;

    private GameObject cardUIObject;
    private CardUIController cardUIController;

    #region Base Card Properties
    public string CardName { get => cardName; }
    public string CardDescription { get => cardDescription; }
    public CardType CardType { get => cardType; }
    public CardCategory CardCategory { get => cardCategory; }
    public Channels PossibleChannels { get => possibleChannels; }
    public AffectedChannels AffectedChannels { get => affectedChannels; }
    public Channels SelectedChannels { get => selectedChannels; set => SelectChannel(value); }
    public int EnergyCost { get => energyCost; }
    public int BaseDamage { get => baseDamage; }
    public List<SOCardEffectObject> CardEffects { get => cardEffects; }
    #endregion

    #region Runtime Properties
    public GameObject CardUIObject { get => cardUIObject; set => UpdateCardUIObject(value); }
    public CardUIController CardUIController { get => cardUIController; }
    #endregion

    #region Constructor
    public CardDataObject(SOItemDataObject data)
    {
        cardName = data.CardName;
        cardDescription = data.CardDescription;
        
        cardType = data.CardType;
        cardCategory = data.CardCategory;
        possibleChannels = data.PossibleChannels;
        affectedChannels = data.AffectedChannels;
        selectedChannels = Channels.None;

        energyCost = data.EnergyCost;
        baseDamage = data.BaseDamage;

        cardEffects = data.CardEffects;
    }
    #endregion

    private void SelectChannel(Channels channel)
    {
        selectedChannels = channel;
        cardUIObject.GetComponent<CardUIController>().UpdateSelectedChannel(selectedChannels);
    }

    private void UpdateCardUIObject(GameObject cardUI)
    {
        cardUIObject = cardUI;
        cardUIController = cardUI.GetComponent<CardUIController>();
    }
}
