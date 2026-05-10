using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;      // Dành cho nhạc nền
    public AudioSource sfxSource;        // Dành cho sound effects

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip levelSelectMusic;
    public AudioClip[] levelMusic;

    [Header("SFX Clips")]
    public AudioClip explosion;
    public AudioClip gemBreak;
    public AudioClip levelComplete;
    public AudioClip stoneBreak;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private bool musicEnabled = true;
    private bool sfxEnabled = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Setup AudioSources nếu chưa có
        SetupAudioSources();

        // Load saved settings
        LoadAudioSettings();
    }

    private void Start()
    {
        // Setup music source
        if (musicSource != null)
        {
            musicSource.loop = true;
            musicSource.volume = musicVolume;
        }

        // Setup SFX source
        if (sfxSource != null)
        {
            sfxSource.loop = false;
            sfxSource.volume = sfxVolume;
        }

        // Play music theo scene hiện tại
        CheckSceneMusic();

        // Đăng ký sự kiện khi chuyển scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void SetupAudioSources()
    {
        // Tạo music source nếu chưa có
        if (musicSource == null)
        {
            AudioSource[] sources = GetComponents<AudioSource>();
            if (sources.Length > 0)
            {
                musicSource = sources[0];
            }
            else
            {
                musicSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Tạo SFX source nếu chưa có
        if (sfxSource == null)
        {
            AudioSource[] sources = GetComponents<AudioSource>();
            if (sources.Length > 1)
            {
                sfxSource = sources[1];
            }
            else
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    // Load settings từ PlayerPrefs
    private void LoadAudioSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        // Apply settings
        if (musicSource != null)
        {
            musicSource.volume = musicEnabled ? musicVolume : 0f;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
            Debug.Log("SFX Enabled: " + sfxEnabled);
            sfxSource.mute = !sfxEnabled;
        }
    }

    // Lưu settings vào PlayerPrefs
    private void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        PlayerPrefs.SetInt("SFXEnabled", sfxEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    // ==================== MUSIC METHODS ===================

    // Thiết lập Music Volume (0-1)
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);

        if (musicSource != null && musicEnabled)
        {
            musicSource.volume = musicVolume;
        }
        SaveAudioSettings();
    }

    // Lấy Music Volume
    public float GetMusicVolume()
    {
        return musicVolume;
    }

    // Toggle Music On/Off
    public void ToggleMusic(bool isOn)
    {
        musicEnabled = isOn;

        Debug.Log("Music Enabled: " + musicEnabled);

        if (musicSource != null)
        {
            if (isOn)
            {
                musicSource.volume = musicVolume;
                if (!musicSource.isPlaying)
                {
                    CheckSceneMusic();
                }
            }
            else
            {
                musicSource.volume = 0f;
            }
        }
        SaveAudioSettings();
    }

    // Play Main Menu Music
    public void PlayMenuMusic()
    {
        if (menuMusic != null && musicSource != null)
        {
            if (musicSource.clip != menuMusic)
            {
                musicSource.clip = menuMusic;
                musicSource.Play();
            }
        }
    }

    // Play Level Select Music
    public void PlayLevelSelectMusic()
    {
        if (levelSelectMusic != null && musicSource != null)
        {
            if (musicSource.clip != levelSelectMusic)
            {
                musicSource.clip = levelSelectMusic;
                musicSource.Play();
            }
        }
    }

    // Play Level Music 
    public void PlayLevelMusic()
    {
        if (levelMusic != null && levelMusic.Length > 0 && musicSource != null)
        {
            int randomIndex = Random.Range(0, levelMusic.Length);
            AudioClip selectedClip = levelMusic[randomIndex];

            if (musicSource.clip != selectedClip)
            {
                musicSource.clip = selectedClip;
                musicSource.Play();
            }
        }
    }

    // Tự động phát nhạc theo scene
    private void CheckSceneMusic()
    {
        if (!musicEnabled) return;

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Menu")
        {
            PlayMenuMusic();
        }
        else if (sceneName == "LevelSelect")
        {
            PlayLevelSelectMusic();
        }
        else if (sceneName.Contains("Level"))
        {
            PlayLevelMusic();
        }
    }

    // Callback khi scene thay đổi
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneMusic();
    }

    // ==================== SFX METHODS ====================
    // Thiết lập SFX Volume (0-1)
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);

        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }

        SaveAudioSettings();
    }

    // Lấy SFX Volume
    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    // Toggle SFX On/Off
    public void ToggleSFX(bool isOn)
    {
        sfxEnabled = isOn;

        if (sfxSource != null)
        {
            sfxSource.mute = !isOn;
        }

        SaveAudioSettings();
    }

    // Play SFX
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null && sfxEnabled)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    // Play SFX tại vị trí cụ thể
    public void PlaySFXAtPosition(AudioClip clip, Vector3 position)
    {
        if (clip != null && sfxEnabled)
        {
            AudioSource.PlayClipAtPoint(clip, position, sfxVolume);
        }
    }

    // ==================== SPECIFIC SFX METHODS ====================
    // Play Explosion Sound
    public void PlayExplosionSound()
    {
        PlaySFX(explosion);
    }

    // Play gem break
    public void PlayGemBreakSound()
    {
        PlaySFX(gemBreak);
    }

    // Play level Complete
    public void PlayLevelCompleteSound()
    {
        PlaySFX(levelComplete);
    }

    // Play stone break
    public void PlayStoneBreakSound()
    {
        PlaySFX(stoneBreak);
    }

    // ==================== UTILITY ====================
    public void StopAllSounds()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }

        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
    }

    // Pause Music
    public void PauseMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    // Resume Music
    public void ResumeMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.UnPause();
        }
    }

    private void OnDestroy()
    {
        // Unregister scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
