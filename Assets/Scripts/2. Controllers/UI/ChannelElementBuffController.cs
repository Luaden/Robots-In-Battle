using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChannelElementBuffController : BaseUIElement<Dictionary<Channels, List<ElementStackObject>>>
{
    [SerializeField] private GameObject highChannelIceEffect;
    [SerializeField] private GameObject highChannelAcidEffect;
    [SerializeField] private TMP_Text highChannelIceEffectText;
    [SerializeField] private TMP_Text highChannelAcidEffectText;
    [SerializeField] private GameObject midChannelIceEffect;
    [SerializeField] private GameObject midChannelAcidEffect;
    [SerializeField] private TMP_Text midChannelIceEffectText;
    [SerializeField] private TMP_Text midChannelAcidEffectText;
    [SerializeField] private GameObject lowChannelIceEffect;
    [SerializeField] private GameObject lowChannelAcidEffect;
    [SerializeField] private TMP_Text lowChannelIceEffectText;
    [SerializeField] private TMP_Text lowChannelAcidEffectText;


    public override void UpdateUI(Dictionary<Channels, List<ElementStackObject>> primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        List<ElementStackObject> checkValue = new List<ElementStackObject>();

        if (primaryData.TryGetValue(Channels.High, out checkValue))
        {
            foreach(ElementStackObject elementStack in checkValue)
            {
                if(elementStack.ElementType == ElementType.Ice)
                {
                    highChannelIceEffect.SetActive(true);
                    highChannelIceEffectText.text = elementStack.ElementStacks.ToString();
                }

                if (elementStack.ElementType == ElementType.Acid)
                {
                    highChannelAcidEffect.SetActive(true);
                    highChannelAcidEffectText.text = elementStack.ElementStacks.ToString();
                }
            }

            if (!checkValue.Select(x => x.ElementType).Contains(ElementType.Ice))
            {
                highChannelIceEffect.SetActive(false);
                highChannelIceEffectText.text = "";
            }
            if (!checkValue.Select(x => x.ElementType).Contains(ElementType.Acid))
            {
                highChannelAcidEffect.SetActive(false);
                highChannelAcidEffectText.text = "";
            }
        }

        if (primaryData.TryGetValue(Channels.Mid, out checkValue))
        {
            foreach (ElementStackObject elementStack in checkValue)
            {
                if (elementStack.ElementType == ElementType.Ice)
                {
                    midChannelIceEffect.SetActive(true);
                    midChannelIceEffectText.text = elementStack.ElementStacks.ToString();
                }

                if (elementStack.ElementType == ElementType.Acid)
                {
                    midChannelAcidEffect.SetActive(true);
                    midChannelAcidEffectText.text = elementStack.ElementStacks.ToString();
                }
            }
            
            if (!checkValue.Select(x => x.ElementType).Contains(ElementType.Ice))
            {
                midChannelIceEffect.SetActive(false);
                midChannelIceEffectText.text = "";
            }
            if (!checkValue.Select(x => x.ElementType).Contains(ElementType.Acid))
            {
                midChannelAcidEffect.SetActive(false);
                midChannelAcidEffectText.text = "";
            }
        }

        if (primaryData.TryGetValue(Channels.Low, out checkValue))
        {
            foreach (ElementStackObject elementStack in checkValue)
            {
                if (elementStack.ElementType == ElementType.Ice)
                {
                    lowChannelIceEffect.SetActive(true);
                    lowChannelIceEffectText.text = elementStack.ElementStacks.ToString();
                }

                if (elementStack.ElementType == ElementType.Acid)
                {
                    lowChannelAcidEffect.SetActive(true);
                    lowChannelAcidEffectText.text = elementStack.ElementStacks.ToString();
                }
            }

            if (!checkValue.Select(x => x.ElementType).Contains(ElementType.Ice))
            {
                lowChannelIceEffect.SetActive(false);
                lowChannelIceEffectText.text = "";
            }
            if (!checkValue.Select(x => x.ElementType).Contains(ElementType.Acid))
            {
                lowChannelAcidEffect.SetActive(false);
                lowChannelAcidEffectText.text = "";
            }
        }
    }

    protected override bool ClearedIfEmpty(Dictionary<Channels, List<ElementStackObject>> newData)
    {
        List<ElementStackObject> checkValue = new List<ElementStackObject>();

        if (newData.Keys.Count == 0)
        {
            highChannelIceEffectText.text = null;
            highChannelAcidEffectText.text = null;
            midChannelIceEffectText.text = null;
            midChannelAcidEffectText.text = null;
            lowChannelIceEffectText.text = null;
            lowChannelAcidEffectText.text = null;

            highChannelIceEffect.SetActive(false);
            highChannelAcidEffect.SetActive(false);
            midChannelIceEffect.SetActive(false);
            midChannelAcidEffect.SetActive(false);
            lowChannelIceEffect.SetActive(false);
            lowChannelAcidEffect.SetActive(false);

            return true;
        }

        if (!newData.TryGetValue(Channels.High, out checkValue))
        {
            highChannelIceEffectText.text = null;
            highChannelAcidEffectText.text = null;

            highChannelIceEffect.SetActive(false);
            highChannelAcidEffect.SetActive(false);
        }

        if (!newData.TryGetValue(Channels.Mid, out checkValue))
        {
            midChannelIceEffectText.text = null;
            midChannelAcidEffectText.text = null;

            midChannelIceEffect.SetActive(false);
            midChannelAcidEffect.SetActive(false);
        }

        if (!newData.TryGetValue(Channels.Low, out checkValue))
        {
            lowChannelIceEffectText.text = null;
            lowChannelAcidEffectText.text = null;

            lowChannelIceEffect.SetActive(false);
            lowChannelAcidEffect.SetActive(false);
        }

        return false;
    }
}
