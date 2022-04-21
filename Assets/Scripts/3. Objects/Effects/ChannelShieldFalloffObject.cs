using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelShieldFalloffObject
{
    private int startingShieldPerTurn;
    private int fallOffPerTurn;

    public int StartingShieldPerTurn { get => startingShieldPerTurn; set => startingShieldPerTurn = value; }
    public int FalloffPerTurn { get => fallOffPerTurn; set => fallOffPerTurn = value; }
}
