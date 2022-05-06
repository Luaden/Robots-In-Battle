using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Module", menuName = "AI/Behavior Module")]
public class SOAIBehaviorObject : ScriptableObject
{
    [Tooltip("A value range between 0 and 9 that represents variance for AI priority preference. This value is added or subtracted from each possible move's " +
        "valuation in order to add variance from rigid strategy. 0 represents an accurate strategy of the weighted values below while 9 represents the most " +
        "added randomness. Chaos reigns.")]
    [Range(0, 9)] [SerializeField] private int randomizationWeight;
    [Tooltip("A value range between 0 and 9 that represents the AI preference for B Slot offense or defense. A value of 5 means the AI will value damaging " +
        "Neutral cards more than Defense cards.")]
    [Range(0, 9)] [SerializeField] private int aggressivenessWeight;

    [Tooltip("A value range between 0 and 9 that represents the AI preference for A Slot offense or defense. A value of 5 means the AI will value defensive " +
        "Neutral cards more than Attack cards.")]
    [Range(0, 9)] [SerializeField] private int defensivenessWeight;

    [Tooltip("A value range between 0 and 9 that represents the AI preference for base damage. A value of 5 means the AI will value attacks that deal more" +
    "base damage more than those that do less.")]
    [Range(0, 9)] [SerializeField] private int baseDamageWeight;

    [Tooltip("(Component Damage Multiplier) A value range between 0 and 9 that represents the AI preference for component damage. A value of 5 means the AI " +
        "will value attacks that deal or benefit from bonus component damage more than those that do not.")]
    [Range(0, 9)] [SerializeField] private int cDMWeight;

    [Tooltip("A value range between 0 and 9 that represents the AI preference for targeting weaker components. A value of 5 means that the AI will value attacks " +
        "that can target weaker components more.")]
    [Range(0, 9)] [SerializeField] private int componentHealthWeight;


    public int RandomizationWeight { get => randomizationWeight; }
    public int AggressivenessWeight { get => aggressivenessWeight; }
    public int DefensivenessWeight { get => defensivenessWeight; }
    public int BaseDamageWeight { get => baseDamageWeight; }
    public int CDMWeight { get => cDMWeight; }
    public int ComponentHealthWeight { get => componentHealthWeight; }
}
