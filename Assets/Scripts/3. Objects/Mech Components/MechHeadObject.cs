using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechHeadObject
{
    [SerializeField] protected MechComponent componentType;
    [SerializeField] protected Sprite componentSprite;
    [SerializeField] protected int componentHP;
    [SerializeField] protected List<MechComponentEffect> componentEffects;

    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentSprite; }
    public int ComponentHP { get => componentHP; }
    public List<MechComponentEffect> ComponentEffects { get => componentEffects; }

    public MechHeadObject(SOMechComponent armsScriptableObject)
    {
        componentType = armsScriptableObject.ComponentType;
        componentSprite = armsScriptableObject.ComponentSprite;
        componentHP = armsScriptableObject.ComponentHP;
        componentEffects = new List<MechComponentEffect>();

        foreach (MechComponentEffect effect in armsScriptableObject.ComponentEffects)
            componentEffects.Add(effect);
    }
}
