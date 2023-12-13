using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundAudio : MonoBehaviour
{
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "LoseScreen" || SceneManager.GetActiveScene().name != "WinScreen")
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "LoseScreen" || SceneManager.GetActiveScene().name == "WinScreen")
        {
            Destroy(this.gameObject);
        }
    }
}
