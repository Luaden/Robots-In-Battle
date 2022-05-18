using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDataObject 
{
    private FighterPilotUIObject pilotUIObject;
    private PassiveEffects pilotPassiveEffects;
    private ActiveEffects pilotActiveEffects;
    private List<MechComponentDataObject> playerInventory;
    private FighterDataObject playerFighterData;
    private List<FighterDataObject> otherFighters;

    private SOCompleteCharacter completeCharacterBase;

    private int currentWinCount;
    private float timeLeftToSpend;
    private int currencyToSpend;

    public SOCompleteCharacter CompletePilot { get => completeCharacterBase; }
    public FighterPilotUIObject PilotCharacter { get => pilotUIObject; }
    public PassiveEffects PilotPassiveEffects { get => pilotPassiveEffects; }
    public ActiveEffects PilotActiveEffects { get => pilotActiveEffects; }
    public List<MechComponentDataObject> PlayerInventory { get => playerInventory; set => playerInventory = value; }
    public int CurrentWinCount { get => currentWinCount; set => currentWinCount = value; }
    public float TimeLeftToSpend { get => timeLeftToSpend; set => timeLeftToSpend = value; }
    public int CurrencyToSpend { get => currencyToSpend; set => currencyToSpend = value; }
    public FighterDataObject PlayerFighterData { get => playerFighterData; }
    public List<FighterDataObject> OtherFighters { get => otherFighters; set => otherFighters = value; }

    public PlayerDataObject()
    {
        playerInventory = new List<MechComponentDataObject>();
        otherFighters = new List<FighterDataObject>();
    }

    public PlayerDataObject(SOCompleteCharacter newPlayableCharacter)
    {
        pilotPassiveEffects = newPlayableCharacter.PilotPassiveEffects;
        pilotActiveEffects = newPlayableCharacter.PilotActiveEffcts;
        currencyToSpend = newPlayableCharacter.StartingCurrency;

        playerInventory = new List<MechComponentDataObject>();
        otherFighters = new List<FighterDataObject>();

        completeCharacterBase = newPlayableCharacter;
        pilotUIObject = new FighterPilotUIObject(completeCharacterBase.FighterPilotUIObject.FighterHair,
                                                        completeCharacterBase.FighterPilotUIObject.FighterEyes,
                                                        completeCharacterBase.FighterPilotUIObject.FighterNose,
                                                        completeCharacterBase.FighterPilotUIObject.FighterMouth,
                                                        completeCharacterBase.FighterPilotUIObject.FighterClothes,
                                                        completeCharacterBase.FighterPilotUIObject.FighterBody);


        playerFighterData = new FighterDataObject(newPlayableCharacter);
        playerFighterData.FighterUIObject = pilotUIObject;
        playerFighterData.FighterMech = GameManager.instance.PlayerMechController.BuildNewMech(newPlayableCharacter.MechObject);
        playerFighterData.FighterDeck = newPlayableCharacter.DeckList;
    }
}
