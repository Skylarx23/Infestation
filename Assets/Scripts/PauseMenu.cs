using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.UIElements;
=======
>>>>>>> 99c338c1d0277761e4a3c6fc6071a60e05a18974
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
<<<<<<< HEAD
=======
        Cursor.lockState = CursorLockMode.Locked;
>>>>>>> 99c338c1d0277761e4a3c6fc6071a60e05a18974
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    void Pause()
    {
<<<<<<< HEAD
=======
        Cursor.lockState = CursorLockMode.None;
>>>>>>> 99c338c1d0277761e4a3c6fc6071a60e05a18974
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
