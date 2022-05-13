using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventDialoguePopupController : BaseUIElement<SOEventObject>
{
    [Header("Base Popup Data")]
    [SerializeField] protected GameObject popupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Buttons")]
    [SerializeField] private GameObject skipDialogueButton;
    [SerializeField] private GameObject acceptDialogueButton;
    [SerializeField] private GameObject refuseDialogueButton;
    [SerializeField] private GameObject endDialogue;

    private SOEventObject currentEvent;
    private Queue<string> dialogueQueue = new Queue<string>();
    private string currentDialogue;
    private Queue<char> completeDialogue;
    private float currentTimer = 0f;
    private bool dialogueComplete = true;

    public override void UpdateUI(SOEventObject primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        currentEvent = primaryData;

        nameText.text = "No Name Input";

        foreach (string dialogue in primaryData.EventDialogue)
            dialogueQueue.Enqueue(dialogue);

        foreach (char letter in dialogueQueue.Dequeue())
            completeDialogue.Enqueue(letter);

        skipDialogueButton.SetActive(true);
        popupObject.SetActive(true);
        dialogueComplete = false;
    }

    public void UpdateUI()
    {
        if (completeDialogue.Count > 0)
        {
            int letterCount = completeDialogue.Count;
            
            for (int i = 0; i < letterCount; i++)
                currentDialogue += completeDialogue.Dequeue();

            dialogueText.text = currentDialogue;

            return;
        }
        else if(completeDialogue.Count == 0 && dialogueQueue.Count > 0)
        {
            string newDialogue = dialogueQueue.Dequeue();

            foreach (char letter in newDialogue)
                completeDialogue.Enqueue(letter);

            dialogueText.text = string.Empty;
            currentDialogue = string.Empty;

            return;
        }
        else
        {
            UpdateUI(null);
        }
    }

    protected override bool ClearedIfEmpty(SOEventObject primaryData)
    {
        nameText.text = string.Empty;
        dialogueText.text = string.Empty;
        currentDialogue = string.Empty;
        completeDialogue = new Queue<char>();
        dialogueQueue = new Queue<string>();

        skipDialogueButton.SetActive(false);
        endDialogue.SetActive(false);
        acceptDialogueButton.SetActive(false);
        refuseDialogueButton.SetActive(false);
        popupObject.SetActive(false);
        dialogueComplete = true;

        if (primaryData == null)
            return true;

        return false;
    }

    private void Start()
    {
        dialogueQueue = new Queue<string>();
        completeDialogue = new Queue<char>();
    }

    private void Update()
    {
        UpdateTextOverTime();
    }

    private void UpdateTextOverTime()
    {
        if (dialogueComplete)
            return;

        if (completeDialogue.Count != 0)
        {
            if (CheckTimer())
            {
                currentDialogue += completeDialogue.Dequeue();
                dialogueText.text = currentDialogue;
            }
        }

        if(completeDialogue.Count == 0 && dialogueQueue.Count == 0)
        {
            if(currentEvent.EventCanBeAccepted)
            {
                acceptDialogueButton.SetActive(true);
                refuseDialogueButton.SetActive(true);
                skipDialogueButton.SetActive(false);
                endDialogue.SetActive(false);
                dialogueComplete = true;
            }
            if(!currentEvent.EventCanBeAccepted)
            {
                acceptDialogueButton.SetActive(false);
                refuseDialogueButton.SetActive(false);
                skipDialogueButton.SetActive(false);
                endDialogue.SetActive(true);
                dialogueComplete = true;
            }
        }
    }

    private bool CheckTimer()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= DowntimeManager.instance.PopupUIManager.TextPace)
        {
            currentTimer = 0f;
            return true;
        }

        return false;
    }
}
