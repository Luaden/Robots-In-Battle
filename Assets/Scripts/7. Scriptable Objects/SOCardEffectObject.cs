using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardEffect", menuName = "Card Effects/New Card Effect", order = 0)]
public class SOCardEffectObject : ScriptableObject
{
    [SerializeField] private CardEffectTypes cardEffect;
    [SerializeField] private CardCategory cardTypeToBoost = CardCategory.None;
    [SerializeField] private CardKeyWord cardKeyWord = CardKeyWord.None;
    [SerializeField] private int effectMagnitude = 0;
    [SerializeField] private int effectDuration = 0;


    public CardEffectTypes EffectType { get => cardEffect; }
    public CardCategory CardTypeToBoost { get => cardTypeToBoost; }
    public CardKeyWord CardKeyWord { get => cardKeyWord; }
    public int EffectMagnitude { get => effectMagnitude; }
    public int EffectDuration { get => effectDuration; }

}
