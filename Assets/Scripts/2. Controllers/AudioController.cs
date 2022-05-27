using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioLookup audioLookup;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource dialogueAudioSource;

    private AudioClip queuedMusic;
    private AudioClip nextMusicInQueue;
    private Queue<AudioClip> queuedSounds;
    private float masterVolume = .5f;
    private float sfxAudioVolume = .5f;
    private float bgmAudioVolume = .5f;
    private float dialogueAudioVolume = .5f;
    private bool musicQueued = false;
    private float previousBGMVolume;

    public static AudioController instance;

    public float MasterVolume { get => masterVolume; set => UpdateMasterVolume(value); }
    public float SFXVolume { get => SFXVolume; set => UpdateSFXVolume(value); }
    public float BGMVolume { get => BGMVolume; set => UpdateBGMVolume(value); }
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
            queuedSounds.Enqueue(audioLookup.GetSound(sound));
    }

    public void PlayMusic(AudioClip clipToPlay)
    {
        if (bgmAudioSource.clip == clipToPlay)
            return;
        Debug.Log("Got past the guard.");
        if (bgmAudioSource.clip != null)
        {
            queuedMusic = clipToPlay;
            musicQueued = true;
            previousBGMVolume = bgmAudioVolume;

            Debug.Log("Music is queued.");
        }
        else
        {
            Debug.Log("Playing music immediately.");
            bgmAudioSource.clip = clipToPlay;
            bgmAudioSource.Play();
            previousBGMVolume = bgmAudioVolume;
        }
    }

    public void QueueMusic(ThemeType theme)
    {
        Debug.Log("Adding a theme request to the queue.");
        nextMusicInQueue = audioLookup.GetMusic(theme);
    }

    public void PlayMusic(ThemeType theme)
    {
        Debug.Log("Recieved a theme request.");
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
                Debug.Log("Switching songs.");
                bgmAudioSource.clip = queuedMusic;
                bgmAudioSource.Play();

                queuedMusic = null;
            }
        }

        if(musicQueued && queuedMusic == null)
        {
            UpdateBGMVolume(bgmAudioVolume + Time.deltaTime);

            if(bgmAudioVolume >= previousBGMVolume)
            {
                bgmAudioVolume = previousBGMVolume;
                musicQueued = false;
                previousBGMVolume = 0f;
            }
        }

        if(queuedMusic == null && nextMusicInQueue != null && !bgmAudioSource.isPlaying)
        {
            Debug.Log("Playing next in queue.");
            bgmAudioSource.clip = nextMusicInQueue;
            bgmAudioSource.Play();
            nextMusicInQueue = null;
        }

        if(queuedSounds.Count > 0 && !sfxAudioSource.isPlaying)
        {
            sfxAudioSource.clip = queuedSounds.Dequeue();
            sfxAudioSource.Play();
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

    [System.Serializable]
    private class AudioLookup
    {
        [Header("Themes/BGM")]
        [SerializeField] private AudioClip titleTheme;
        [SerializeField] private AudioClip workShopTheme;
        [SerializeField] private AudioClip combatIntro;
        [SerializeField] private AudioClip combatTheme;
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
