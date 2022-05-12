using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryFadeController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryObject;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private bool fadeIn = false;

    private bool movingToInventory = false;

    public delegate void onReadyToMoveFromInventory();
    public static event onReadyToMoveFromInventory OnReadyToMoveFromInventory;

    private void Start()
    {
        ShopCameraBoomMoveController.OnInventoryPositionReached += MoveToInventory;
        DowntimeManager.OnMoveToShop += MoveAwayFromInventory;
    }

    private void Update()
    {
        if (movingToInventory)
        {
            FadeCanvasGroup();
        }
    }

    private void OnDestroy()
    {
        ShopCameraBoomMoveController.OnInventoryPositionReached -= MoveToInventory;
        DowntimeManager.OnMoveToShop -= MoveAwayFromInventory;
    }

    private void FadeCanvasGroup()
    {
        if (fadeIn)
        {
            if (canvasGroup.alpha == 1)
                return;

            if (!inventoryObject.activeSelf)
                inventoryObject.SetActive(true);

            canvasGroup.alpha = canvasGroup.alpha + fadeSpeed * Time.deltaTime;
        }
        else
        {
            if (canvasGroup.alpha == 0)
                return;

            canvasGroup.alpha = canvasGroup.alpha - fadeSpeed * Time.deltaTime;

            if (canvasGroup.alpha == 0f)
            {
                OnReadyToMoveFromInventory?.Invoke();
                inventoryObject.SetActive(false);
            }
        }
    }

    private void MoveToInventory()
    {
        movingToInventory = true;
        fadeIn = true;
    }

    private void MoveAwayFromInventory()
    {
        fadeIn = false;
    }
}
