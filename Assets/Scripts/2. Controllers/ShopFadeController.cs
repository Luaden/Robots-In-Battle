using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopFadeController : MonoBehaviour
{
    [SerializeField] private GameObject shopObject;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private bool fadeIn = false;

    private bool eventsComplete = false;

    public delegate void onReadyToMoveFromShop();
    public static event onReadyToMoveFromShop OnReadyToMoveFromShop;

    private void Start()
    {
        ShopCameraBoomMoveController.OnShopPositionReached += MoveToShop;
        DowntimeManager.OnMoveToInventory += MoveAwayFromShop;
        EventManager.OnEventsCompleted += EventsComplete;
    }

    private void Update()
    {
        if(eventsComplete)
        {
            FadeCanvasGroup();
        }
    }

    private void OnDestroy()
    {
        ShopCameraBoomMoveController.OnShopPositionReached -= MoveToShop;
        DowntimeManager.OnMoveToInventory -= MoveAwayFromShop;
        EventManager.OnEventsCompleted -= EventsComplete;
    }

    private void FadeCanvasGroup()
    {
        if(fadeIn)
        {
            if (canvasGroup.alpha == 1)
                return;

            if(!shopObject.activeSelf)
                shopObject.SetActive(true);

            canvasGroup.alpha = canvasGroup.alpha + fadeSpeed * Time.deltaTime;

            if(canvasGroup.alpha == 1f && !DowntimeManager.instance.ShopInitialized)
            {
                DowntimeManager.instance.InitializeShop();
                DowntimeManager.instance.ShopInitialized = true;
            }
        }
        else
        {
            if (canvasGroup.alpha == 0)
                return;

            canvasGroup.alpha = canvasGroup.alpha - fadeSpeed * Time.deltaTime;
            
            if(canvasGroup.alpha == 0f)
            {
                OnReadyToMoveFromShop?.Invoke();
                shopObject.SetActive(false);
            }
        }
    }

    private void EventsComplete()
    {
        eventsComplete = true;
        fadeIn = true;
    }

    private void MoveToShop()
    {
        fadeIn = true;
    }

    private void MoveAwayFromShop()
    {
        fadeIn = false;
    }
}
