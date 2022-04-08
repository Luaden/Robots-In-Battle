using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechArmsObject
{
    private MechComponent componentType = MechComponent.Arms;
    private Sprite componentSprite;
    private int componentMaxHP;
    private int componentCurrentHP;
    private List<ActiveFighterEffect> activeComponentEffects;
    private List<PassiveFighterEffect> passiveComponentEffects;

    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentSprite; }
    public int ComponentMaxHP { get => componentMaxHP; }
    public int ComponentCurrentHP { get => componentCurrentHP; }
    public List<ActiveFighterEffect> ActiveComponentEffects { get => activeComponentEffects; }
    public List<PassiveFighterEffect> PassiveComponentEffects { get => passiveComponentEffects; }

    public MechArmsObject(SOMechComponent soArmScriptableObject)
    {
        componentSprite = soArmScriptableObject.ComponentSprite;
        componentMaxHP = soArmScriptableObject.ComponentHP;
        componentCurrentHP = componentMaxHP;
        activeComponentEffects = new List<ActiveFighterEffect>();
        passiveComponentEffects = new List<PassiveFighterEffect>();

        foreach (ActiveFighterEffect effect in soArmScriptableObject.ActiveComponentEffects)
            activeComponentEffects.Add(effect);
        foreach (PassiveFighterEffect effect in soArmScriptableObject.PassiveComponentEffects)
            passiveComponentEffects.Add(effect);
    }
}
