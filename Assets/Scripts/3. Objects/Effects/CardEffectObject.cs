using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardEffectObject 
{
    [SerializeField] private CardEffectTypes cardEffect;
    [SerializeField] private EffectTarget effectTarget;
    [SerializeField] private int effectMagnitude;
    [SerializeField] private int effectDuration;


    public CardEffectTypes EffectType { get => cardEffect; }
    public EffectTarget EffectTarget { get => effectTarget; }
    public int EffectMagnitude { get => effectMagnitude; }
    public int EffectDuration { get => effectDuration; }
}
