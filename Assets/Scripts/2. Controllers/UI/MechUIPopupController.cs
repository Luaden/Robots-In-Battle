using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechUIPopupController : MonoBehaviour
{
    [SerializeField] protected GameObject popupObject;
    private RectTransform rectTransform;

    public void HandlePopup(MechObject mechObject,
                            Transform transform,
                            Vector3 cursorPosition)
    {
        //MechPopupObject mechPopup;
        // create a popup anchored to transform location to display card details
    }

    public void ClearPopup()
    {
        popupObject.SetActive(false);
    }

}
