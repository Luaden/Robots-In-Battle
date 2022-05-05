using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationManager : MonoBehaviour
{
    [SerializeField] private MechAnimationController playerMechAnimationController;
    [SerializeField] private MechAnimationController opponentMechAnimationController;
    [SerializeField] private BurnPileController burnPileController;
    //[SerializeField] private BuffAnimationController buffAnimationController;

    private bool animationsComplete = true;

    public bool AnimationsComplete { get => animationsComplete; }

    public void AddAnimationToQueue(CharacterSelect firstMech, AnimationType firstAnimation, CharacterSelect secondMech, AnimationType secondAnimation)
    {
        animationsComplete = false;

        if (firstMech == CharacterSelect.Player)
            playerMechAnimationController.SetMechAnimation(firstAnimation);
        if (firstMech == CharacterSelect.Opponent)
            opponentMechAnimationController.SetMechAnimation(firstAnimation);

        if (secondMech == CharacterSelect.Player)
            playerMechAnimationController.SetMechAnimation(secondAnimation);
        if (secondMech == CharacterSelect.Opponent)
            opponentMechAnimationController.SetMechAnimation(secondAnimation);
    }

    public void AddAnimationToQueue(AnimationQueueObject newAnimation)
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
