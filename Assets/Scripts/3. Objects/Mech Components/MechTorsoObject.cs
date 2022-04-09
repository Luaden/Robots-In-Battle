using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechTorsoObject
{
    private MechComponent componentType = MechComponent.Torso;
    private Sprite componentSprite;
    private int componentMaxHP;
    private int componentCurrentHP;
    private List<ActiveFighterEffect> activeComponentEffects;
    private List<PassiveFighterEffect> passiveComponentEffects;

    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentSprite; }
    public int ComponentCurrentHP { get => componentCurrentHP; }
    public int ComponentMaxHP { get => componentMaxHP; }
    public List<ActiveFighterEffect> ActiveComponentEffects { get => activeComponentEffects; }
    public List<PassiveFighterEffect> PassiveComponentEffects { get => passiveComponentEffects; }

    public MechTorsoObject(SOMechComponent soTorsoScriptableObject)
    {
        componentSprite = soTorsoScriptableObject.ComponentSprite;
        componentMaxHP = soTorsoScriptableObject.ComponentHP;
        componentCurrentHP = componentMaxHP;
        activeComponentEffects = new List<ActiveFighterEffect>();
        passiveComponentEffects = new List<PassiveFighterEffect>();

        if (soTorsoScriptableObject.ActiveComponentEffects != null)
            foreach (ActiveFighterEffect effect in soTorsoScriptableObject.ActiveComponentEffects)
                activeComponentEffects.Add(effect);

        if (soTorsoScriptableObject.PassiveComponentEffects != null)
            foreach (PassiveFighterEffect effect in soTorsoScriptableObject.PassiveComponentEffects)
                passiveComponentEffects.Add(effect);
    }
}
