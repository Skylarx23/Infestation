using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using System;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    GameManager gameManager;

    // Camera Shake
    public Text ShakeText;
    public Slider ShakeSlider;

    // Volume Shake
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
        if (isFullscreen)
        {
            FullscreenText.text = "Windowed";
            isFullscreen = false;
        }
        else
        {
            FullscreenText.text = "Fullscreen";
            isFullscreen = true;
        }
    }

    public void ResUpdate()
    {
        int i = (int)ResSlider.value;
        ResText.text = "Resolution: " + resolutions[i].width + "x" + resolutions[i].height + "@" + resolutions[i].refreshRate + "Hz";

        Width = resolutions[i].width;
        Height = resolutions[i].height;
        Hz = resolutions[i].refreshRate;
    }

    public void ShakeUpdate()
    {
        if ((ShakeSlider.value) == 50) ShakeText.text = "Shake: Normal";
        else if ((ShakeSlider.value) == 100) ShakeText.text = "Shake: Maximum";
        else if ((ShakeSlider.value) == 0) ShakeText.text = "Shake: Minimum";
        else ShakeText.text = "Shake: " + (ShakeSlider.value).ToString() + "%";
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
        BrightSlider.value = PlayerPrefs.GetFloat("Brightness");
        VolumeSlider.value = PlayerPrefs.GetFloat("Volume");

        ShakeSlider.value = PlayerPrefs.GetFloat("Screenshake");

        isFullscreen = Convert.ToBoolean(PlayerPrefs.GetString("IsFullscreen"));
        ResText.text = PlayerPrefs.GetString("Resolution");

        Screen.SetResolution(PlayerPrefs.GetInt("ResWidth"), PlayerPrefs.GetInt("ResHeight"),
        Convert.ToBoolean(PlayerPrefs.GetString("IsFullscreen")));

        UpdateAll();
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("Brightness", BrightSlider.value);
        PlayerPrefs.SetFloat("Volume", VolumeSlider.value);

        PlayerPrefs.SetFloat("Screenshake", ShakeSlider.value);

        //PlayerPrefs.SetFloat("Screenshake", ShakeSlider.value);

        PlayerPrefs.SetString("IsFullscreen", Convert.ToString(Screen.fullScreen));

        // Resolution
        PlayerPrefs.SetInt("ResWidth", Width);
        PlayerPrefs.SetInt("ResHeight", Height);
    }

    void UpdateAll()
    {
        BrightnessUpdate();
        VolumeUpdate();
        ShakeUpdate();
        FullscreenUpdate();
        ResUpdate();
    }

    public void ExitOptions()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 0;
    }
}

