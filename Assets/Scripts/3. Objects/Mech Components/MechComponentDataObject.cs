using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechComponentDataObject
{
    //Base attributes
    private string componentName;
    private MechComponent componentType;
    private ElementType componentElement;
    private Sprite componentSprite;
    private int componentMaxHP;
    private int componentEnergy;
    private int componentCurrentHP;
   
    //Bonus effects
    private int bonusDamageFromComponent;
    private bool bonusDamageAsPercent;
    private int reduceDamageToComponent;
    private bool reduceDamageAsPercent;
    private int extraElementStacks;
    private int energyGainModifier;
    private SOItemDataObject sOItemDataObject;

    public string ComponentName { get => componentName; }
    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentSprite; }
    public int ComponentCurrentHP { get => componentCurrentHP; }
    public int ComponentMaxHP { get => componentMaxHP; }
    public int ComponentMaxEnergy { get => componentEnergy; }
    public ElementType ComponentElement { get => componentElement; }
    public int BonusDamageFromComponent { get => bonusDamageFromComponent; }
    public bool BonusDamageAsPercent { get => bonusDamageAsPercent; }
    public int ReduceDamageToComponent { get => reduceDamageToComponent; }
    public bool ReduceDamageAsPercent { get => reduceDamageAsPercent; }
    public int ExtraElementStacks { get => extraElementStacks; }
    public int EnergyGainModifier { get => energyGainModifier; }
    public SOItemDataObject SOItemDataObject { get => sOItemDataObject; }

    public MechComponentDataObject(SOItemDataObject sOMechComponent)
    {
        if (sOMechComponent == null)
            Debug.Log("No SO given.");
        componentName = sOMechComponent.ComponentName;
        componentType = sOMechComponent.ComponentType;
        componentElement = sOMechComponent.ComponentElement;
        componentSprite = sOMechComponent.ComponentSprite;
        componentMaxHP = sOMechComponent.ComponentHP;
        componentEnergy = sOMechComponent.ComponentEnergy;
        componentCurrentHP = componentMaxHP;

        componentElement = sOMechComponent.ComponentElement;
        bonusDamageFromComponent = sOMechComponent.BonusDamageFromComponent;
        bonusDamageAsPercent = sOMechComponent.BonusDamageAsPercent;
        reduceDamageToComponent = sOMechComponent.ReduceDamageToComponent;
        reduceDamageAsPercent = sOMechComponent.ReduceDamageAsPercent;
        extraElementStacks = sOMechComponent.ExtraElementStacks;
        energyGainModifier = sOMechComponent.EnergyGainModifier;
        sOItemDataObject = sOMechComponent;
    }
}
