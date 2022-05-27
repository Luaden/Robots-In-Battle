using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VFXPopupController : BaseUIElement<Channels>
{
    [SerializeField] private Animator bangVFXAnim;
    [SerializeField] private Animator boomVFXAnim;
    [SerializeField] private Animator powVFXAnim;


    public override void UpdateUI(Channels primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        switch (primaryData)
        {
            case Channels.High:
                bangVFXAnim.SetTrigger("BANG");
                break;
            case Channels.Mid:
                boomVFXAnim.SetTrigger("BOOM");
                break;
            case Channels.Low:
                powVFXAnim.SetTrigger("POW");
                break;
        }
    }

    protected override bool ClearedIfEmpty(Channels newData)
    {
        if (newData == Channels.None)
            return true;

        bangVFXAnim.ResetTrigger("BANG");
        boomVFXAnim.ResetTrigger("BOOM");
        powVFXAnim.ResetTrigger("POW");

        return false;
    }
}
