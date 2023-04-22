using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject optionsScreen;
    // Start is called before the first frame update
    public void PressStart()
    {
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
