using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopFadeController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeSpeed;

    private bool fadeComplete = false;

    private void Update()
    {
        if(!fadeComplete)
            FadeCanvasGroup();
    }

    private void FadeCanvasGroup()
    {
        canvasGroup.alpha = canvasGroup.alpha + fadeSpeed * Time.deltaTime;

        if (canvasGroup.alpha == 1f)
        {
            fadeComplete = true;
            DowntimeManager.instance.InitializeShop();
        }
    }
}
