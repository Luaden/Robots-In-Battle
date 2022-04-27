using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDataObject
{
    private List<SOItemDataObject> playerDeck;
    private MechObject playerMech;
    private List<MechComponentDataObject> playerInventory;
    //We need to save map data here as well as the position of the player on the map.
    //We need to store the win/loss record here as well maybe?

    private float timeLeftToSpend;
    private int currencyToSpend;

    public PlayerDataObject()
    {
        playerInventory = new List<MechComponentDataObject>();
        playerDeck = new List<SOItemDataObject>();
    }

    public float TimeLeftToSpend { get => timeLeftToSpend; set => timeLeftToSpend = value; }
    public int CurrencyToSpend { get => currencyToSpend; set => currencyToSpend = value; }
    public List<SOItemDataObject> PlayerDeck { get => playerDeck; set => playerDeck = value; }
    public MechObject PlayerMech { get => playerMech; set => playerMech = value; }
    public List<MechComponentDataObject> PlayerInventory { get => playerInventory; set => playerInventory = value; }

}
