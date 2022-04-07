using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPopupObject : MonoBehaviour
{
    private List<PopupData> popupDataObjects;
    private Text textName;
    private Text textDescription;

    public List<PopupData> PopupDataObjects { get => popupDataObjects; }

    public void Assign(PopupData popupDataObject)
    {
        textName.text = popupDataObject.Name;
        textDescription.text = popupDataObject.Description;

        popupDataObjects.Add(popupDataObject);
    }

}
