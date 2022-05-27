using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    [SerializeField] private Canvas mainCanvas;

    private void Start()
    {
        GameManager.instance.CurrentMainCanvas = mainCanvas;
        AudioController.instance.PlayMusic(ThemeType.Credits);
    }
}
