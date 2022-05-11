using UnityEngine;
using TMPro;

public class NodeUIPopupController : BaseUIElement<NodeUIController>
{
    [Header("Node Popup Attributes")]
    [SerializeField] protected GameObject popupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text difficultyText;



    public override void UpdateUI(NodeUIController primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        popupObject.SetActive(true);
    }

    protected override bool ClearedIfEmpty(NodeUIController newData)
    {
        if (newData == null)
        {
            nameText.text = string.Empty;
            descriptionText.text = string.Empty;
            healthText.text = string.Empty;
            energyText.text = string.Empty;
            difficultyText.text = string.Empty;

            popupObject.SetActive(false);
            return true;
        }

        return false;
    }
}
