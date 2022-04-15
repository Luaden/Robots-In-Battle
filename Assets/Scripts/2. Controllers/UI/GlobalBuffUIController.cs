using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBuffUIController : BaseUIElement<List<SOCardEffectObject>>
{
    [SerializeField] private GameObject buffIconPrefab;
    [SerializeField] private Transform parentTransform;
    public override void UpdateUI(List<SOCardEffectObject> primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        foreach(SOCardEffectObject effect in primaryData)
        {
            GameObject buffIcon = Instantiate(buffIconPrefab, parentTransform);
            
        }
    }

    protected override bool ClearedIfEmpty(List<SOCardEffectObject> newData)
    {
        //Check new Data, clear old Data
        //return true;

        return false;
    }
}
