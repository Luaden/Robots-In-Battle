using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechTorsoObject
{
    private MechComponent componentType = MechComponent.Torso;
    private Sprite componentSprite;
    private int componentHP;
    private List<ActiveFighterEffect> activeComponentEffects;
    private List<PassiveFighterEffect> passiveComponentEffects;

    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentSprite; }
    public int ComponentHP { get => componentHP; }
    public List<ActiveFighterEffect> ActiveComponentEffects { get => activeComponentEffects; }
    public List<PassiveFighterEffect> PassiveComponentEffects { get => passiveComponentEffects; }

    public MechTorsoObject(SOMechComponent soTorsoScriptableObject)
    {
        componentSprite = soTorsoScriptableObject.ComponentSprite;
        componentHP = soTorsoScriptableObject.ComponentHP;
        activeComponentEffects = new List<ActiveFighterEffect>();
        passiveComponentEffects = new List<PassiveFighterEffect>();

        foreach (ActiveFighterEffect effect in soTorsoScriptableObject.ActiveComponentEffects)
            activeComponentEffects.Add(effect);
        foreach (PassiveFighterEffect effect in soTorsoScriptableObject.PassiveComponentEffects)
            passiveComponentEffects.Add(effect);
    }
}
