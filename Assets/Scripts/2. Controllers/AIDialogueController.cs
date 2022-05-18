using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDialogueController : MonoBehaviour
{
    [Tooltip("Chance for one of the random dialogues to happen between rounds.")]
    [Range(1, 100)] [SerializeField] private int chanceForRandomDialogue;

    private List<string> aIIntroDialogue = new List<string>();
    private List<string> aIWinDialogue = new List<string>();
    private List<string> aILoseDialogue = new List<string>();
    private List<string> fightDialogue = new List<string>();

    public delegate void onDialogueStarted();
    public static event onDialogueStarted OnDialogueStarted;

    public delegate void onDialogueComplete();
    public static event onDialogueComplete OnDialogueComplete;

    public void PlayIntroDialogue()
    {
        OnDialogueStarted?.Invoke();

        ConversationObject newConversation = new ConversationObject();
        newConversation.firstCharacter = CombatManager.instance.OpponentFighter;
        newConversation.secondCharacter = CombatManager.instance.PlayerFighter;
        newConversation.firstCharacterStartsDialogue = true;
        newConversation.firstCharacterIsPlayer = false;
        newConversation.firstCharacterDialogue.Add(GetRandomDialogue(aIIntroDialogue));
        newConversation.secondCharacterDialogue.Add(
            GetRandomDialogue(CombatManager.instance.PlayerFighter.FighterCompleteCharacter.DialogueModule.IntroResponseDialogue));

        CombatManager.instance.PopupUIManager.HandlePopup(newConversation);
    }

    public void PlayAIWinDialogue()
    {
        OnDialogueStarted?.Invoke();
        CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.OpponentFighter.FighterName, 
            GetRandomDialogue(aIWinDialogue));
    }

    public void PlayAILoseDialogue()
    {
        OnDialogueStarted?.Invoke();
        CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.OpponentFighter.FighterName, 
            GetRandomDialogue(aILoseDialogue));
    }

    public void CheckPlayDialogue()
    {
        if(CombatManager.instance.GameOver)
        {
            OnDialogueComplete?.Invoke();
            return;
        }

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
        AIConversationPopupController.OnAIDialogueComplete += OnAIDialoguePopupComplete;
    }

    private void OnDestroy()
    {
        PilotEffectManager.OnTurnComplete -= CheckPlayDialogue;
        AIDialoguePopupController.OnAIDialogueComplete -= OnAIDialoguePopupComplete;
        AIConversationPopupController.OnAIDialogueComplete -= OnAIDialoguePopupComplete;
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

    private string GetRandomDialogue(List<string> dialogueChoices)
    {
        return dialogueChoices[Random.Range(0, dialogueChoices.Count)];
    }
}
