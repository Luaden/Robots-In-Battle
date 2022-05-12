using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDBuffUIPopupController : BaseUIElement<HUDBuffElement>
{
    [SerializeField] private GameObject popupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private SOHUDBuffDescription shieldDescription;
    [SerializeField] private SOHUDBuffDescription flurryDescription;
    [SerializeField] private SOHUDBuffDescription jazzersizeDescription;


    private float currentTimer;
    private bool popupQueued;


    public override void UpdateUI(HUDBuffElement primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        switch (primaryData)
        {
            case HUDBuffElement.None:
                break;
            case HUDBuffElement.Shield:
                nameText.text = shieldDescription.NameText;
                descriptionText.text = shieldDescription.DescriptionText;
                popupQueued = true;
                break;
            case HUDBuffElement.DamageModifier:
                break;
            case HUDBuffElement.Flurry:
                nameText.text = flurryDescription.NameText;
                descriptionText.text = flurryDescription.DescriptionText;
                popupQueued = true;
                break;
            case HUDBuffElement.Ice:
                break;
            case HUDBuffElement.Acid:
                break;
            case HUDBuffElement.Fire:
                break;
            case HUDBuffElement.Plasma:
                break;
            case HUDBuffElement.Jazzersize:
                nameText.text = jazzersizeDescription.NameText;
                descriptionText.text = jazzersizeDescription.DescriptionText;
                popupQueued = true;
                break;
        }
    }

    private void Update()
    {
        if (popupQueued)
        {
            CheckTimer();
        }
    }

    protected override bool ClearedIfEmpty(HUDBuffElement newData)
    {
        currentTimer = 0f;

        if (newData == HUDBuffElement.None)
        {
            popupObject.SetActive(false);
            popupQueued = false;
            return true;
        }

        return false;
    }

    private void CheckTimer()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= CombatManager.instance.PopupUIManager.GeneralHUDPopupDelay)
        {
            popupQueued = false;
            popupObject.SetActive(true);
        }
    }
}
