using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterDataObject
{
    private string fighterName;
    private FighterCharacterObject fighterCharacter;
    private MechObject fighterMech;
    private List<SOItemDataObject> fighterDeck;
    private PassiveEffects fighterPassiveEffects;
    private ActiveEffects fighterActiveEffects;
    private int nodeIndex;

    private SOCompleteCharacter completeCharacterBase;

    public string FighterName { get => fighterName; }
    public FighterCharacterObject FighterCharacter { get => fighterCharacter; set => fighterCharacter = value; }
    public PassiveEffects FighterPassiveEffects { get => fighterPassiveEffects; }
    public ActiveEffects FighterActiveEffects { get => fighterActiveEffects; }
    public MechObject FighterMech { get => fighterMech; set => fighterMech = value; }
    public List<SOItemDataObject> FighterDeck { get => fighterDeck; set => fighterDeck = value; }
    public SOAIDialogueObject AIDialogueModule { get => completeCharacterBase.DialogueModule; }
    public SOAIBehaviorObject AIBehaviorModule { get => completeCharacterBase.BehaviorModule; }
    public int FighterNodeIndex { get => nodeIndex; set => nodeIndex = value; }

    public FighterDataObject(PlayerDataObject player)
    {
        fighterName = player.CompletePilot.PilotName;
        fighterCharacter = player.PilotCharacter;
        fighterPassiveEffects = player.PilotPassiveEffects;
        fighterActiveEffects = player.PilotActiveEffects;

        completeCharacterBase = player.CompletePilot; 
    }

    public FighterDataObject(SOCompleteCharacter opponent)
    {
        fighterCharacter = opponent.PilotCharacter;
        fighterName = opponent.PilotName;
        fighterMech = GameManager.instance.PlayerMechController.BuildNewMech(opponent.MechObject);
        fighterDeck = opponent.DeckList;
        fighterPassiveEffects = opponent.PilotPassiveEffects;
        fighterActiveEffects = opponent.PilotActiveEffcts;
        completeCharacterBase = opponent;
    }
}
