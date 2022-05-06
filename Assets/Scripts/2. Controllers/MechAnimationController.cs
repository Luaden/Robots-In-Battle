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
            case AnimationType.Punch:
                animator.SetTrigger("isPunching");
                break;

            case AnimationType.Kick:
                animator.SetTrigger("isKicking");
                break;

            case AnimationType.Special:
                animator.SetTrigger("isKicking");
                Debug.Log("We're kicking here, but only until we get a proper animation.");
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

            case AnimationType.Win:
                animator.SetTrigger("isWinning");
                break;

            case AnimationType.Lose:
                animator.SetTrigger("isLosing");
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
