using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationManager : MonoBehaviour
{
    [SerializeField] private MechAnimationController playerMechAnimationController;
    [SerializeField] private MechAnimationController opponentMechAnimationController;
    [SerializeField] private BurnPileController burnPileController;

    private Queue<AnimationType> playerAnimations = new Queue<AnimationType>();
    private Queue<AnimationType> opponentAnimations = new Queue<AnimationType>();

    public delegate void onStartingAnimation();
    public static event onStartingAnimation OnStartingAnimation;

    public void SetMechAnimation(CharacterSelect firstMech, AnimationType firstAnimation, CharacterSelect secondMech, AnimationType secondAnimation)
    {
        if (firstMech == CharacterSelect.Player)
            playerAnimations.Enqueue(firstAnimation);
        else
            opponentAnimations.Enqueue(firstAnimation);

        if (secondMech == CharacterSelect.Player)
            playerAnimations.Enqueue(secondAnimation);
        else
            opponentAnimations.Enqueue(secondAnimation);
    }

    public AnimationType GetAnimationFromCategory(CardCategory cardCategory)
    {
        switch (cardCategory)
        {
            case CardCategory.Punch:
                return AnimationType.Punch;

            case CardCategory.Kick:
                return AnimationType.Kick;

            case CardCategory.Special:
                return AnimationType.Special;

            case CardCategory.Guard:
                return AnimationType.Guard;

            case CardCategory.Counter:
                return AnimationType.Counter;
        }

        return AnimationType.Idle;
    }

    public void SetCardPlayAnimation(CardUIController firstCard, CharacterSelect firstCardOwner, CardUIController secondCard = null)
    {
        burnPileController.SetCardOnBurnPile(firstCard, firstCardOwner, secondCard);
    }

    private void Start()
    { 
        if(burnPileController == null)
        burnPileController = FindObjectOfType<BurnPileController>(true);
    }

    private void Update()
    {
        PlayMechAnimations();
    }

    private bool CheckMechIsAnimating(CharacterSelect mechToCheck)
    {
        if (mechToCheck == CharacterSelect.Player)
            return playerMechAnimationController.IsAnimating;
        else
            return opponentMechAnimationController.IsAnimating;
    }

    private void PlayMechAnimations()
    {
        if (CheckMechIsAnimating(CharacterSelect.Player) || CheckMechIsAnimating(CharacterSelect.Opponent))
            return;

        OnStartingAnimation?.Invoke();

        if (playerAnimations.Count > 0)
            playerMechAnimationController.SetMechAnimation(playerAnimations.Dequeue());

        if (opponentAnimations.Count > 0)
            opponentMechAnimationController.SetMechAnimation(opponentAnimations.Dequeue());
    }
}
