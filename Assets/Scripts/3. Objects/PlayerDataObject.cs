using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDataObject
{
    private List<SOItemDataObject> playerDeck;
    private MechObject playerMech;
    private List<MechComponentDataObject> playerInventory;
    //We need to store the win/loss record here as well maybe?

    private int currentWinCount;
    private float timeLeftToSpend;
    private int currencyToSpend;

    public PlayerDataObject()
    {
        playerInventory = new List<MechComponentDataObject>();
        playerDeck = new List<SOItemDataObject>();
    }
    public PlayerDataObject(int startCurrency, float startTimeToSpend)
    {
        currencyToSpend = startCurrency;
        timeLeftToSpend = startTimeToSpend;

        playerInventory = new List<MechComponentDataObject>();
        playerDeck = new List<SOItemDataObject>();
    }

    public int CurrentWinCount { get => currentWinCount; set => currentWinCount = value; }
    public float TimeLeftToSpend { get => timeLeftToSpend; set => timeLeftToSpend = value; }
    public int CurrencyToSpend { get => currencyToSpend; set => currencyToSpend = value; }
    public MechObject PlayerMech { get => playerMech; set => playerMech = value; }

    public List<SOItemDataObject> PlayerDeck { get => playerDeck; set => playerDeck = value; }
    public List<MechComponentDataObject> PlayerInventory { get => playerInventory; set => playerInventory = value; }

}
