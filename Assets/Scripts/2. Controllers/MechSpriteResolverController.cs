using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class MechSpriteResolverController : MonoBehaviour
{
    [SerializeField] private SpriteResolver headSpriteResolver;
    [SerializeField] private SpriteResolver torsoSpriteResolver;
    [SerializeField] private SpriteResolver leftArmPrimarySpriteResolver;
    [SerializeField] private SpriteResolver leftArmSecondarySpriteResolver;
    [SerializeField] private SpriteResolver rightArmPrimarySpriteResolver;
    [SerializeField] private SpriteResolver rightArmSecondarySpriteResolver;
    [SerializeField] private SpriteResolver leftLegPrimarySpriteResolver;
    [SerializeField] private SpriteResolver leftLegSecondarySpriteResolver;
    [SerializeField] private SpriteResolver leftLegTertiarySpriteResolver;
    [SerializeField] private SpriteResolver rightLegPrimarySpriteResolver;
    [SerializeField] private SpriteResolver rightLegSecondarySpriteResolver;
    [SerializeField] private SpriteResolver rightLegTertiarySpriteResolver;
    
    public void UpdateHeadSprite(MechComponentDataObject newHead)
    {
        headSpriteResolver.SetCategoryAndLabel(headSpriteResolver.GetCategory(), newHead.PrimaryComponentSpriteID);
    }

    public void UpdateTorsoSprite(MechComponentDataObject newTorso)
    {
        torsoSpriteResolver.SetCategoryAndLabel(torsoSpriteResolver.GetCategory(), newTorso.PrimaryComponentSpriteID);
    }

    public void UpdateArmSprites(MechComponentDataObject newArm)
    {
        leftArmPrimarySpriteResolver.SetCategoryAndLabel(leftArmPrimarySpriteResolver.GetCategory(), newArm.AltPrimaryComponentSpriteID);
        leftArmSecondarySpriteResolver.SetCategoryAndLabel(leftArmSecondarySpriteResolver.GetCategory(), newArm.AltSecondaryComponentSpriteID);

        rightArmPrimarySpriteResolver.SetCategoryAndLabel(rightArmPrimarySpriteResolver.GetCategory(), newArm.PrimaryComponentSpriteID);
        rightArmSecondarySpriteResolver.SetCategoryAndLabel(rightArmSecondarySpriteResolver.GetCategory(), newArm.SecondaryComponentSpriteID);
    }

    public void UpdateLegSprites(MechComponentDataObject newLegs)
    {
        leftLegPrimarySpriteResolver.SetCategoryAndLabel(leftLegPrimarySpriteResolver.GetCategory(), newLegs.AltPrimaryComponentSpriteID);
        leftLegSecondarySpriteResolver.SetCategoryAndLabel(leftLegSecondarySpriteResolver.GetCategory(), newLegs.AltSecondaryComponentSpriteID);
        leftLegTertiarySpriteResolver.SetCategoryAndLabel(leftLegTertiarySpriteResolver.GetCategory(), newLegs.AltTertiaryComponentSpriteID);

        rightLegPrimarySpriteResolver.SetCategoryAndLabel(rightLegPrimarySpriteResolver.GetCategory(), newLegs.PrimaryComponentSpriteID);
        rightLegSecondarySpriteResolver.SetCategoryAndLabel(rightLegSecondarySpriteResolver.GetCategory(), newLegs.SecondaryComponentSpriteID);
        rightLegTertiarySpriteResolver.SetCategoryAndLabel(rightLegTertiarySpriteResolver.GetCategory(), newLegs.TertiaryComponentSpriteID);
    }
}
