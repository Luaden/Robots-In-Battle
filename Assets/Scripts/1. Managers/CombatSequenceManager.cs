using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSequenceManager : MonoBehaviour
{
    private CombatQueueObject currentCombatSequence;
    private Queue<CombatQueueObject> combatSequenceCollection;

    public delegate void onStartCombat();
    public static event onStartCombat OnStartCombat;
    public delegate void onRoundComplete();
    public static event onRoundComplete OnRoundComplete;
    public delegate void onCombatComplete();
    public static event onCombatComplete OnCombatComplete;

    private bool startedCombatSequences = false;
    private bool animationsComplete = true;
    private bool combatComplete = true;

    public void AddCombatSequenceToQueue(CombatQueueObject newCombatSequence)
    {
        if(currentCombatSequence == null)
            OnStartCombat?.Invoke();

        combatSequenceCollection.Enqueue(newCombatSequence);
        combatComplete = false;
    }

    public void ClearCombatQueue()
    {
        combatSequenceCollection.Clear();
        currentCombatSequence = null;
    }

    private void Awake()
    {
        combatSequenceCollection = new Queue<CombatQueueObject>();
    }

    private void Update()
    {
        RunCombatSequence();
    }

    private void RunCombatSequence()
    {
        if (combatComplete || !CombatManager.instance.CombatAnimationManager.AnimationsComplete)
            return;

        if (CheckCurrentCombatSequenceComplete())
        {
            GetNewCombatSequence();
        }
        else
            RunCurrentCombatSequence();
    }

    private bool CheckCurrentCombatSequenceComplete()
    {
        if (currentCombatSequence == null)
            return true;

        if (currentCombatSequence.animationQueue.Count > 0)
            return false;

        if (currentCombatSequence.damageQueue.Count > 0)
            return false;

        return true;
    }

    private void GetNewCombatSequence()
    {
        if (!combatComplete && combatSequenceCollection.Count == 0)
        {
            combatComplete = true;
            startedCombatSequences = false;
            currentCombatSequence = null;
            CombatManager.instance.CombatAnimationManager.BurnCurrentCards();
            Debug.Log("Combat complete.");
            OnCombatComplete?.Invoke();
        }
        else if (!combatComplete) 
        {
            if (startedCombatSequences)
            {
                Debug.Log("Round complete.");
                CombatManager.instance.CombatAnimationManager.BurnCurrentCards();
                CombatManager.instance.RemoveEnergyFromMechs(currentCombatSequence.energyRemovalObject);
                OnRoundComplete?.Invoke();
            }
            
            startedCombatSequences = true;
            currentCombatSequence = combatSequenceCollection.Dequeue();
        }
    }

    private void RunCurrentCombatSequence()
    {
        StartAnimations();
    }

    private void StartAnimations()
    {
        if(startedCombatSequences && animationsComplete)
        {
            CombatManager.instance.CombatAnimationManager.AddAnimationToQueue(currentCombatSequence.animationQueue.Dequeue());
            CombatManager.instance.CombatAnimationManager.PrepCardsToBurn(currentCombatSequence.cardBurnObject);
            animationsComplete = false;
        }
        
        if(!animationsComplete && CombatManager.instance.CombatAnimationManager.AnimationsComplete)
        {
            DealCombatEffects();
            animationsComplete = true;
        }
    }

    private void DealCombatEffects()
    {
        CombatManager.instance.EffectManager.EnableEffects(currentCombatSequence.preCombatEffectQueue.Dequeue());
        CombatManager.instance.RemoveHealthFromMech(currentCombatSequence.damageQueue.Dequeue());
        CombatManager.instance.EffectManager.EnableEffects(currentCombatSequence.postCombatEffectQueue.Dequeue());
    }
}
