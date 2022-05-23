using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSequenceManager : MonoBehaviour
{
    private CombatQueueObject currentCombatSequence;
    private Queue<CombatQueueObject> combatSequenceCollection;

    public delegate void onStartCombat();
    public static event onStartCombat OnCombatStart;
    public delegate void onRoundComplete();
    public static event onRoundComplete OnRoundComplete;
    public delegate void onCombatComplete();
    public static event onCombatComplete OnCombatComplete;

    private bool startedCombatSequences = false;
    private bool animationsComplete = true;
    private bool combatComplete = true;

    public void AddCombatSequenceToQueue(CombatQueueObject newCombatSequence)
    {
        if (currentCombatSequence == null)
            OnCombatStart?.Invoke();

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
            OnCombatComplete?.Invoke();
        }
        else if (!combatComplete)
        {
            if (startedCombatSequences)
            {
                CombatManager.instance.CombatAnimationManager.BurnCurrentCards();
                OnRoundComplete?.Invoke();
            }

            startedCombatSequences = true;

            if (combatSequenceCollection.Count > 0)
            {
                currentCombatSequence = combatSequenceCollection.Dequeue();
            }
        }
    }

    private void RunCurrentCombatSequence()
    {
        if (startedCombatSequences && animationsComplete)
        {
            if (currentCombatSequence.damageQueue.Peek() != null)
            {
                if (currentCombatSequence.damageQueue.Peek().CardCharacterPairA.cardChannelPair.CardData.ApplyEffectsFirst && !currentCombatSequence.damageQueue.Peek().DenyOffensiveEffects)
                    CombatManager.instance.CombatEffectManager.EnableCombatEffects(currentCombatSequence.damageQueue.Peek().CardCharacterPairA);
                if (currentCombatSequence.damageQueue.Peek().CardCharacterPairB != null &&
                    currentCombatSequence.damageQueue.Peek().CardCharacterPairB.cardChannelPair.CardData.ApplyEffectsFirst)
                    CombatManager.instance.CombatEffectManager.EnableCombatEffects(currentCombatSequence.damageQueue.Peek().CardCharacterPairB);
            }

            CombatManager.instance.PopupUIManager.HandlePopup(currentCombatSequence.damageQueue.Peek());
            CombatManager.instance.PopupUIManager.HandlePopup(currentCombatSequence.damageQueue.Peek().CardCharacterPairA.cardChannelPair);
            CombatManager.instance.CombatAnimationManager.SetMechAnimation(currentCombatSequence.animationQueue.Dequeue());
            CombatManager.instance.CombatAnimationManager.PrepCardsToBurn(currentCombatSequence.cardBurnObject);
            animationsComplete = false;
        }

        if (!animationsComplete && CombatManager.instance.CombatAnimationManager.AnimationsComplete)
        {
            DealCombatEffects();
            animationsComplete = true;
        }
    }

    private void DealCombatEffects()
    {
        if (currentCombatSequence.damageQueue.Count > 0)
        {
            DamageMechPairObject currentDamage = currentCombatSequence.damageQueue.Dequeue();            
            CombatManager.instance.RemoveHealthFromMech(currentDamage);
            if (!currentDamage.CardCharacterPairA.cardChannelPair.CardData.ApplyEffectsFirst && !currentDamage.DenyOffensiveEffects)
                CombatManager.instance.CombatEffectManager.EnableCombatEffects(currentDamage.CardCharacterPairA);
            if (currentDamage.CardCharacterPairB != null &&
                !currentDamage.CardCharacterPairB.cardChannelPair.CardData.ApplyEffectsFirst)
                CombatManager.instance.CombatEffectManager.EnableCombatEffects(currentDamage.CardCharacterPairB);

            CombatManager.instance.RemoveEnergyFromMechs(currentCombatSequence.energyRemovalObject);
        }
    }
}
