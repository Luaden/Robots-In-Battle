using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBuilderController : MonoBehaviour
{
    public MechObject BuildNewMech(SOItemDataObject mechHead, SOItemDataObject mechTorso, SOItemDataObject mechArms, SOItemDataObject mechLegs)
    {
        MechComponentDataObject head = new MechComponentDataObject(mechHead);
        MechComponentDataObject torso = new MechComponentDataObject(mechTorso);
        MechComponentDataObject legs = new MechComponentDataObject(mechArms);
        MechComponentDataObject arms = new MechComponentDataObject(mechLegs);

        MechObject newMech = new MechObject(head, torso, arms, legs);

        return newMech;
    }

    public void SwapMechPart(MechObject currentMech, MechComponentDataObject newComponent)
    {
        MechComponentDataObject oldComponent;

        oldComponent = currentMech.ReplaceComponent(newComponent);
        GameManager.instance.InventoryController.AddItemToInventory(oldComponent);
    }

    public void SwapMechPart(MechObject currentMech, SOItemDataObject SOMechComponentDataObject)
    {
        MechComponentDataObject newComponent = new MechComponentDataObject(SOMechComponentDataObject);
        MechComponentDataObject oldComponent;

        oldComponent = currentMech.ReplaceComponent(newComponent);
        GameManager.instance.InventoryController.AddItemToInventory(oldComponent);
    }
}
