using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterPairObject
{
    private FighterDataObject fighterA;
    private FighterDataObject fighterB;

    public FighterDataObject FighterA { get => fighterA; }
    public FighterDataObject FighterB { get => fighterB; }

    public FighterPairObject(FighterDataObject fighterA, FighterDataObject fighterB)
    {
        this.fighterA = fighterA;
        this.fighterB = fighterB;
    }

}
