using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechObject
{
    [SerializeField] private MechHeadObject mechHead;
    [SerializeField] private MechTorsoObject mechTorso;
    [SerializeField] private MechArmsObject mechArms;
    [SerializeField] private MechLegsObject mechLegs;
    [SerializeField] private int mechTotalHP;
}
