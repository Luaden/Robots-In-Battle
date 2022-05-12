using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCameraBoomMoveController : MonoBehaviour
{
    [SerializeField] private Animator boomAnimator;

    public delegate void onInventoryPositionReached();
    public static event onInventoryPositionReached OnInventoryPositionReached;

    public delegate void onShopPositionReached();
    public static event onShopPositionReached OnShopPositionReached;

    private void Start()
    {
        ShopFadeController.OnReadyToMoveFromShop += MoveToInventoryPosition;
        InventoryFadeController.OnReadyToMoveFromInventory += MoveToShopPosition;
    }

    private void OnDestroy()
    {
        ShopFadeController.OnReadyToMoveFromShop -= MoveToInventoryPosition;
        InventoryFadeController.OnReadyToMoveFromInventory -= MoveToShopPosition;
    }

    public void MoveToShopPosition()
    {
        boomAnimator.SetTrigger("checkingShop");
    }

    public void MoveToInventoryPosition()
    {
        boomAnimator.SetTrigger("checkingInventory");
    }

    private void ReachedInventory()
    {
        boomAnimator.ResetTrigger("checkingInventory");
        OnInventoryPositionReached?.Invoke();
    }

    private void ReachedShop()
    {
        boomAnimator.ResetTrigger("checkingShop");
        OnShopPositionReached?.Invoke();
    }
}
