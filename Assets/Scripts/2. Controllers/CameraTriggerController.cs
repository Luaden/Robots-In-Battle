using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerController : MonoBehaviour
{
    [SerializeField] private Animator cameraAnim;
    [SerializeField] private CameraMoveController cameraMoveController;
    private Vector3 startPos;

    private void Start()
    {
        cameraAnim = GetComponent<Animator>();
        startPos = transform.position;
    }

    public void ResetTriggers()
    {
        cameraAnim.ResetTrigger("isAttackingOpponent");
        cameraAnim.ResetTrigger("isAttackingPlayer");
        cameraAnim.ResetTrigger("isFilmingADopeTrailer");

        transform.position = startPos;
        cameraMoveController.EnablePlayerHasControl();
    }    
}
