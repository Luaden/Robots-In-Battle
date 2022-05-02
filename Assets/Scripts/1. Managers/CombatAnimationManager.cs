using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationManager : MonoBehaviour
{
    [SerializeField] private MechAnimationController playerMechAnimationController;
    [SerializeField] private MechAnimationController opponentMechAnimationController;
    [SerializeField] private BurnPileController burnPileController;
    //[SerializeField] private BuffAnimationController buffAnimationController;

    private bool startedAnimations = false;
    private bool allAnimationsComplete = true;
    private bool turnComplete = true;

    private Queue<AnimationQueueObject> animationQueue = new Queue<AnimationQueueObject>();

    public delegate void onStartNewAnimation();
    public static event onStartNewAnimation OnStartNewAnimation;
    public delegate void onEndAnimation();
    public static event onEndAnimation OnEndedAnimation;
    public delegate void onAnimationsComplete();
    public static event onAnimationsComplete OnAnimationsComplete;

    public void AddAnimationToQueue(CharacterSelect firstMech, AnimationType firstAnimation, CharacterSelect secondMech, AnimationType secondAnimation)
    {
        AnimationQueueObject newAnimation = new AnimationQueueObject();
        newAnimation.firstMech = firstMech;
        newAnimation.firstAnimation = firstAnimation;
        newAnimation.secondMech = secondMech;
        newAnimation.secondAnimation = secondAnimation;

        animationQueue.Enqueue(newAnimation);

        allAnimationsComplete = false;
        turnComplete = false;
    }

    public void ClearAnimationQueue()
    {
        animationQueue.Clear();
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

        if(startedAnimations)
            OnEndedAnimation?.Invoke();

        OnStartNewAnimation?.Invoke();

        if (animationQueue.Count > 0)
        {
            AnimationQueueObject currentAnimation = animationQueue.Dequeue();

            if (currentAnimation.firstMech == CharacterSelect.Player)
                playerMechAnimationController.SetMechAnimation(currentAnimation.firstAnimation);
            if (currentAnimation.firstMech == CharacterSelect.Opponent) 
                opponentMechAnimationController.SetMechAnimation(currentAnimation.firstAnimation);

            if (currentAnimation.secondMech == CharacterSelect.Player)
                playerMechAnimationController.SetMechAnimation(currentAnimation.secondAnimation);
            if (currentAnimation.secondMech == CharacterSelect.Opponent)
                opponentMechAnimationController.SetMechAnimation(currentAnimation.secondAnimation);

            startedAnimations = true;
        }

        if (!CheckMechIsAnimating(CharacterSelect.Player) || !CheckMechIsAnimating(CharacterSelect.Opponent))
            if (!allAnimationsComplete && animationQueue.Count == 0)
            {
                allAnimationsComplete = true;
                startedAnimations = false;
                OnEndedAnimation?.Invoke();
            }
    }

    private void CheckAllAnimationsComplete()
    {
        if (CheckMechIsAnimating(CharacterSelect.Player) || CheckMechIsAnimating(CharacterSelect.Opponent))
            return;

        if (allAnimationsComplete && burnPileController.BurnComplete && !turnComplete)
        {
            OnAnimationsComplete.Invoke();
            turnComplete = true;
        }
    }

    private class AnimationQueueObject
    {
        public CharacterSelect firstMech;
        public AnimationType firstAnimation;
        public CharacterSelect secondMech;
        public AnimationType secondAnimation;
    }
}
