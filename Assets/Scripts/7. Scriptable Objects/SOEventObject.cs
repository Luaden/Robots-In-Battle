using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Events/New Event")]
public class SOEventObject : ScriptableObject
{
    #region Introduction
    [Header("Event Introduction")]
    [Tooltip("This will play sequentially and can be the introduction or follow on of a previous event object.")]
    [TextArea(5, 10)]
    [SerializeField] private List<string> eventDialogue;
    [Tooltip("This should be true unless the purpose of this event object is to act as the end result of a dialogue or event chain. Ticking this box indicates" +
        "that the event dialogue should be played, but that the player will have no requirements and receive no outcomes after the dialogue completes.")]
    [SerializeField] private bool eventCanBeAccepted;
    [Tooltip("These are requirements for the player to successfully accept the event. Unsuccessful acceptance will be the result if the requirement cannot be met.")]
    [SerializeField] private List<SOEventRequirementObject> eventRequirements;
    [Tooltip("This is objective will be logged and checked the next time the player returns to the workshop.")]
    [SerializeField] private List<SOEventObjectiveObject> eventObjectives;
    #endregion

    #region Successful Accept
    [Header("Event Succesful Acceptance Options")]
    [Tooltip("This is an outcome that is applied immediately on successful acceptance. It should only be used if there are immediate effects following acceptance.")]
    [SerializeField] private List<SOEventOutcomeObject> successfulAcceptanceOutcomes;
    [Tooltip("This is dialogue that immediately follows the successful acceptance dialogue and affirmation outcome. Repeatable or follow-on objectives go here. Ex. If " +
        "the event asks the player to take damage for money an indefinite number of times, a 'follow up' dialogue object should be a shortened version of the " +
        "'intro' version of that event with the same outcomes and be placed here.")]
    [SerializeField] private SOEventObject successfulAcceptanceDialogue;
    #endregion

    #region Unsuccessful Accept
    [Header("Event Unsuccessful Acceptance Options")]
    [Tooltip("This is dialogue that immediately follows the attempted acceptance of the event. It should be used for events that have alternate paths the player may be " +
        "able to accept if the initial requirements cannot be met. Ex. If the player cannot pay a high cost for a nice component, a lower cost alternative can be added " +
        "here.")]
    [SerializeField] private SOEventObject unsuccessfulAcceptanceDialogue;
    #endregion

    #region Refusal
    [Header("Event Refusal Options")]
    [Tooltip("This is an outcome that is applied immediately on rejection. It should only be used if there are immediate effects following refusal.")]
    [SerializeField] private List<SOEventOutcomeObject> refusalOutcomes;
    [Tooltip("This is dialogue that immediately follows the rejection dialogue and rejection outcome. Repeatable or follow-on objectives go here. Ex. If " +
        "the event asks the player to take damage for money and has an alternative path on rejection, it should be placed here.")]
    [SerializeField] private SOEventObject refusalDialogue;
    #endregion

    #region Event Completion
    [Header("Event Completion Options")]
    [Tooltip("This is an outcome that is applied upon returning to the Workshop on completion of the objective.")]
    [SerializeField] private List<SOEventOutcomeObject> objectiveCompletionOutcomes;
    [Tooltip("This is a follow on event or dialogue closing the current event out.")]
    [SerializeField] private SOEventObject objectiveCompletionDialogue;
    #endregion

    #region Event Failure
    [Header("Event Failure Acceptance Options")]
    [Tooltip("TThis is an outcome that is applied upon returning to the Workshop on failure of the objective.")]
    [SerializeField] private List<SOEventOutcomeObject> objectiveFailureOutcomes;
    [Tooltip("This is a follow on event or dialogue closing the current event out.")]
    [SerializeField] private SOEventObject objectiveFailureDialogue;
    #endregion

    private FighterDataObject associatedFighter;

    public FighterDataObject AssociatedFighter { get => associatedFighter; set => associatedFighter = value; }

    #region Event Intro
    public List<string> EventDialogue { get => eventDialogue; }
    public bool EventCanBeAccepted { get => eventCanBeAccepted; }
    public List<SOEventRequirementObject> EventRequirements { get => eventRequirements; }
    public List<SOEventObjectiveObject> EventObjectives { get => eventObjectives; }
    #endregion

    #region Event Follow On Dialogues
    public SOEventObject SuccessfulAcceptanceDialogue { get => successfulAcceptanceDialogue; }
    public SOEventObject UnsuccessfulAcceptanceDialogue { get => unsuccessfulAcceptanceDialogue; }
    public SOEventObject RefusalDialogue { get => refusalDialogue; }
    public SOEventObject ObjectiveCompletionDialogue { get => objectiveCompletionDialogue; }
    public SOEventObject ObjectiveFailureDialogue { get => objectiveFailureDialogue; }
    #endregion

    #region Outcomes
    public List<SOEventOutcomeObject> SuccessfulAcceptanceOutcomes { get => successfulAcceptanceOutcomes; }
    public List<SOEventOutcomeObject> RefusalOutcomes { get => refusalOutcomes; }
    public List<SOEventOutcomeObject> ObjectiveCompletionOutcomes { get => objectiveCompletionOutcomes; }
    public List<SOEventOutcomeObject> ObjectiveFailureOutcomes { get => objectiveFailureOutcomes; }
    #endregion
}