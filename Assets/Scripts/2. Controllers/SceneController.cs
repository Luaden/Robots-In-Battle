using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
       if(SceneManager.GetSceneByName(sceneName).IsValid())
            SceneManager.LoadScene(sceneName);
    }
}
