using System;
using UnityEngine;
using UnityEngine.UI;

public class EditSound : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    public Toggle musicToggle;
    public Toggle sfxToggle;

    private void Start()
    {
        // Load volume hiện tại từ AudioManager hoặc PlayerPrefs
        if (AudioManager.Instance != null)
        {
            musicSlider.value = AudioManager.Instance.GetMusicVolume();
            sfxSlider.value = AudioManager.Instance.GetSFXVolume();
        }
        else
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }

        // Lấy trạng thái bật/tắt đã lưu từ PlayerPrefs và cập nhật các toggle
        musicToggle.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        sfxToggle.isOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        // Gắn sự kiện khi slider thay đổi
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        // Gắn sự kiện khi toggle thay đổi
        musicToggle.onValueChanged.AddListener(OnMusicToggled);
        sfxToggle.onValueChanged.AddListener(OnSFXToggled);
    }

    private void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
        else
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
            PlayerPrefs.Save();
        }
    }

    private void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save();
        }
    }

    public void OnMusicToggled(bool isOn)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleMusic(isOn);
        }

        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();

        musicSlider.interactable = isOn; // Vô hiệu hóa slider nếu tắt nhạc
    }

    public void OnSFXToggled(bool isOn)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleSFX(isOn);
        }
        PlayerPrefs.SetInt("SFXEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();

        sfxSlider.interactable = isOn; // Vô hiệu hóa slider nếu tắt âm thanh
    }

    private void OnDestroy()
    {
        // Cleanup listeners để tránh memory leak
        musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        musicToggle.onValueChanged.RemoveListener(OnMusicToggled);
        sfxToggle.onValueChanged.RemoveListener(OnSFXToggled);
    }
}
