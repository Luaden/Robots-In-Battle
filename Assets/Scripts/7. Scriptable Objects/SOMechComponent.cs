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
    [SerializeField] protected List<MechComponentEffect> componentEffects;

    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentSprite; }
    public int ComponentHP { get => componentHP; }
    public List<MechComponentEffect> ComponentEffects { get => componentEffects; }
}
