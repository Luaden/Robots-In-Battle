using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneralHUDUIPopupController : BaseUIElement<GeneralHUDElement>
{
    [SerializeField] private GameObject popupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private SOHUDElementPopupObject highChannelPopupObject;
    [SerializeField] private SOHUDElementPopupObject midChannelPopupObject;
    [SerializeField] private SOHUDElementPopupObject lowChannelPopupObject;

    [SerializeField] private SOHUDElementPopupObject healthPopupObject;
    [SerializeField] private SOHUDElementPopupObject energyPopupObject;

    [SerializeField] private SOHUDElementPopupObject attackSlotPopupObject;
    [SerializeField] private SOHUDElementPopupObject defenseSlotPopupObject;

    private float currentTimer;
    private bool popupQueued;


    public override void UpdateUI(GeneralHUDElement primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        switch (primaryData)
        {
            case GeneralHUDElement.None:
                break;
            case GeneralHUDElement.HighChannel:
                nameText.text = highChannelPopupObject.NameText;
                descriptionText.text = highChannelPopupObject.DescriptionText;
                popupQueued = true;
                break;

            case GeneralHUDElement.MidChannel:
                nameText.text = midChannelPopupObject.NameText;
                descriptionText.text = midChannelPopupObject.DescriptionText;
                popupQueued = true;
                break;
            
            case GeneralHUDElement.LowChannel:
                nameText.text = lowChannelPopupObject.NameText;
                descriptionText.text = lowChannelPopupObject.DescriptionText;
                popupQueued = true;
                break;

            case GeneralHUDElement.Health:
                nameText.text = healthPopupObject.NameText;
                descriptionText.text = healthPopupObject.DescriptionText;
                popupQueued = true;
                break;

            case GeneralHUDElement.Energy:
                nameText.text = energyPopupObject.NameText;
                descriptionText.text = energyPopupObject.DescriptionText;
                popupQueued = true;
                break;

            case GeneralHUDElement.AttackSlot:
                nameText.text = attackSlotPopupObject.NameText;
                descriptionText.text = attackSlotPopupObject.DescriptionText;
                popupQueued = true;
                break;

            case GeneralHUDElement.DefenseSlot:
                nameText.text = defenseSlotPopupObject.NameText;
                descriptionText.text = defenseSlotPopupObject.DescriptionText;
                popupQueued = true;
                break;
        }
    }

    private void Update()
    {
        //if(popupQueued)
        //{
        //    CheckTimer();
        //}
    }

    protected override bool ClearedIfEmpty(GeneralHUDElement newData)
    {
        currentTimer = 0f;

        if (newData == GeneralHUDElement.None)
        {
            popupObject.SetActive(false);
            return true;
        }

        return false;
    }

    private void CheckTimer()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= CombatManager.instance.PopupUIManager.TextPace)
        {
            popupQueued = false;
            popupObject.SetActive(true);
        }
    }
}
