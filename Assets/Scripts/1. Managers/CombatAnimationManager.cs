using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationManager : MonoBehaviour
{
    [SerializeField] private MechAnimationController playerMechAnimationController;
    [SerializeField] private MechAnimationController opponentMechAnimationController;
    [SerializeField] private BurnPileController burnPileController;

    private bool startedAnimations = false;
    private bool allAnimationsComplete = true;
    private bool turnComplete = true;

    private Queue<AnimationType> playerAnimations = new Queue<AnimationType>();
    private Queue<AnimationType> opponentAnimations = new Queue<AnimationType>();

    public delegate void onStartNewAnimation();
    public static event onStartNewAnimation OnStartNewAnimation;
    public delegate void onEndAnimation();
    public static event onEndAnimation OnEndedAnimation;
    public delegate void onAnimationsComplete();
    public static event onAnimationsComplete OnAnimationsComplete;

    public void SetMechAnimation(CharacterSelect firstMech, AnimationType firstAnimation, CharacterSelect secondMech, AnimationType secondAnimation)
    {
        Debug.Log(firstMech + " animating: " + firstAnimation);
        Debug.Log(secondMech + " animating: " + secondAnimation);

        if (firstMech == CharacterSelect.Player)
            playerAnimations.Enqueue(firstAnimation);
        else if(firstMech == CharacterSelect.Opponent)
            opponentAnimations.Enqueue(firstAnimation);

        if (secondMech == CharacterSelect.Player)
            playerAnimations.Enqueue(secondAnimation);
        else if (secondMech == CharacterSelect.Opponent)
            opponentAnimations.Enqueue(secondAnimation);

        allAnimationsComplete = false;
        turnComplete = false;
    }

    public void SetCardOnBurnPile(CardUIController firstCard, CharacterSelect firstCardOwner, CardUIController secondCard = null)
    {
        burnPileController.SetCardOnBurnPile(firstCard, firstCardOwner, secondCard);
    }

    private void Awake()
    {
        if (burnPileController == null)
            GetComponent<BurnPileController>();
    }

    private void Update()
    {
        PlayMechAnimations();
        CheckAllAnimationsComplete();
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
        if (turnComplete)
            return;

        if (CheckMechIsAnimating(CharacterSelect.Player) || CheckMechIsAnimating(CharacterSelect.Opponent))
            return;

        if (startedAnimations)
            OnEndedAnimation?.Invoke();

        OnStartNewAnimation?.Invoke();

        if (playerAnimations.Count > 0)
        {
            playerMechAnimationController.SetMechAnimation(playerAnimations.Dequeue());
            startedAnimations = true;
        }

        if (opponentAnimations.Count > 0)
        {
            opponentMechAnimationController.SetMechAnimation(opponentAnimations.Dequeue());
            startedAnimations = true;
        }

        if (!allAnimationsComplete && playerAnimations.Count == 0 && opponentAnimations.Count == 0)
        {
            Debug.Log("All animations complete.");
            allAnimationsComplete = true;
            startedAnimations = false;
        }
    }

    private void CheckAllAnimationsComplete()
    {
        if (CheckMechIsAnimating(CharacterSelect.Player) || CheckMechIsAnimating(CharacterSelect.Opponent))
            return;

        if (allAnimationsComplete && burnPileController.BurnComplete && !turnComplete)
        {
            Debug.Log("Raising event.");
            OnAnimationsComplete.Invoke();
            turnComplete = true;
        }
    }
}
