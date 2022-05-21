using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterDataObject
{
    private string fighterName;
    private MechObject fighterMech;
    private AudioClip fighterDialogueSound;
    private List<SOItemDataObject> fighterDeck;
    private PassiveEffects fighterPassiveEffects;
    private ActiveEffects fighterActiveEffects;
    private int nodeIndex;

    private SOCompleteCharacter completeCharacterBase;
    private FighterPilotUIObject fighterPilotUIObject;

    public string FighterName { get => fighterName; }
    public FighterPilotUIObject FighterUIObject { get => fighterPilotUIObject; set => fighterPilotUIObject = value; }
    public SOCompleteCharacter FighterCompleteCharacter { get => completeCharacterBase; }
    public PassiveEffects FighterPassiveEffects { get => fighterPassiveEffects; }
    public ActiveEffects FighterActiveEffects { get => fighterActiveEffects; }
    public MechObject FighterMech { get => fighterMech; set => fighterMech = value; }
    public AudioClip FighterDialogueSound { get => fighterDialogueSound; set => fighterDialogueSound = value; }
    public List<SOItemDataObject> FighterDeck { get => fighterDeck; set => fighterDeck = value; }
    public SOAIDialogueObject AIDialogueModule { get => completeCharacterBase.DialogueModule; }
    public SOAIBehaviorObject AIBehaviorModule { get => completeCharacterBase.BehaviorModule; }
    public int FighterNodeIndex { get => nodeIndex; set => nodeIndex = value; }

    public FighterDataObject(SOCompleteCharacter opponent)
    {
        fighterName = opponent.PilotName;
        fighterMech = GameManager.instance.PlayerMechController.BuildNewMech(opponent.MechObject);
        fighterDeck = opponent.DeckList;
        fighterPassiveEffects = opponent.PilotPassiveEffects;
        fighterActiveEffects = opponent.PilotActiveEffcts;
        fighterDialogueSound = opponent.DialogueSound;
        completeCharacterBase = opponent;
    }
}
