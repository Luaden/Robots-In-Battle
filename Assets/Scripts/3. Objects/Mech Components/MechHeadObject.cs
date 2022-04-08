using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechHeadObject
{
    private MechComponent componentType = MechComponent.Head;
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

    public MechHeadObject(SOMechComponent soHeadScriptableObject)
    {
        componentSprite = soHeadScriptableObject.ComponentSprite;
        componentMaxHP = soHeadScriptableObject.ComponentHP;
        componentCurrentHP = componentMaxHP;
        activeComponentEffects = new List<ActiveFighterEffect>();
        passiveComponentEffects = new List<PassiveFighterEffect>();

        foreach (ActiveFighterEffect effect in soHeadScriptableObject.ActiveComponentEffects)
            activeComponentEffects.Add(effect);
        foreach (PassiveFighterEffect effect in soHeadScriptableObject.PassiveComponentEffects)
            passiveComponentEffects.Add(effect);
    }
}
