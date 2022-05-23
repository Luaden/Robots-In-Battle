using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUISlotController : BaseSlotController<CardUIController>
{
    [SerializeField] private Channels channelFlag;
    [SerializeField] private Image channelImage;
    [SerializeField] private Color fullColor;
    [SerializeField] private Color fadeColor;

    private bool flashChannel = false;
    private bool combatStarted = false;
    private bool combatComplete = true;
    private bool fadeOut = true;

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData == null)
            return; 
        
        if(eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<CardUIController>() == null)
                return;

            if (CombatManager.instance.CanPlayCards && eventData.pointerDrag.GetComponent<CardUIController>().IsPlayerCard)
            {
                slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<CardUIController>(), this);
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if(CombatManager.instance != null)
        {
            CardUIController newEventData = CombatManager.instance.CardClickController.GetCardFromClick();

            if (newEventData == null)
                return;

            if (CombatManager.instance.CanPlayCards && newEventData.IsPlayerCard)
                slotManager.HandleDrop(eventData, newEventData, this);
        }
    }

    private void Update()
    {
        if(channelFlag != Channels.None)
            FlashChannelColor();

        if(combatStarted && channelFlag != Channels.None)
        {
            if(!combatComplete)
            {
                FadeChannelColorOnCombat();
            }
            else
            {
                GainColorOnCombatComplete();
            }
        }
    }

    private void Awake()
    {
        if(channelFlag != Channels.None)
            CardUIController.OnPickUp += CheckPickUpFlash;

        CombatSequenceManager.OnCombatStart += OnCombatStart;
        CombatSequenceManager.OnCombatComplete += OnCombatComplete;
    }

    private void OnDestroy()
    {
        if (channelFlag != Channels.None)
            CardUIController.OnPickUp -= CheckPickUpFlash;

        CombatSequenceManager.OnCombatStart -= OnCombatStart;
        CombatSequenceManager.OnCombatComplete -= OnCombatComplete;
    }

    private void CheckPickUpFlash(Channels possibleChannels, MechSelect destinationMech, Channels originChannel)
    {
        if (possibleChannels == Channels.None)
            flashChannel = false;

        if (possibleChannels.HasFlag(channelFlag))
            flashChannel = true;
    }

    private void OnCombatStart()
    {
        combatStarted = true;
        combatComplete = false;
    }

    private void OnCombatComplete()
    {
        combatComplete = true;
    }

    private void FadeChannelColorOnCombat()
    {
        if(channelImage.color.a > fadeColor.a)
        {
            channelImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, channelImage.color.a -
                    (CombatManager.instance.ChannelsUISlotManager.ChannelFadeTimeModifier * Time.deltaTime));
        }
    }

    private void GainColorOnCombatComplete()
    {
        channelImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, channelImage.color.a +
                (CombatManager.instance.ChannelsUISlotManager.ChannelFadeTimeModifier * Time.deltaTime));

        if (channelImage.color.a >= fullColor.a)
        {
            channelImage.color = fullColor;
            combatStarted = false;
        }
    }

    private void FlashChannelColor()
    {
        if(flashChannel)
        {
            if(fadeOut)
            {
                channelImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, channelImage.color.a - 
                    (CombatManager.instance.ChannelsUISlotManager.ChannelFadeTimeModifier * Time.deltaTime));

                if (channelImage.color.a <= fadeColor.a)
                    fadeOut = false;
            }

            if(!fadeOut)
            {
                channelImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, channelImage.color.a + 
                    (CombatManager.instance.ChannelsUISlotManager.ChannelFadeTimeModifier * Time.deltaTime));

                if (channelImage.color.a >= fullColor.a)
                    fadeOut = true;
            }
        }
        else
            channelImage.color = Color.Lerp(channelImage.color, fullColor, 
                (CombatManager.instance.ChannelsUISlotManager.ChannelFadeTimeModifier * Time.deltaTime));
    }
}
