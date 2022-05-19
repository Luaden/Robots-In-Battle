using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterSelect mechController;
    private bool isAnimating;

    public delegate void onAttackingPlayer();
    public static event onAttackingPlayer OnAttackingPlayer;

    public delegate void onAttackingOpponent();
    public static event onAttackingOpponent OnAttackingOpponent;
    public bool IsAnimating { get => isAnimating; }

    public void SetMechAnimation(AnimationType animationType)
    {
        if(animationType == AnimationType.Idle)
        {
            Debug.Log(mechController + " is resetting animations.");
            animator.ResetTrigger("isPunchingHigh");
            animator.ResetTrigger("isPunchingMid");
            animator.ResetTrigger("isSpecialingMid");
            animator.ResetTrigger("isKickingMid");
            animator.ResetTrigger("isKickingLow");
            animator.ResetTrigger("isGuarding");
            animator.ResetTrigger("isCountering");
            animator.ResetTrigger("isDamaged");
            animator.ResetTrigger("isJazzersizing");
            animator.ResetTrigger("isWinning");
            animator.ResetTrigger("isLosing");
            animator.SetTrigger("isIdling");
            isAnimating = false;
            return;
        }

        isAnimating = true;

        switch (animationType)
        {
            case AnimationType.PunchHigh:
                animator.SetTrigger("isPunchingHigh");
                break;

            case AnimationType.PunchMid:
                animator.SetTrigger("isPunchingMid");
                break;

            case AnimationType.KickMid:
                animator.SetTrigger("isKickingMid");
                break;

            case AnimationType.KickLow:
                animator.SetTrigger("isKickingLow");
                break;

            case AnimationType.SpecialMid:
                animator.SetTrigger("isSpecialingMid");
                break;

            case AnimationType.Guard:
                animator.SetTrigger("isGuarding");
                break;

            case AnimationType.Counter:
                animator.SetTrigger("isCountering");
                break;

            case AnimationType.Damaged:
                animator.SetTrigger("isDamaged");
                break;

            case AnimationType.Jazzersize:
                animator.SetTrigger("isJazzersizing");
                break;

            case AnimationType.Win:
                animator.SetTrigger("isWinning");
                break;

            case AnimationType.Lose:
                animator.SetTrigger("isLosing");
                break;
            case AnimationType.Idle:
                break;
        }
    }

    public void SetMechElements(MechObject mechObject)
    {
        switch (mechObject.MechArms.ComponentElement)
        {
            case ElementType.None:
                break;
            case ElementType.Fire:
                animator.SetBool("punchHasFire", true);
                break;
            case ElementType.Ice:
                animator.SetBool("punchHasIce", true);
                break;
            case ElementType.Plasma:
                animator.SetBool("punchHasPlasma", true);
                break;
            case ElementType.Acid:
                animator.SetBool("punchHasAcid", true);
                break;
            case ElementType.Void:
                break;
        }

        switch (mechObject.MechLegs.ComponentElement)
        {
            case ElementType.None:
                break;
            case ElementType.Fire:
                animator.SetBool("kickHasFire", true);
                break;
            case ElementType.Ice:
                animator.SetBool("kickHasIce", true);
                break;
            case ElementType.Plasma:
                animator.SetBool("kickHasPlasma", true);
                break;
            case ElementType.Acid:
                animator.SetBool("kickHasAcid", true);
                break;
            case ElementType.Void:
                break;
        }
    }

    public void SetMechBossStatus(bool isBoss)
    {
        Debug.Log(mechController + " boss status: " + isBoss);
        animator.SetBool("isBoss", isBoss);
    }

    private void MoveCameraToAttack()
    {
        if(mechController == CharacterSelect.Player)
            OnAttackingOpponent?.Invoke();
        else
            OnAttackingPlayer?.Invoke();
    }
}
