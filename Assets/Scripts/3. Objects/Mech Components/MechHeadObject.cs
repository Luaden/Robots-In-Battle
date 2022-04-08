using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechHeadObject
{
    private MechComponent componentType = MechComponent.Head;
    private Sprite componentSprite;
    private int componentHP;
    private List<ActiveFighterEffect> activeComponentEffects;
    private List<PassiveFighterEffect> passiveComponentEffects;

    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentSprite; }
    public int ComponentHP { get => componentHP; }
    public List<ActiveFighterEffect> ActiveComponentEffects { get => activeComponentEffects; }
    public List<PassiveFighterEffect> PassiveComponentEffects { get => passiveComponentEffects; }

    public MechHeadObject(SOMechComponent soHeadScriptableObject)
    {
        componentSprite = soHeadScriptableObject.ComponentSprite;
        componentHP = soHeadScriptableObject.ComponentHP;
        activeComponentEffects = new List<ActiveFighterEffect>();
        passiveComponentEffects = new List<PassiveFighterEffect>();

        foreach (ActiveFighterEffect effect in soHeadScriptableObject.ActiveComponentEffects)
            activeComponentEffects.Add(effect);
        foreach (PassiveFighterEffect effect in soHeadScriptableObject.PassiveComponentEffects)
            passiveComponentEffects.Add(effect);
    }
}
