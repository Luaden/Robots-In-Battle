using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    public void LoadTitleScene()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadWorkshopScene()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadCombatScene()
    {
        SceneManager.LoadScene(2);
    }

    private void Start()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
