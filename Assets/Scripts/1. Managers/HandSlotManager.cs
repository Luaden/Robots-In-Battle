using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandSlotManager : BaseSlotManager<CardUIObject>
{
    public override void HandleDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.GetComponent<CardUIObject>() == null)
        {
            Debug.Log("Could not find appropriate data for slot.");
            //Tell card to move to previous slot.
            return;
        }


    }
}
