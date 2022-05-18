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
    private Sprite componentShopSprite;
    private int componentMaxHP;
    private int componentMaxEnergy;
    private int componentCurrentHP;
    private string primaryComponentSpriteID;
    private string secondaryComponentSpriteID;
    private string tertiaryComponentSpriteID;
    private string altPrimaryComponentSpriteID;
    private string altSecondaryComponentSpriteID;
    private string altTertiaryComponentSpriteID;

    //Bonus effects
    private float cDMFromComponent;
    private float cDMToComponent;
    private int extraElementStacks;
    private int energyGainModifier;
    private SOItemDataObject sOItemDataObject;

    public string ComponentName { get => componentName; }
    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentShopSprite; }
    public string PrimaryComponentSpriteID { get => primaryComponentSpriteID; }
    public string SecondaryComponentSpriteID { get => secondaryComponentSpriteID; }
    public string TertiaryComponentSpriteID { get => tertiaryComponentSpriteID; }
    public string AltPrimaryComponentSpriteID { get => altPrimaryComponentSpriteID; }
    public string AltSecondaryComponentSpriteID { get => altSecondaryComponentSpriteID; }
    public string AltTertiaryComponentID { get => altTertiaryComponentSpriteID; }
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
        componentShopSprite = sOMechComponent.ItemShopSprite;
        primaryComponentSpriteID = sOMechComponent.PrimaryComponentSpriteID;
        secondaryComponentSpriteID = sOMechComponent.SecondaryComponentSpriteID;
        tertiaryComponentSpriteID = sOMechComponent.TertiaryComponentID;
        altPrimaryComponentSpriteID = sOMechComponent.AltPrimaryComponentSpriteID;
        altSecondaryComponentSpriteID = sOMechComponent.AltSecondaryComponentSpriteID;
        altTertiaryComponentSpriteID = sOMechComponent.AltTertiaryComponentSpriteID;
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
