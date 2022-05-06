using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Complete AI", menuName = "AI/Complete AI")]
public class SOAICompleteObject : ScriptableObject
{
    [SerializeField] private SOPilotEffectObject pilotEffects;
    [SerializeField] private SOAIDialogueObject aIDialogueModule;
    [SerializeField] private SOAIBehaviorObject behaviorModule;
    [SerializeField] private SOAIMechObject mechModule;

    public SOPilotEffectObject PilotEffects { get => pilotEffects; }
    public SOAIDialogueObject DialogueModule { get => aIDialogueModule; }
    public SOAIBehaviorObject BehaviorModule { get => behaviorModule; }
    public SOAIMechObject MechObject { get => mechModule; }
}
