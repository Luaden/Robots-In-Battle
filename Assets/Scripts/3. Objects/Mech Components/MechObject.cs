using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechObject
{
    private int mechTotalHP;
    private MechHeadObject mechHead;
    private MechTorsoObject mechTorso;
    private MechArmsObject mechArms;
    private MechLegsObject mechLegs;

    public MechHeadObject MechHead { get => mechHead; }
    public MechTorsoObject MechTorso { get => mechTorso; }
    public MechArmsObject MechArms { get => mechArms; }
    public MechLegsObject MechLegs { get => mechLegs; }
}
