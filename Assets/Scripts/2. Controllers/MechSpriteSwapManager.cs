using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechSpriteSwapManager : MonoBehaviour
{
    [SerializeField] private MechSpriteResolverController playerMech;
    [SerializeField] private MechSpriteResolverController opponentMech;
    private MechSpriteResolverController currentResolverController;

    public void UpdateMechSprites(MechObject mech, MechSpriteResolverController activeResolver)
    {
        activeResolver.UpdateHeadSprite(mech.MechHead);
        activeResolver.UpdateArmSprites(mech.MechArms);
        activeResolver.UpdateTorsoSprite(mech.MechTorso);
        activeResolver.UpdateLegSprites(mech.MechLegs);
    }

    public void UpdateMechSprites(MechObject mech, CharacterSelect character)
    {
        if (character == CharacterSelect.Player)
            currentResolverController = playerMech;
        else
            currentResolverController = opponentMech;

        UpdateMechSprites(mech, currentResolverController);
    }
}
