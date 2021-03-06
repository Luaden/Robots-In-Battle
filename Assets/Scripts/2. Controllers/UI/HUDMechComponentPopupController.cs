using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDMechComponentPopupController : BaseUIElement<MechSelect>
{
    [Header("Opponent Mech")]
    [SerializeField] private GameObject opponentComponentPopupObject;
    [SerializeField] private GameObject opponentHighChannelTypeObject;
    [SerializeField] private TMP_Text opponentHighChannelComponentHealthText;
    [SerializeField] private TMP_Text opponentHighChannelElementTypeText;
    [SerializeField] private GameObject opponentMidChannelTypeObject;
    [SerializeField] private TMP_Text opponentMidChannelComponentHealthText;
    [SerializeField] private TMP_Text opponentMidChannelElementTypeText;
    [SerializeField] private GameObject opponentLowChannelTypeObject;
    [SerializeField] private TMP_Text opponentLowChannelComponentHealthText;
    [SerializeField] private TMP_Text opponentLowChannelElementTypeText;
    [SerializeField] private TMP_Text opponentMechEnergyRegenerationText;
    [Space]
    [Header("Player Mech")]
    [SerializeField] private GameObject playerComponentPopupObject;
    [SerializeField] private GameObject playerHighChannelTypeObject;
    [SerializeField] private TMP_Text playerHighChannelComponentHealthText;
    [SerializeField] private TMP_Text playerHighChannelElementTypeText;
    [SerializeField] private GameObject playerMidChannelTypeObject;
    [SerializeField] private TMP_Text playerMidChannelComponentHealthText;
    [SerializeField] private TMP_Text playerMidChannelElementTypeText;
    [SerializeField] private GameObject playerLowChannelTypeObject;
    [SerializeField] private TMP_Text playerLowChannelComponentHealthText;
    [SerializeField] private TMP_Text playerLowChannelElementTypeText;
    [SerializeField] private TMP_Text playerMechEnergyRegenerationText;
    [Space]
    [SerializeField] private float fadeAlpha;

    private MechComponentDataObject playerHeadComponent;
    private MechComponentDataObject playerTorsoComponent;
    private MechComponentDataObject playerLegsComponent;

    private MechComponentDataObject opponentHeadComponent;
    private MechComponentDataObject opponentTorsoComponent;
    private MechComponentDataObject opponentLegsComponent;

    private float currentTimer = 0f;
    private bool playerPopupQueued = false;
    private bool opponentPopupQueued = false;

    private void Awake()
    {
        CardUIController.OnPickUp += OnSelectChannel;
    }

    private void Start()
    {

        opponentHeadComponent = CombatManager.instance.OpponentFighter.FighterMech.MechHead;
        opponentTorsoComponent = CombatManager.instance.OpponentFighter.FighterMech.MechTorso;
        opponentLegsComponent = CombatManager.instance.OpponentFighter.FighterMech.MechLegs;
        opponentMechEnergyRegenerationText.text = "Energy Regen: " + (CombatManager.instance.OpponentFighter.FighterMech.MechEnergyGain + CombatManager.instance.MechEnergyGain).ToString();

        playerHeadComponent = CombatManager.instance.PlayerFighter.FighterMech.MechHead;
        playerTorsoComponent = CombatManager.instance.PlayerFighter.FighterMech.MechTorso;
        playerLegsComponent = CombatManager.instance.PlayerFighter.FighterMech.MechLegs;
        playerMechEnergyRegenerationText.text = "Energy Regen: " + (CombatManager.instance.PlayerFighter.FighterMech.MechEnergyGain + CombatManager.instance.MechEnergyGain).ToString();

        opponentHighChannelElementTypeText.text = "Component Element: " + Enum.GetName(typeof(ElementType), opponentHeadComponent.ComponentElement);
        opponentMidChannelElementTypeText.text = "Component Element: " + Enum.GetName(typeof(ElementType), opponentTorsoComponent.ComponentElement);
        opponentLowChannelElementTypeText.text = "Component Element: " + Enum.GetName(typeof(ElementType), opponentLegsComponent.ComponentElement);

        playerHighChannelElementTypeText.text = "Component Element: " + Enum.GetName(typeof(ElementType), playerHeadComponent.ComponentElement);
        playerMidChannelElementTypeText.text = "Component Element: " + Enum.GetName(typeof(ElementType), playerTorsoComponent.ComponentElement);
        playerLowChannelElementTypeText.text = "Component Element: " + Enum.GetName(typeof(ElementType), playerLegsComponent.ComponentElement);
    }

    private void OnDestroy()
    {
        CardUIController.OnPickUp -= OnSelectChannel;
    }

    public override void UpdateUI(MechSelect character)
    {
        if (ClearedIfEmpty(character))
            return;

        if (character == MechSelect.Player)
        {
            if (playerHeadComponent.ComponentCurrentHP > 0)
                playerHighChannelComponentHealthText.text = "Component Health: " + Mathf.Clamp(playerHeadComponent.ComponentCurrentHP, 0, int.MaxValue).ToString();
            else
                playerHighChannelComponentHealthText.text = "Component Health: BROKEN!";

            if (playerTorsoComponent.ComponentCurrentHP > 0)
                playerMidChannelComponentHealthText.text = "Component Health: " + Mathf.Clamp(playerTorsoComponent.ComponentCurrentHP, 0, int.MaxValue).ToString();
            else
                playerMidChannelComponentHealthText.text = "Component Health: BROKEN!";

            if (playerLegsComponent.ComponentCurrentHP > 0)
                playerLowChannelComponentHealthText.text = "Component Health: " + Mathf.Clamp(playerLegsComponent.ComponentCurrentHP, 0, int.MaxValue).ToString();
            else
                playerLowChannelComponentHealthText.text = "Component Health: BROKEN!";
            
            foreach (Channels channel in CombatManager.instance.GetChannelListFromFlags(Channels.All))
                SetComponentPopups(MechSelect.Player, channel, true);

            playerMechEnergyRegenerationText.gameObject.SetActive(true);
            playerPopupQueued = true;
        }

        if(character == MechSelect.Opponent)
        {
            if (opponentHeadComponent.ComponentCurrentHP > 0)
                opponentHighChannelComponentHealthText.text = "Component Health: " + Mathf.Clamp(opponentHeadComponent.ComponentCurrentHP, 0, int.MaxValue).ToString();
            else
                opponentHighChannelComponentHealthText.text = "Component Health: BROKEN!";

            if (opponentTorsoComponent.ComponentCurrentHP > 0)
                opponentMidChannelComponentHealthText.text = "Component Health: " + Mathf.Clamp(opponentTorsoComponent.ComponentCurrentHP, 0, int.MaxValue).ToString();
            else
                opponentMidChannelComponentHealthText.text = "Component Health: BROKEN!";

            if (opponentLegsComponent.ComponentCurrentHP > 0)
                opponentLowChannelComponentHealthText.text = "Component Health: " + Mathf.Clamp(opponentLegsComponent.ComponentCurrentHP, 0, int.MaxValue).ToString();
            else
                opponentLowChannelComponentHealthText.text = "Component Health: BROKEN!";

            foreach (Channels channel in CombatManager.instance.GetChannelListFromFlags(Channels.All))
                SetComponentPopups(MechSelect.Opponent, channel, true);

            opponentMechEnergyRegenerationText.gameObject.SetActive(true);

            opponentPopupQueued = true;
        }
    }

    protected override bool ClearedIfEmpty(MechSelect newData)
    {
        if (newData == MechSelect.None)
        {
            playerPopupQueued = false;
            opponentPopupQueued = false;

            currentTimer = 0f;

            opponentComponentPopupObject.SetActive(false);
            playerComponentPopupObject.SetActive(false);

            return true;
        }

        return false;
    }

    private void OnSelectChannel(Channels destinationChannel, MechSelect destinationMech, Channels originChannels)
    {
        if (GameManager.instance.isTrailerMaking)
            return;

        if(destinationChannel == Channels.None || destinationMech == MechSelect.None)
        {
            UpdateUI(MechSelect.None);
            return;
        }

        foreach (Channels channel in CombatManager.instance.GetChannelListFromFlags(Channels.All))
        {
            if (destinationChannel.HasFlag(channel))
                SetComponentPopups(destinationMech, channel, true);
            else
                SetComponentPopups(destinationMech, channel, false);
        }

        if (originChannels != Channels.None)
            foreach(Channels channel in CombatManager.instance.GetChannelListFromFlags(Channels.All))
            {
                if (originChannels == channel)
                    SetComponentPopups(MechSelect.Player, channel, true);
                else
                    SetComponentPopups(MechSelect.Player, channel, false);
            }

        opponentMechEnergyRegenerationText.gameObject.SetActive(false);
        playerMechEnergyRegenerationText.gameObject.SetActive(false);
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
                playerComponentPopupObject.SetActive(true);
                playerPopupQueued = false;
                currentTimer = 0f;
            }

            if(character == MechSelect.Opponent)
            {
                opponentComponentPopupObject.SetActive(true);
                opponentPopupQueued = false;
                currentTimer = 0f;
            }
        }
    }

    private void SetComponentPopups(MechSelect mech, Channels channel, bool activeStatus)
    {
        if(mech == MechSelect.Opponent)
        {
            switch (channel)
            {
                case Channels.High:
                    opponentHighChannelTypeObject.SetActive(activeStatus);
                    opponentHighChannelComponentHealthText.gameObject.SetActive(activeStatus);
                    opponentHighChannelComponentHealthText.text = "Component Health: " + Mathf.Clamp(opponentHeadComponent.ComponentCurrentHP, 0, int.MaxValue).ToString();
                    opponentHighChannelElementTypeText.gameObject.SetActive(activeStatus);
                    break;
                case Channels.Mid:
                    opponentMidChannelTypeObject.SetActive(activeStatus);
                    opponentMidChannelComponentHealthText.gameObject.SetActive(activeStatus);
                    opponentMidChannelComponentHealthText.text = "Component Health: " + Mathf.Clamp(opponentTorsoComponent.ComponentCurrentHP, 0, int.MaxValue).ToString();
                    opponentMidChannelElementTypeText.gameObject.SetActive(activeStatus);
                    break;
                case Channels.Low:
                    opponentLowChannelTypeObject.SetActive(activeStatus);
                    opponentLowChannelComponentHealthText.gameObject.SetActive(activeStatus);
                    opponentLowChannelComponentHealthText.text = "Component Health: " + Mathf.Clamp(opponentLegsComponent.ComponentCurrentHP, 0, int.MaxValue).ToString();
                    opponentLowChannelElementTypeText.gameObject.SetActive(activeStatus);
                    break;
            }

            opponentComponentPopupObject.SetActive(true);
        }

        if(mech == MechSelect.Player)
        {
            switch (channel)
            {
                case Channels.High:
                    playerHighChannelTypeObject.SetActive(activeStatus);
                    playerHighChannelComponentHealthText.gameObject.SetActive(activeStatus);
                    playerHighChannelComponentHealthText.text = "Component Health: " + Mathf.Clamp(playerHeadComponent.ComponentCurrentHP, 0, int.MaxValue).ToString();
                    playerHighChannelElementTypeText.gameObject.SetActive(activeStatus);
                    break;
                case Channels.Mid:
                    playerMidChannelTypeObject.SetActive(activeStatus);
                    playerMidChannelComponentHealthText.gameObject.SetActive(activeStatus);
                    playerMidChannelComponentHealthText.text = "Component Health: " + Mathf.Clamp(playerTorsoComponent.ComponentCurrentHP, 0, int.MaxValue).ToString();
                    playerMidChannelElementTypeText.gameObject.SetActive(activeStatus);
                    break;
                case Channels.Low:
                    playerLowChannelTypeObject.SetActive(activeStatus);
                    playerLowChannelComponentHealthText.gameObject.SetActive(activeStatus);
                    playerLowChannelElementTypeText.gameObject.SetActive(activeStatus);
                    playerLowChannelComponentHealthText.text = "Component Health: " + Mathf.Clamp(playerLegsComponent.ComponentCurrentHP, 0, int.MaxValue).ToString();
                    break;
            }

            playerComponentPopupObject.SetActive(true);
        }
    }
}
