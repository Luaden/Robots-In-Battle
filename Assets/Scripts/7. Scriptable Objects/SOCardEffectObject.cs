using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOCardEffectObject : ScriptableObject
{
    [SerializeField] private CardEffectTypes cardEffect;
    [SerializeField] private CardCategory cardTypeToBoost;
    [SerializeField] private CardKeyWord cardKeyWord;
    [SerializeField] private int effectMagnitude;
    [SerializeField] private int effectDuration;


    public CardEffectTypes EffectType { get => cardEffect; }
    public CardCategory CardTypeToBoost { get => cardTypeToBoost; }
    public CardKeyWord CardKeyWord { get => cardKeyWord; }
    public int EffectMagnitude { get => effectMagnitude; }
    public int EffectDuration { get => effectDuration; }

}
