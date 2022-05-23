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

    public void UpdateHeadSprite(string newHead)
    {
        headSpriteResolver.SetCategoryAndLabel(headSpriteResolver.GetCategory(), newHead);
    }

    public void UpdateTorsoSprite(MechComponentDataObject newTorso)
    {
        torsoSpriteResolver.SetCategoryAndLabel(torsoSpriteResolver.GetCategory(), newTorso.PrimaryComponentSpriteID);
    }
    public void UpdateTorsoSprite(string newTorso)
    {
        torsoSpriteResolver.SetCategoryAndLabel(torsoSpriteResolver.GetCategory(), newTorso);
    }

    public void UpdateArmSprites(MechComponentDataObject newArm)
    {
        leftArmPrimarySpriteResolver.SetCategoryAndLabel(leftArmPrimarySpriteResolver.GetCategory(), newArm.AltPrimaryComponentSpriteID);
        leftArmSecondarySpriteResolver.SetCategoryAndLabel(leftArmSecondarySpriteResolver.GetCategory(), newArm.AltSecondaryComponentSpriteID);

        rightArmPrimarySpriteResolver.SetCategoryAndLabel(rightArmPrimarySpriteResolver.GetCategory(), newArm.PrimaryComponentSpriteID);
        rightArmSecondarySpriteResolver.SetCategoryAndLabel(rightArmSecondarySpriteResolver.GetCategory(), newArm.SecondaryComponentSpriteID);
    }

    public void UpdateArmSprites(string newBicep1, string newBicep2, string newArm1, string newArm2)
    {
        leftArmPrimarySpriteResolver.SetCategoryAndLabel(leftArmPrimarySpriteResolver.GetCategory(), newBicep2);
        leftArmSecondarySpriteResolver.SetCategoryAndLabel(leftArmSecondarySpriteResolver.GetCategory(), newArm2);

        rightArmPrimarySpriteResolver.SetCategoryAndLabel(rightArmPrimarySpriteResolver.GetCategory(), newBicep1);
        rightArmSecondarySpriteResolver.SetCategoryAndLabel(rightArmSecondarySpriteResolver.GetCategory(), newArm1);
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

    public void UpdateLegSprites(string newThigh1, string newThigh2, string newLeg1, string newLeg2, string newFoot1, string newFoot2)
    {
        leftLegPrimarySpriteResolver.SetCategoryAndLabel(leftLegPrimarySpriteResolver.GetCategory(), newThigh2);
        leftLegSecondarySpriteResolver.SetCategoryAndLabel(leftLegSecondarySpriteResolver.GetCategory(), newLeg2);
        leftLegTertiarySpriteResolver.SetCategoryAndLabel(leftLegTertiarySpriteResolver.GetCategory(), newFoot2);

        rightLegPrimarySpriteResolver.SetCategoryAndLabel(rightLegPrimarySpriteResolver.GetCategory(), newThigh1);
        rightLegSecondarySpriteResolver.SetCategoryAndLabel(rightLegSecondarySpriteResolver.GetCategory(), newLeg1);
        rightLegTertiarySpriteResolver.SetCategoryAndLabel(rightLegTertiarySpriteResolver.GetCategory(), newFoot1);
    }
}
