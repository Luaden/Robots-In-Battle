using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Events/Event")]
public class SOEventObject : ScriptableObject
{
    [SerializeField] private SOEventDialogueObject dialogue;
    [SerializeField] private EventType possibleAffirmationOutcomes;
    [SerializeField] private EventType possibleRejectionOutcomes;
}
