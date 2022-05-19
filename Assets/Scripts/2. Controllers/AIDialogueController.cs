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
    private int dialogueIndex = 0;

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
            GetRandomDialogue(aIWinDialogue), CharacterSelect.Opponent);
    }

    public void PlayAILoseDialogue()
    {
        OnDialogueStarted?.Invoke();
        CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.OpponentFighter.FighterName, 
            GetRandomDialogue(aILoseDialogue), CharacterSelect.Opponent);
    }

    public void CheckPlayDialogue()
    {
        if(CombatManager.instance.GameOver || GameManager.instance.SceneController.CheckIsTutorialScene())
        {
            OnDialogueComplete?.Invoke();
            return;
        }

        int roll = Random.Range(1, 101);

        if(roll >= chanceForRandomDialogue)
        {
            roll = Random.Range(0, fightDialogue.Count);

            OnDialogueStarted?.Invoke();
            CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.OpponentFighter.FighterName, 
                fightDialogue[roll], CharacterSelect.Opponent);
        }
        else
        {
            OnDialogueComplete?.Invoke();
        }
    }

    public void PlayDialogueInOrder()
    {
        if(dialogueIndex == fightDialogue.Count - 1)
        {
            OnDialogueComplete?.Invoke();
            return;
        }

        CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.OpponentFighter.FighterName, 
            fightDialogue[dialogueIndex], CharacterSelect.Opponent);
        dialogueIndex++;
        OnDialogueStarted?.Invoke();
    }

    private void Start()
    {
        PilotEffectManager.OnTurnComplete += CheckPlayDialogue;
        AIDialoguePopupController.OnAIDialogueComplete += OnAIDialoguePopupComplete;
        AIConversationPopupController.OnAIConversationComplete += OnAIDialoguePopupComplete;
    }

    private void OnDestroy()
    {
        PilotEffectManager.OnTurnComplete -= CheckPlayDialogue;
        AIDialoguePopupController.OnAIDialogueComplete -= OnAIDialoguePopupComplete;
        AIConversationPopupController.OnAIConversationComplete -= OnAIDialoguePopupComplete;
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
