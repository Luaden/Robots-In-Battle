using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterDataObject
{
    private MechObject fighterMech;
    private List<SOCardDataObject> fighterDeck;
    private PilotDataObject fighterPilot;

    private int damageModifier;
    private int resistanceModifier;

    public MechObject FighterMech { get => fighterMech; }
    public List<SOCardDataObject> FighterDeck { get => fighterDeck; }
    public PilotDataObject FighterPilot { get => fighterPilot; }

    public FighterDataObject(MechObject mech, PilotDataObject pilot, List<SOCardDataObject> deck)
    {
        fighterMech = mech;
        fighterPilot = pilot;
        fighterDeck = deck;        
    }
}
