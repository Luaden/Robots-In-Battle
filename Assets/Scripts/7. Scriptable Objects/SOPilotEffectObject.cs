using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pilot Module", menuName = "Pilots/Pilot Module")]
public class SOPilotEffectObject : ScriptableObject
{
    [SerializeField] private PilotEffects pilotEffects;
    [SerializeField] private List<SOItemDataObject> startingDeck;
    [SerializeField] private SOItemDataObject mechHead;
    [SerializeField] private SOItemDataObject mechTorso;
    [SerializeField] private SOItemDataObject mechArms;
    [SerializeField] private SOItemDataObject mechLegs;
    [SerializeField] private SOItemDataObject mechBack;
    [SerializeField] private int startingMoney;

    public PilotEffects PilotEffects { get => pilotEffects; }
    public List<SOItemDataObject> StartingDeck { get => startingDeck; }
    public SOItemDataObject MechHead { get => mechHead; }
    public SOItemDataObject MechTorso { get => mechTorso; }
    public SOItemDataObject MechArms { get => mechArms; }
    public SOItemDataObject MechLegs { get => mechLegs; }
    public SOItemDataObject MechBack { get => mechBack; }
    public int StartingMoney { get => startingMoney; }
}
