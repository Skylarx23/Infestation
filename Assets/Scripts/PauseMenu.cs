using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Pausing
    public static bool isPaused = false;
    public GameObject pauseMenu;

    // Cooldown
    [SerializeField] float EscapeCooldown = 1;
    public bool IsHeldDown;

    GameManager gameManager;

    void Update()
    {
        // Pauses/Resumes the game whenever a player presses "pause"
        if (Input.GetKey(KeyCode.P) && !IsHeldDown)
        {
            if (isPaused) Resume(); else Pause();
            IsHeldDown = true;
        }
        else if(Input.GetKeyUp(KeyCode.P)) {IsHeldDown = false;}
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Options()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Options");
    }

}
