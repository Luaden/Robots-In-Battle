using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AIConversationPopupController : BaseUIElement<SOCompleteCharacter, string, MechSelect>
{
    [Header("Opponent Big Popup")]
    [SerializeField] private Image opponentHairImage;
    [SerializeField] private Image opponentBodyImage;
    [SerializeField] private Image opponentEyesImage;
    [SerializeField] private Image opponentNoseImage;
    [SerializeField] private Image opponentMouthImage;
    [SerializeField] private Image opponentClothesImage;
    [SerializeField] private TMP_Text bigPopupOpponentNameText;
    [SerializeField] private TMP_Text bigPopupOpponentDialogueText;
    [SerializeField] private GameObject opponentDialoguePopupObject;
    [Space]
    [Header("Player Big Popup")]
    [SerializeField] private Image playerHairImage;
    [SerializeField] private Image playerBodyImage;
    [SerializeField] private Image playerEyesImage;
    [SerializeField] private Image playerNoseImage;
    [SerializeField] private Image playerMouthImage;
    [SerializeField] private Image playerClothesImage;
    [SerializeField] private TMP_Text bigPopupPlayerNameText;
    [SerializeField] private TMP_Text bigPopupPlayerDialogueText;
    [SerializeField] private GameObject playerDialoguePopupObject;
    [Space]
    [SerializeField] private GameObject dialogueButton;
    [SerializeField] private GameObject bigPopupObject;

    private MechSelect characterSpeaking;

    public delegate void onAIDialogueComplete();
    public static event onAIDialogueComplete OnAIDialogueComplete;

    private string currentDialogue;
    private Queue<char> completeDialogue;
    private float currentTimer = 0f;

    public override void UpdateUI(SOCompleteCharacter primaryData, string secondaryData, MechSelect character)
    {
        if (ClearedIfEmpty(primaryData, secondaryData, character))
            return;

        if (character == MechSelect.Opponent)
        {
            characterSpeaking = character;
            bigPopupOpponentNameText.text = primaryData.PilotName;

            if (primaryData.PilotUIObject.FighterHair == null)
                opponentHairImage.color = new Color(1, 1, 1, 0);
            opponentHairImage.sprite = primaryData.PilotUIObject.FighterHair;
            opponentHairImage.SetNativeSize();

            if (primaryData.PilotUIObject.FighterEyes == null)
                opponentEyesImage.color = new Color(1, 1, 1, 0);
            opponentEyesImage.sprite = primaryData.PilotUIObject.FighterEyes;
            opponentEyesImage.SetNativeSize();

            if (primaryData.PilotUIObject.FighterNose == null)
                opponentNoseImage.color = new Color(1, 1, 1, 0);
            opponentNoseImage.sprite = primaryData.PilotUIObject.FighterNose;
            opponentNoseImage.SetNativeSize();

            if (primaryData.PilotUIObject.FighterMouth == null)
                opponentMouthImage.color = new Color(1, 1, 1, 0);
            opponentMouthImage.sprite = primaryData.PilotUIObject.FighterMouth;
            opponentMouthImage.SetNativeSize();

            if (primaryData.PilotUIObject.FighterClothes == null)
                opponentClothesImage.color = new Color(1, 1, 1, 0);
            opponentClothesImage.sprite = primaryData.PilotUIObject.FighterClothes;
            opponentClothesImage.SetNativeSize();

            if (primaryData.PilotUIObject.FighterBody == null)
                opponentBodyImage.color = new Color(1, 1, 1, 0);
            opponentBodyImage.sprite = primaryData.PilotUIObject.FighterBody;
            opponentBodyImage.SetNativeSize();

            foreach (char letter in secondaryData)
                completeDialogue.Enqueue(letter);

            dialogueButton.SetActive(true);
            bigPopupObject.SetActive(true);
            opponentDialoguePopupObject.SetActive(true);
        }

        if (character == MechSelect.Player)
        {
            characterSpeaking = character;
            bigPopupPlayerNameText.text = primaryData.PilotName;

            if (primaryData.PilotUIObject.FighterHair == null)
                playerHairImage.color = new Color(1, 1, 1, 0);
            playerHairImage.sprite = primaryData.PilotUIObject.FighterHair;
            playerHairImage.SetNativeSize();

            if (primaryData.PilotUIObject.FighterEyes == null)
                playerEyesImage.color = new Color(1, 1, 1, 0);
            playerEyesImage.sprite = primaryData.PilotUIObject.FighterEyes;
            playerEyesImage.SetNativeSize();

            if (primaryData.PilotUIObject.FighterNose == null)
                playerNoseImage.color = new Color(1, 1, 1, 0);
            playerNoseImage.sprite = primaryData.PilotUIObject.FighterNose;
            playerNoseImage.SetNativeSize();

            if (primaryData.PilotUIObject.FighterMouth == null)
                playerMouthImage.color = new Color(1, 1, 1, 0);
            playerMouthImage.sprite = primaryData.PilotUIObject.FighterMouth;
            playerMouthImage.SetNativeSize();

            if (primaryData.PilotUIObject.FighterClothes == null)
                playerClothesImage.color = new Color(1, 1, 1, 0);
            playerClothesImage.sprite = primaryData.PilotUIObject.FighterClothes;
            playerClothesImage.SetNativeSize();

            if (primaryData.PilotUIObject.FighterBody == null)
                playerBodyImage.color = new Color(1, 1, 1, 0);
            playerBodyImage.sprite = primaryData.PilotUIObject.FighterBody;
            playerBodyImage.SetNativeSize();

            foreach (char letter in secondaryData)
                completeDialogue.Enqueue(letter);

            dialogueButton.SetActive(true);
            bigPopupObject.SetActive(true);
            playerDialoguePopupObject.SetActive(true);
        }
    }

    public void SkipText()
    {
        if (completeDialogue.Count > 0)
        {
            int letterCount = completeDialogue.Count;
            for (int i = 0; i < letterCount; i++)
                currentDialogue += completeDialogue.Dequeue();

            if (characterSpeaking == MechSelect.Opponent)
            {
                bigPopupOpponentDialogueText.text = currentDialogue;
            }

            if (characterSpeaking == MechSelect.Player)
            {
                bigPopupPlayerDialogueText.text = currentDialogue;
            }
            return;
        }
        else
        {
            OnAIDialogueComplete?.Invoke();
            UpdateUI(null, null, MechSelect.None);
        }
    }

    protected override bool ClearedIfEmpty(SOCompleteCharacter newData, string secondNewData, MechSelect tertiaryData)
    {
        if (newData == null || secondNewData == null || tertiaryData == MechSelect.None)
        {
            bigPopupPlayerNameText.text = string.Empty;
            bigPopupPlayerDialogueText.text = string.Empty;
            bigPopupOpponentNameText.text = string.Empty;
            bigPopupOpponentDialogueText.text = string.Empty;
            currentDialogue = string.Empty;
            completeDialogue = new Queue<char>();

            dialogueButton.SetActive(false);
            bigPopupObject.SetActive(false);

            return true;
        }

        if (secondNewData.Length == 0)
        {
            bigPopupOpponentNameText.text = string.Empty;
            bigPopupOpponentDialogueText.text = string.Empty;
            currentDialogue = string.Empty;
            completeDialogue = new Queue<char>();

            dialogueButton.SetActive(false);
            bigPopupObject.SetActive(false);

            return true;
        }

        return false;
    }

    private void Start()
    {
        completeDialogue = new Queue<char>();
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

                if(characterSpeaking == MechSelect.Opponent)
                {
                    bigPopupOpponentDialogueText.text = currentDialogue;
                }
                if(characterSpeaking == MechSelect.Player)
                {
                    bigPopupPlayerDialogueText.text = currentDialogue;
                }
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
}
