using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCombatUIButton : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.BuildMech();
    }
}
