using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event Requirement", menuName = "Events/Event Requirement")]
public class SOEventRequirementObject : ScriptableObject
{
    [Tooltip("Condition to be met for event success.")]
    [SerializeField] private EventRequirement eventRequirement;
    [Tooltip("Magnitude of the condition to be met for event success.")]
    [SerializeField] int requirementMagnitude;

    public EventRequirement EventRequirement { get => eventRequirement; }
    public int RequirementMagnitude { get => requirementMagnitude; }
}
