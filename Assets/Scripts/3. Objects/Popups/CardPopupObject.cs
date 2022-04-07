using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPopupObject : MonoBehaviour
{
    private List<PopupData> popupDataToDisplay;
    [SerializeField] protected Text textName;
    [SerializeField] protected Text textDescription;

    public List<PopupData> PopupToDisplay { get => popupDataToDisplay; }

    private void Awake()
    {
        popupDataToDisplay = new List<PopupData>();
    }
    public void Assign(PopupData popupDataObject)
    {
        popupDataToDisplay.Clear();
        textName.text = popupDataObject.Name;
        textDescription.text = popupDataObject.Description;

        popupDataToDisplay.Add(popupDataObject);
    }

}
