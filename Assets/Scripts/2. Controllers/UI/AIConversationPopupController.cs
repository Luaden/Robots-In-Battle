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
        if (ClearedIfEmpty(primaryData))
            return;

        if (primaryData.firstCharacterIsPlayer)
        {
            AssignPlayerSprites(primaryData.firstCharacter);
            AssignOpponentSprites(primaryData.secondCharacter);

            playerNameText.text = primaryData.firstCharacter.FighterName;
            opponentNameText.text = primaryData.secondCharacter.FighterName;

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

            opponentNameText.text = primaryData.firstCharacter.FighterName;
            playerNameText.text = primaryData.secondCharacter.FighterName;

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

    private void AssignPlayerSprites(FighterDataObject fighter)
    {
        if (fighter.FighterUIObject.FighterHair == null)
            playerHairImage.color = new Color(1, 1, 1, 0);
        playerHairImage.sprite = fighter.FighterUIObject.FighterHair;
        playerHairImage.SetNativeSize();

        if (fighter.FighterUIObject.FighterEyes == null)
            playerEyesImage.color = new Color(1, 1, 1, 0);
        playerEyesImage.sprite = fighter.FighterUIObject.FighterEyes;
        playerEyesImage.SetNativeSize();

        if (fighter.FighterUIObject.FighterNose == null)
            playerNoseImage.color = new Color(1, 1, 1, 0);
        playerNoseImage.sprite = fighter.FighterUIObject.FighterNose;
        playerNoseImage.SetNativeSize();

        if (fighter.FighterUIObject.FighterMouth == null)
            playerMouthImage.color = new Color(1, 1, 1, 0);
        playerMouthImage.sprite = fighter.FighterUIObject.FighterMouth;
        playerMouthImage.SetNativeSize();

        if (fighter.FighterUIObject.FighterClothes == null)
            playerClothesImage.color = new Color(1, 1, 1, 0);
        playerClothesImage.sprite = fighter.FighterUIObject.FighterClothes;
        playerClothesImage.SetNativeSize();

        if (fighter.FighterUIObject.FighterBody == null)
            playerBodyImage.color = new Color(1, 1, 1, 0);
        playerBodyImage.sprite = fighter.FighterUIObject.FighterBody;
        playerBodyImage.SetNativeSize();
    }

    private void AssignOpponentSprites(FighterDataObject fighter)
    {
        if (fighter.FighterUIObject.FighterHair == null)
            opponentHairImage.color = new Color(1, 1, 1, 0);
        opponentHairImage.sprite = fighter.FighterUIObject.FighterHair;
        opponentHairImage.SetNativeSize();

        if (fighter.FighterUIObject.FighterEyes == null)
            opponentEyesImage.color = new Color(1, 1, 1, 0);
        opponentEyesImage.sprite = fighter.FighterUIObject.FighterEyes;
        opponentEyesImage.SetNativeSize();

        if (fighter.FighterUIObject.FighterNose == null)
            opponentNoseImage.color = new Color(1, 1, 1, 0);
        opponentNoseImage.sprite = fighter.FighterUIObject.FighterNose;
        opponentNoseImage.SetNativeSize();

        if (fighter.FighterUIObject.FighterMouth == null)
            opponentMouthImage.color = new Color(1, 1, 1, 0);
        opponentMouthImage.sprite = fighter.FighterUIObject.FighterMouth;
        opponentMouthImage.SetNativeSize();

        if (fighter.FighterUIObject.FighterClothes == null)
            opponentClothesImage.color = new Color(1, 1, 1, 0);
        opponentClothesImage.sprite = fighter.FighterUIObject.FighterClothes;
        opponentClothesImage.SetNativeSize();

        if (fighter.FighterUIObject.FighterBody == null)
            opponentBodyImage.color = new Color(1, 1, 1, 0);
        opponentBodyImage.sprite = fighter.FighterUIObject.FighterBody;
        opponentBodyImage.SetNativeSize();
    }
}
