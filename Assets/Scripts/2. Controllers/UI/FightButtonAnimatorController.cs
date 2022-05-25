using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightButtonAnimatorController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        ChannelsUISlotManager.OnSlotFilledOrRemoved += CheckFightButtonStatus;
        animator = GetComponent<Animator>();
    }

    private void CheckFightButtonStatus(int slotsFilled)
    {
        if (slotsFilled == 2)
            FightButtonAppear();
        else if (slotsFilled < 2)
            FightButtonDisappear();           
    }

    private void FightButtonAppear()
    {
        if(!animator.isActiveAndEnabled)
            animator.enabled = true;

        animator.SetTrigger("isAppearing");
        animator.ResetTrigger("isDisappearing");
    }

    private void FightButtonDisappear()
    {
        if (!animator.isActiveAndEnabled)
            animator.enabled = true;

        animator.SetTrigger("isDisappearing");
        animator.ResetTrigger("isAppearing");
    }
}
