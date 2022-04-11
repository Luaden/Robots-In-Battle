using UnityEngine;

[System.Serializable]
public class CardEffectObject
{
    private CardEffectTypes cardEffect;
    private CardCategory cardCategoryToBoost;
    private CardKeyWord cardKeyWord;
    private int effectMagnitude;
    private int effectDuration;
    private int currentTurn = 0;


    public CardEffectTypes EffectType { get => cardEffect; }
    public CardCategory CardTypeToBoost { get => cardCategoryToBoost; }
    public int EffectMagnitude { get => effectMagnitude; set => effectMagnitude = value; }
    public int EffectDuration { get => effectDuration; set => effectDuration = value; }
    public int CurrentTurn { get => currentTurn; set => currentTurn = value; }

    public CardEffectObject(SOCardEffectObject sOCardEffect)
    {
        cardEffect = sOCardEffect.EffectType;
        cardCategoryToBoost = sOCardEffect.CardTypeToBoost;
        cardKeyWord = sOCardEffect.CardKeyWord;
        effectMagnitude = sOCardEffect.EffectMagnitude;
        effectDuration = sOCardEffect.EffectDuration;
    }
}
