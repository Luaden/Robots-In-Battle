using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMechComponent", menuName = "Mech Components/NewComponent", order = 0)]
[System.Serializable]
public class SOMechComponent : ScriptableObject
{
    [SerializeField] protected MechComponent componentType;
    [SerializeField] protected Sprite componentSprite;
    [SerializeField] protected int componentHP;
    [SerializeField] protected List<ActiveFighterEffect> activeMechComponentEffects;
    [SerializeField] protected List<PassiveFighterEffect> passiveMechComponentEffects;

    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentSprite; }
    public int ComponentHP { get => componentHP; }
    public List<ActiveFighterEffect> ActiveComponentEffects { get => activeMechComponentEffects; }
    public List<PassiveFighterEffect> PassiveComponentEffects { get => passiveMechComponentEffects; }

}
