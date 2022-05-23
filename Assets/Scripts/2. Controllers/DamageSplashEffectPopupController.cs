using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSplashEffectPopupController : BaseUIElement<CardChannelPairObject>
{
    [SerializeField] private GameObject highChannelSplashEffect;
    [SerializeField] private GameObject midChannelSplashEffect;
    [SerializeField] private GameObject lowChannelSplashEffect;

    private GameObject currentSplashEffect;

    public override void UpdateUI(CardChannelPairObject primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        switch (primaryData.CardData.SelectedChannels)
        {
            case Channels.High:
                highChannelSplashEffect.SetActive(true);
                break;
            case Channels.Mid:
                midChannelSplashEffect.SetActive(true);
                break;
            case Channels.Low:
                lowChannelSplashEffect.SetActive(true);
                break;
            case Channels.All:
                break;
            case Channels.HighMid:
                highChannelSplashEffect.SetActive(true);
                midChannelSplashEffect.SetActive(true);
                break;
            case Channels.LowMid:
                midChannelSplashEffect.SetActive(true);
                lowChannelSplashEffect.SetActive(true);
                break;
        }
    }

    protected override bool ClearedIfEmpty(CardChannelPairObject newData)
    {
        if(newData.CardData.CardCategory != CardCategory.Offensive)
        {
            highChannelSplashEffect.SetActive(false);
            midChannelSplashEffect.SetActive(false);
            lowChannelSplashEffect.SetActive(false);
            return true;
        }

        return false;
    }
}
