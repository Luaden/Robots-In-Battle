using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Canvas mainCanvas;

    public void LoadCombatScene()
    {
        GameManager.instance.LoadCombatScene();
    }

    public void LoadWorkShopScene()
    {
        GameManager.instance.LoadWorkshopScene();
    }

    public void OpenOptionsCanvas()
    {
        GameManager.instance.OpenOptionsCanvas();
    }

    public void LoadCreditsScene()
    {
        GameManager.instance.LoadCreditsScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadBossScene()
    {
        GameManager.instance.LoadBossScene();
    }

    private void Start()
    {
        GameManager.instance.CurrentMainCanvas = mainCanvas;
        AudioController.instance.PlayMusic(ThemeType.Title);
    }
}
