using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreditsDialoguePopupController : MonoBehaviour
{
    [SerializeField] private Animator cameraAnim;
    [Header("Popup Texts")]
    [SerializeField] private TMP_Text popupDialogue1;
    [SerializeField] private TMP_Text popupDialogue2;
    [Space]
    [Header("Popup Objects")]
    [SerializeField] private GameObject popupDialogue1Object;
    [SerializeField] private GameObject popupDialogue2Object;
    [Space]
    [SerializeField] private string defaultText1;
    [SerializeField] private string defaultText2;
    [SerializeField] private float textPace;

    public bool startDialogue = false;

    private float currentTimer = 0f;
    private Queue<char> completeDialogue1;
    private Queue<char> completeDialogue2;
    private string currentDialogue1;
    private string currentDialogue2;

    public delegate void onAIDialogueComplete();
    public static event onAIDialogueComplete OnAIDialogueComplete;

    public void LoadTitleScreen()
    {
        GameManager.instance.LoadTitleScene();
    }

    public void StartDialogue()
    {
        startDialogue = true;
        popupDialogue1Object.SetActive(true);
        popupDialogue2Object.SetActive(true);
    }

    private void Awake()
    {
        SceneTransitionController.OnSecondPageTurned += StartCameraCrawl;
    }

    private void Start()
    {
        completeDialogue1 = new Queue<char>();
        completeDialogue2 = new Queue<char>();

        foreach (char letter in defaultText1)
            completeDialogue1.Enqueue(letter);

        foreach (char letter in defaultText2)
            completeDialogue2.Enqueue(letter);
    }

    private void Update()
    {
        if (startDialogue == false)
            return;
        UpdateTextOverTime();
    }

    private void OnDestroy()
    {
        SceneTransitionController.OnSecondPageTurned -= StartCameraCrawl;
    }

    private void StartCameraCrawl()
    {
        cameraAnim.SetTrigger("onPageTwoTurned");
    }

    private void UpdateTextOverTime()
    {
        if(CheckTimer())
        {
            if (completeDialogue2.Count != 0)
            {
                currentDialogue2 += completeDialogue2.Dequeue();
                popupDialogue2.text = currentDialogue2;
            }

            if (completeDialogue1.Count != 0)
            {
                currentDialogue1 += completeDialogue1.Dequeue();
                popupDialogue1.text = currentDialogue1;
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
