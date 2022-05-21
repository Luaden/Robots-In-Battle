using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBuildController : MonoBehaviour
{
    [SerializeField] private List<Sprite> hairOptions;
    [SerializeField] private List<Sprite> eyesOptions;
    [SerializeField] private List<Sprite> noseOptions;
    [SerializeField] private List<Sprite> mouthOptions;
    [SerializeField] private List<Sprite> clothesOptions;
    [SerializeField] private List<Sprite> bodyOptions;
    [SerializeField] private List<AudioClip> dialogueSoundOptions;

    [SerializeField] private List<SOCompleteCharacter> potentialAIBuilds;


    public FighterDataObject GetRandomFighter()
    {
        FighterDataObject newFighter = new FighterDataObject(GetRandomAIBehavior());
        newFighter.FighterUIObject = new FighterPilotUIObject(GetRandomSpriteFromList(hairOptions), GetRandomSpriteFromList(eyesOptions), GetRandomSpriteFromList(noseOptions),
                                                                 GetRandomSpriteFromList(mouthOptions), GetRandomSpriteFromList(clothesOptions), GetRandomSpriteFromList(bodyOptions));
        newFighter.FighterDialogueSound = GetRandomSoundFromList(dialogueSoundOptions);

        return newFighter;
    }
    
    private SOCompleteCharacter GetRandomAIBehavior()
    {
        return potentialAIBuilds[Random.Range(0, potentialAIBuilds.Count)];
    }

    private Sprite GetRandomSpriteFromList(List<Sprite> spriteList)
    {
        return spriteList[Random.Range(0, spriteList.Count)];
    }

    private AudioClip GetRandomSoundFromList(List<AudioClip> audioClips)
    {
        return audioClips[Random.Range(0, audioClips.Count)];
    }
}
