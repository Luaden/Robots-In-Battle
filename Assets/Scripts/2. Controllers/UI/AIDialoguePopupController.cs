using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AIDialoguePopupController : BaseUIElement<string, string>
{
    [SerializeField] private GameObject popupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private GameObject bigPopupObject;
    [SerializeField] private Image hairImage;
    [SerializeField] private Image bodyImage;
    [SerializeField] private Image eyesImage;
    [SerializeField] private Image noseImage;
    [SerializeField] private Image mouthImage;
    [SerializeField] private Image clothesImage;
    [SerializeField] private TMP_Text bigNameText;
    [SerializeField] private TMP_Text bigDialogueText;
    [Space]
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

    public void UpdateUI(SOCompleteCharacter primaryData, string secondaryData)
    {
        if (ClearedIfEmpty(primaryData, secondaryData))
            return;

        bigNameText.text = primaryData.PilotName;

        if (primaryData.PilotUIObject.FighterHair == null)
        {
            hairImage.color = new Color(1, 1, 1, 0);
            Debug.Log("Hair isn't found.");
        }
        hairImage.sprite = primaryData.PilotUIObject.FighterHair;
        hairImage.SetNativeSize();

        if (primaryData.PilotUIObject.FighterEyes == null)
        {
            eyesImage.color = new Color(1, 1, 1, 0);
            Debug.Log("Eyes aren't found.");
        }
        eyesImage.sprite = primaryData.PilotUIObject.FighterEyes;
        eyesImage.SetNativeSize();

        if (primaryData.PilotUIObject.FighterNose == null)
        {
            noseImage.color = new Color(1, 1, 1, 0);
            Debug.Log("Nose isn't found.");

        }
        noseImage.sprite = primaryData.PilotUIObject.FighterNose;
        noseImage.SetNativeSize();

        if (primaryData.PilotUIObject.FighterMouth == null)
        {
            mouthImage.color = new Color(1, 1, 1, 0);
            Debug.Log("Mouth isn't found.");
        }
        mouthImage.sprite = primaryData.PilotUIObject.FighterMouth;
        mouthImage.SetNativeSize();

        if (primaryData.PilotUIObject.FighterClothes == null)
        {
            clothesImage.color = new Color(1, 1, 1, 0);
            Debug.Log("Clothes aren't found.");
        }
        clothesImage.sprite = primaryData.PilotUIObject.FighterClothes;
        clothesImage.SetNativeSize();

        if (primaryData.PilotUIObject.FighterBody == null)
        {
            bodyImage.color = new Color(1, 1, 1, 0);
            Debug.Log("Body isn't found.");
        }
        bodyImage.sprite = primaryData.PilotUIObject.FighterBody;
        bodyImage.SetNativeSize();

        foreach (char letter in secondaryData)
            completeDialogue.Enqueue(letter);

        dialogueButton.SetActive(true);
        bigPopupObject.SetActive(true);
    }

    public void SkipText()
    {
        if (completeDialogue.Count > 0)
        {
            int letterCount = completeDialogue.Count;
            for (int i = 0; i < letterCount; i++)
                currentDialogue += completeDialogue.Dequeue();

            dialogueText.text = currentDialogue;
            bigDialogueText.text = currentDialogue;
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

    protected bool ClearedIfEmpty(SOCompleteCharacter newData, string secondNewData)
    {
        if (newData == null || secondNewData == null)
        {
            bigNameText.text = string.Empty;
            bigDialogueText.text = string.Empty;
            currentDialogue = string.Empty;
            completeDialogue = new Queue<char>();

            dialogueButton.SetActive(false);
            bigPopupObject.SetActive(false);

            return true;
        }

        if (secondNewData.Length == 0)
        {
            bigNameText.text = string.Empty;
            bigDialogueText.text = string.Empty;
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
                dialogueText.text = currentDialogue;
                bigDialogueText.text = currentDialogue;
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
