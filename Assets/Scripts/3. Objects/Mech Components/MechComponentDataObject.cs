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
    private int componentMaxEnergy;
    private int componentCurrentHP;
   
    //Bonus effects
    private float cDMFromComponent;
    private float cDMToComponent;
    private int extraElementStacks;
    private int energyGainModifier;
    private SOItemDataObject sOItemDataObject;

    public string ComponentName { get => componentName; }
    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentSprite; }
    public int ComponentCurrentHP { get => componentCurrentHP; set => componentCurrentHP = value; }
    public int ComponentMaxHP { get => componentMaxHP; }
    public int ComponentMaxEnergy { get => componentMaxEnergy; }
    public ElementType ComponentElement { get => componentElement; }
    public float CDMFromComponent { get => cDMFromComponent; }
    public float CDMToComponent { get => cDMToComponent; }
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
        componentMaxEnergy = sOMechComponent.ComponentEnergy;
        componentCurrentHP = componentMaxHP;

        componentElement = sOMechComponent.ComponentElement;
        cDMFromComponent = sOMechComponent.CDMFromComponent;
        cDMToComponent = sOMechComponent.CDMToComponent;
        extraElementStacks = sOMechComponent.ExtraElementStacks;
        energyGainModifier = sOMechComponent.EnergyGainModifier;
        sOItemDataObject = sOMechComponent;
    }

    public void HealComponent()
    {
        componentCurrentHP = componentMaxHP;
    }
}
