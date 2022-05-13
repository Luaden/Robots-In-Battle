using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBuildController : MonoBehaviour
{
    [SerializeField] private GameObject aiUIPrefab;
    [SerializeField] private List<SOCompleteCharacter> potentialAIBuilds;


    public FighterDataObject GetRandomFighter()
    {
        FighterDataObject newFighter = new FighterDataObject(GetRandomAIBehavior());
        newFighter.FighterSpriteObject = GetRandomAIUI();

        return newFighter;
    }

    private GameObject GetRandomAIUI()
    {
        GameObject newAIUI = Instantiate(aiUIPrefab, transform);
        newAIUI.GetComponent<SpriteResolverRandomizer>().RandomizeAllSprites();

        return newAIUI;
    }
    
    private SOCompleteCharacter GetRandomAIBehavior()
    {
        return potentialAIBuilds[Random.Range(0, potentialAIBuilds.Count)];
    }
}
