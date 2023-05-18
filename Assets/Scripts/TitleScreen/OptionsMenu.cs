using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class OptionsMenu : MonoBehaviour
{
    GameManager gameManager;

    // Camera Sensitivity
    public Text SensitivityText;
    public Slider SensitivitySlider;

    // Volume Sensitivity
    public Text VolumeText;
    public Slider VolumeSlider;

    // Fullscreen
    public Text FullscreenText;
    bool isFullscreen;
    int Width, Height, Hz;

    // Resolution
    public Text ResText;
    public Slider ResSlider;
    Resolution[] resolutions;

    // Brightness
    public Text BrightText;
    public Slider BrightSlider;

    public GameObject titleScreen;
    public GameObject optionsScreen;

    private void Start()
    {
        // Gets a List of All Supported Resolutions Then Sets the Max Value to the Amount Of Resolutions and Sets it To that Max
        resolutions = Screen.resolutions;
        ResSlider.maxValue = resolutions.Length - 1;
        ResSlider.value = resolutions.Length;

        ApplyOptions();
    }

    public void FullscreenUpdate()
    {
        isFullscreen = !isFullscreen;

        if (isFullscreen) FullscreenText.text = "Windowed";
        else FullscreenText.text = "Fullscreen";
    }

    public void ResUpdate()
    {
        int i = (int)ResSlider.value;
        ResText.text = "Resolution: " + resolutions[i].width + "x" + resolutions[i].height + "@" + resolutions[i].refreshRate + "Hz";

        Width = resolutions[i].width;
        Height = resolutions[i].height;
        Hz = resolutions[i].refreshRate;
    }

    public void SensitivityUpdate()
    {
        if ((SensitivitySlider.value) == 50) SensitivityText.text = "Mouse Sensitivity: Normal";
        else if ((SensitivitySlider.value) == 100) SensitivityText.text = "Mouse Sensitivity: Maximum";
        else if ((SensitivitySlider.value) == 0) SensitivityText.text = "Mouse Sensitivity: Minimum";
        else SensitivityText.text = "Mouse Sensitivity: " + (SensitivitySlider.value).ToString() + "%";
    }

    public void VolumeUpdate()
    {
        if ((VolumeSlider.value) == 50) VolumeText.text = "Volume: Normal";
        else if ((VolumeSlider.value) == 100) VolumeText.text = "Volume: Maximum";
        else if ((VolumeSlider.value) == 0) VolumeText.text = "Volume: Minimum";
        else VolumeText.text = "Volume: " + (VolumeSlider.value).ToString() + "%";
    }

    public void BrightnessUpdate()
    {
        if ((BrightSlider.value) == 50) BrightText.text = "Brightness: Normal";
        else if ((BrightSlider.value) == 100) BrightText.text = "Brightness: Maximum";
        else if ((BrightSlider.value) == 0) BrightText.text = "Brightness: Minimum";
        else BrightText.text = "Brightness: " + (BrightSlider.value).ToString() + "%";
    }

    void ApplyOptions()
    {
        // Sets Res and Fullscreen
        Screen.SetResolution(PlayerPrefs.GetInt("ResWidth"), PlayerPrefs.GetInt("ResHeight"),
        Convert.ToBoolean(PlayerPrefs.GetString("isFullscreen")), PlayerPrefs.GetInt("ResHz"));

        AudioListener.volume = PlayerPrefs.GetFloat("Volume") / 50;
        Screen.brightness = PlayerPrefs.GetFloat("Brightness") / 50;

        MouseLook.mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity") * 5;

        UpdateAll();
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("Brightness", BrightSlider.value);
        PlayerPrefs.SetFloat("Volume", VolumeSlider.value);

        PlayerPrefs.SetFloat("Sensitivity", SensitivitySlider.value);

        PlayerPrefs.SetString("isFullscreen", Convert.ToString(isFullscreen));

        // Resolution
        PlayerPrefs.SetInt("ResWidth", Width);
        PlayerPrefs.SetInt("ResHeight", Height);
        PlayerPrefs.SetInt("ResHz", Hz);

        ApplyOptions();
    }

    void UpdateAll()
    {

        BrightSlider.value = PlayerPrefs.GetFloat("Brightness");
        VolumeSlider.value = PlayerPrefs.GetFloat("Volume");
        SensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");

        if (Convert.ToBoolean(PlayerPrefs.GetString("isFullscreen"))) FullscreenText.text = "Fullscreen";
        else FullscreenText.text = "Windowed";

        BrightnessUpdate();
        VolumeUpdate();
        SensitivityUpdate();
        FullscreenUpdate();
        ResUpdate();
    }

    public void ExitOptions()
    {
        optionsScreen.gameObject.SetActive(false);
        titleScreen.gameObject.SetActive(true);
    }
}
