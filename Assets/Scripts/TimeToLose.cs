using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeToLose : MonoBehaviour
{
    [SerializeField] [Range(.5f, 8)] private float initialTime = 8.0f; // Tiempo inicial en minutos (ajusta el valor en el Inspector)
    private TextMeshProUGUI textMeshPro;
    [SerializeField] private AudioSource normalBreathing;
    [SerializeField] private AudioSource mediumBreathing;
    [SerializeField] private AudioSource fastBreathing;
    [SerializeField] private AudioSource demondSound;
    [SerializeField] private GameObject endGameMenu; 
    public float currentTime { get; private set; }
    private float breathTime;
    private PlayerMovement playerMovement;
    private GameManager gameManager;
    private void Start()
    {
        InitializeComponents();
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        PlayAppropriateBreathingSound();
        breathTime -= Time.deltaTime;

        if (Math.Abs(breathTime - 8) < 1)
        {
           //Debug.Log("Time: "+breathTime);
            demondSound.Play();
        }
        UpdateTime();
        if (gameManager.IsDeath())
        {
            HandleGameEnd();
        }
    }

    private void InitializeComponents()
    {
        breathTime = initialTime * 60;
        currentTime = initialTime * 60; // Convierte minutos a segundos
        textMeshPro = GetComponent<TextMeshProUGUI>();
        playerMovement = FindObjectOfType<PlayerMovement>(); // referencia al script ImprovedPlayerMovement
        UpdateTimeText();
    }

    private void UpdateTime()
    {
        if (currentTime > 0)
        {
            ReduceTime();
            UpdateTimeText();
        }
        else
        {
            HandleGameEnd();
        }
    }

    private void ReduceTime()
    {
        currentTime -= Time.deltaTime;
    }

    private void HandleGameEnd()
    {
        Debug.Log("¡Perdiste!");
        ShowEndGameMenu();
        DisablePlayerLook();
        SetCursorVisible();
        SetCursorLockState();
        StopGameTime();
    }

    private void ShowEndGameMenu()
    {
        endGameMenu.GetComponent<MenuController>().SetLoseScreen();
    }

    private void DisablePlayerLook()
    {
        playerMovement.SetIsAbleToLook(false);
    }

    private void SetCursorVisible()
    {
        Cursor.visible = true;
    }

    private void SetCursorLockState()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void StopGameTime()
    {
        Time.timeScale = 0f;
    }

    private void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        string timeText = string.Format("ESCAPE! : {0:00}:{1:00}", minutes, seconds);
        textMeshPro.text = timeText;

    }

    private void PlayAppropriateBreathingSound()
    {
        if (breathTime > (initialTime * 60) * 0.5f) // Si el tiempo es más del 50%
        {
            PlayNormalBreathing();
        }
        else if (breathTime > (initialTime * 60) * 0.3f) // Si el tiempo es más del 15%
        {
            PlayMediumBreathing();
        }
        else // Si el tiempo es menor o igual al 15%
        {
            PlayFastBreathing();
        }
    }

    private void PlayNormalBreathing()
    {
        if (!normalBreathing.isPlaying)
        {
            StopAllBreathing();
            normalBreathing.Play();
        }
    }

    private void PlayMediumBreathing()
    {
        if (!mediumBreathing.isPlaying)
        {
            StopAllBreathing();
            Debug.Log("Mid Breath");
            mediumBreathing.Play();
        }
    }

    private void PlayFastBreathing()
    {
        if (!fastBreathing.isPlaying)
        {
            StopAllBreathing();
            Debug.Log("Fast Breath");
            fastBreathing.Play();
        }
    }

    private void StopAllBreathing()
    {
        normalBreathing.Stop();
        mediumBreathing.Stop();
        fastBreathing.Stop();
    }
}
