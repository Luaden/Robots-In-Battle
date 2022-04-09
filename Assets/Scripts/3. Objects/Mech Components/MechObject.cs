using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechObject
{
    private int mechMaxHP;
    private int mechCurrentHP;
    private int mechMaxEnergy;
    private int mechCurrentEnergy;
    private MechHeadObject mechHead;
    private MechTorsoObject mechTorso;
    private MechArmsObject mechArms;
    private MechLegsObject mechLegs;

    public MechHeadObject MechHead { get => mechHead; }
    public MechTorsoObject MechTorso { get => mechTorso; }
    public MechArmsObject MechArms { get => mechArms; }
    public MechLegsObject MechLegs { get => mechLegs; }
    public int MechMaxHP { get => mechMaxHP; }
    public int MechMaxEnergy { get => mechMaxEnergy; }
    public int MechCurrentHP { get => mechCurrentHP; set => mechCurrentHP = value; }
    public int MechCurrentEnergy { get => mechCurrentEnergy; set => mechCurrentEnergy = value; }

    public MechObject(MechHeadObject head, MechTorsoObject torso, MechArmsObject arms, MechLegsObject legs, int maxHP, int maxEnergy)
    {
        mechHead = head;
        mechTorso = torso;
        mechArms = arms;
        mechLegs = legs;
        mechMaxHP = maxHP;
        mechMaxEnergy = maxEnergy;
        //This actually shouldn't reset like this when we implement downtime and repairs.
        mechCurrentHP = mechMaxHP;
        mechCurrentEnergy = mechMaxEnergy;
    }
}
