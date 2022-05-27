using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdaterController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private GameObject musicOffIcon;
    [SerializeField] private GameObject musicOnIcon;
    [SerializeField] private GameObject soundOffIcon;
    [SerializeField] private GameObject soundOnIcon;

    private void OnEnable()
    {
        musicSlider.value = AudioController.instance.BGMVolume * 10;
        sfxSlider.value = AudioController.instance.SFXVolume * 10;
    }

    private void Update()
    {
        if (sfxSlider.value == 0)
        {
            soundOffIcon.SetActive(true);
            soundOnIcon.SetActive(false);
        }
        else
        {
            soundOnIcon.SetActive(true);
            soundOffIcon.SetActive(false);
        }

        if (musicSlider.value == 0)
        {
            musicOffIcon.SetActive(true);
            musicOnIcon.SetActive(false);
        }
        else
        {
            musicOnIcon.SetActive(true);
            musicOffIcon.SetActive(false);
        }
    }
}
