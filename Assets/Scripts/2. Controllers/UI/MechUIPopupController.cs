using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechUIPopupController : BaseUIElement<Channels>
{
    [SerializeField] protected GameObject highChannelPopupObject;
    [SerializeField] protected GameObject midChannelPopupObject;
    [SerializeField] protected GameObject lowChannelPopupObject;

    public override void UpdateUI(Channels primaryData)
    {
        throw new System.NotImplementedException();
    }

    protected override bool ClearedIfEmpty(Channels newData)
    {
        throw new System.NotImplementedException();
    }
}
