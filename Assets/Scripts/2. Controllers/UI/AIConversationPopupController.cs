using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AIConversationPopupController : BaseUIElement<ConversationObject>
{
    [Header("Opponent Big Popup")]
    [SerializeField] private Image opponentHairImage;
    [SerializeField] private Image opponentBodyImage;
    [SerializeField] private Image opponentEyesImage;
    [SerializeField] private Image opponentNoseImage;
    [SerializeField] private Image opponentMouthImage;
    [SerializeField] private Image opponentClothesImage;
    [SerializeField] private TMP_Text opponentNameText;
    [SerializeField] private TMP_Text opponentDialogueText;
    [SerializeField] private GameObject opponentDialoguePopupObject;
    [Space]
    [Header("Player Big Popup")]
    [SerializeField] private Image playerHairImage;
    [SerializeField] private Image playerBodyImage;
    [SerializeField] private Image playerEyesImage;
    [SerializeField] private Image playerNoseImage;
    [SerializeField] private Image playerMouthImage;
    [SerializeField] private Image playerClothesImage;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text playerDialogueText;
    [SerializeField] private GameObject playerDialoguePopupObject;
    [Space]
    [SerializeField] private GameObject conversationButton;
    [SerializeField] private GameObject bigPopupObject;

    private CharacterSelect characterSpeaking;

    public delegate void onAIDialogueComplete();
    public static event onAIDialogueComplete OnAIDialogueComplete;

    Queue<string> dialogueChain = new Queue<string>();

    private GameObject currentDialoguePopupObject;
    private TMP_Text currentDialogueText; 

    private string currentDialogue;
    private Queue<char> completeDialogue;
    private float currentTimer = 0f;

    public override void UpdateUI(ConversationObject primaryData)
    {
        Debug.Log("New conversation starting.");
        if (ClearedIfEmpty(primaryData))
            return;

        if (primaryData.firstCharacterIsPlayer)
        {
            AssignPlayerSprites(primaryData.firstCharacter);
            AssignOpponentSprites(primaryData.secondCharacter);

            playerNameText.text = primaryData.firstCharacter.PilotName;
            opponentNameText.text = primaryData.secondCharacter.PilotName;

            if(primaryData.firstCharacterStartsDialogue)
            {
                characterSpeaking = CharacterSelect.Player;

                currentDialogueText = playerDialogueText;
                currentDialoguePopupObject = playerDialoguePopupObject;

                for(int i = 0; i < primaryData.firstCharacterDialogue.Count; i++)
                {
                    dialogueChain.Enqueue(primaryData.firstCharacterDialogue[i]);
                    dialogueChain.Enqueue(primaryData.secondCharacterDialogue[i]);
                }
            }
            else
            {
                characterSpeaking = CharacterSelect.Opponent;

                currentDialogueText = opponentDialogueText;
                currentDialoguePopupObject = opponentDialoguePopupObject;

                for(int i = 0; i < primaryData.firstCharacterDialogue.Count; i++)
                {
                    dialogueChain.Enqueue(primaryData.secondCharacterDialogue[i]);
                    dialogueChain.Enqueue(primaryData.firstCharacterDialogue[i]);
                }
            }
        }
        else
        {
            AssignPlayerSprites(primaryData.secondCharacter);
            AssignOpponentSprites(primaryData.firstCharacter);

            opponentNameText.text = primaryData.firstCharacter.PilotName;
            playerNameText.text = primaryData.secondCharacter.PilotName;

            if (primaryData.firstCharacterStartsDialogue)
            {
                characterSpeaking = CharacterSelect.Opponent;

                currentDialogueText = opponentDialogueText;
                currentDialoguePopupObject = opponentDialoguePopupObject;

                for (int i = 0; i < primaryData.firstCharacterDialogue.Count; i++)
                {
                    dialogueChain.Enqueue(primaryData.firstCharacterDialogue[i]);
                    dialogueChain.Enqueue(primaryData.secondCharacterDialogue[i]);
                }
            }
            else
            {
                characterSpeaking = CharacterSelect.Player;

                currentDialogueText = playerDialogueText;
                currentDialoguePopupObject = playerDialoguePopupObject;

                for (int i = 0; i < primaryData.firstCharacterDialogue.Count; i++)
                {
                    dialogueChain.Enqueue(primaryData.secondCharacterDialogue[i]);
                    dialogueChain.Enqueue(primaryData.firstCharacterDialogue[i]);
                }
            }
        }

        foreach (char letter in dialogueChain.Dequeue())
            completeDialogue.Enqueue(letter);

        conversationButton.SetActive(true);
        bigPopupObject.SetActive(true);
        currentDialoguePopupObject.SetActive(true);
        Debug.Log(dialogueChain.Count);
    }

    public void SkipText()
    {
        if (completeDialogue.Count > 0)
        {
            int letterCount = completeDialogue.Count;
            for (int i = 0; i < letterCount; i++)
                currentDialogue += completeDialogue.Dequeue();

            currentDialogueText.text = currentDialogue;

            return;
        }

        if (completeDialogue.Count == 0 && dialogueChain.Count > 0)
        {
            if(characterSpeaking == CharacterSelect.Player)
            {
                Debug.Log("Player speaking.");
                currentDialoguePopupObject.SetActive(false);
                currentDialogueText.text = string.Empty;

                currentDialoguePopupObject = opponentDialoguePopupObject;
                currentDialogueText = opponentDialogueText;

                string newDialogue = dialogueChain.Dequeue();
                completeDialogue.Clear();
                currentDialogue = string.Empty;

                foreach (char letter in newDialogue)
                    completeDialogue.Enqueue(letter);

                currentDialoguePopupObject.SetActive(true);
                characterSpeaking = CharacterSelect.Opponent;
                return;
            }
            else
            {
                Debug.Log("Opponent speaking.");
                currentDialoguePopupObject.SetActive(false);
                currentDialogueText.text = string.Empty;

                currentDialoguePopupObject = playerDialoguePopupObject;
                currentDialogueText = playerDialogueText;

                string newDialogue = dialogueChain.Dequeue();
                completeDialogue.Clear();
                currentDialogue = string.Empty;

                foreach (char letter in newDialogue)
                    completeDialogue.Enqueue(letter);

                currentDialoguePopupObject.SetActive(true);
                characterSpeaking = CharacterSelect.Player;
                return;
            }
        }

        if (completeDialogue.Count == 0 && dialogueChain.Count == 0)
        {
            Debug.Log("Dialogue Complete.");
            OnAIDialogueComplete?.Invoke();
            UpdateUI(null);
        }
    }

    protected override bool ClearedIfEmpty(ConversationObject newData)
    {
        if (newData == null)
        {
            playerNameText.text = string.Empty;
            playerDialogueText.text = string.Empty;
            opponentNameText.text = string.Empty;
            opponentDialogueText.text = string.Empty;
            currentDialoguePopupObject = null;
            currentDialogue = string.Empty;
            completeDialogue = new Queue<char>();

            playerDialoguePopupObject.SetActive(false);
            opponentDialoguePopupObject.SetActive(false);

            conversationButton.SetActive(false);
            bigPopupObject.SetActive(false);

            return true;
        }

        return false;
    }

    private void Start()
    {
        completeDialogue = new Queue<char>();
        dialogueChain = new Queue<string>();
    }

    private void Update()
    {
        UpdateTextOverTime();
    }

    private void UpdateTextOverTime()
    {
        if (completeDialogue.Count != 0)
        {
            if (CheckTimer())
            {
                currentDialogue += completeDialogue.Dequeue();
                currentDialogueText.text = currentDialogue;
            }
        }
    }

    private bool CheckTimer()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= CombatManager.instance.PopupUIManager.TextPace)
        {
            currentTimer = 0f;
            return true;
        }

        return false;
    }

    private void AssignPlayerSprites(SOCompleteCharacter character)
    {
        if (character.PilotUIObject.FighterHair == null)
            playerHairImage.color = new Color(1, 1, 1, 0);
        playerHairImage.sprite = GameManager.instance.Player.CompletePilot.PilotUIObject.FighterHair;
        playerHairImage.SetNativeSize();

        if (character.PilotUIObject.FighterEyes == null)
            playerEyesImage.color = new Color(1, 1, 1, 0);
        playerEyesImage.sprite = character.PilotUIObject.FighterEyes;
        playerEyesImage.SetNativeSize();

        if (character.PilotUIObject.FighterNose == null)
            playerNoseImage.color = new Color(1, 1, 1, 0);
        playerNoseImage.sprite = character.PilotUIObject.FighterNose;
        playerNoseImage.SetNativeSize();

        if (character.PilotUIObject.FighterMouth == null)
            playerMouthImage.color = new Color(1, 1, 1, 0);
        playerMouthImage.sprite = character.PilotUIObject.FighterMouth;
        playerMouthImage.SetNativeSize();

        if (character.PilotUIObject.FighterClothes == null)
            playerClothesImage.color = new Color(1, 1, 1, 0);
        playerClothesImage.sprite = character.PilotUIObject.FighterClothes;
        playerClothesImage.SetNativeSize();

        if (character.PilotUIObject.FighterBody == null)
            playerBodyImage.color = new Color(1, 1, 1, 0);
        playerBodyImage.sprite = character.PilotUIObject.FighterBody;
        playerBodyImage.SetNativeSize();
    }

    private void AssignOpponentSprites(SOCompleteCharacter character)
    {
        if (character.PilotUIObject.FighterHair == null)
            opponentHairImage.color = new Color(1, 1, 1, 0);
        opponentHairImage.sprite = character.PilotUIObject.FighterHair;
        opponentHairImage.SetNativeSize();

        if (character.PilotUIObject.FighterEyes == null)
            opponentEyesImage.color = new Color(1, 1, 1, 0);
        opponentEyesImage.sprite = character.PilotUIObject.FighterEyes;
        opponentEyesImage.SetNativeSize();

        if (character.PilotUIObject.FighterNose == null)
            opponentNoseImage.color = new Color(1, 1, 1, 0);
        opponentNoseImage.sprite = character.PilotUIObject.FighterNose;
        opponentNoseImage.SetNativeSize();

        if (character.PilotUIObject.FighterMouth == null)
            opponentMouthImage.color = new Color(1, 1, 1, 0);
        opponentMouthImage.sprite = character.PilotUIObject.FighterMouth;
        opponentMouthImage.SetNativeSize();

        if (character.PilotUIObject.FighterClothes == null)
            opponentClothesImage.color = new Color(1, 1, 1, 0);
        opponentClothesImage.sprite = character.PilotUIObject.FighterClothes;
        opponentClothesImage.SetNativeSize();

        if (character.PilotUIObject.FighterBody == null)
            opponentBodyImage.color = new Color(1, 1, 1, 0);
        opponentBodyImage.sprite = character.PilotUIObject.FighterBody;
        opponentBodyImage.SetNativeSize();
    }
}
