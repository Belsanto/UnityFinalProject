using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject die;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject endBG;
    [SerializeField] private GameObject endBG2;
    [SerializeField] private GameObject pauseBG;
    [SerializeField] private GameObject game;
    
    public bool isPaused = false;
    public bool isEnd { set; get; }


    void Update()
    {
        // Check for user input to pause or resume the game
        if (Input.GetKeyDown(KeyCode.Escape) && isEnd == false)
        {
            if (isPaused == false)
            {
                PauseGame();
            } else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // Pause the game time
        isPaused = true;
        pauseMenu.SetActive(true); // Show the pause menu (you need to set this in the Unity Inspector)
        game.SetActive(false);
        // Make the cursor visible and unlock it
        UnlockCursor(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game time
        isPaused = false;
        pauseMenu.SetActive(false); // Hide the pause menu
        game.SetActive(true);
        UnlockCursor(false); // hide cursor
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1; // Make sure the game is not paused
        game.SetActive(true);
        ActiveEndBG(false);
        UnlockCursor(true);
        SceneManager.LoadScene(0); // Replace '0' with the build index of your main menu scene
    }
    public void TryAgain()
    {
        Time.timeScale = 1; // Resume the game time
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        game.SetActive(true);
        ActiveEndBG(false);
        UnlockCursor(false);
        SceneManager.LoadScene(buildIndex);
    }

    public void SetLoseScreen()
    {
        ActiveEndBG(true);
        game.SetActive(false);
        gameOver.SetActive(true);
        pauseMenu.SetActive(false);
        win.SetActive(false);
        die.SetActive(true);
        UnlockCursor(true);
    }

    public void SetWinScreen()
    {
        ActiveEndBG(true);
        game.SetActive(false);
        gameOver.SetActive(true);
        pauseMenu.SetActive(false);
        die.SetActive(false);
        win.SetActive(true);
        UnlockCursor(true);
    }

    private void ActiveEndBG(bool active)
    {
        endBG.SetActive(active);
        endBG2.SetActive(active);
        pauseBG.SetActive(!active);
    }
    private void UnlockCursor(bool active)
    {
        if (active)
        {
            // Make the cursor visible and unlock it
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Make the cursor not visible and lock it
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}