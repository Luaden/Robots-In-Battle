using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadTitleScene()
    {
        if(SceneManager.GetSceneAt(0).IsValid())
            SceneManager.LoadScene(0);
    }
    public void LoadDowntimeScene()
    {
        if (SceneManager.GetSceneAt(1).IsValid())
            SceneManager.LoadScene(1);
    }
    public void LoadCombatScene()
    {
        if (SceneManager.GetSceneAt(2).IsValid())
            SceneManager.LoadScene(2);
    }
}
