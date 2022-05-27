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
    private AnimationType currentAnimation;
    private bool punchHasElement = false;
    private bool kickHasElement = false;
    private bool torsoHasElement = false;

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
                currentAnimation = AnimationType.PunchHigh;
                break;

            case AnimationType.PunchMid:
                animator.SetTrigger("isPunchingMid");
                currentAnimation = AnimationType.PunchMid;
                break;

            case AnimationType.KickMid:
                animator.SetTrigger("isKickingMid");
                currentAnimation = AnimationType.KickMid;
                break;

            case AnimationType.KickLow:
                animator.SetTrigger("isKickingLow");
                currentAnimation = AnimationType.KickLow;
                break;

            case AnimationType.SpecialMid:
                animator.SetTrigger("isSpecialingMid");
                currentAnimation = AnimationType.SpecialMid;
                break;

            case AnimationType.Guard:
                animator.SetTrigger("isGuarding");
                currentAnimation = AnimationType.Guard;
                break;

            case AnimationType.Counter:
                animator.SetTrigger("isCountering");
                currentAnimation = AnimationType.Counter;
                break;

            case AnimationType.Damaged:
                animator.SetTrigger("isDamaged");
                currentAnimation = AnimationType.Damaged;
                break;

            case AnimationType.Jazzersize:
                animator.SetTrigger("isJazzersizing");
                currentAnimation = AnimationType.Jazzersize;
                break;

            case AnimationType.Win:
                animator.SetTrigger("isWinning");
                currentAnimation = AnimationType.Win;
                break;

            case AnimationType.DamagedFire:
                animator.SetTrigger("isDamagedFire");
                currentAnimation = AnimationType.DamagedFire;
                break;

            case AnimationType.DamagedPlasma:
                animator.SetTrigger("isDamagedPlasma");
                currentAnimation = AnimationType.DamagedPlasma;
                break;

            case AnimationType.Lose:
                animator.SetTrigger("isLosing");
                currentAnimation = AnimationType.Lose;
                break;
            case AnimationType.Idle:
                break;

            case AnimationType.WorkshopIdle:
                animator.SetBool("isInForRepairs", true);
                currentAnimation = AnimationType.WorkshopIdle;
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
                punchHasElement = true;
                break;
            case ElementType.Ice:
                animator.SetBool("punchHasIce", true);
                punchHasElement = true;
                break;
            case ElementType.Plasma:
                animator.SetBool("punchHasPlasma", true);
                punchHasElement = true;
                break;
            case ElementType.Acid:
                animator.SetBool("punchHasAcid", true);
                punchHasElement = true;
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
                kickHasElement = true;
                break;
            case ElementType.Ice:
                animator.SetBool("kickHasIce", true);
                kickHasElement = true;
                break;
            case ElementType.Plasma:
                animator.SetBool("kickHasPlasma", true);
                kickHasElement = true;
                break;
            case ElementType.Acid:
                animator.SetBool("kickHasAcid", true);
                kickHasElement = true;
                break;
            case ElementType.Void:
                break;
        }

        if (mechObject.MechTorso.ComponentElement != ElementType.None)
            torsoHasElement = true;

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
                legDamageEffect.SetActive(true);
                break;
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

        switch (currentAnimation)
        {
            case AnimationType.Idle:
                break;
            case AnimationType.PunchHigh:
                if (!punchHasElement)
                {
                    CombatManager.instance.PopupUIManager.HandlePopup(Channels.High);
                    AudioController.instance.PlaySound(SoundType.Punch);
                }
                break;
            case AnimationType.PunchMid:
                if (!punchHasElement)
                {
                    CombatManager.instance.PopupUIManager.HandlePopup(Channels.Mid);
                    AudioController.instance.PlaySound(SoundType.Punch);
                }
                break;
            case AnimationType.KickMid:
                if (!kickHasElement)
                {
                    CombatManager.instance.PopupUIManager.HandlePopup(Channels.Mid);
                    AudioController.instance.PlaySound(SoundType.Kick);
                }
                break;
            case AnimationType.KickLow:
                if (!kickHasElement)
                {
                    CombatManager.instance.PopupUIManager.HandlePopup(Channels.Low);
                    AudioController.instance.PlaySound(SoundType.Kick);
                }
                break;
            case AnimationType.SpecialMid:
                AudioController.instance.PlaySound(SoundType.Beam);

                break;
            case AnimationType.Guard:
                {
                    AudioController.instance.PlaySound(SoundType.Block);
                }
                break;
            case AnimationType.Counter:
                break;
            case AnimationType.Damaged:
                break;
            case AnimationType.Jazzersize:
                break;
            case AnimationType.Win:
                break;
            case AnimationType.Lose:
                break;
            case AnimationType.DamagedFire:
                AudioController.instance.PlaySound(SoundType.Fire);
                break;
            case AnimationType.DamagedPlasma:
                AudioController.instance.PlaySound(SoundType.Plasma);
                break;
            case AnimationType.WorkshopIdle:
                break;
        }
    }
}
