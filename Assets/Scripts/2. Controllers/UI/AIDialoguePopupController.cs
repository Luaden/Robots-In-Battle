using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AIDialoguePopupController : BaseUIElement<string, string>
{
    [Header("Default Opponent Popup")]
    [SerializeField] private TMP_Text smallPopupNameText;
    [SerializeField] private TMP_Text smallPopupDialogueText;
    [Space]
    [Header("Opponent Big Popup")]
    [SerializeField] private Image opponentHairImage;
    [SerializeField] private Image opponentBodyImage;
    [SerializeField] private Image opponentEyesImage;
    [SerializeField] private Image opponentNoseImage;
    [SerializeField] private Image opponentMouthImage;
    [SerializeField] private Image opponentClothesImage;
    [SerializeField] private TMP_Text bigPopupOpponentNameText;
    [SerializeField] private TMP_Text bigPopupOpponentDialogueText;
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
    [Space]
    [Header("Global Popup Objects")]
    [SerializeField] private GameObject dialogueButton;
    [SerializeField] private GameObject bigPopupObject;
    [SerializeField] private GameObject popupObject;


    private CharacterSelect characterSpeaking;

    public delegate void onAIDialogueComplete();
    public static event onAIDialogueComplete OnAIDialogueComplete;

    private string currentDialogue;
    private Queue<char> completeDialogue;
    private bool completeTextNow;
    private float currentTimer = 0f;

    public override void UpdateUI(string primaryData, string secondaryData)
    {
        if (ClearedIfEmpty(primaryData, secondaryData))
            return;

        smallPopupNameText.text = primaryData;

        foreach(char letter in secondaryData)
            completeDialogue.Enqueue(letter);

        dialogueButton.SetActive(true);
        popupObject.SetActive(true);
    }

    public void UpdateUI(SOCompleteCharacter primaryData, string secondaryData, CharacterSelect character)
    {
        if (ClearedIfEmpty(primaryData, secondaryData))
            return;

        if(character == CharacterSelect.Opponent)
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
        }

        if(character == CharacterSelect.Player)
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
        }
    }

    public void SkipText()
    {
        if (completeDialogue.Count > 0)
        {
            int letterCount = completeDialogue.Count;
            for (int i = 0; i < letterCount; i++)
                currentDialogue += completeDialogue.Dequeue();

            smallPopupDialogueText.text = currentDialogue;

            if(characterSpeaking == CharacterSelect.Opponent)
            {
                bigPopupOpponentDialogueText.text = currentDialogue;
            }

            if(characterSpeaking == CharacterSelect.Player)
            {
                bigPopupPlayerDialogueText.text = currentDialogue;
            }
            return;
        }
        else
        {
            OnAIDialogueComplete?.Invoke();
            UpdateUI(null, null);
        }
    }

    protected override bool ClearedIfEmpty(string newData, string secondNewData)
    {
        if (newData == null || secondNewData == null)
        {
            smallPopupNameText.text = string.Empty;
            smallPopupDialogueText.text = string.Empty;
            currentDialogue = string.Empty;
            completeDialogue = new Queue<char>();

            dialogueButton.SetActive(false);
            popupObject.SetActive(false);

            return true;
        }

        if (newData.Length == 0 || secondNewData.Length == 0)
        {
            smallPopupNameText.text = string.Empty;
            smallPopupDialogueText.text = string.Empty;
            currentDialogue = string.Empty;
            completeDialogue = new Queue<char>();

            dialogueButton.SetActive(false);
            popupObject.SetActive(false);

            return true;
        }

        return false;
    }

    protected bool ClearedIfEmpty(SOCompleteCharacter newData, string secondNewData)
    {
        if (newData == null || secondNewData == null)
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
        if(completeDialogue.Count != 0)
        {
            if(CheckTimer())
            {
                currentDialogue += completeDialogue.Dequeue();
                smallPopupDialogueText.text = currentDialogue;
                bigPopupOpponentDialogueText.text = currentDialogue;
            }
        }
    }

    private bool CheckTimer()
    {
        currentTimer += Time.deltaTime;
        if(currentTimer >= CombatManager.instance.PopupUIManager.TextPace)
        {
            currentTimer = 0f;
            return true;
        }

        return false;
    }
}
