using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AIDialoguePopupController : BaseUIElement<string, string>
{
    [SerializeField] protected GameObject popupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialogueButton;

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

        nameText.text = primaryData;

        foreach(char letter in secondaryData)
            completeDialogue.Enqueue(letter);

        dialogueButton.SetActive(true);
        popupObject.SetActive(true);
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
            nameText.text = string.Empty;
            dialogueText.text = string.Empty;
            currentDialogue = string.Empty;
            completeDialogue = new Queue<char>();

            dialogueButton.SetActive(false);
            popupObject.SetActive(false);

            return true;
        }

        if (newData.Length == 0 || secondNewData.Length == 0)
        {
            nameText.text = string.Empty;
            dialogueText.text = string.Empty;
            currentDialogue = string.Empty;
            completeDialogue = new Queue<char>();

            dialogueButton.SetActive(false);
            popupObject.SetActive(false);

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
                dialogueText.text = currentDialogue;
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
