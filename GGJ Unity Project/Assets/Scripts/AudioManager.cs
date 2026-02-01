using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip randomMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip loseMusic;

    [Header("Wave Music")]
    [SerializeField] private AudioClip wave1To10Music;
    [SerializeField] private AudioClip wave11To20Music;

    [Header("UI SFX")]
    [SerializeField] private AudioClip selectMp3;
    [SerializeField] private AudioClip selectWav;
    [SerializeField] private AudioClip menuUp;
    [SerializeField] private AudioClip menuDown;
    [SerializeField] private AudioClip pauseScreen;

    [Header("Player SFX")]
    [SerializeField] private AudioClip swosh;
    [SerializeField] private AudioClip harleyAttack;
    [SerializeField] private AudioClip perryAttack;
    [SerializeField] private AudioClip swapHappyLong;
    [SerializeField] private AudioClip swapHappyShort;
    [SerializeField] private AudioClip swapSadLong;
    [SerializeField] private AudioClip swapSadShort;

    [Header("Enemy SFX")]
    [SerializeField] private AudioClip spitSfx;
    [SerializeField] private AudioClip redBloodCellSfx;
    [SerializeField] private AudioClip whiteBloodCellSfx;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource stingerSource;

    [Header("Volumes")]
    [Range(0f, 1f)] [SerializeField] private float musicVolume = 0.7f;
    [Range(0f, 1f)] [SerializeField] private float sfxVolume = 0.9f;
    [Range(0f, 1f)] [SerializeField] private float stingerVolume = 0.9f;

    private bool _useLongHappy = true;
    private bool _useLongSad = true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance != null)
        {
            return;
        }

        var go = new GameObject("AudioManager");
        go.AddComponent<AudioManager>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        EnsureSources();
        ApplyVolumes();
        TryAutoLoadClips();

        SceneManager.sceneLoaded += HandleSceneLoaded;

        var activeScene = SceneManager.GetActiveScene();
        if (activeScene.IsValid() && activeScene.isLoaded)
        {
            HandleSceneLoaded(activeScene, LoadSceneMode.Single);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }
    }

    private void EnsureSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
        }

        if (stingerSource == null)
        {
            stingerSource = gameObject.AddComponent<AudioSource>();
            stingerSource.loop = false;
        }
    }

    private void ApplyVolumes()
    {
        if (musicSource != null) musicSource.volume = musicVolume;
        if (sfxSource != null) sfxSource.volume = sfxVolume;
        if (stingerSource != null) stingerSource.volume = stingerVolume;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneName.IndexOf("lose", System.StringComparison.OrdinalIgnoreCase) >= 0)
        {
            PlayLoseMusic();
            return;
        }

        if (sceneName.IndexOf("win", System.StringComparison.OrdinalIgnoreCase) >= 0)
        {
            PlayWinMusic();
            return;
        }

        if (sceneName.IndexOf("mainmenu", System.StringComparison.OrdinalIgnoreCase) >= 0)
        {
            PlayMenuMusic();
            return;
        }

        PlayRandomMusic();
    }

    public float MenuSelectDuration
    {
        get
        {
            if (selectMp3 != null) return selectMp3.length;
            if (selectWav != null) return selectWav.length;
            return 0f;
        }
    }

    public void PlayMenuMusic() => PlayMusic(mainMenuMusic);
    public void PlayRandomMusic() => PlayMusic(randomMusic);
    public void PlayWinMusic() => PlayMusic(winMusic);
    public void PlayLoseMusic() => PlayMusic(loseMusic);

    public void PlayMenuSelect() => PlaySfx(selectMp3 != null ? selectMp3 : selectWav);
    public void PlayInGameSelect() => PlaySfx(selectWav != null ? selectWav : selectMp3);
    public void PlayMenuUp() => PlaySfx(menuUp);
    public void PlayMenuDown() => PlaySfx(menuDown);
    public void PlayPause() => PlaySfx(pauseScreen);

    public void PlaySwosh() => PlaySfx(swosh);
    public void PlayHarleyAttack() => PlaySfx(harleyAttack);
    public void PlayPerryAttack() => PlaySfx(perryAttack);
    public void PlaySpit() => PlaySfx(spitSfx);
    public void PlayRedBloodCell() => PlaySfx(redBloodCellSfx);
    public void PlayWhiteBloodCell() => PlaySfx(whiteBloodCellSfx);

    public void PlaySwap(bool toSupport)
    {
        if (toSupport)
        {
            PlaySfx(_useLongHappy ? swapHappyLong : swapHappyShort);
            _useLongHappy = !_useLongHappy;
        }
        else
        {
            PlaySfx(_useLongSad ? swapSadLong : swapSadShort);
            _useLongSad = !_useLongSad;
        }
    }

    public void PlayWaveStinger(int waveNumber)
    {
        if (waveNumber <= 0)
        {
            return;
        }

        if (waveNumber <= 10)
        {
            PlayStinger(wave1To10Music);
        }
        else
        {
            PlayStinger(wave11To20Music);
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null)
        {
            return;
        }

        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    private void PlayStinger(AudioClip clip)
    {
        if (clip == null || stingerSource == null)
        {
            return;
        }

        stingerSource.PlayOneShot(clip);
    }

    private void TryAutoLoadClips()
    {
#if UNITY_EDITOR
        if (mainMenuMusic == null) mainMenuMusic = LoadClip("Assets/Sounds/Main Menu Music.wav");
        if (randomMusic == null) randomMusic = LoadClip("Assets/Sounds/Random Music.wav");
        if (winMusic == null) winMusic = LoadClip("Assets/Sounds/Win Music.wav");
        if (loseMusic == null) loseMusic = LoadClip("Assets/Sounds/Lose Music.wav");

        if (wave1To10Music == null) wave1To10Music = LoadClip("Assets/Sounds/Wave 1-10.wav");
        if (wave11To20Music == null) wave11To20Music = LoadClip("Assets/Sounds/Wave 11-20.wav");

        if (selectMp3 == null) selectMp3 = LoadClip("Assets/Sounds/Select.mp3");
        if (selectWav == null) selectWav = LoadClip("Assets/Sounds/Select.wav");
        if (menuUp == null) menuUp = LoadClip("Assets/Sounds/Menu Up.wav");
        if (menuDown == null) menuDown = LoadClip("Assets/Sounds/Menu Down.wav");
        if (pauseScreen == null) pauseScreen = LoadClip("Assets/Sounds/Pause Screen.wav");

        if (swosh == null) swosh = LoadClip("Assets/Sounds/Swosh.mp3");
        if (harleyAttack == null) harleyAttack = LoadClip("Assets/Sounds/Harley Attack.wav");
        if (perryAttack == null) perryAttack = LoadClip("Assets/Sounds/Perry Attack.wav");
        if (swapHappyLong == null) swapHappyLong = LoadClip("Assets/Sounds/Swap Happy Long.wav");
        if (swapHappyShort == null) swapHappyShort = LoadClip("Assets/Sounds/Swap Happy Short.wav");
        if (swapSadLong == null) swapSadLong = LoadClip("Assets/Sounds/Swap Sad Long.wav");
        if (swapSadShort == null) swapSadShort = LoadClip("Assets/Sounds/Swap Sad Short.wav");

        if (spitSfx == null) spitSfx = LoadClip("Assets/Sounds/Spit SFX.wav");
        if (redBloodCellSfx == null) redBloodCellSfx = LoadClip("Assets/Sounds/Red Blood Cell SFX.wav");
        if (whiteBloodCellSfx == null) whiteBloodCellSfx = LoadClip("Assets/Sounds/White Blood Cell SFX.wav");
#endif
    }

#if UNITY_EDITOR
    private static AudioClip LoadClip(string path)
    {
        return AssetDatabase.LoadAssetAtPath<AudioClip>(path);
    }
#endif
}
