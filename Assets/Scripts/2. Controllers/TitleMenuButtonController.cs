using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenuButtonController : MonoBehaviour
{
    public void LoadCombatScene()
    {
        GameManager.instance.LoadCombatScene();
    }
}
