using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatCardUIPopupController : BaseUIElement<CardDataObject>
{
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private RectTransform popupRect;
    [SerializeField] protected GameObject popupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text energyCostText;
    [SerializeField] private TMP_Text damageDealtText;

    public override void UpdateUI(CardDataObject primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        nameText.text = primaryData.CardName;
        descriptionText.text = primaryData.CardDescription;
        energyCostText.text = ("Energy: ") + primaryData.EnergyCost.ToString();
        damageDealtText.text = ("Damage: ") + primaryData.BaseDamage.ToString();

        Vector3 mousePosition = Input.mousePosition / mainCanvas.scaleFactor;

        if (mousePosition.x + popupObject.GetComponent<RectTransform>().rect.width > mainCanvas.GetComponent<RectTransform>().rect.width)
        {
            mousePosition.x = mainCanvas.GetComponent<RectTransform>().rect.width - popupObject.GetComponent<RectTransform>().rect.width;
        }

        if (mousePosition.y + popupObject.GetComponent<RectTransform>().rect.height > mainCanvas.GetComponent<RectTransform>().rect.height)
        {
            mousePosition.y = mainCanvas.GetComponent<RectTransform>().rect.height - popupObject.GetComponent<RectTransform>().rect.height;
        }

        popupRect.anchoredPosition = mousePosition;

        popupObject.SetActive(true);
    }

    protected override bool ClearedIfEmpty(CardDataObject newData)
    {
        if (newData == null)
        {
            nameText.text = string.Empty;
            descriptionText.text = string.Empty;
            energyCostText.text = string.Empty;
            damageDealtText.text = string.Empty;

            popupObject.SetActive(false);
            return true;
        }

        return false;
    }
}
