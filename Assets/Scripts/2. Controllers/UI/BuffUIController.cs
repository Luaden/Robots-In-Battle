using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUIController : BaseUIElement<SOCardEffectObject>
{
    public override void UpdateUI(SOCardEffectObject primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;


    }

    protected override bool ClearedIfEmpty(SOCardEffectObject newData)
    {
        //Check new Data, clear old Data
        //return true;

        return false;
    }
}
