using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDialogueController : MonoBehaviour
{
    [Tooltip("Chance for one of the random dialogues to happen between rounds.")]
    [Range(1, 100)] [SerializeField] private int chanceForRandomDialogue;

    private string aIIntroDialogue;
    private string aIWinDialogue;
    private string aILoseDialogue;
    private List<string> fightDialogue = new List<string>();

    public delegate void onDialogueStarted();
    public static event onDialogueStarted OnDialogueStarted;

    public delegate void onDialogueComplete();
    public static event onDialogueComplete OnDialogueComplete;

    public void PlayIntroDialogue()
    {
        OnDialogueStarted?.Invoke();
        CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.OpponentFighter.FighterName, aIIntroDialogue);
    }

    public void PlayAIWinDialogue()
    {
        OnDialogueStarted?.Invoke();
        CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.OpponentFighter.FighterName, aIWinDialogue);
    }

    public void PlayAILoseDialogue()
    {
        OnDialogueStarted?.Invoke();
        CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.OpponentFighter.FighterName, aILoseDialogue);
    }

    public void CheckPlayDialogue()
    {
        int roll = Random.Range(1, 101);

        if(roll >= chanceForRandomDialogue)
        {
            roll = Random.Range(0, fightDialogue.Count);

            OnDialogueStarted?.Invoke();
            CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.OpponentFighter.FighterName, fightDialogue[roll]);
        }
        else
        {
            OnDialogueComplete?.Invoke();
        }
    }

    private void Start()
    {
        PilotEffectManager.OnTurnComplete += CheckPlayDialogue;
        AIDialoguePopupController.OnAIDialogueComplete += OnAIDialoguePopupComplete;
    }

    private void OnDestroy()
    {
        PilotEffectManager.OnTurnComplete -= CheckPlayDialogue;
        AIDialoguePopupController.OnAIDialogueComplete -= OnAIDialoguePopupComplete;
    }

    public void LoadCombatDialogue(SOAIDialogueObject opponentDialogue)
    {
        foreach (string newFightDialogue in opponentDialogue.RandomFightDialogue)
            fightDialogue.Add(newFightDialogue);

        aIIntroDialogue = opponentDialogue.IntroDialogue;
        aIWinDialogue = opponentDialogue.AIWinDialogue;
        aILoseDialogue = opponentDialogue.AILoseDialogue;
    }

    private void OnAIDialoguePopupComplete()
    {
        OnDialogueComplete?.Invoke();
    }
}
