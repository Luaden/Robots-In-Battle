using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    [SerializeField] private float xDriftMaximum;
    [SerializeField] private float yDriftMaximum;
    [SerializeField] private float yDriftMinimum;
    [SerializeField] private float driftSpeed;

    [SerializeField] private Animator cameraAnim;

    private Vector3 startPos;
    private Vector3 mousePosMax;
    private float xMax;
    private float yMax;
    private float yMin;
    private bool playerHasControl = true;

    private void Start()
    {
        xMax = transform.position.x + xDriftMaximum;
        yMax = transform.position.y + yDriftMaximum;
        yMin = transform.position.y - yDriftMinimum;
        startPos = transform.position;

        CombatSequenceManager.OnCombatComplete += EnablePlayerHasControl;
        MechAnimationController.OnAttackingPlayer += AttackingPlayer;
        MechAnimationController.OnAttackingOpponent += AttackingOpponent;
    }

    private void Update()
    {
        DriftCameraWithMouse();
    }

    private void OnDestroy()
    {
        CombatSequenceManager.OnCombatComplete -= EnablePlayerHasControl;
        MechAnimationController.OnAttackingPlayer += AttackingPlayer;
        MechAnimationController.OnAttackingOpponent += AttackingOpponent;
    }

    public void AttackingOpponent()
    {
        playerHasControl = false;
        cameraAnim.SetTrigger("isAttackingOpponent");
    }

    public void AttackingPlayer()
    {
        playerHasControl = false;
        cameraAnim.SetTrigger("isAttackingPlayer");
    }

    private void DriftCameraWithMouse()
    {
        if (playerHasControl)
        {
            mousePosMax = new Vector3(Mathf.Clamp(Input.mousePosition.x - (Screen.width / 2), -xMax, xMax),
                                  Mathf.Clamp(Input.mousePosition.y - (Screen.height / 2), yMin, yMax),
                                  startPos.z);

            transform.position = Vector3.Slerp(Camera.main.transform.position, mousePosMax, driftSpeed * Time.deltaTime);

        }

        if (!playerHasControl)
        {
            //xMax = transform.position.x + xDriftMaximum;
            //yMax = transform.position.y + yDriftMaximum;
            //yMin = transform.position.y - yDriftMinimum;
            //transform.position = Vector3.Slerp(transform.position, startPos, driftSpeed * 10 * Time.deltaTime);
        }

    }

    //private void DisablePlayerHasControl()
    //{
    //    playerHasControl = false;
    //}

    private void EnablePlayerHasControl()
    {
        playerHasControl = true;
    }
}
