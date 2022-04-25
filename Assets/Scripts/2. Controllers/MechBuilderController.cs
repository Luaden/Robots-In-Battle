using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBuilderController
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

    public void BuildNewPlayerMech(SOItemDataObject mechHead, SOItemDataObject mechTorso, SOItemDataObject mechArms, SOItemDataObject mechLegs)
    {
        MechComponentDataObject head = new MechComponentDataObject(mechHead);
        MechComponentDataObject torso = new MechComponentDataObject(mechTorso);
        MechComponentDataObject legs = new MechComponentDataObject(mechArms);
        MechComponentDataObject arms = new MechComponentDataObject(mechLegs);
        MechObject newMech = new MechObject(head, torso, arms, legs);

        GameManager.instance.PlayerData.PlayerMech = newMech;
    }

    public void SwapPlayerMechPart(SOItemDataObject SOMechComponentDataObject)
    {
        MechComponentDataObject newComponent = new MechComponentDataObject(SOMechComponentDataObject);

        SwapPlayerMechPart(newComponent);
    }

    public void SwapPlayerMechPart(MechComponentDataObject newComponent)
    {
        MechComponentDataObject oldComponent;

        oldComponent = GameManager.instance.PlayerData.PlayerMech.ReplaceComponent(newComponent);
        GameManager.instance.InventoryController.AddItemToInventory(oldComponent);
    }
}
