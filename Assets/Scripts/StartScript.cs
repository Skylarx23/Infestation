using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void PressStart()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
