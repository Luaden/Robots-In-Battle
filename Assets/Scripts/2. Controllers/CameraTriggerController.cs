using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerController : MonoBehaviour
{
    [SerializeField] private Animator cameraAnim;

    private void Start()
    {
        cameraAnim = GetComponent<Animator>();
    }

    public void ResetTriggers()
    {
        cameraAnim.ResetTrigger("isAttackingOpponent");
        cameraAnim.ResetTrigger("isAttackingPlayer");
    }    
}
