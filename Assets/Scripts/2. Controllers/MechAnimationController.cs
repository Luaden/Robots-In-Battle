using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterSelect mechController;
    [SerializeField] private GameObject headDamageEffect;
    [SerializeField] private GameObject torsoDamageEffect;
    [SerializeField] private GameObject legDamageEffect;

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
            animator.ResetTrigger("isDamagedFire");
            animator.ResetTrigger("isDamagedPlasma");
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

            case AnimationType.DamagedFire:
                animator.SetTrigger("isDamagedFire");
                break;

            case AnimationType.DamagedPlasma:
                animator.SetTrigger("isDamagedPlasma");
                break;

            case AnimationType.Lose:
                animator.SetTrigger("isLosing");
                break;
            case AnimationType.Idle:
                break;

            case AnimationType.WorkshopIdle:
                animator.SetBool("isInForRepairs", true);
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

        if (mechObject.MechHead.ComponentCurrentHP <= 0)
            BreakComponent(MechComponent.Head);
        if (mechObject.MechTorso.ComponentCurrentHP <= 0)
            BreakComponent(MechComponent.Torso);
        if (mechObject.MechLegs.ComponentCurrentHP <= 0)
            BreakComponent(MechComponent.Legs);
    }

    public void SetMechBossStatus(bool isBoss)
    {
        animator.SetBool("isBoss", isBoss);
    }

    public void BreakComponent(MechComponent componentType)
    {
        switch (componentType)
        {
            case MechComponent.None:
                break;
            case MechComponent.Head:
                headDamageEffect.SetActive(true);
                break;
            case MechComponent.Torso:
                torsoDamageEffect.SetActive(true);
                break;
            case MechComponent.Arms:
                break;
            case MechComponent.Legs:
                break;
                legDamageEffect.SetActive(true);
            case MechComponent.Back:
                break;
        }
    }

    private void MoveCameraToAttack()
    {
        if(mechController == CharacterSelect.Player)
            OnAttackingOpponent?.Invoke();
        else
            OnAttackingPlayer?.Invoke();
    }
}
