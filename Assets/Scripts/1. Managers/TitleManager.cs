using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Canvas mainCanvas;

    private void Start()
    {
        GameManager.instance.CurrentMainCanvas = mainCanvas;
        AudioController.instance.PlayMusic(ThemeType.Title);
    }

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
}
