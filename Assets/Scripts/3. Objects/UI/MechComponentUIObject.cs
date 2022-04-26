using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechComponentUIObject
{
    private string componentName;
    private MechComponent componentType;
    private ElementType componentElement;
    private Sprite componentSprite;
    private int componentMaxHP;
    private int componentEnergy;
    private GameObject mechComponentUIController;
    private MechComponentDataObject mechComponentData;

    public string ComponentName { get => componentName; }
    public MechComponent ComponentType { get => componentType; }
    public ElementType ComponentElement { get => componentElement; }
    public Sprite ComponentSprite { get => componentSprite; }
    public int ComponentMaxHP { get => componentMaxHP; }
    public int ComponentEnergy { get => componentEnergy; }
    public GameObject MechComponentUIController { get => mechComponentUIController; set => mechComponentUIController = value; }
    public MechComponentDataObject MechComponentData { get => mechComponentData; }


    public MechComponentUIObject(MechComponentDataObject mechComponentData)
    {
        this.componentName = mechComponentData.ComponentName;
        this.componentType = mechComponentData.ComponentType;
        this.componentElement = mechComponentData.ComponentElement;
        this.componentSprite = mechComponentData.ComponentSprite;
        this.componentEnergy = mechComponentData.ComponentMaxEnergy;

        this.mechComponentData = mechComponentData;
    }
}
