using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraMoveController : MonoBehaviour
{
    [SerializeField] private float xDriftMaximum;
    [SerializeField] private float yDriftMaximum;
    [SerializeField] private float yDriftMinimum;
    [SerializeField] private float mouseDriftSpeed;
    [SerializeField] private float mobileDriftSpeed;

    [SerializeField] private float xRotationMax = 10;
    [SerializeField] private float yRotationMax = 10;
    [SerializeField] private Animator cameraAnim;

    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 mousePosMax;
    private float xMax;
    private float yMax;
    private float yMin;
    [SerializeField] private bool playerHasControl = true;
    [SerializeField] private bool cameraMovementDisabled = false;

    private Gyroscope gyroscope;
    private bool gyroEnabled = false;

    public void ToggleCameraMovement()
    {
        cameraMovementDisabled = !cameraMovementDisabled;
    }

    private void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyroEnabled = true;
            gyroscope = Input.gyro;
            gyroscope.enabled = true;
        }

        xMax = transform.position.x + xDriftMaximum;
        yMax = transform.position.y + yDriftMaximum;
        yMin = transform.position.y - yDriftMinimum;
        startPos = transform.localPosition;
        startRot = transform.rotation;


        CombatSequenceManager.OnCombatComplete += EnablePlayerHasControl;
        MechAnimationController.OnAttackingPlayer += AttackingPlayer;
        MechAnimationController.OnAttackingOpponent += AttackingOpponent;

        if (cameraAnim == null)
            cameraAnim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!gyroEnabled)
            DriftCameraWithMouse();
        else
            DriftCameraWithGyro();
    }

    private void OnDestroy()
    {
        CombatSequenceManager.OnCombatComplete -= EnablePlayerHasControl;
        MechAnimationController.OnAttackingPlayer -= AttackingPlayer;
        MechAnimationController.OnAttackingOpponent -= AttackingOpponent;
    }

    public void AttackingOpponent()
    {
        if (!cameraMovementDisabled)
        {
            if (gyroEnabled)
                transform.rotation = startRot;
            else
                transform.position = startPos;

            playerHasControl = false;
            cameraAnim.SetTrigger("isAttackingOpponent");
        }
    }

    public void AttackingPlayer()
    {
        if (!cameraMovementDisabled)
        {
            if (gyroEnabled)
                transform.rotation = startRot;
            else
                transform.position = startPos;

            playerHasControl = false;
            cameraAnim.SetTrigger("isAttackingPlayer");
        }
    }

    private void DriftCameraWithMouse()
    {
        if (playerHasControl && !cameraMovementDisabled)
        {
            mousePosMax = new Vector3(Mathf.Clamp(Input.mousePosition.x - (Screen.width / 2), -xMax, xMax),
                                  Mathf.Clamp(Input.mousePosition.y - (Screen.height / 2), yMin, yMax),
                                  startPos.z);

            transform.localPosition = Vector3.Slerp(Camera.main.transform.position, mousePosMax, mouseDriftSpeed * Time.deltaTime);
        }

        if (cameraMovementDisabled)
        {
            if (transform.position == startPos)
                return;

            transform.position = startPos;
        }
    }

    private void DriftCameraWithGyro()
    {
        if (playerHasControl&& !cameraMovementDisabled)
        {
            float rotationXInput = gyroscope.rotationRateUnbiased.x;
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationXInput, -Vector3.right);
            Quaternion temp = transform.localRotation * xQuaternion;

            if (Quaternion.Angle(startRot, temp) < yRotationMax && Quaternion.Angle(startRot, temp) > -yRotationMax)
                transform.localRotation = Quaternion.Lerp(transform.localRotation, new Quaternion(temp.x, temp.y, transform.localRotation.z, transform.localRotation.w), mobileDriftSpeed * Time.deltaTime);

            float rotationYInput = gyroscope.rotationRateUnbiased.y;
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationYInput, -Vector3.up);
            temp = transform.localRotation * yQuaternion;

            if (Quaternion.Angle(startRot, temp) < yRotationMax && Quaternion.Angle(startRot, temp) > -yRotationMax)
                transform.localRotation = Quaternion.Lerp(transform.localRotation, new Quaternion(temp.x, temp.y, transform.localRotation.z, transform.localRotation.w), mobileDriftSpeed * Time.deltaTime);
           
        }

        if (cameraMovementDisabled)
        {
            if (transform.position == startPos)
                return;

            transform.position = startPos;
        }
    }

    private void EnablePlayerHasControl()
    {
        Debug.Log("Player has control again.");
        playerHasControl = true;
    }
}
