using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SpriteResolverRandomizer : MonoBehaviour
{
    [SerializeField] private SpriteResolver eyesResolver;
    [SerializeField] private SpriteResolver hairResolver;
    [SerializeField] private SpriteResolver noseResolver;
    [SerializeField] private SpriteResolver mouthResolver;
    [SerializeField] private SpriteResolver clothesResolver;
    [SerializeField] private SpriteResolver bodyResolver;

    public void RandomizeAllSprites()
    {
        eyesResolver.SetCategoryAndLabel(eyesResolver.GetCategory(), "Eyes" + GetRandomInt().ToString());
        hairResolver.SetCategoryAndLabel(hairResolver.GetCategory(), "Hair" + GetRandomInt().ToString());
        noseResolver.SetCategoryAndLabel(noseResolver.GetCategory(), "Nose" + GetRandomInt().ToString());
        mouthResolver.SetCategoryAndLabel(mouthResolver.GetCategory(), "Mouth" + GetRandomInt().ToString());
        clothesResolver.SetCategoryAndLabel(clothesResolver.GetCategory(), "Clothes" + GetRandomInt().ToString());
        bodyResolver.SetCategoryAndLabel(bodyResolver.GetCategory(), "Body" + GetRandomInt().ToString());
    }

    public int GetRandomInt()
    {
        int randomInt = Random.Range(1, 4);
        return randomInt;
    }
}
