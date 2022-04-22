using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDataObject : MonoBehaviour
{
    private List<CardDataObject> playerDeck;
    private MechObject playerMech;
    private List<MechComponentDataObject> playerInventory;
    //We need to save map data here as well as the position of the player on the map.
    //We need to store the win/loss record here as well maybe?

    private float timeLeftToSpend;
    private float currencyToSpend;

    public float TimeLeftToSpend { get => timeLeftToSpend; set => timeLeftToSpend = value; }
    public float CurrencyToSpend { get => currencyToSpend; set => currencyToSpend = value; }
    public List<CardDataObject> PlayerDeck { get => playerDeck; set => playerDeck = value; }
    public MechObject PlayerMech { get => playerMech; }//set => PlayerMechComponentCheck(value);  }
    public List<MechComponentDataObject> PlayerInventory { get => playerInventory; set => playerInventory = value; }

    private void PlayerMechComponentCheck(List<MechComponentDataObject> mechParts)
    {
        //Checks if we are sent the correct number of parts. We currently only NEED 4, but we have the possibility for 5. This would include
        //a back component, but it is still currently in the design process.
        if (mechParts.Count > 5)
        {
            Debug.LogError("Too many components were sent to the Player Mech.");
            return;
        }
        if(mechParts.Count < 4)
        {
            Debug.LogError("Too few components were sent to the Player Mech.");
            return;
        }

        if(!mechParts.Select(x => x.ComponentType).Contains(MechComponent.Head))
        {
            Debug.LogError("Player Mech invalid. Player was sent a Mech without a Head.");
            return;
        }

        if (!mechParts.Select(x => x.ComponentType).Contains(MechComponent.Torso))
        {
            Debug.LogError("Player Mech invalid. Player was sent a Mech without a Torso.");
            return;
        }

        if (!mechParts.Select(x => x.ComponentType).Contains(MechComponent.Arms))
        {
            Debug.LogError("Player Mech invalid. Player was sent a Mech without Arms.");
            return;
        }

        if (!mechParts.Select(x => x.ComponentType).Contains(MechComponent.Legs))
        {
            Debug.LogError("Player Mech invalid. Player was sent a Mech without a Legs.");
            return;
        }

        //playerMech = mechParts;
    }
}
