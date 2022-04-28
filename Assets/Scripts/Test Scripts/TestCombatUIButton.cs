using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCombatUIButton : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.BuildMech();
    }

    public void LoadTitle()
    {
        GameManager.instance.ReloadGame();
    }

    public void LoadShoppingScene()
    {
        GameManager.instance.LoadShoppingScene();
    }
}