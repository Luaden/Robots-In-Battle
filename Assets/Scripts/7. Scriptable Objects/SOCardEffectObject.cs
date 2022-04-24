using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

[CreateAssetMenu(fileName = "NewCardEffect", menuName = "Card Effects/New Card Effect", order = 0)]
public class SOCardEffectObject : ScriptableObject
{
    [Header("General Effect Attributes")]
    [SerializeField] private CardEffectTypes effectType;
    [SerializeField] private CardCategory cardTypeToBoost = CardCategory.None;
    [SerializeField] private CardKeyWord cardKeyWord = CardKeyWord.None;
    [SerializeField] private int effectMagnitude = 0;
    [SerializeField] private int effectDuration = 0;
    [SerializeField] private int fallOffPerTurn = 1;
    
    #region EditorGUI
    [SerializeField] private bool showAllStats;
    #endregion

    public CardEffectTypes EffectType { get => effectType; }
    public CardCategory CardTypeToBoost { get => cardTypeToBoost; }
    public CardKeyWord CardKeyWord { get => cardKeyWord; }
    public int EffectMagnitude { get => effectMagnitude; }
    public int EffectDuration { get => effectDuration; }
    public int EffectFallOffPerTurn { get => fallOffPerTurn; }
}
