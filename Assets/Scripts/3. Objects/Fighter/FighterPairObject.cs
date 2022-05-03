using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterPairObject
{
    private FighterDataObject fighterA; // contains previous node?
    private FighterDataObject fighterB;
    // node to previous, next

    public FighterDataObject FighterA { get => fighterA; }
    public FighterDataObject FighterB { get => fighterB; }

    public FighterPairObject(FighterDataObject fighterA, FighterDataObject fighterB)
    {
        this.fighterA = fighterA;
        this.fighterB = fighterB;
    }

}
