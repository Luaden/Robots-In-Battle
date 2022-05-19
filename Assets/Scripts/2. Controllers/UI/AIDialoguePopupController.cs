using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AIDialoguePopupController : BaseUIElement<string, string, CharacterSelect>
{
    [Header("Default Player Popup")]
    [SerializeField] private TMP_Text playerPopupNameText;
    [SerializeField] private TMP_Text playerPopupDialogueText;
    [Space]
    [Header("Default Opponent Popup")]
    [SerializeField] private TMP_Text opponentPopupNameText;
    [SerializeField] private TMP_Text opponentPopupDialogueText;
    [Space]
    [Header("Global Popup Objects")]
    [SerializeField] private GameObject dialogueButton;
    [SerializeField] private GameObject opponentPopupObject;
    [SerializeField] private GameObject playerPopupObject;

    public delegate void onAIDialogueComplete();
    public static event onAIDialogueComplete OnAIDialogueComplete;

    private CharacterSelect currentSpeaker;
    private string currentDialogue;
    private Queue<char> completeDialogue;
    private float currentTimer = 0f;

    public override void UpdateUI(string primaryData, string secondaryData, CharacterSelect character)
    {
        if (ClearedIfEmpty(primaryData, secondaryData, character))
            return;

        currentSpeaker = character;

        if (character == CharacterSelect.Opponent)
        {
            opponentPopupObject.SetActive(true);
            opponentPopupNameText.text = primaryData;
        }
        else
        {
            playerPopupNameText.text = primaryData;
            playerPopupObject.SetActive(true);
        }

        foreach (char letter in secondaryData)
            completeDialogue.Enqueue(letter);

        dialogueButton.SetActive(true);
    }

    public void SkipText()
    {
        if (completeDialogue.Count > 0)
        {
            int letterCount = completeDialogue.Count;

            for (int i = 0; i < letterCount; i++)
                currentDialogue += completeDialogue.Dequeue();

            if (currentSpeaker == CharacterSelect.Opponent)
                opponentPopupDialogueText.text = currentDialogue;
            else
                playerPopupDialogueText.text = currentDialogue;

            return;
        }
        else
        {
            OnAIDialogueComplete?.Invoke();
            UpdateUI(null, null, CharacterSelect.Opponent);
        }
    }

    protected override bool ClearedIfEmpty(string newData, string secondNewData, CharacterSelect character)
    {
        if (newData == null || secondNewData == null)
        {
            playerPopupNameText.text = string.Empty;
            playerPopupDialogueText.text = string.Empty;
            opponentPopupNameText.text = string.Empty;
            opponentPopupDialogueText.text = string.Empty;
            currentDialogue = string.Empty;
            completeDialogue = new Queue<char>();

            dialogueButton.SetActive(false);
            opponentPopupObject.SetActive(false);
            playerPopupObject.SetActive(false);

            return true;
        }

        if (newData.Length == 0 || secondNewData.Length == 0)
        {
            playerPopupNameText.text = string.Empty;
            playerPopupDialogueText.text = string.Empty;
            opponentPopupNameText.text = string.Empty;
            opponentPopupDialogueText.text = string.Empty;
            currentDialogue = string.Empty;
            completeDialogue = new Queue<char>();

            dialogueButton.SetActive(false);
            opponentPopupObject.SetActive(false);
            playerPopupObject.SetActive(false);

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

                if (currentSpeaker == CharacterSelect.Opponent)
                    opponentPopupDialogueText.text = currentDialogue;
                else
                    playerPopupDialogueText.text = currentDialogue;
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
