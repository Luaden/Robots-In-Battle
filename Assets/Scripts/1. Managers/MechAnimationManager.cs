using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechAnimationManager : MonoBehaviour
{
    [SerializeField] private MechAnimationController playerMechAnimationController;
    [SerializeField] private MechAnimationController opponentMechAnimationController;

    private Queue<AnimationType> playerAnimations = new Queue<AnimationType>();
    private Queue<AnimationType> opponentAnimations = new Queue<AnimationType>();

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

        return AnimationType.Damaged;
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
        if (playerMechAnimationController.IsAnimating || opponentMechAnimationController.IsAnimating)
            return;

        if (playerAnimations.Count > 0)
            playerMechAnimationController.SetMechAnimation(playerAnimations.Dequeue());
        if (opponentAnimations.Count > 0)
            opponentMechAnimationController.SetMechAnimation(opponentAnimations.Dequeue());
    }
}
