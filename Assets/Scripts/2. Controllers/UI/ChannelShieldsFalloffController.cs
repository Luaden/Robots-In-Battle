using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChannelShieldsFalloffController : BaseUIElement<Dictionary<Channels, List<ChannelShieldFalloffObject>>>
{
    [SerializeField] private GameObject highChannelShieldUIElement;
    [SerializeField] private TMP_Text highChannelShieldGainText;
    [SerializeField] private TMP_Text highChannelShieldFalloffText;
    [SerializeField] private GameObject midChannelShieldUIElement;
    [SerializeField] private TMP_Text midChannelShieldGainText;
    [SerializeField] private TMP_Text midChannelShieldFalloffText;
    [SerializeField] private GameObject lowChannelShieldUIElement;
    [SerializeField] private TMP_Text lowChannelShieldGainText;
    [SerializeField] private TMP_Text lowChannelShieldFalloffText;


    public override void UpdateUI(Dictionary<Channels, List<ChannelShieldFalloffObject>> primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        List<ChannelShieldFalloffObject> checkValue;
        int startingShield;
        int falloffAmount;

        if (primaryData.TryGetValue(Channels.High, out checkValue))
        {
            startingShield = 0;
            falloffAmount = 0;

            foreach (ChannelShieldFalloffObject channelShieldObj in checkValue)
            {
                startingShield += channelShieldObj.StartingShieldPerTurn;
                falloffAmount += channelShieldObj.FalloffPerTurn;
            }

            highChannelShieldGainText.text = startingShield.ToString();
            highChannelShieldFalloffText.text = falloffAmount.ToString();
            highChannelShieldUIElement.SetActive(true);
        }

        if (primaryData.TryGetValue(Channels.Mid, out checkValue))
        {
            startingShield = 0;
            falloffAmount = 0;

            foreach (ChannelShieldFalloffObject channelShieldObj in checkValue)
            {
                startingShield += channelShieldObj.StartingShieldPerTurn;
                falloffAmount += channelShieldObj.FalloffPerTurn;
            }

            midChannelShieldGainText.text = startingShield.ToString();
            midChannelShieldFalloffText.text = falloffAmount.ToString();
            midChannelShieldUIElement.SetActive(true);
        }

        if (primaryData.TryGetValue(Channels.Low, out checkValue))
        {
            startingShield = 0;
            falloffAmount = 0;

            foreach (ChannelShieldFalloffObject channelShieldObj in checkValue)
            {
                startingShield += channelShieldObj.StartingShieldPerTurn;
                falloffAmount += channelShieldObj.FalloffPerTurn;
            }

            lowChannelShieldGainText.text = startingShield.ToString();
            lowChannelShieldFalloffText.text = falloffAmount.ToString();
            lowChannelShieldUIElement.SetActive(true);
        }
    }

    protected override bool ClearedIfEmpty(Dictionary<Channels, List<ChannelShieldFalloffObject>> newData)
    {
        List<ChannelShieldFalloffObject> checkValue;

        if (newData.Keys.Count == 0)
        {
            highChannelShieldGainText.text = null;
            highChannelShieldFalloffText.text = null;
            midChannelShieldGainText.text = null;
            midChannelShieldFalloffText.text = null;
            lowChannelShieldGainText.text = null;

            highChannelShieldUIElement.SetActive(false);
            midChannelShieldUIElement.SetActive(false);
            lowChannelShieldUIElement.SetActive(false);
            return true;
        }

        if (!newData.TryGetValue(Channels.High, out checkValue))
        {
            highChannelShieldGainText.text = null;
            highChannelShieldFalloffText.text = null;
            highChannelShieldUIElement.SetActive(false);
        }

        if (!newData.TryGetValue(Channels.Mid, out checkValue))
        {
            midChannelShieldGainText.text = null;
            midChannelShieldFalloffText.text = null;
            midChannelShieldUIElement.SetActive(false);
        }

        if (!newData.TryGetValue(Channels.Low, out checkValue))
        {
            lowChannelShieldGainText.text = null;
            lowChannelShieldFalloffText.text = null;
            lowChannelShieldUIElement.SetActive(false);
        }

        return false;
    }
}
