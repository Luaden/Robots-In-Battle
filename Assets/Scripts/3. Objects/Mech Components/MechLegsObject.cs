using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechLegsObject
{
    private MechComponent componentType = MechComponent.Legs;
    private Sprite componentSprite;
    private int componentHP;
    private List<ActiveFighterEffect> activeComponentEffects;
    private List<PassiveFighterEffect> passiveComponentEffects;

    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentSprite; }
    public int ComponentHP { get => componentHP; }
    public List<ActiveFighterEffect> ActiveComponentEffects { get => activeComponentEffects; }
    public List<PassiveFighterEffect> PassiveComponentEffects { get => passiveComponentEffects; }

    public MechLegsObject(SOMechComponent soLegsScriptableObject)
    {
        componentSprite = soLegsScriptableObject.ComponentSprite;
        componentHP = soLegsScriptableObject.ComponentHP;
        activeComponentEffects = new List<ActiveFighterEffect>();
        passiveComponentEffects = new List<PassiveFighterEffect>();

        foreach (ActiveFighterEffect effect in soLegsScriptableObject.ActiveComponentEffects)
            activeComponentEffects.Add(effect);
        foreach (PassiveFighterEffect effect in soLegsScriptableObject.PassiveComponentEffects)
            passiveComponentEffects.Add(effect);
    }
}
