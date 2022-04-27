using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUISlotController : BaseSlotController<CardUIController>
{
    [SerializeField] private Channels channelFlag;
    [SerializeField] private Image channelImage;
    [SerializeField] private Color fullColor;
    [SerializeField] private Color fadeColor;
    [Range(.01f, .99f)][SerializeField] private float fadeTimeModifier;

    private bool flashChannel = false;
    private bool fadeOut = true;

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
                Debug.Log("Flashing, fading out.");

                channelImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, channelImage.color.a - (fadeTimeModifier * Time.deltaTime));

                if (channelImage.color.a <= fadeColor.a)
                    fadeOut = false;
            }

            if(!fadeOut)
            {
                Debug.Log("Flashing, fading in.");
                channelImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, channelImage.color.a + (fadeTimeModifier * Time.deltaTime));

                if (channelImage.color.a >= fullColor.a)
                    fadeOut = true;
            }

            return;
        }

        if(!flashChannel)
        {
            channelImage.color = Color.Lerp(channelImage.color, fullColor, (fadeTimeModifier * Time.deltaTime));
        }
    }
}
