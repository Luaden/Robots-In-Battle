using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComponentUIPopupController : BaseUIElement<SOItemDataObject>
{
    [SerializeField] protected GameObject popupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text cDMText;
    [SerializeField] private TMP_Text elementText;

    public override void UpdateUI(SOItemDataObject primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        nameText.text = primaryData.ItemName;
        healthText.text = ("Health: ") + primaryData.ComponentHP.ToString();
        energyText.text = ("Energy: ") + primaryData.ComponentEnergy.ToString();
        cDMText.text = ("Component Damage Multiplier: ") + primaryData.CDMFromComponent.ToString();
        elementText.text = ("Element: ") + System.Enum.GetName(typeof(ElementType), primaryData.ComponentElement);

        popupObject.SetActive(true);
    }

    protected override bool ClearedIfEmpty(SOItemDataObject newData)
    {
        if (newData == null)
        {
            nameText.text = string.Empty;
            healthText.text = string.Empty;
            energyText.text = string.Empty;
            cDMText.text = string.Empty;
            elementText.text = string.Empty;

            popupObject.SetActive(false);
            return true;
        }

        return false;
    }
}