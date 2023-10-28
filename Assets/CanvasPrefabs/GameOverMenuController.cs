using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuController : MonoBehaviour
{
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject die;
    public void TryAgain()
    {
        Time.timeScale = 1; // Resume the game time
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1; // Make sure the game is not paused
        int buildIndex = SceneManager.GetActiveScene().buildIndex - 1;
        SceneManager.LoadScene(buildIndex); // Replace '0' with the build index of your main menu scene
    }

    public void SetLoseScreen()
    {
        win.SetActive(false);
        die.SetActive(true);
    }
}
