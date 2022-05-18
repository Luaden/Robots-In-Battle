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
            animator.ResetTrigger("isPunching");
            animator.ResetTrigger("isKicking");
            animator.ResetTrigger("isGuarding");
            animator.ResetTrigger("isCountering");
            animator.ResetTrigger("isDamaged");
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
                isAnimating = false;
                break;

            case AnimationType.Win:
                animator.SetTrigger("isWinning");
                isAnimating = false;
                break;

            case AnimationType.Lose:
                animator.SetTrigger("isLosing");
                isAnimating = false;
                break;
        }
    }

    private void MoveCameraToAttack()
    {
        if(mechController == CharacterSelect.Player)
            OnAttackingOpponent?.Invoke();
        if (mechController == CharacterSelect.Opponent)
            OnAttackingPlayer?.Invoke();
    }
}
