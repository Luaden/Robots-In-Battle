using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterDataObject
{
    private MechObject fighterMech;
    private List<CardDataObject> fighterDeck;
    private PilotDataObject fighterPilot;

    private int damageModifier;
    private int resistanceModifier;

    public MechObject FighterMech { get => fighterMech; }
    public List<CardDataObject> FighterDeck { get => fighterDeck; set => fighterDeck = value; }
    public PilotDataObject FighterPilot { get => fighterPilot; }

    public FighterDataObject(MechObject mech, PilotDataObject pilot, List<CardDataObject> deck)
    {
        fighterMech = mech;
        fighterPilot = pilot;
        fighterDeck = deck;        
    }
}
