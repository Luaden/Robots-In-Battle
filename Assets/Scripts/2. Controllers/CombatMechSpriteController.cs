using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMechSpriteController : MonoBehaviour
{
    [SerializeField] private MechSpriteResolverController playerMech;
    [SerializeField] private MechSpriteResolverController opponentMech;

    public void UpdateMechSprites(MechObject mech, MechSpriteResolverController mechSprites)
    {
        
    }

    public void UpdateMechSprites(MechObject mech, CharacterSelect character)
    {
        if(character == CharacterSelect.Player)
        {
            playerMech.UpdateHeadSprite(mech.MechHead);
            playerMech.UpdateArmSprites(mech.MechArms);
        }
    }
}
