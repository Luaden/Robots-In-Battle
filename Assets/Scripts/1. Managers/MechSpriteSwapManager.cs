using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechSpriteSwapManager : MonoBehaviour
{
    [SerializeField] private MechSpriteResolverController playerMech;
    [SerializeField] private MechSpriteResolverController opponentMech;
    [SerializeField] private bool randomizeOnCombat = false;
    [SerializeField] private float randomizePace;
    [SerializeField] private List<string> headSprites;
    [SerializeField] private List<string> torsoSprites;
    [SerializeField] private List<string> bicep1Sprites;
    [SerializeField] private List<string> arm1Sprites;
    [SerializeField] private List<string> bicep2Sprites;
    [SerializeField] private List<string> arm2Sprites;
    [SerializeField] private List<string> thigh1Sprites;
    [SerializeField] private List<string> leg1Sprites;
    [SerializeField] private List<string> foot1Sprites;
    [SerializeField] private List<string> thigh2Sprites;
    [SerializeField] private List<string> leg2Sprites;
    [SerializeField] private List<string> foot2Sprites;

    private MechSpriteResolverController currentResolverController;
    private bool randomizeSprites = false;
    private float currentTimer;

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

    private void Start()
    {
        if (randomizeOnCombat)
        {
            CombatSequenceManager.OnCombatStart += OnCombatStart;
            CombatSequenceManager.OnCombatComplete += OnCombatComplete;
        }
    }

    private void OnDestroy()
    {
        CombatSequenceManager.OnCombatStart -= OnCombatStart;
        CombatSequenceManager.OnCombatComplete -= OnCombatComplete;
    }

    private void OnCombatStart()
    {
        randomizeSprites = true;
    }

    private void OnCombatComplete()
    {
        randomizeSprites = false;
    }

    private void Update()
    {
        if(randomizeSprites && GameManager.instance.PlayerWins == 0)
        {
            RandomizeSprites();
        }
    }

    private void RandomizeSprites()
    {
        if(CheckTimer())
        {
            playerMech.UpdateHeadSprite(GetRandomStringFromList(headSprites));
            playerMech.UpdateTorsoSprite(GetRandomStringFromList(torsoSprites));

            playerMech.UpdateArmSprites(GetRandomStringFromList(bicep1Sprites), 
                                        GetRandomStringFromList(bicep2Sprites), 
                                        GetRandomStringFromList(arm1Sprites), 
                                        GetRandomStringFromList(arm2Sprites));

            playerMech.UpdateLegSprites(GetRandomStringFromList(thigh1Sprites), 
                                        GetRandomStringFromList(thigh2Sprites), 
                                        GetRandomStringFromList(leg1Sprites), 
                                        GetRandomStringFromList(leg2Sprites),
                                        GetRandomStringFromList(foot1Sprites), 
                                        GetRandomStringFromList(foot2Sprites));
        }
    }

    private bool CheckTimer()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= randomizePace)
        {
            currentTimer = 0f;
            return true;
        }

        return false;
    }


    private string GetRandomStringFromList(List<string> spriteList)
    {
        return spriteList[Random.Range(0, spriteList.Count)];
    }
}
