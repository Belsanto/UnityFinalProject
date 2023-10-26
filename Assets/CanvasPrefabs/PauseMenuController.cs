using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenu;

    void Update()
    {
        // Check for user input to pause or resume the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // Pause the game time
        isPaused = true;
        pauseMenu.SetActive(true); // Show the pause menu (you need to set this in the Unity Inspector)
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game time
        isPaused = false;
        pauseMenu.SetActive(false); // Hide the pause menu
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1; // Make sure the game is not paused
        SceneManager.LoadScene(0); // Replace '0' with the build index of your main menu scene
    }
}