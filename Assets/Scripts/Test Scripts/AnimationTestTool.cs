using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestTool : MonoBehaviour
{
    [SerializeField] private AnimationType playerMechAnimation;
    [SerializeField] private AnimationType opponentMechAnimation;

    private CombatAnimationManager mechAnimationManager;

    public void TestAnimations()
    {
        AnimationQueueObject newAnimation = new AnimationQueueObject(CharacterSelect.Player, playerMechAnimation, CharacterSelect.Opponent, opponentMechAnimation);
        mechAnimationManager.SetMechAnimation(newAnimation);
    }

    private void Start()
    {
        mechAnimationManager = FindObjectOfType<CombatAnimationManager>();
    }
}
