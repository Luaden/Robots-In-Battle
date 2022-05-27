using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationManager : MonoBehaviour
{
    [SerializeField] private MechAnimationController playerMechAnimationController;
    [SerializeField] private MechAnimationController opponentMechAnimationController;
    [SerializeField] private BurnPileController burnPileController;

    private bool animationsComplete = true;

    public bool AnimationsComplete { get => animationsComplete; }

    public void SetMechAnimation(AnimationQueueObject newAnimation)
    {
        animationsComplete = false;

        if (newAnimation.firstMech == CharacterSelect.Player)
            playerMechAnimationController.SetMechAnimation(newAnimation.firstAnimation);
        if (newAnimation.firstMech == CharacterSelect.Opponent)
            opponentMechAnimationController.SetMechAnimation(newAnimation.firstAnimation);

        if (newAnimation.secondMech == CharacterSelect.Player)
            playerMechAnimationController.SetMechAnimation(newAnimation.secondAnimation);
        if (newAnimation.secondMech == CharacterSelect.Opponent)
            opponentMechAnimationController.SetMechAnimation(newAnimation.secondAnimation);
    }

    public void SetMechAnimation(CharacterSelect character, AnimationType animation)
    {
        animationsComplete = false;

        if (character == CharacterSelect.Player)
            playerMechAnimationController.SetMechAnimation(animation);
        if (character == CharacterSelect.Opponent)
            opponentMechAnimationController.SetMechAnimation(animation);
    }

    public void BreakComponent(MechComponent componentType, CharacterSelect character)
    {
        if (character == CharacterSelect.Player)
            playerMechAnimationController.BreakComponent(componentType);
        else
            opponentMechAnimationController.BreakComponent(componentType);
    }

    public void SetMechStartingAnimations(MechObject mech, CharacterSelect character, bool isBoss = false)
    {
        if(character == CharacterSelect.Player)
        {
            Debug.Log("Player is boss:" + isBoss);
            playerMechAnimationController.SetMechBossStatus(isBoss);
            playerMechAnimationController.SetMechElements(mech);
        }
        else
        {
            Debug.Log("Opponent is boss:" + isBoss);
            opponentMechAnimationController.SetMechBossStatus(isBoss);
            opponentMechAnimationController.SetMechElements(mech);
        }
    }

    public void PrepCardsToBurn(CardBurnObject cardBurnObject)
    {
        burnPileController.PrepCardsToBurn(cardBurnObject);
    }
    
    public void BurnCurrentCards()
    {
        burnPileController.BurnCards();
    }

    private void Awake()
    {
        if (burnPileController == null)
            GetComponent<BurnPileController>();
    }


    private void Update()
    {
        CheckAllAnimationsComplete();
    }

    private void CheckAllAnimationsComplete()
    {
        if(!animationsComplete)
        {
            if (playerMechAnimationController.IsAnimating)
                return;
            if (opponentMechAnimationController.IsAnimating)
                return;

            animationsComplete = true;
        }
    }
}
