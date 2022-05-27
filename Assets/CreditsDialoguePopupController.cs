using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreditsDialoguePopupController : MonoBehaviour
{
    [Header("Popup Texts")]
    [SerializeField] private TMP_Text popupDialogue1;
    [SerializeField] private TMP_Text popupDialogue2;
    [Space]
    [Header("Popup Objects")]
    [SerializeField] private GameObject popupDialogue1Object;
    [SerializeField] private GameObject popupDialogue2Object;
    [Space]
    [SerializeField] private string defaultText;
    [SerializeField] private float textPace;

    public bool startDialogue = false;

    private float currentTimer = 0f;
    private Queue<char> completeDialogue;
    private string currentDialogue;

    public delegate void onAIDialogueComplete();
    public static event onAIDialogueComplete OnAIDialogueComplete;

    public void LoadTitleScreen()
    {
        GameManager.instance.LoadTitleScene();
    }

    private void Start()
    {
        completeDialogue = new Queue<char>();

        foreach (char letter in defaultText)
            completeDialogue.Enqueue(letter);
    }

    private void Update()
    {
        if (startDialogue == false)
            return;
        UpdateTextOverTime();
    }

    public void StartDialogue()
    {
        startDialogue = true;
        popupDialogue1Object.SetActive(true);
        popupDialogue2Object.SetActive(true);
    }

    private void UpdateTextOverTime()
    {
        if (completeDialogue.Count != 0)
        {
            if (CheckTimer())
            {
                currentDialogue += completeDialogue.Dequeue();
                
                popupDialogue1.text = currentDialogue;
                popupDialogue2.text = currentDialogue;
            }
        }
    }

    private bool CheckTimer()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= textPace)
        {
            currentTimer = 0f;
            return true;
        }

        return false;
    }
}
