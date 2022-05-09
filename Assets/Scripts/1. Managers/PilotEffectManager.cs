using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotEffectManager : MonoBehaviour
{
    private FighterDataObject playerFighter;
    private FighterDataObject opponentFighter;

    private ActiveEffects playerEffects;
    private ActiveEffects opponentEffects;

    public delegate void onRoundEnded();
    public static event onRoundEnded OnRoundEnded;

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

    private void OnDestroy()
    {
        CombatSequenceManager.OnCombatComplete -= OnCombatComplete;
    }

    [ContextMenu("Check Effects")]
    private void OnCombatComplete()
    {
        EnablePilotCombatEffects(CharacterSelect.Player, playerEffects);
        EnablePilotCombatEffects(CharacterSelect.Opponent, opponentEffects);
        
        //If animations/sequence complete, OnRoundEnded?.Invoke();
    }

    private void OnFightComplete()
    {

    }

    private void EnablePilotCombatEffects(CharacterSelect character, ActiveEffects effects)
    {
        if (effects.HasFlag(ActiveEffects.Jazzersize))
            Debug.Log("One and two and three and four.");
    }
}
