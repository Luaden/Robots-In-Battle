using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event Outcome", menuName = "Events/Event Outcome")]
public class SOEventOutcomeObject : ScriptableObject
{
    [SerializeField] private EventOutcome eventOutcome;
    [SerializeField] private int outcomeMagnitude;
    [SerializeField] private SOItemDataObject itemToAdd;

    public EventOutcome EventOutcome { get => eventOutcome; }
    public int OutcometMagnitude { get => outcomeMagnitude; }
    public SOItemDataObject ItemToAdd { get => itemToAdd; }
}
