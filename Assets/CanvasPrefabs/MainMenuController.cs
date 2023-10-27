using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    private Transform initialPosition;
    private bool canInteract = true;
    private void Start()
    {

        initialPosition = transform;
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);

        if (settingsPanel != null) settingsPanel.SetActive(false);
        settingsPanel.transform.Translate(Vector3.down * 1000);

        if (creditsPanel != null) creditsPanel.SetActive(false);
        creditsPanel.transform.Translate(Vector3.down * 1000);

    }

    public void Play()
    {
        canInteract = false;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void Settings()
    {
        // Enable the settings panel.
        if (canInteract)
        {
            canInteract = false;
            mainMenuPanel.transform.DOMoveY(2000f, 0.8f).OnComplete(() =>
            {
                mainMenuPanel.SetActive(false);
            });
            if (settingsPanel != null) settingsPanel.SetActive(true);
            settingsPanel.transform.DOMoveY(initialPosition.position.y + 90, 1f).OnComplete(() =>
            {
                canInteract = true;
            });
        }

    }

    public void Credits()
    {
        // Enable the credits panel.
        if (canInteract)
        {
            canInteract = false;
            mainMenuPanel.transform.DOMoveY(2000f, 0.8f);
            if (creditsPanel != null) creditsPanel.SetActive(true);
            creditsPanel.transform.DOMoveY(initialPosition.position.y + 90, 1f).OnComplete(() =>
            {
                canInteract = true;
            });
        }
    }

    public void BackToMainMenu()
    {
        if (canInteract)
        {
            canInteract = false; 
            // Add a "Back" button functionality in your Settings and Credits panels to return to the main menu.
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                settingsPanel.transform.DOMoveY(-600f, 1f).OnComplete(() =>
                {
                    settingsPanel.SetActive(false);
                });

            }

            if (creditsPanel != null && creditsPanel.activeSelf)
            {
                creditsPanel.transform.DOMoveY(-600f, 1f).OnComplete(() =>
                {
                    creditsPanel.SetActive(false);
                });

            }

            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
            mainMenuPanel.transform.DOMoveY(initialPosition.position.y + 90, 1f).OnComplete(() =>
            {
                canInteract = true;
            });

        }
    }
}
