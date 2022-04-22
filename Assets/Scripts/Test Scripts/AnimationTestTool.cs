using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestTool : MonoBehaviour
{
    [SerializeField] private AnimationType playerMechAnimation;
    [SerializeField] private AnimationType opponentMechAnimation;

    private MechAnimationManager mechAnimationManager;

    public void TestAnimations()
    {
        mechAnimationManager.SetMechAnimation(CharacterSelect.Player, playerMechAnimation, CharacterSelect.Opponent, opponentMechAnimation);
    }

    private void Start()
    {
        mechAnimationManager = FindObjectOfType<MechAnimationManager>();
    }
}
