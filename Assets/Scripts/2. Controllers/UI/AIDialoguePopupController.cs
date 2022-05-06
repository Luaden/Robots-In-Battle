using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AIDialoguePopupController : BaseUIElement<string, string>
{
    [SerializeField] protected GameObject popupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;

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

        popupObject.SetActive(true);
    }

    public void UpdateUI()
    {
        int letterCount = completeDialogue.Count;
        for(int i = 0; i < letterCount; i++)
            currentDialogue += completeDialogue.Dequeue();
    }

    protected override bool ClearedIfEmpty(string newData, string secondNewData)
    {
        nameText.text = string.Empty;
        dialogueText.text = string.Empty;
        currentDialogue = string.Empty;
        completeDialogue = new Queue<char>();

        popupObject.SetActive(false);

        if (newData == null || secondNewData == null)
        {
            return true;
        }

        return false;
    }

    private void Start()
    {
        PopupUIManager.OnSkipText += UpdateUI;
        completeDialogue = new Queue<char>();
    }

    private void OnDestroy()
    {
        PopupUIManager.OnSkipText -= UpdateUI;
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
