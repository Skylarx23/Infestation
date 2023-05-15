using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScene : MonoBehaviour
{
    public AudioSource source;
    public AudioClip deathClip;
    public AudioClip endSongClip;
    public AudioClip[] deathSounds;
    public GameObject endUI;
    // Start is called before the first frame update
    void Start()
    {
        endUI.SetActive(false);
        StartCoroutine(QueenDeath());
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
    }

    private IEnumerator QueenDeath()
    {
        yield return new WaitForSeconds(0.1f);
        source.PlayOneShot(deathClip, 0.05f);
        yield return new WaitForSeconds(1f);
        source.PlayOneShot(deathSounds[0], 0.05f);
        yield return new WaitForSeconds(1.5f);
        source.PlayOneShot(deathSounds[2], 0.05f);
        yield return new WaitForSeconds(2f);
        source.PlayOneShot(deathSounds[1], 0.05f);
        yield return new WaitForSeconds(4f);
        source.PlayOneShot(endSongClip, 0.06f);
        Time.timeScale = 0f;
        EndUI();
    }

    public void EndUI()
    {
        endUI.SetActive(true);
    }

    public void PressMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
