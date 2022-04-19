using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChannelShieldsController : BaseUIElement<Dictionary<Channels, int>>
{
    [SerializeField] private GameObject highChannelShield;
    [SerializeField] private TMP_Text highChannelShieldText;
    [SerializeField] private GameObject midChannelShield;
    [SerializeField] private TMP_Text midChannelShieldText;
    [SerializeField] private GameObject lowChannelShield;
    [SerializeField] private TMP_Text lowChannelShieldText;

    public override void UpdateUI(Dictionary<Channels, int> primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        int checkValue;

        if (primaryData.TryGetValue(Channels.High, out checkValue))
        {
            highChannelShieldText.text = checkValue.ToString();
            highChannelShield.SetActive(true);
        }

        if (primaryData.TryGetValue(Channels.Mid, out checkValue))
        {
            midChannelShieldText.text = checkValue.ToString();
            midChannelShield.SetActive(true);
        }

        if (primaryData.TryGetValue(Channels.Low, out checkValue))
        {
            lowChannelShieldText.text = checkValue.ToString();
            lowChannelShield.SetActive(true);
        }
    }

    protected override bool ClearedIfEmpty(Dictionary<Channels, int> newData)
    {
        int checkValue;

        if(newData.Keys.Count == 0)
        {
            highChannelShieldText.text = null;
            midChannelShieldText.text = null;
            lowChannelShieldText.text = null;

            highChannelShield.SetActive(false);
            midChannelShield.SetActive(false);
            lowChannelShield.SetActive(false);
            return true;
        }

        if(!newData.TryGetValue(Channels.High, out checkValue))
        {
            highChannelShieldText.text = null;
            highChannelShield.SetActive(false);
        }

        if (!newData.TryGetValue(Channels.Mid, out checkValue))
        {
            midChannelShieldText.text = null;
            midChannelShield.SetActive(false);
        }

        if (!newData.TryGetValue(Channels.Low, out checkValue))
        {
            lowChannelShieldText.text = null;
            lowChannelShield.SetActive(false);
        }

        return false;
    }
}
