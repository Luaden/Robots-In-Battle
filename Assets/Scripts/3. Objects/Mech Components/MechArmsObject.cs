using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechArmsObject
{
    private MechComponent componentType = MechComponent.Arms;
    private Sprite componentSprite;
    private int componentHP;
    private List<ActiveFighterEffect> activeComponentEffects;
    private List<PassiveFighterEffect> passiveComponentEffects;

    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentSprite; }
    public int ComponentHP { get => componentHP; }
    public List<ActiveFighterEffect> ActiveComponentEffects { get => activeComponentEffects; }
    public List<PassiveFighterEffect> PassiveComponentEffects { get => passiveComponentEffects; }

    public MechArmsObject(SOMechComponent soArmScriptableObject)
    {
        componentSprite = soArmScriptableObject.ComponentSprite;
        componentHP = soArmScriptableObject.ComponentHP;
        activeComponentEffects = new List<ActiveFighterEffect>();
        passiveComponentEffects = new List<PassiveFighterEffect>();

        foreach (ActiveFighterEffect effect in soArmScriptableObject.ActiveComponentEffects)
            activeComponentEffects.Add(effect);
        foreach (PassiveFighterEffect effect in soArmScriptableObject.PassiveComponentEffects)
            passiveComponentEffects.Add(effect);
    }
}
