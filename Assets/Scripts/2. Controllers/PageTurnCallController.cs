using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageTurnCallController : MonoBehaviour
{
    [SerializeField] private SceneTransitionController sceneTransitionController;

    public void EndPageTurn()
    {
        sceneTransitionController.FirstPageTurned();
    }

    public void EndPageTwoTurn()
    {
        //sceneTransitionController.SecondPageTurned();
    }
}
