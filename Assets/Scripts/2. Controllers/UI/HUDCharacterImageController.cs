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

    public void UpdateCharacterUI(FighterPilotUIObject newSprites)
    {
        if (newSprites.FighterHair == null)
            hairImage.color = new Color(1, 1, 1, 0);
        hairImage.sprite = newSprites.FighterHair;
        hairImage.SetNativeSize();

        if (newSprites.FighterEyes == null)
            eyesImage.color = new Color(1, 1, 1, 0);
        eyesImage.sprite = newSprites.FighterEyes;
        eyesImage.SetNativeSize();

        if (newSprites.FighterNose == null)
            noseImage.color = new Color(1, 1, 1, 0);
        noseImage.sprite = newSprites.FighterNose;
        noseImage.SetNativeSize();

        if (newSprites.FighterMouth == null)
            mouthImage.color = new Color(1, 1, 1, 0);
        mouthImage.sprite = newSprites.FighterMouth;
        mouthImage.SetNativeSize();

        if (newSprites.FighterClothes == null)
            clothesImage.color = new Color(1, 1, 1, 0);
        clothesImage.sprite = newSprites.FighterClothes;
        clothesImage.SetNativeSize();

        if (newSprites.FighterBody == null)
            bodyImage.color = new Color(1, 1, 1, 0);
        bodyImage.sprite = newSprites.FighterBody;
        bodyImage.SetNativeSize();
    }
}
