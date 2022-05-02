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

    private Queue<Queue<AnimationQueueObject>> animationCollection = new Queue<Queue<AnimationQueueObject>>();
    private Queue<AnimationQueueObject> currentAnimationQueue = new Queue<AnimationQueueObject>();
    private AnimationQueueObject currentAnimationObject;

    public delegate void onStartNewAnimation();
    public static event onStartNewAnimation OnStartNewAnimation;
    public delegate void onEndAnimation();
    public static event onEndAnimation OnEndedAnimation;
    public delegate void onEndRound();
    public static event onEndRound OnRoundEnded;
    public delegate void onAnimationsComplete();
    public static event onAnimationsComplete OnAnimationsComplete;

    public void AddAnimationToQueue(Queue<AnimationQueueObject> animations)
    {
        animationCollection.Enqueue(animations);

        allAnimationsComplete = false;
        turnComplete = false;
    }

    public void AddAnimationToQueue(AnimationQueueObject animation)
    {
        Queue<AnimationQueueObject> newAnimations = new Queue<AnimationQueueObject>();
        newAnimations.Enqueue(animation);

        animationCollection.Enqueue(newAnimations);

        allAnimationsComplete = false;
        turnComplete = false;
    }

    public void ClearAnimationQueue()
    {
        animationCollection.Clear();
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
        {
            OnEndedAnimation?.Invoke();
            currentAnimationObject = null;
        }

        OnStartNewAnimation?.Invoke();

        if (currentAnimationObject == null)
        {
            if (currentAnimationQueue.Count == 0)
            {
                if (animationCollection.Count > 0)
                {
                    currentAnimationQueue = animationCollection.Dequeue();
                    currentAnimationObject = currentAnimationQueue.Dequeue();
                    OnRoundEnded?.Invoke();
                    return;
                }
                else
                {
                    if (!CheckMechIsAnimating(CharacterSelect.Player) || !CheckMechIsAnimating(CharacterSelect.Opponent))
                        if (!allAnimationsComplete && animationCollection.Count == 0)
                        {
                            allAnimationsComplete = true;
                            startedAnimations = false;
                            //OnEndedAnimation?.Invoke();
                            return;
                        }
                    return;
                }
            }
            else
                currentAnimationObject = currentAnimationQueue.Dequeue();
        }
        else
        {
            if (currentAnimationObject.firstMech == CharacterSelect.Player)
                playerMechAnimationController.SetMechAnimation(currentAnimationObject.firstAnimation);
            if (currentAnimationObject.firstMech == CharacterSelect.Opponent)
                opponentMechAnimationController.SetMechAnimation(currentAnimationObject.firstAnimation);

            if (currentAnimationObject.secondMech == CharacterSelect.Player)
                playerMechAnimationController.SetMechAnimation(currentAnimationObject.secondAnimation);
            if (currentAnimationObject.secondMech == CharacterSelect.Opponent)
                opponentMechAnimationController.SetMechAnimation(currentAnimationObject.secondAnimation);
            startedAnimations = true;
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
}
