/* Este script maneja el tiempo restante en el juego,
 reproduce sonidos de respiración en función del tiempo restante 
 y controla el final del juego si el jugador muere o se queda sin tiempo.
 date: 4/11/2023 belsanto
 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeToLose : MonoBehaviour
{
    [SerializeField] [Range(.5f, 8)] private float initialTime = 8.0f; // Tiempo inicial en minutos (ajusta el valor en el Inspector)
    [SerializeField] TextMeshProUGUI textMeshPro; // Texto para mostrar el tiempo restante
    [SerializeField] TextMeshProUGUI textMeshPro2; // Texto duplicado para mostrar el tiempo restante
    [SerializeField] TextMeshProUGUI textMeshPro3; // Texto duplicado para mostrar el tiempo restante
    [SerializeField] TextMeshProUGUI textMeshPro4; // Texto duplicado para mostrar el tiempo restante
    [SerializeField] private AudioSource normalBreathing; // Sonido de respiración normal
    [SerializeField] private AudioSource mediumBreathing; // Sonido de respiración media
    [SerializeField] private AudioSource fastBreathing; // Sonido de respiración rápida
    [SerializeField] private AudioSource demondSound; // Sonido especial que se reproduce cerca del final del tiempo
    [SerializeField] private GameObject endGameMenu; // Menú de fin de juego
    public float currentTime { get; private set; } // Tiempo restante en segundos
    private float breathTime; // Tiempo de respiración
    private PlayerMovement playerMovement; // Referencia al script PlayerMovement
    private GameManager gameManager; // Referencia al GameManager

    private void Start()
    {
        InitializeComponents(); // Inicializa los componentes al comienzo del juego
        gameManager = GameManager.Instance; // Obtiene una instancia de GameManager
    }

    private void Update()
    {
        PlayAppropriateBreathingSound(); // Reproduce el sonido de respiración apropiado según el tiempo restante
        breathTime -= Time.deltaTime; // Reduce el tiempo de respiración

        if (Math.Abs(breathTime - 30) < 1)
        {
           //Debug.Log("Time: "+breathTime);
            demondSound.Play(); // Reproduce un sonido especial si el tiempo de respiración es cercano a 30 segundos
        }
        UpdateTime(); // Actualiza el tiempo restante
        if (gameManager.IsDeath())
        {
            HandleGameEnd(); // Maneja el final del juego si el jugador muere
        }
    }

    // Inicializa los componentes necesarios al inicio del juego
    private void InitializeComponents()
    {
        breathTime = initialTime * 60; // Convierte el tiempo inicial de minutos a segundos
        currentTime = initialTime * 60; // Convierte el tiempo inicial de minutos a segundos
        playerMovement = FindObjectOfType<PlayerMovement>(); // Encuentra el script PlayerMovement en la escena
        UpdateTimeText(); // Actualiza el texto de tiempo en la interfaz de usuario
    }

    // Actualiza el tiempo restante en cada frame
    private void UpdateTime()
    {
        if (currentTime > 0)
        {
            ReduceTime(); // Reduce el tiempo restante
            UpdateTimeText(); // Actualiza el texto de tiempo en la interfaz de usuario
        }
        else
        {
            HandleGameEnd(); // Maneja el final del juego si el tiempo llega a cero
        }
    }

    // Reduce el tiempo restante en cada frame
    private void ReduceTime()
    {
        currentTime -= Time.deltaTime;
    }

    // Maneja el final del juego
    private void HandleGameEnd()
    {
        Debug.Log("¡Perdiste!"); // Muestra un mensaje en la consola
        if (!ShowEndGameMenu()) return; // Muestra el menú de fin de juego
        DisablePlayerLook(); // Deshabilita la capacidad del jugador para mirar
        SetCursorVisible(); // Muestra el cursor
        SetCursorLockState(); // Cambia el estado del bloqueo del cursor
        StopGameTime(); // Detiene el tiempo del juego
    }

    // Muestra el menú de fin de juego
    private bool ShowEndGameMenu()
    {
        return endGameMenu.GetComponent<MenuController>().SetLoseScreen(); // Activa la pantalla de perder en el menú de fin de juego
    }

    // Deshabilita la capacidad del jugador para mirar
    private void DisablePlayerLook()
    {
        playerMovement.SetIsAbleToLook(false);
    }

    // Muestra el cursor en la pantalla
    private void SetCursorVisible()
    {
        Cursor.visible = true;
    }

    // Cambia el estado del bloqueo del cursor
    private void SetCursorLockState()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Detiene el tiempo del juego
    private void StopGameTime()
    {
        Time.timeScale = 0f;
    }

    // Actualiza el texto de tiempo en la interfaz de usuario
    private void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60); // Calcula los minutos restantes
        int seconds = Mathf.FloorToInt(currentTime % 60); // Calcula los segundos restantes
        string timeText = string.Format("EVACUATE! {0:00}:{1:00}", minutes, seconds); // Formatea el tiempo restante en el formato "EVACUATE! MM:SS"
        textMeshPro.text = timeText; // Actualiza el texto de tiempo en la interfaz de usuario
        textMeshPro2.text = timeText; // Actualiza el texto duplicado de tiempo en la interfaz de usuario
        textMeshPro3.text = timeText; // Actualiza el texto duplicado de tiempo en la interfaz de usuario
        textMeshPro4.text = timeText; // Actualiza el texto duplicado de tiempo en la interfaz de usuario
    }

    // Reproduce el sonido de respiración apropiado según el tiempo restante
    private void PlayAppropriateBreathingSound()
    {
        if (breathTime < (initialTime * 60) * 0.7f && (breathTime > (initialTime * 60) * 0.3f)) // Si el tiempo restante es menos del 60%
        {
            PlayNormalBreathing(); // Reproduce el sonido de respiración normal
        }
        
        if (breathTime < (initialTime * 60) * 0.3f && (breathTime > (initialTime * 60) * 0.15f) ) // Si el tiempo restante es menos del 30%
        {
            PlayMediumBreathing(); // Reproduce el sonido de respiración media
        }
        
        if (breathTime < (initialTime * 60) * 0.15f) // Si el tiempo restante es menos del 15%
        {
            PlayFastBreathing(); // Reproduce el sonido de respiración rápida
        }
    }

    // Reproduce el sonido de respiración normal
    private void PlayNormalBreathing()
    {
        if (!normalBreathing.isPlaying)
        {
            StopAllBreathing(); // Detiene todos los sonidos de respiración
            normalBreathing.Play(); // Reproduce el sonido de respiración normal
        }
    }

    // Reproduce el sonido de respiración media
    private void PlayMediumBreathing()
    {
        if (!mediumBreathing.isPlaying)
        {
            StopAllBreathing(); // Detiene todos los sonidos de respiración
            Debug.Log("Mid Breath");
            mediumBreathing.Play(); // Reproduce el sonido de respiración media
        }
    }

    // Reproduce el sonido de respiración rápida
    private void PlayFastBreathing()
    {
        if (!fastBreathing.isPlaying)
        {
            StopAllBreathing(); // Detiene todos los sonidos de respiración
            Debug.Log("Fast Breath");
            fastBreathing.Play(); // Reproduce el sonido de respiración rápida
        }
    }

    // Detiene todos los sonidos de respiración
    private void StopAllBreathing()
    {
        normalBreathing.Stop(); // Detiene el sonido de respiración normal
        mediumBreathing.Stop(); // Detiene el sonido de respiración media
        fastBreathing.Stop(); // Detiene el sonido de respiración rápida
    }
}
