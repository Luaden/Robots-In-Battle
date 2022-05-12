using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDMechComponentPopupController : BaseUIElement<MechSelect>
{
    [SerializeField] private GameObject opponentHighChannelPopupObject;
    [SerializeField] private GameObject opponentMidChannelPopupObject;
    [SerializeField] private GameObject opponentLowChannelPopupObject;

    [SerializeField] private GameObject playerHighChannelPopupObject;
    [SerializeField] private GameObject playerMidChannelPopupObject;
    [SerializeField] private GameObject playerLowChannelPopupObject;

    private float currentTimer = 0f;
    private bool playerPopupQueued = false;
    private bool opponentPopupQueued = false;

    private void Awake()
    {
        CardUIController.OnPickUp += OnSelectChennel;
    }

    private void OnDestroy()
    {
        CardUIController.OnPickUp -= OnSelectChennel;
    }

    public override void UpdateUI(MechSelect character)
    {
        if (ClearedIfEmpty(character))
            return;

        if (character == MechSelect.Player)
        {
            playerPopupQueued = true;
        }
        if(character == MechSelect.Opponent)
        {
            opponentPopupQueued = true;
        }
    }

    protected override bool ClearedIfEmpty(MechSelect newData)
    {
        if (newData == MechSelect.None)
        {
            playerPopupQueued = false;
            opponentPopupQueued = false;
            playerHighChannelPopupObject.SetActive(false);
            playerMidChannelPopupObject.SetActive(false);
            playerLowChannelPopupObject.SetActive(false);
            opponentHighChannelPopupObject.SetActive(false);
            opponentMidChannelPopupObject.SetActive(false);
            opponentLowChannelPopupObject.SetActive(false);
            return true;
        }

        return false;
    }

    private void OnSelectChennel(Channels channel)
    {
        if(channel == Channels.None)
        {
            UpdateUI(MechSelect.None);
            return;
        }

        if (channel.HasFlag(Channels.High))
        {
            opponentHighChannelPopupObject.SetActive(true);
        }

        if(channel.HasFlag(Channels.Mid))
        {
            opponentMidChannelPopupObject.SetActive(true);
        }

        if(channel.HasFlag(Channels.Low))
        {
            opponentLowChannelPopupObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (playerPopupQueued)
        {
            CheckTimer(MechSelect.Player);
        }

        if(opponentPopupQueued)
        {
            CheckTimer(MechSelect.Opponent);
        }
    }

    private void CheckTimer(MechSelect character)
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= CombatManager.instance.PopupUIManager.GeneralHUDPopupDelay)
        {
            if(character == MechSelect.Player)
            {
                playerHighChannelPopupObject.SetActive(true);
                playerMidChannelPopupObject.SetActive(true);
                playerLowChannelPopupObject.SetActive(true);
                playerPopupQueued = false;
                currentTimer = 0f;
            }

            if(character == MechSelect.Opponent)
            {
                opponentHighChannelPopupObject.SetActive(true);
                opponentMidChannelPopupObject.SetActive(true);
                opponentLowChannelPopupObject.SetActive(true);
                opponentPopupQueued = false;
                currentTimer = 0f;
            }
        }
    }
}
