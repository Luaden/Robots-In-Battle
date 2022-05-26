using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataObject
{
    [Header("Card UI")]
    private string cardName;
    private string cardDescription;

    [Header("Card Attributes")]
    private CardType cardType;
    private CardCategory cardCategory;
    private AnimationType animationType;
    private Channels possibleChannels;
    private AffectedChannels affectedChannels;
    private Channels selectedChannels;
    private int energyCost;
    private int baseDamage;
    private bool applyEffectsFirst = false;

    [Header("Effect Attributes")]
    private List<SOCardEffectObject> cardEffects;

    private SOItemDataObject sOItemDataObject;
    private GameObject cardUIObject;
    private CardUIController cardUIController;

    #region Base Card Properties
    public string CardName { get => cardName; }
    public string CardDescription { get => cardDescription; }
    public CardType CardType { get => cardType; }
    public CardCategory CardCategory { get => cardCategory; }
    public AnimationType AnimationType { get => animationType; }
    public Channels PossibleChannels { get => possibleChannels; }
    public AffectedChannels AffectedChannels { get => affectedChannels; }
    public Channels SelectedChannels { get => selectedChannels; set => SelectChannel(value); }
    public int EnergyCost { get => energyCost; }
    public int BaseDamage { get => baseDamage; }
    public bool ApplyEffectsFirst { get => applyEffectsFirst; }
    public List<SOCardEffectObject> CardEffects { get => cardEffects; }
    public SOItemDataObject SOItemDataObject { get => sOItemDataObject; }
    #endregion

    #region Runtime Properties
    public GameObject CardUIObject { get => cardUIObject; set => UpdateCardUIObject(value); }
    public CardUIController CardUIController { get => cardUIController; }
    #endregion

    #region Constructor
    public CardDataObject(SOItemDataObject data)
    {
        sOItemDataObject = data;
        cardName = data.CardName;
        cardDescription = data.CardDescription;
        
        cardType = data.CardType;
        cardCategory = data.CardCategory;
        possibleChannels = data.PossibleChannels;
        affectedChannels = data.AffectedChannels;
        selectedChannels = Channels.None;

        energyCost = data.EnergyCost;
        baseDamage = data.BaseDamage;
        applyEffectsFirst = data.ApplyEffectsFirst;

        cardEffects = data.CardEffects;
    }
    #endregion

    private void SelectChannel(Channels channel)
    {
        selectedChannels = channel;
        cardUIObject.GetComponent<CardUIController>().UpdateSelectedChannel(selectedChannels);

        switch (cardCategory)
        {
            case CardCategory.None:
                Debug.Log(cardName + " wasn't flagged for a channel? SUPER weird.");
                break;
            case CardCategory.Punch:
                if(channel == Channels.High)
                    animationType = AnimationType.PunchHigh;
                if (channel == Channels.Mid)
                    animationType = AnimationType.PunchMid;
                break;
            case CardCategory.Kick:
                if (channel == Channels.Mid)
                    animationType = AnimationType.KickMid;
                if (channel == Channels.Low)
                    animationType = AnimationType.KickLow;
                break;
            case CardCategory.Special:
                if (channel == Channels.Mid)
                    animationType = AnimationType.SpecialMid;
                break;
            case CardCategory.Guard:
                animationType = AnimationType.Guard;
                break;
            case CardCategory.Counter:
                animationType = AnimationType.Counter;
                break;
            case CardCategory.Offensive:
                Debug.Log(cardName + " was flagged for an incorrect channel.");
                break;
            case CardCategory.Defensive:
                Debug.Log(cardName + " was flagged for an incorrect channel.");
                break;
            case CardCategory.All:
                Debug.Log(cardName + " was flagged for an incorrect channel.");
                break;
        }
    }

    private void UpdateCardUIObject(GameObject cardUI)
    {
        cardUIObject = cardUI;
        cardUIController = cardUI.GetComponent<CardUIController>();
    }
}
