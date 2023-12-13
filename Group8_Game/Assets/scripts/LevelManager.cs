using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void StartInstructions()
    {
        SceneManager.LoadScene("InstructionScreen");
    }

    public void LoadLevelTwo()
    {
        SceneManager.LoadScene("Level_2");
    }

    public void LoadLevelThree()
    {
        SceneManager.LoadScene("Level_3");
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("LoseScreen");
    }

    public void LoadVictory()
    {
        SceneManager.LoadScene("WinScreen");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
        //Comment out unity Editor before making the final build
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
