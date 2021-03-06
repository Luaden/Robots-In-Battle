using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private GameObject workShopMenu;
    [SerializeField] private List<SOEventObject> possibleEvents;
    [SerializeField] [Range(0, 100)] private int chanceToSpawnEvent;
    
    private List<SOEventObject> uncheckedAcceptedEvents;
    private List<SOEventObject> newAcceptedEvents;

    private SOEventObject currentEvent;
    private bool hasRolledNewEvent = false;

    public delegate void onEventsComplete();
    public static event onEventsComplete OnEventsCompleted;

    public void RemoveEventDeadEnd()
    {
        currentEvent = null;
        DowntimeManager.instance.PopupUIManager.HandlePopup(currentEvent);
        CheckEventLog();
    }

    public void AcceptCurrentEvent()
    {
        if (CheckCurrentEventRequirements())
        {
            foreach (SOEventOutcomeObject outcome in currentEvent.SuccessfulAcceptanceOutcomes)
                ApplyOutcomeEffect(outcome);

            if(currentEvent.EventObjectives != null)
            {
                newAcceptedEvents.Add(currentEvent);
            }

            currentEvent = currentEvent.SuccessfulAcceptanceDialogue;

            PlayCurrentEventDialogue();
        }
        else
            UnsuccessfulAcceptCurrentEvent();
    }

    public void RefuseCurrentEvent()
    {
        foreach (SOEventOutcomeObject outcome in currentEvent.RefusalOutcomes)
            ApplyOutcomeEffect(outcome);

        currentEvent = currentEvent.RefusalDialogue;
        PlayCurrentEventDialogue();
    }

    private void Start()
    {
        DowntimeManager.OnLoadCombatScene += StashEventLog;

        uncheckedAcceptedEvents = new List<SOEventObject>();
        newAcceptedEvents = new List<SOEventObject>();

        if (GameManager.instance.RemainingPotentialEvents != null)
            possibleEvents = new List<SOEventObject>(GameManager.instance.RemainingPotentialEvents);

        LoadEventLog();
    }

    private void OnDestroy()
    {
        DowntimeManager.OnLoadCombatScene -= StashEventLog;
    }

    private void LoadEventLog()
    {
        uncheckedAcceptedEvents = GameManager.instance.ActiveEvents;
        CheckEventLog();
    }

    private void StashEventLog()
    {
        GameManager.instance.StashCurrentEvents(newAcceptedEvents);
        GameManager.instance.RemainingPotentialEvents = new List<SOEventObject>(possibleEvents);
    }

    private void CheckEventLog()
    {
        if (currentEvent != null)
        {
            PlayCurrentEventDialogue();
            return;
        }

        if (uncheckedAcceptedEvents == null)
            uncheckedAcceptedEvents = new List<SOEventObject>();

        if (uncheckedAcceptedEvents.Count > 0)
        {
            CheckAcceptedEvents();
            return;
        }

        if (!hasRolledNewEvent)
        {
            if (possibleEvents.Count > 0)
            {
                RollForNewEvent();
            }

            hasRolledNewEvent = true;
        }

        if (hasRolledNewEvent && uncheckedAcceptedEvents.Count == 0 && currentEvent == null)
        {
            OnEventsCompleted?.Invoke();
        }
    }

    private void RollForNewEvent()
    {
        int roll = Random.Range(0, 101);

        if (roll <= chanceToSpawnEvent)
        {
            roll = Random.Range(0, possibleEvents.Count);

            currentEvent = possibleEvents[roll];
            possibleEvents.Remove(possibleEvents[roll]);
            CheckEventLog();
        }            
    }

    private void CheckAcceptedEvents()
    {
        if (uncheckedAcceptedEvents.Count == 0)
            return;

        currentEvent = uncheckedAcceptedEvents[0];
        
        if(CheckCurrentEventObjectiveCompletion())
        {
            foreach (SOEventOutcomeObject outcome in currentEvent.ObjectiveCompletionOutcomes)
                ApplyOutcomeEffect(outcome);

            uncheckedAcceptedEvents.Remove(currentEvent);
            
            currentEvent = currentEvent.ObjectiveCompletionDialogue;
        }
        else
        {
            foreach (SOEventOutcomeObject outcome in currentEvent.ObjectiveFailureOutcomes)
                ApplyOutcomeEffect(outcome);

            uncheckedAcceptedEvents.Remove(currentEvent);
            
            currentEvent = currentEvent.ObjectiveFailureDialogue;
        }

        CheckEventLog();
    }

    private bool CheckCurrentEventRequirements()
    {
        int requiredCurrency = 0;

        if (currentEvent.EventRequirements == null || currentEvent.EventRequirements.Count == 0)
            return true;

        foreach(SOEventRequirementObject requirement in currentEvent.EventRequirements)
        {
            switch (requirement.EventRequirement)
            {
                case EventRequirement.HasEnoughMoney:
                    requiredCurrency = requirement.RequirementMagnitude;
                    if (GameManager.instance.PlayerBankController.GetPlayerCurrency() >= requirement.RequirementMagnitude)
                        return true;
                    break;

                default:
                    break;
            }
        }

        return false;
    }

    private bool CheckCurrentEventObjectiveCompletion()
    {
        MechObject playerMech = GameManager.instance.PlayerMechController.PlayerMech;

        foreach(SOEventObjectiveObject objective in currentEvent.EventObjectives)
        {
            switch (objective.EventObjective)
            {
                case EventObjective.EndFightBelowHPPercent:
                    if ((playerMech.MechCurrentHP / playerMech.MechMaxHP) * 100 < objective.EventObjectiveMagnitude)
                        return false;
                    break;

                case EventObjective.EndFightWithEquippedItem:
                    if (playerMech.MechHead.SOItemDataObject != objective.EventObjectiveRequiredComponent &&
                        playerMech.MechTorso.SOItemDataObject != objective.EventObjectiveRequiredComponent &&
                        playerMech.MechHead.SOItemDataObject != objective.EventObjectiveRequiredComponent && 
                        playerMech.MechLegs.SOItemDataObject != objective.EventObjectiveRequiredComponent)
                        return false;
                        break;
            }
        }

        return true;
    }

    private void ApplyOutcomeEffect(SOEventOutcomeObject eventOutcome)
    {
        switch (eventOutcome.EventOutcome)
        {
            case EventOutcome.DamageNextEnemy:
                GameManager.instance.EnemyHealthModifier = eventOutcome.OutcometMagnitude;
                break;

            case EventOutcome.DamagePlayer:
                GameManager.instance.PlayerMechController.PlayerMech.DamageWholeMechHP(eventOutcome.OutcometMagnitude);
                break;

            case EventOutcome.GainOrLoseMoney:
                if (eventOutcome.OutcometMagnitude > 0)
                    GameManager.instance.PlayerBankController.AddPlayerCurrency(eventOutcome.OutcometMagnitude);
                else
                    GameManager.instance.PlayerBankController.SpendPlayerCurrency(Mathf.Abs(eventOutcome.OutcometMagnitude));
                break;

            case EventOutcome.TempWinningsModifier:
                GameManager.instance.PlayerCurrencyGainOnWin += eventOutcome.OutcometMagnitude;
                break;

            case EventOutcome.AddItemToPlayer:
                if(eventOutcome.ItemToAdd.ItemType == ItemType.Component)
                    GameManager.instance.PlayerInventoryController.AddItemToInventory(eventOutcome.ItemToAdd);
                if(eventOutcome.ItemToAdd.ItemType == ItemType.Card)
                    GameManager.instance.PlayerDeckController.AddCardToPlayerDeck(eventOutcome.ItemToAdd);
                break;

            case EventOutcome.GainOrLoseTime:
                if (eventOutcome.OutcometMagnitude > 0)
                    GameManager.instance.PlayerBankController.AddPlayerTime(eventOutcome.OutcometMagnitude);
                if (eventOutcome.OutcometMagnitude < 0)
                    GameManager.instance.PlayerBankController.SpendPlayerTime(eventOutcome.OutcometMagnitude);
                break;
        }
    }

    private void UnsuccessfulAcceptCurrentEvent()
    {
        currentEvent = currentEvent.UnsuccessfulAcceptanceDialogue;
        PlayCurrentEventDialogue();
    }

    private void PlayCurrentEventDialogue()
    {
        DowntimeManager.instance.PopupUIManager.HandlePopup(currentEvent);
    }
}
