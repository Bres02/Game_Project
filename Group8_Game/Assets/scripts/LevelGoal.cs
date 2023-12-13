using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    [SerializeField] AudioSource teleportSound;
    [SerializeField] AudioClip teleportClip;
    public bool lvl1, lvl2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        teleportSound.clip = teleportClip;
        teleportSound.Play();
       if(other.tag == "Player")
        {
            if (lvl1)
            {
                SceneManager.LoadScene("Level_2");
            }else if (lvl2)
            {
                SceneManager.LoadScene("Level_3");
            }else
            {
                SceneManager.LoadScene("WinScreen");
            }
        }
    }
}
