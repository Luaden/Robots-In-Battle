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
    private bool fadeOut = true;

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<CardUIController>() == null)
        {
            Debug.Log("Item was dropped in a slot that does not fit it.");
            return;
        }

        if (CombatManager.instance.CanPlayCards && eventData.pointerDrag.GetComponent<CardUIController>().IsPlayerCard)
        {
            slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<CardUIController>(), this);
        }
    }
    private void Update()
    {
        if(channelFlag != Channels.None)
            FlashChannelColor();
    }

    private void Awake()
    {
        if(channelFlag != Channels.None)
            CardUIController.OnPickUp += CheckPickUpFlash;
    }

    private void OnDestroy()
    {
        if (channelFlag != Channels.None)
            CardUIController.OnPickUp -= CheckPickUpFlash;
    }

    private void CheckPickUpFlash(Channels possibleChannels)
    {
        if (possibleChannels == Channels.None)
            flashChannel = false;

        if (possibleChannels.HasFlag(channelFlag))
            flashChannel = true;
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
