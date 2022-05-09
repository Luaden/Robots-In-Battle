using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterDataObject
{
    private string fighterName;
    private PilotType fighterType;
    private Sprite fighterSprite;
    private Sprite fighterHair;
    private Sprite fighterHead;
    private Sprite fighterEyes;
    private Sprite fighterMouth;
    private Sprite fighterBody;
    private MechObject fighterMech;
    private List<SOItemDataObject> fighterDeck;
    private PassiveEffects fighterPassiveEffects;
    private ActiveEffects fighterActiveEffects;
    private int nodeIndex;

    private SOCompleteCharacter completeCharacterBase;

    public string FighterName { get => fighterName; }
    public PilotType FighterType { get => fighterType; }
    public Sprite FighterSprite { get => fighterSprite; }
    public PassiveEffects FighterPassiveEffects { get => fighterPassiveEffects; }
    public ActiveEffects FighterActiveEffects { get => fighterActiveEffects; }
    public MechObject FighterMech { get => fighterMech; }
    public List<SOItemDataObject> FighterDeck { get => fighterDeck; set => fighterDeck = value; }
    public SOAIDialogueObject AIDialogueModule { get => completeCharacterBase.DialogueModule; }
    public SOAIBehaviorObject AIBehaviorModule { get => completeCharacterBase.BehaviorModule; }

    public FighterDataObject(PlayerDataObject player)
    {
        fighterName = player.CompletePilot.PilotName;
        fighterSprite = player.PilotSprite;
        fighterMech = player.PlayerMech;
        fighterDeck = player.PlayerDeck;
        fighterPassiveEffects = player.PilotPassiveEffects;
        fighterActiveEffects = player.PilotActiveEffects;
        fighterType = PilotType.Unique;

        completeCharacterBase = player.CompletePilot; 
    }

    public FighterDataObject(SOCompleteCharacter opponent)
    {
        if (opponent.PilotType == PilotType.Unique)
            fighterSprite = opponent.PilotSprite;
        //else
        //{
        //    GameManager.instance.AICharacterBuilder.DoSomething();
        //}

        fighterName = opponent.PilotName;
        fighterMech = GameManager.instance.PlayerMechController.BuildNewMech(opponent.MechObject);
        fighterDeck = opponent.DeckList;
        fighterPassiveEffects = opponent.PilotPassiveEffects;
        fighterActiveEffects = opponent.PilotActiveEffcts;
        completeCharacterBase = opponent;
    }
}
