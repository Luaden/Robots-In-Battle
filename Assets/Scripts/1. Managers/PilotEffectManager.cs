using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotEffectManager : MonoBehaviour
{
    [SerializeField] private float effectDelayTimer;
    private float currentTimer;

    private FighterDataObject playerFighter;
    private FighterDataObject opponentFighter;

    private bool activatingPlayerEffects = false;
    private bool activatingOpponentEffects = false;
    private bool effectsComplete = false;
    private bool checkingEffects = false;

    private ActiveEffects playerEffects;
    private ActiveEffects opponentEffects;

    public delegate void onTurnComplete();
    public static event onTurnComplete OnTurnComplete;

    public void InitPilotEffectManager()
    {
        playerFighter = CombatManager.instance.PlayerFighter;
        opponentFighter = CombatManager.instance.OpponentFighter;

        playerEffects = playerFighter.FighterActiveEffects;
        opponentEffects = opponentFighter.FighterActiveEffects;
    }

    private void Start()
    {
        CombatSequenceManager.OnCombatComplete += OnCombatComplete;
    }

    private void Update()
    {
        EnablePilotEffects();
    }

    private void OnDestroy()
    {
        CombatSequenceManager.OnCombatComplete -= OnCombatComplete;
    }

    [ContextMenu("Check Effects")]
    private void OnCombatComplete()
    {
        if (CombatManager.instance.GameOver)
        {
            OnTurnComplete?.Invoke();
            return;
        }

        effectsComplete = false;
        checkingEffects = true;
        activatingPlayerEffects = true;
        activatingOpponentEffects = true;
        currentTimer = 0f;
    }

    private void EnablePilotEffects()
    {
        if (effectsComplete || !CombatManager.instance.CombatAnimationManager.AnimationsComplete)
            return;

        if(EffectTimer())
            return;

        if(activatingPlayerEffects)
        {
            if (playerEffects.HasFlag(ActiveEffects.Jazzersize))
            {
                CombatManager.instance.CombatAnimationManager.SetMechAnimation(new AnimationQueueObject(CharacterSelect.Player, AnimationType.SpecialMid,
                                                                                                           CharacterSelect.Opponent, AnimationType.Idle));
                CombatManager.instance.CombatEffectManager.EnablePilotEffects(CharacterSelect.Player, ActiveEffects.Jazzersize);

                activatingPlayerEffects = false;
                return;
            }
            
            activatingPlayerEffects = false;
        }

        if (activatingOpponentEffects)
        {
            if (opponentEffects.HasFlag(ActiveEffects.Jazzersize))
            {
                CombatManager.instance.CombatAnimationManager.SetMechAnimation(new AnimationQueueObject(CharacterSelect.Opponent, AnimationType.SpecialMid,
                                                                                                           CharacterSelect.Player, AnimationType.Idle));
                CombatManager.instance.CombatEffectManager.EnablePilotEffects(CharacterSelect.Opponent, ActiveEffects.Jazzersize);

                activatingOpponentEffects = false;
                return;
            }

            activatingOpponentEffects = false;
        }

        if(!activatingPlayerEffects && !activatingOpponentEffects && checkingEffects)
        {
            effectsComplete = true;
            checkingEffects = false;
            OnTurnComplete?.Invoke();
        }
    }

    private bool EffectTimer()
    {
        currentTimer += Time.deltaTime;

        if (currentTimer >= effectDelayTimer)
        {
            currentTimer = 0f;
            return false;
        }
        else
            return true;
    }
}
