using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private SceneTransitionController sceneTransitionController;
    
    private int sceneToLoad;
    private bool sceneQueued;

    public static SceneController instance;

    public void LoadTitleScene()
    {
        sceneToLoad = 0;
        sceneQueued = true;
        sceneTransitionController.LoadTransitionScene();
    }
    public void LoadWorkshopScene()
    {
        sceneToLoad = 1;
        sceneQueued = true;
        sceneTransitionController.LoadTransitionScene();
    }
    public void LoadCombatScene()
    {
        sceneToLoad = 2;
        sceneQueued = true;
        sceneTransitionController.LoadTransitionScene();
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

        sceneTransitionController = GetComponent<SceneTransitionController>();
        SceneTransitionController.OnFirstPageTurned += LoadScene;
    }

    private void OnDestroy()
    {
        SceneTransitionController.OnFirstPageTurned -= LoadScene;
    }

    private void LoadScene()
    {
        if(sceneQueued)
        {
            Debug.Log("Loading scene");
            SceneManager.LoadScene(sceneToLoad);
            sceneQueued = false;
        }
    }
}
