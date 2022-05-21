using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource dialogueAudioSource;

    private List<AudioClip> dialogueSounds;
    private AudioClip queuedMusic;
    private float masterVolume = 1f;
    private float sfxAudioVolume = 1f;
    private float bgmAudioVolume = 1f;
    private float dialogueAudioVolume = 1f;
    private bool musicQueued = false;
    private float previousBGMVolume;

    public static AudioController instance;

    public float MasterVolume { get => masterVolume; set => UpdateMasterVolume(value); }
    public float SFXVolume { get => SFXVolume; set => UpdateSFXVolume(value); }
    public float BGMVolume { get => BGMVolume; set => UpdateBGMVolume(value); }
    public float DialogueVolumet { get => dialogueAudioVolume; }

    public void PlaySound(AudioClip clipToPlay) => sfxAudioSource.PlayOneShot(clipToPlay);

    public void PlayMusic(AudioClip clipToPlay)
    {
        if (bgmAudioSource.clip == clipToPlay)
            return;

        if (bgmAudioSource.clip != null)
        {
            queuedMusic = clipToPlay;
            musicQueued = true;
            previousBGMVolume = bgmAudioVolume;
        }
    }

    public void PlayDialogue(AudioClip clipToPlay)
    {
        dialogueAudioSource.clip = clipToPlay;
        dialogueAudioSource.Play();
    }

    [ContextMenu("Do it")]
    public void PlayAgain()
    {
        dialogueAudioSource.Play();
    }

    public void StopDialogue()
    {
        dialogueAudioSource.Stop();
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

        bgmAudioSource.loop = true;
        dialogueAudioSource.loop = true;

        //We'll use this if we develop a playerprefs setup for players.
        //MasterVolume = GameManager.Instance.Config.MasterVolume;
        //SFXVolume = GameManager.Instance.Config.SFXVolume;
        //BGMVolume = GameManager.Instance.Config.BGMVolume;

        dialogueAudioSource.volume = sfxAudioVolume;
    }

    private void Update()
    {
        ChangeBGM();
    }

    private void ChangeBGM()
    {
        if(musicQueued && queuedMusic != null)
        {
            UpdateBGMVolume(bgmAudioVolume - Time.deltaTime);

            if(bgmAudioVolume <= 0f)
            {
                bgmAudioSource.clip = queuedMusic;
                bgmAudioSource.Play();

                queuedMusic = null;
            }
        }

        if(musicQueued && queuedMusic == null)
        {
            UpdateBGMVolume(bgmAudioVolume + Time.deltaTime);

            if(bgmAudioVolume <= previousBGMVolume)
            {
                bgmAudioVolume = previousBGMVolume;
                musicQueued = false;
                previousBGMVolume = 0f;
            }
        }
    }

    private void UpdateMasterVolume(float value)
    {
        masterVolume = value;
        sfxAudioSource.volume = sfxAudioVolume * masterVolume;
        bgmAudioSource.volume = bgmAudioVolume * masterVolume;

        //We'll use this if we develop a playerprefs setup for players.
        //GameManager.Instance.Config.MasterVolume = value;
    }

    private void UpdateSFXVolume(float value)
    {
        sfxAudioVolume = value;
        sfxAudioSource.volume = sfxAudioVolume * masterVolume;

        dialogueAudioSource.volume = sfxAudioVolume * masterVolume;
        
        //We'll use this if we develop a playerprefs setup for players.
        //GameManager.Instance.Config.SFXVolume = value;

    }

    private void UpdateBGMVolume(float value)
    {
        bgmAudioVolume = value;
        bgmAudioSource.volume = bgmAudioVolume * masterVolume;

        //We'll use this if we develop a playerprefs setup for players.
        //GameManager.Instance.Config.BGMVolume = value;
    }

    private void UpdateDialogueVolume(float value)
    {
        dialogueAudioVolume = value;
        dialogueAudioSource.volume = dialogueAudioVolume * masterVolume;

        //We'll use this if we develop a playerprefs setup for players.
        //GameManager.Instance.Config.DialogueVolume = value;
    }
}
