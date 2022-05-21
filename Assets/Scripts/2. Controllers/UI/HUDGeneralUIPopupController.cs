using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDGeneralUIPopupController : BaseUIElement<HUDGeneralElement>
{
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private RectTransform thisRect;
    [SerializeField] private GameObject popupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private SOGeneralHUDElementDescription highChannelPopupObject;
    [SerializeField] private SOGeneralHUDElementDescription midChannelPopupObject;
    [SerializeField] private SOGeneralHUDElementDescription lowChannelPopupObject;

    [SerializeField] private SOGeneralHUDElementDescription healthPopupObject;
    [SerializeField] private SOGeneralHUDElementDescription energyPopupObject;

    [SerializeField] private SOGeneralHUDElementDescription attackSlotPopupObject;
    [SerializeField] private SOGeneralHUDElementDescription defenseSlotPopupObject;

    private float currentTimer;
    private bool popupQueued;


    public override void UpdateUI(HUDGeneralElement primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        switch (primaryData)
        {
            case HUDGeneralElement.None:
                break;
            case HUDGeneralElement.HighChannel:
                nameText.text = highChannelPopupObject.NameText;
                descriptionText.text = highChannelPopupObject.DescriptionText;
                popupQueued = true;
                break;

            case HUDGeneralElement.MidChannel:
                nameText.text = midChannelPopupObject.NameText;
                descriptionText.text = midChannelPopupObject.DescriptionText;
                popupQueued = true;
                break;
            
            case HUDGeneralElement.LowChannel:
                nameText.text = lowChannelPopupObject.NameText;
                descriptionText.text = lowChannelPopupObject.DescriptionText;
                popupQueued = true;
                break;

            case HUDGeneralElement.Health:
                nameText.text = healthPopupObject.NameText;
                descriptionText.text = healthPopupObject.DescriptionText;
                popupQueued = true;
                break;

            case HUDGeneralElement.Energy:
                nameText.text = energyPopupObject.NameText;
                descriptionText.text = energyPopupObject.DescriptionText;
                popupQueued = true;
                break;

            case HUDGeneralElement.AttackSlot:
                nameText.text = attackSlotPopupObject.NameText;
                descriptionText.text = attackSlotPopupObject.DescriptionText;
                popupQueued = true;
                break;

            case HUDGeneralElement.DefenseSlot:
                nameText.text = defenseSlotPopupObject.NameText;
                descriptionText.text = defenseSlotPopupObject.DescriptionText;
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

    protected override bool ClearedIfEmpty(HUDGeneralElement newData)
    {
        currentTimer = 0f;

        if (newData == HUDGeneralElement.None)
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
            //Vector2 mousePos;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.transform as RectTransform, Input.mousePosition, mainCanvas.worldCamera, out mousePos);


            //Vector3 adjustedPosition = mainCanvas.transform.TransformPoint(mousePos);
            Vector3 mousePosition = Input.mousePosition / mainCanvas.scaleFactor;

            if(mousePosition.x + popupObject.GetComponent<RectTransform>().rect.width > mainCanvas.GetComponent<RectTransform>().rect.width)
            {
                mousePosition.x = mainCanvas.GetComponent<RectTransform>().rect.width - popupObject.GetComponent<RectTransform>().rect.width;
            }

            if (mousePosition.y + popupObject.GetComponent<RectTransform>().rect.height > mainCanvas.GetComponent<RectTransform>().rect.height)
            {
                mousePosition.y = mainCanvas.GetComponent<RectTransform>().rect.height - popupObject.GetComponent<RectTransform>().rect.height;
            }

            thisRect.anchoredPosition = mousePosition;

            popupObject.SetActive(true);
            popupQueued = false;
        }
    }
}
