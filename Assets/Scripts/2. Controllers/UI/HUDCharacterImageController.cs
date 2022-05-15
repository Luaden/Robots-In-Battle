using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class HUDCharacterImageController : MonoBehaviour
{
    [SerializeField] private Image hairImage;
    [SerializeField] private Image eyesImage;
    [SerializeField] private Image noseImage;
    [SerializeField] private Image mouthImage;
    [SerializeField] private Image clothesImage;
    [SerializeField] private Image bodyImage;

    public void UpdateCharacterUI(FighterCharacterObject newSprites)
    {
        hairImage.sprite = newSprites.FighterHair;
        hairImage.SetNativeSize();
        eyesImage.sprite = newSprites.FighterEyes;
        eyesImage.SetNativeSize();
        noseImage.sprite = newSprites.FighterNose;
        noseImage.SetNativeSize();
        mouthImage.sprite = newSprites.FighterMouth;
        mouthImage.SetNativeSize();
        clothesImage.sprite = newSprites.FighterClothes;
        clothesImage.SetNativeSize();
        bodyImage.sprite = newSprites.FighterBody;
        bodyImage.SetNativeSize();
    }
}
