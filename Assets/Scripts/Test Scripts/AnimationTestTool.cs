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
        mechAnimationManager.AddAnimationToQueue(CharacterSelect.Player, playerMechAnimation, CharacterSelect.Opponent, opponentMechAnimation);
    }

    private void Start()
    {
        mechAnimationManager = FindObjectOfType<CombatAnimationManager>();
    }
}
