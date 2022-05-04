using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterDataObject
{
    private MechObject fighterMech;
    private List<SOItemDataObject> fighterDeck;
    private PilotDataObject fighterPilot;

    public MechObject FighterMech { get => fighterMech; }
    public List<SOItemDataObject> FighterDeck { get => fighterDeck; set => fighterDeck = value; }
    public PilotDataObject FighterPilot { get => fighterPilot; }

    public FighterDataObject(MechObject mech, PilotDataObject pilot, List<SOItemDataObject> deck)
    {
        fighterMech = mech;
        fighterPilot = pilot;
        fighterDeck = deck;        
    }
}
