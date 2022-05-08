using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event Objective", menuName = "Events/New Event Objective")]
public class SOEventObjectiveObject : ScriptableObject
{
    [SerializeField] private EventObjective eventObjective;
    [SerializeField] private int eventObjectiveMagnitude;
    [SerializeField] private SOItemDataObject eventObjectiveRequiredComponent;

    public EventObjective EventObjective { get => eventObjective; }
    public int EventObjectiveMagnitude { get => eventObjectiveMagnitude; }
    public SOItemDataObject EventObjectiveRequiredComponent { get => eventObjectiveRequiredComponent; }
}
