using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopCameraMoveController : MonoBehaviour
{
    [SerializeField] private float xDriftMaximum;
    [SerializeField] private float yDriftMaximum;
    [SerializeField] private float yDriftMinimum;
    [SerializeField] private float mouseDriftSpeed;

    private Vector3 startPos;
    private Vector3 mousePosMax;
    private float xMax;
    private float yMax;
    private float yMin;
    private bool playerControlsCamera = true;

    public void ResetStartPos()
    {
        startPos = transform.position;
        xMax = transform.position.x + xDriftMaximum;
        yMax = transform.position.y + yDriftMaximum;
        yMin = transform.position.y - yDriftMinimum;
        playerControlsCamera = true;
    }
    
    private void Start()
    {
        xMax = transform.position.x + xDriftMaximum;
        yMax = transform.position.y + yDriftMaximum;
        yMin = transform.position.y - yDriftMinimum;
        startPos = transform.position;

        ShopCameraBoomMoveController.OnInventoryPositionReached += ResetStartPos;
        ShopCameraBoomMoveController.OnShopPositionReached += ResetStartPos;
        DowntimeManager.OnMoveToInventory += DisablePlayerControlsCamera;
        DowntimeManager.OnMoveToShop += DisablePlayerControlsCamera;
    }

    private void Update()
    {
        if(playerControlsCamera)
        {
            DriftCameraWithMouse();
        }
    }

    private void OnDestroy()
    {
        ShopCameraBoomMoveController.OnInventoryPositionReached -= ResetStartPos;
        ShopCameraBoomMoveController.OnShopPositionReached -= ResetStartPos;
        DowntimeManager.OnMoveToInventory -= DisablePlayerControlsCamera;
        DowntimeManager.OnMoveToShop -= DisablePlayerControlsCamera;
    }

    private void DriftCameraWithMouse()
    {
        mousePosMax = new Vector3(Mathf.Clamp(Input.mousePosition.x - (Screen.width / 2), -xMax, xMax), 
                                  Mathf.Clamp(Input.mousePosition.y - (Screen.height / 2), yMin, yMax),
                                startPos.z);

        transform.position = Vector3.Slerp(transform.position, mousePosMax, mouseDriftSpeed * Time.deltaTime);
    }

    private void DisablePlayerControlsCamera()
    {
        playerControlsCamera = false;
    }
}
