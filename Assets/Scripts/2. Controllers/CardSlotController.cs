using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotController : BaseSlotController<CardUIController>
{
    private void Start()
    {
        slotManager.AddSlotToList(this);
    }
}
