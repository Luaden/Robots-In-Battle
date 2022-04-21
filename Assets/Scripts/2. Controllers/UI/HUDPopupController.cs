using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDPopupController : MonoBehaviour
{
    [SerializeField] protected GameObject popupObject;
    private RectTransform rectTransform;
    public void HandlePopup(List<SOCardEffectObject> cardEffectObjects,
                            Transform transform,
                            Vector3 cursorPosition)
    {
        //BuffPopupObject buffPopup;
        // create a popup anchored to transform location

    }

    public void InactivatePopup()
    {
        popupObject.SetActive(false);
    }
}
