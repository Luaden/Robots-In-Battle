using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDataObject 
{
    private FighterCharacterObject pilotCharacter;
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
    public FighterCharacterObject PilotCharacter { get => pilotCharacter; }
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
    }

    public PlayerDataObject(SOCompleteCharacter newPlayableCharacter)
    {
        pilotCharacter = newPlayableCharacter.PilotCharacter;
        pilotPassiveEffects = newPlayableCharacter.PilotPassiveEffects;
        pilotActiveEffects = newPlayableCharacter.PilotActiveEffcts;
        currencyToSpend = newPlayableCharacter.StartingMoney;

        playerInventory = new List<MechComponentDataObject>();
        completeCharacterBase = newPlayableCharacter;

        playerFighterData = new FighterDataObject(newPlayableCharacter);
        playerFighterData.FighterMech = GameManager.instance.PlayerMechController.BuildNewMech(newPlayableCharacter.MechObject);
        playerFighterData.FighterDeck = newPlayableCharacter.DeckList;
    }
}
