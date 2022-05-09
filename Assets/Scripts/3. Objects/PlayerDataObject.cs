using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDataObject 
{
    private Sprite pilotSprite;
    private PassiveEffects pilotPassiveEffects;
    private ActiveEffects pilotActiveEffects;
    private List<SOItemDataObject> playerDeck;
    private MechObject playerMech;
    private List<MechComponentDataObject> playerInventory;
    private List<FighterDataObject> otherFighters;

    private SOCompleteCharacter completeCharacterBase;

    private int currentWinCount;
    private float timeLeftToSpend;
    private int currencyToSpend;

    public SOCompleteCharacter CompletePilot { get => completeCharacterBase; }
    public Sprite PilotSprite { get => pilotSprite; }
    public PassiveEffects PilotPassiveEffects { get => pilotPassiveEffects; }
    public ActiveEffects PilotActiveEffects { get => pilotActiveEffects; }

    public MechObject PlayerMech { get => playerMech; set => playerMech = value; }
    public List<SOItemDataObject> PlayerDeck { get => playerDeck; set => playerDeck = value; }
    public List<MechComponentDataObject> PlayerInventory { get => playerInventory; set => playerInventory = value; }

    public int CurrentWinCount { get => currentWinCount; set => currentWinCount = value; }
    public float TimeLeftToSpend { get => timeLeftToSpend; set => timeLeftToSpend = value; }
    public int CurrencyToSpend { get => currencyToSpend; set => currencyToSpend = value; }

    public List<FighterDataObject> OtherFighters { get => otherFighters; set => otherFighters = value; }

    public PlayerDataObject()
    {
        playerInventory = new List<MechComponentDataObject>();
        playerDeck = new List<SOItemDataObject>();
    }

    public PlayerDataObject(SOCompleteCharacter newPlayableCharacter)
    {
        pilotSprite = newPlayableCharacter.PilotSprite;
        pilotPassiveEffects = newPlayableCharacter.PilotPassiveEffects;
        pilotActiveEffects = newPlayableCharacter.PilotActiveEffcts;
        currencyToSpend = newPlayableCharacter.StartingMoney;

        playerInventory = new List<MechComponentDataObject>();
        playerDeck = new List<SOItemDataObject>();

        foreach (SOItemDataObject item in newPlayableCharacter.DeckList)
            playerDeck.Add(item);

        playerMech = GameManager.instance.PlayerMechController.BuildNewMech(newPlayableCharacter.MechObject);

        completeCharacterBase = newPlayableCharacter;
    }
}
