using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadTitleScene()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadDowntimeScene()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadCombatScene()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadTutorialScene()
    {
        SceneManager.LoadScene(3);
    }

    public bool CheckIsTutorialScene()
    {
        if (SceneManager.GetActiveScene().name == "Dont Touch (BattleScene Tutorial)")
            return true;
        else
            return false;
    }
}
