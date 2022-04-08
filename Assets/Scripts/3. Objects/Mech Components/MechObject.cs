using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechObject
{
    private int mechMaxHP;
    private int mechCurrentHP;
    private MechHeadObject mechHead;
    private MechTorsoObject mechTorso;
    private MechArmsObject mechArms;
    private MechLegsObject mechLegs;

    public MechHeadObject MechHead { get => mechHead; }
    public MechTorsoObject MechTorso { get => mechTorso; }
    public MechArmsObject MechArms { get => mechArms; }
    public MechLegsObject MechLegs { get => mechLegs; }
    public int MechCurrentHP { get => mechCurrentHP; set => mechCurrentHP = value; }
    public int MechMaxHP { get => mechMaxHP; }

    public MechObject(MechHeadObject head, MechTorsoObject torso, MechArmsObject arms, MechLegsObject legs, int maxHP)
    {
        mechHead = head;
        mechTorso = torso;
        mechArms = arms;
        mechLegs = legs;
        mechMaxHP = maxHP;
        mechCurrentHP = mechMaxHP;
    }
}
