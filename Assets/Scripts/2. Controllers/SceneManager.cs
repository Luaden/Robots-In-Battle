using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private SceneTransitionController sceneTransitionController;
    
    private int sceneToLoad;
    private bool sceneQueued;

    public static SceneManager instance;

    public void LoadTitleScene()
    {
        AudioController.instance.PlaySound(SoundType.RecordScratch);
        sceneToLoad = 0;
        sceneQueued = true;
        sceneTransitionController.LoadTransitionScene();
    }

    public void LoadWorkshopScene()
    {
        AudioController.instance.PlaySound(SoundType.RecordScratch);
        sceneToLoad = 1;
        sceneQueued = true;
        sceneTransitionController.LoadTransitionScene();
    }

    public void LoadCombatScene()
    {
        AudioController.instance.PlaySound(SoundType.RecordScratch);
        sceneToLoad = 2;
        sceneQueued = true;
        sceneTransitionController.LoadTransitionScene();
    }

    public void LoadCreditsScene()
    {
        AudioController.instance.PlaySound(SoundType.RecordScratch);
        sceneToLoad = 3;
        sceneQueued = true;
        sceneTransitionController.LoadTransitionScene();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
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
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
            sceneQueued = false;
        }
    }
}
