using System;
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
    [SerializeField] private AudioListener camUI;
    private GameManager gameManager;
    
    public bool isPaused = false;
    public bool isEnd { set; get; }
    private bool isWin;
    

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

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
        camUI.enabled = true;
        // Make the cursor visible and unlock it
        UnlockCursor(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game time
        isPaused = false;
        pauseMenu.SetActive(false); // Hide the pause menu
        camUI.enabled = false;
        game.SetActive(true);
        UnlockCursor(false); // hide cursor
    }
    public void ReturnToMainMenu()
    {
        gameManager.ResetItems();
        Time.timeScale = 1; // Make sure the game is not paused
        camUI.enabled = false;
        game.SetActive(true);
        ActiveEndBG(false, true);
        UnlockCursor(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1); // Replace '0' with the build index of your main menu scene
    }
    public void TryAgain()
    {
        gameManager.ResetItems();
        Time.timeScale = 1; // Resume the game time
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        camUI.enabled = false;
        game.SetActive(true);
        ActiveEndBG(false, true);
        UnlockCursor(false);
        SceneManager.LoadScene(buildIndex);
    }

    public bool SetLoseScreen()
    {
        if (isWin) return false;
        ActiveEndBG(true, true);
        game.SetActive(false);
        camUI.enabled = true;
        gameOver.SetActive(true);
        pauseMenu.SetActive(false);
        win.SetActive(false);
        die.SetActive(true);
        UnlockCursor(true);
        return true;
    }

    public void SetWinScreen()
    {
        ActiveEndBG(true, false);
        game.SetActive(false);
        camUI.enabled = true;
        gameOver.SetActive(true);
        pauseMenu.SetActive(false);
        die.SetActive(false);
        win.SetActive(true);
        UnlockCursor(true);
    }

    private void ActiveEndBG(bool active, bool lose)
    {
        if (lose)
        {
            endBG.SetActive(active);
        }
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

    public void SetWin()
    {
        isWin = true;
    }
}