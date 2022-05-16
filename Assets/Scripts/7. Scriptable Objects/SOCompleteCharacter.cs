using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Pilots/Complete Character")]
public class SOCompleteCharacter : ScriptableObject
{
    [SerializeField] private string pilotName;
    [SerializeField] private PilotType pilotType;
    [SerializeField] private PassiveEffects pilotPassiveEffects;
    [SerializeField] private ActiveEffects pilotActiveEffects;
    [SerializeField] private FighterCharacterObject pilotCharacter;
    [SerializeField] private SOAIDialogueObject dialogueModule;
    [SerializeField] private SOAIBehaviorObject behaviorModule;
    [SerializeField] private List<SOItemDataObject> deckList;
    [SerializeField] private SOMechObject mechModule;
    [SerializeField] private int startingMoney;

    public string PilotName { get => pilotName; }
    public PilotType PilotType { get => pilotType; }
    public FighterCharacterObject PilotCharacter { get => pilotCharacter; }
    public PassiveEffects PilotPassiveEffects { get => pilotPassiveEffects; }
    public ActiveEffects PilotActiveEffcts { get => pilotActiveEffects; }
    public SOAIDialogueObject DialogueModule { get => dialogueModule; }
    public SOAIBehaviorObject BehaviorModule { get => behaviorModule; }
    public List<SOItemDataObject> DeckList { get => deckList; }
    public SOMechObject MechObject { get => mechModule; }
    public int StartingMoney { get => startingMoney; }

}
