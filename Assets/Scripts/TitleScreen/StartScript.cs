using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Runtime.CompilerServices;

public class StartScript : MonoBehaviour
{
    public GameObject titleScreen, optionsScreen, Loading;

    private void Awake()
    {
        Loading.gameObject.SetActive(false);
    }

    public void PressStart()
    {
        Loading.gameObject.SetActive(true);
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        // SceneManager.LoadScene("Options");
        optionsScreen.gameObject.SetActive(true);
        titleScreen.gameObject.SetActive(false);
    }
}
