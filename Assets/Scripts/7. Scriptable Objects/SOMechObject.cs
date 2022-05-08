using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mech", menuName = "Mech/Mech Module")]
public class SOMechObject : ScriptableObject
{
    [SerializeField] private SOItemDataObject mechHead;
    [SerializeField] private SOItemDataObject mechTorso;
    [SerializeField] private SOItemDataObject mechArms;
    [SerializeField] private SOItemDataObject mechLegs;
    [SerializeField] private SOItemDataObject mechBack;

    public SOItemDataObject MechHead { get => mechHead; }
    public SOItemDataObject MechTorso { get => mechTorso; }
    public SOItemDataObject MechArms { get => mechArms; }
    public SOItemDataObject MechLegs { get => mechLegs; }
    public SOItemDataObject MechBack { get => mechBack; }
}
