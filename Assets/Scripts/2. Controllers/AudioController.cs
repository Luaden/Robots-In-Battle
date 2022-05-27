using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioLookup audioLookup;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource dialogueAudioSource;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;

    private Queue<AudioClip> musicQueue;
    private Queue<AudioClip> soundQueue;
    private float masterVolume = 1f;
    private float sfxAudioVolume = .5f;
    private float bgmAudioVolume = .5f;
    private float dialogueAudioVolume = .5f;
    private bool newImmediateTrack = false;
    private bool newDelayedTrack = false;
    private bool fadeOut = false;
    private bool fadeIn = false;
    private float previousBGMVolume;

    public static AudioController instance;

    public float MasterVolume { get => masterVolume; set => UpdateMasterVolume(value); }
    public float SFXVolume { get => sfxAudioVolume; set => UpdateSFXVolume(value); }
    public float BGMVolume { get => bgmAudioVolume; set => UpdateBGMVolume(value); }
    public float DialogueVolumet { get => dialogueAudioVolume; }

    public void PlaySound(AudioClip clipToPlay) => sfxAudioSource.PlayOneShot(clipToPlay);

    public void PlaySound(SoundType sound)
    {
        PlaySound(audioLookup.GetSound(sound));
    }

    public void QueueSound(SoundType sound)
    {
        if (!sfxAudioSource.isPlaying)
            sfxAudioSource.clip = audioLookup.GetSound(sound);
        else
            soundQueue.Enqueue(audioLookup.GetSound(sound));
    }

    public void PlayMusic(AudioClip clipToPlay, bool playImmediately = true)
    {
        if (bgmAudioSource.clip == clipToPlay)
            return;

        if (bgmAudioSource.clip != null)
        {
            musicQueue.Enqueue(clipToPlay);
            newImmediateTrack = true;
            fadeOut = true;
            Debug.Log("Queueing music.");
            previousBGMVolume = bgmAudioVolume;
        }
        else
        {
            bgmAudioSource.clip = clipToPlay;
            bgmAudioSource.Play();
            previousBGMVolume = bgmAudioVolume;
        }
    }

    public void QueueMusic(ThemeType theme)
    {
        musicQueue.Enqueue(audioLookup.GetMusic(theme));
    }

    public void PlayMusic(ThemeType theme)
    {
        PlayMusic(audioLookup.GetMusic(theme));
    }

    public void PlayDialogue(AudioClip clipToPlay)
    {
        dialogueAudioSource.clip = clipToPlay;
        dialogueAudioSource.Play();
    }

    public void PlayDialogue(SoundType sound)
    {
        PlayDialogue(audioLookup.GetSound(sound));
    }

    public void StopDialogue()
    {
        dialogueAudioSource.Stop();
    }

    private void Start()
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

        bgmAudioSource.loop = true;
        dialogueAudioSource.loop = true;
        soundQueue = new Queue<AudioClip>();
        musicQueue = new Queue<AudioClip>();

        //We'll use this if we develop a playerprefs setup for players.
        //MasterVolume = GameManager.Instance.Config.MasterVolume;
        //SFXVolume = GameManager.Instance.Config.SFXVolume;
        //BGMVolume = GameManager.Instance.Config.BGMVolume;

        dialogueAudioSource.volume = sfxAudioVolume;
    }

    private void Update()
    {
        ChangeBGMImmediately();
    }

    private void ChangeBGMImmediately()
    {
        if(newImmediateTrack && fadeOut)
        {
            Debug.Log("Lowering volume.");
            bgmAudioVolume -= Time.deltaTime;

            if(bgmAudioVolume <= 0f)
            {
                Debug.Log("Fade out complete.");

                Debug.Log("Playing: " + musicQueue.Peek());
                bgmAudioSource.clip = musicQueue.Dequeue();
                bgmAudioSource.Play();

                fadeOut = false;
                newImmediateTrack = false;
                fadeIn = true;
            }
        }

        if(newDelayedTrack && !bgmAudioSource.isPlaying)
        {
            bgmAudioSource.clip = musicQueue.Dequeue();
            bgmAudioSource.Play();
            newDelayedTrack = false;
        }

        if(fadeIn)
        {
            Debug.Log("Fading in.");
            bgmAudioVolume += Time.deltaTime;

            if(bgmAudioVolume >= previousBGMVolume)
            {
                bgmAudioVolume = previousBGMVolume;
                fadeIn = false;
                previousBGMVolume = 0f;
            }
        }

        if(soundQueue.Count > 0 && !sfxAudioSource.isPlaying)
        {
            sfxAudioSource.clip = soundQueue.Dequeue();
            sfxAudioSource.Play();
        }
    }

    private void UpdateMasterVolume(float value)
    {
        float newValue = value / 10f;
        masterVolume = newValue;
        sfxAudioSource.volume = sfxAudioVolume * masterVolume;
        bgmAudioSource.volume = bgmAudioVolume * masterVolume;

        //We'll use this if we develop a playerprefs setup for players.
        //GameManager.Instance.Config.MasterVolume = value;
    }

    private void UpdateSFXVolume(float value)
    {
        float newValue = value / 10f;
        sfxAudioVolume = newValue;
        sfxAudioSource.volume = sfxAudioVolume * masterVolume;
        dialogueAudioSource.volume = sfxAudioVolume * masterVolume;

        Debug.Log("SFX Volume is " + sfxAudioVolume);
        
        //We'll use this if we develop a playerprefs setup for players.
        //GameManager.Instance.Config.SFXVolume = value;

    }

    private void UpdateBGMVolume(float value)
    {
        float newValue = value / 10f;
        bgmAudioVolume = newValue;
        bgmAudioSource.volume = bgmAudioVolume * masterVolume;

        Debug.Log("SFX Volume is " + bgmAudioVolume);

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

    [System.Serializable]
    private class AudioLookup
    {
        [Header("Themes/BGM")]
        [SerializeField] private AudioClip titleTheme;
        [SerializeField] private AudioClip workShopTheme;
        [SerializeField] private AudioClip combatIntro;
        [SerializeField] private AudioClip combatTheme;
        [SerializeField] private AudioClip bossIntro;
        [SerializeField] private AudioClip bossTheme;
        [SerializeField] private AudioClip creditsTheme;
        [SerializeField] private AudioClip winTheme;
        [SerializeField] private AudioClip lossTheme;
        [Space]
        [Header("General SFX")]
        [SerializeField] private AudioClip cashRegisterSound;
        [SerializeField] private AudioClip positiveButtonSound;
        [SerializeField] private AudioClip negativeButtonSound;
        [SerializeField] private AudioClip recordScratchSound;
        [SerializeField] private AudioClip dialogueSound;
        [Space]
        [Header("Combat SFX")]
        [SerializeField] private AudioClip punchSound;
        [SerializeField] private AudioClip kickSound;
        [SerializeField] private AudioClip blockSound;
        [SerializeField] private AudioClip fireSound;
        [SerializeField] private AudioClip iceSound;
        [SerializeField] private AudioClip plasmaSound;
        [SerializeField] private AudioClip acidSound;
        [SerializeField] private AudioClip beamSound;
        
        public AudioClip GetMusic(ThemeType theme)
        {
            switch (theme)
            {
                case ThemeType.Title:
                    return titleTheme;
                case ThemeType.Workshop:
                    return workShopTheme;
                case ThemeType.CombatIntro:
                    return combatIntro;
                case ThemeType.Combat:
                    return combatTheme;
                case ThemeType.Boss:
                    return bossTheme;
                case ThemeType.Credits:
                    return creditsTheme;
                case ThemeType.Win:
                    return winTheme;
                case ThemeType.Loss:
                    return lossTheme;
                case ThemeType.BossIntro:
                    return bossIntro;
                default:
                    return null;
            }
        }

        public AudioClip GetSound(SoundType sound)
        {
            switch (sound)
            {
                case SoundType.CashRegister:
                    return cashRegisterSound;
                case SoundType.PositiveButton:
                    return positiveButtonSound;
                case SoundType.NegativeButton:
                    return negativeButtonSound;
                case SoundType.RecordScratch:
                    return recordScratchSound;
                case SoundType.Punch:
                    return punchSound;
                case SoundType.Kick:
                    return kickSound;
                case SoundType.Block:
                    return blockSound;
                case SoundType.Fire:
                    return fireSound;
                case SoundType.Ice:
                    return iceSound;
                case SoundType.Plasma:
                    return plasmaSound;
                case SoundType.Acid:
                    return acidSound;
                case SoundType.Dialogue:
                    return dialogueSound;
                case SoundType.Beam:
                    return beamSound;
                default:
                    return null;
            }
        }
    }
}
