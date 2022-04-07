using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using System.Linq;

public class TestSwapper : MonoBehaviour
{
    [SerializeField]
    private SpriteLibrary spriteLibrary = default;

    [SerializeField]
    private SpriteResolver targetResolver = default;

    [SerializeField]
    private string targetCategory = default;

    private SpriteLibraryAsset LibraryAsset => spriteLibrary.spriteLibraryAsset;

    
   public void InjectCustom (Sprite customSprite)
    {
        const string customLabel = "customLowerarm_right";
        spriteLibrary.AddOverride(customSprite, targetCategory, customLabel);
        targetResolver.SetCategoryAndLabel(targetCategory, customLabel);
    }
    
     public void SelectRandom()
    {
        string[] labels = LibraryAsset.GetCategoryLabelNames(targetCategory).ToArray();
        int index = Random.Range(0, labels.Length);
        string label = labels[index];

        targetResolver.SetCategoryAndLabel(targetCategory, label);
    } 
    
}
