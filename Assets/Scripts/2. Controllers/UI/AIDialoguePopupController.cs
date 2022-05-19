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

    private CharacterSelect currentSpeaker;
    private float currentTimer = 0f;

    private Queue<Queue<char>> dialogueQueues;
    private Queue<char> currentDialogueQueue;
    private string currentDialogueCompletion;

    public delegate void onAIDialogueComplete();
    public static event onAIDialogueComplete OnAIDialogueComplete;

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

        Queue<char> newQueue = new Queue<char>();

        foreach (char letter in secondaryData)
        {
            if(letter.ToString() == "*")
            {
                dialogueQueues.Enqueue(newQueue);
                newQueue = new Queue<char>();
                continue;
            }
            else
                newQueue.Enqueue(letter);
        }

        dialogueQueues.Enqueue(newQueue);
        currentDialogueQueue = dialogueQueues.Dequeue();

        dialogueButton.SetActive(true);
    }

    public void SkipText()
    {
        if (currentDialogueQueue.Count > 0)
        {
            int letterCount = currentDialogueQueue.Count;

            for (int i = 0; i < letterCount; i++)
                currentDialogueCompletion += currentDialogueQueue.Dequeue();

            if (currentSpeaker == CharacterSelect.Opponent)
                opponentPopupDialogueText.text = currentDialogueCompletion;
            else
                playerPopupDialogueText.text = currentDialogueCompletion;

            return;
        }

        if(currentDialogueQueue.Count == 0 && dialogueQueues.Count > 0)
        {
            currentDialogueQueue = dialogueQueues.Dequeue();
            currentDialogueCompletion = string.Empty;
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
            currentDialogueCompletion = string.Empty;
            currentDialogueQueue = new Queue<char>();
            dialogueQueues = new Queue<Queue<char>>();

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
            currentDialogueCompletion = string.Empty;
            currentDialogueQueue = new Queue<char>();
            dialogueQueues = new Queue<Queue<char>>();

            dialogueButton.SetActive(false);
            opponentPopupObject.SetActive(false);
            playerPopupObject.SetActive(false);

            return true;
        }

        return false;
    }

    private void Start()
    {
        currentDialogueQueue = new Queue<char>();
        dialogueQueues = new Queue<Queue<char>>();
    }

    private void Update()
    {
        UpdateTextOverTime();
    }

    private void UpdateTextOverTime()
    {
        if(currentDialogueQueue.Count != 0)
        {
            if(CheckTimer())
            {
                currentDialogueCompletion += currentDialogueQueue.Dequeue();

                if (currentSpeaker == CharacterSelect.Opponent)
                    opponentPopupDialogueText.text = currentDialogueCompletion;
                else
                    playerPopupDialogueText.text = currentDialogueCompletion;
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
