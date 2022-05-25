using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDBuffUIPopupController : BaseUIElement<HUDBuffElement>
{
    [SerializeField] private GameObject popupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private SOHUDBuffDescription fireDescription;
    [SerializeField] private SOHUDBuffDescription plasmaDescription;
    [SerializeField] private SOHUDBuffDescription iceDescription;
    [SerializeField] private SOHUDBuffDescription acidDescription;
    [SerializeField] private SOHUDBuffDescription damageUpDescription;
    [SerializeField] private SOHUDBuffDescription damageDownDescription;
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
                break;

            case HUDBuffElement.DamageUp:
                nameText.text = damageUpDescription.NameText;
                descriptionText.text = damageUpDescription.DescriptionText;
                break;

            case HUDBuffElement.DamageDown:
                nameText.text = damageDownDescription.NameText;
                descriptionText.text = damageDownDescription.DescriptionText;
                break;

            case HUDBuffElement.Flurry:
                nameText.text = flurryDescription.NameText;
                descriptionText.text = flurryDescription.DescriptionText;
                break;

            case HUDBuffElement.Ice:
                nameText.text = iceDescription.NameText;
                descriptionText.text = iceDescription.DescriptionText;
                break;

            case HUDBuffElement.Acid:
                nameText.text = acidDescription.NameText;
                descriptionText.text = acidDescription.DescriptionText;
                break;

            case HUDBuffElement.Fire:
                nameText.text = fireDescription.NameText;
                descriptionText.text = fireDescription.DescriptionText;
                break;

            case HUDBuffElement.Plasma:
                nameText.text = plasmaDescription.NameText;
                descriptionText.text = plasmaDescription.DescriptionText;
                break;

            case HUDBuffElement.Jazzersize:
                nameText.text = jazzersizeDescription.NameText;
                descriptionText.text = jazzersizeDescription.DescriptionText;
                break;
        }

        popupQueued = true;
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
